using ClickDoc.Database.Entities;
using ClickDoc.Database.Repositories;
using ClickDoc.Utils;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace ClickDoc.ViewModels.Entrepreneurs
{
    public class NewEntrepreneurVM : ViewModelBase
    {
        private string _surname = string.Empty;
        private string _name = string.Empty;
        private string _patronymic = string.Empty;
        private string _ogrnip = string.Empty;

        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigation;
        private readonly IRepository<EntrepreneurEntity> _repository;

        public ICommand CreateCommand { get; }
        public ICommand CloseCommand { get; }

        public NewEntrepreneurVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigation = _serviceProvider.GetRequiredService<INavigationService>();
            _repository = _serviceProvider.GetRequiredService<IRepository<EntrepreneurEntity>>();
            CreateCommand = new RelayCommand(CreateNew);
            CloseCommand = new RelayCommand(Close);
        }

        private void Close()
        {
            _navigation.CloseCurrentWindow();
        }

        private void CreateNew()
        {
            _navigation.CloseCurrentWindow();

            EntrepreneurEntity entity = new()
            {
                Name = _name,
                Surname = _surname,
                Patronymic = _patronymic,
                OGRNIP = _ogrnip,
            };
            _repository.Add(entity);

            MessageBox.Show($"{entity.Surname} {entity.Name} добавлен(а) в БД");
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
