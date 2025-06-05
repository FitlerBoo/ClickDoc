using ClickDoc.Database.Entities;
using ClickDoc.Database.Repositories;
using ClickDoc.Utils;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactiveValidation;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;

namespace ClickDoc.ViewModels.Entrepreneurs
{
    public class NewEntrepreneurVM : ValidatableObject
    {
        private string _surname = string.Empty;
        private string _name = string.Empty;
        private string _patronymic = string.Empty;
        private string _ogrnip = string.Empty;
        private bool _isButtonEnabled = true;
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigation;
        private readonly IRepository<EntrepreneurEntity> _repository;
        private readonly INotificationService _notificationService;

        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set
            {
                _isButtonEnabled = value;
                OnPropertyChanged(nameof(IsButtonEnabled));
            }
        }

        public ICommand CreateCommand { get; }
        public ICommand CloseCommand { get; }

        public NewEntrepreneurVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigation = _serviceProvider.GetRequiredService<INavigationService>();
            _repository = _serviceProvider.GetRequiredService<IRepository<EntrepreneurEntity>>();
            _notificationService = _serviceProvider.GetRequiredService<INotificationService>();
            CreateCommand = new AsyncRelayCommand(CreateNew);
            CloseCommand = new RelayCommand(Close);
        }

        private void Close()
        {
            _navigation.CloseCurrentWindow();
        }

        private async Task CreateNew()
        {
            try
            {
                IsButtonEnabled = false;
                EntrepreneurEntity entity = new()
                {
                    Name = _name,
                    Surname = _surname,
                    Patronymic = _patronymic,
                    OGRNIP = _ogrnip,
                };
                await _repository.Add(entity);
                _notificationService.ShowSuccess($"{entity.Surname} {entity.Name} добавлен(а) в БД");
                IsButtonEnabled = true;
                _navigation.CloseCurrentWindow();
            }
            catch (DbUpdateException)
            {
                _notificationService.ShowError("Ошибка базы данных");
                IsButtonEnabled = true;
            }
            catch (Exception ex)
            {
                _notificationService.ShowError(ex.Message);
                IsButtonEnabled= true;
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Patronymic
        {
            get => _patronymic;
            set
            {
                _patronymic = value;
                OnPropertyChanged(nameof(Patronymic));
            }
        }

        public string OGRNIP
        {
            get => _ogrnip;
            set
            {
                _ogrnip = value;
                OnPropertyChanged(nameof(OGRNIP));
            }
        }
    }
}
