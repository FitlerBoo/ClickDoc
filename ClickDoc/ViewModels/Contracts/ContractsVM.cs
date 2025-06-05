using ClickDoc.Database.Entities;
using ClickDoc.Database.Repositories;
using ClickDoc.Utils;
using ClickDoc.Views.Contracts;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ClickDoc.ViewModels.Contracts
{
    public class ContractsVM : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigationService;
        private readonly IRepository<ContractEntity> _repository;
        private readonly INotificationService _notificationService;
        private ObservableCollection<ContractEntity> _contracts = [];
        private ContractEntity _selectedItem;

        public ICommand CreateNewCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ObservableCollection<ContractEntity> Contracts => _contracts;
        public bool IsItemSelected => SelectedItem != null;

        public ContractEntity SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                OnPropertyChanged(nameof(IsItemSelected));
            }
        }

        public ContractsVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            _repository = _serviceProvider.GetRequiredService<IRepository<ContractEntity>>();
            _notificationService = _serviceProvider.GetRequiredService<INotificationService>();
            _repository.ItemAdded += OnItemAdded;
            _repository.ItemRemoved += OnItemRemoved;

            LoadDataAsync();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CreateNewCommand = new RelayCommand(CreateNew);
            DeleteCommand = new AsyncRelayCommand(Delete);
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var contractors = await _repository.GetAll();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Contracts.Clear();
                    foreach (var e in contractors)
                        Contracts.Add(e);
                });
            }catch (Exception ex)
            {
                _notificationService.ShowError(
                    $"Ошибка загрузки элементов из базы данных:\n{ex.Message}");
            }
        }

        private async Task Delete()
        {
            try
            {
                var contract = SelectedItem;
                await _repository.Delete(SelectedItem.Id);
                _notificationService.ShowSuccess($"{contract.ContractNumber} удален из БД");
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Ошибка удаления из базы данных:\n{ex.Message}");
            }
        }

        private void CreateNew()
        {
            _navigationService.NavigateTo<NewContractWindow>();
        }

        private void OnItemRemoved(ContractEntity entity)
        {
            Application.Current.Dispatcher.Invoke(() =>
            Contracts.Remove(Contracts.FirstOrDefault(x => x.Id == entity.Id)));
        }

        private void OnItemAdded(ContractEntity entity)
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                try
                {
                    var fullEntity = await _repository.GetById(entity.Id);
                    if (fullEntity != null)
                        Contracts.Add(entity);
                }
                catch (Exception ex)
                {
                    _notificationService.ShowError($"Ошибка загрузки элемента из базы данных:\n{ex.Message}");
                }
            });

        }
    }
}
