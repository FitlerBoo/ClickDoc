using ClickDoc.Database.Entities;
using ClickDoc.Database.Repositories;
using ClickDoc.Utils;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;

namespace ClickDoc.ViewModels.Contracts
{
    public class NewContractVM : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigation;

        public ICommand CreateCommand { get; }
        public ICommand CloseCommand { get; }

        private string _contractNumber;
        private EntrepreneurEntity _entrepreneur;
        private ContractorEntity _contractor;

        private readonly IRepository<ContractEntity> _repository;
        private readonly INotificationService _notificationService;
        private ObservableCollection<ContractorEntity> _contractors = [];
        private ObservableCollection<EntrepreneurEntity> _entrepreneurs = [];
        private bool _isButtonEnabled = true;

        public ObservableCollection<ContractorEntity> Contractors
        {
            get => _contractors;
            set
            {
                _contractors = value;
                OnPropertyChanged(nameof(Contractors));
            }
        }
        public ObservableCollection<EntrepreneurEntity> Entrepreneurs
        {
            get => _entrepreneurs;
            set
            {
                _entrepreneurs = value;
                OnPropertyChanged(nameof(Entrepreneurs));
            }
        }

        public string ContractNumber
        {
            get => _contractNumber;
            set
            {
                _contractNumber = value;
                OnPropertyChanged(nameof(ContractNumber));
            }
        }
        public EntrepreneurEntity Entrepreneur
        {
            get => _entrepreneur;
            set
            {
                _entrepreneur = value;
                OnPropertyChanged(nameof(Entrepreneur));
            }
        }
        public ContractorEntity Contractor
        {
            get => _contractor;
            set
            {
                _contractor = value;
                OnPropertyChanged(nameof(Contractor));
            }
        }
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set
            {
                _isButtonEnabled = value;
                OnPropertyChanged(nameof(IsButtonEnabled));
            }
        }

        public NewContractVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigation = _serviceProvider.GetRequiredService<INavigationService>();
            _repository = _serviceProvider.GetRequiredService<IRepository<ContractEntity>>();
            _notificationService = _serviceProvider.GetRequiredService<INotificationService>();
            CreateCommand = new AsyncRelayCommand(CreateNew);
            CloseCommand = new RelayCommand(Close);

            LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            Contractors = await InitializeCollectionAsync<ContractorEntity>();
            Entrepreneurs = await InitializeCollectionAsync<EntrepreneurEntity>();
        }

        private async Task<ObservableCollection<T>> InitializeCollectionAsync<T>()
            where T : Entity
        {
            var collection = new ObservableCollection<T>();
            var repository = _serviceProvider.GetRequiredService<IRepository<T>>();
            try
            {
                var entities = await repository.GetAll();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    collection.Clear();
                    foreach (var e in entities)
                        collection.Add(e);
                });
            }
            catch (Exception ex)
            {
                _notificationService.ShowError(
                    $"Ошибка загрузки элементов из базы данных\n{ex.Message}");
            }
            return collection;
        }

        private void Close()
        {
            _navigation.CloseCurrentWindow();
        }

        private async Task CreateNew()
        {
            IsButtonEnabled = false;

            if (_contractNumber[0] == '№')
                _contractNumber = _contractNumber.Remove(0,1);

            ContractEntity entity = new()
            {
                ContractNumber = _contractNumber,
                EntrepreneurId = _entrepreneur.Id,
                ContractorId = _contractor.Id
            };
            try
            {
                await _repository.Add(entity);
                _notificationService.ShowSuccess($"{entity.ContractNumber} добавлен в БД");
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
                IsButtonEnabled = true;
            }
        }
    }
}
