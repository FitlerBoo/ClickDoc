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
        private ObservableCollection<ContractEntity> _contracts = [];
        private ContractEntity _selectedItem;

        public ICommand CreateNewCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
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
            _repository.ItemAdded += OnItemAdded;
            _repository.ItemRemoved += OnItemRemoved;

            LoadDataAsync();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CreateNewCommand = new RelayCommand(CreateNew);
            DeleteCommand = new RelayCommand(Delete);
            BackCommand = new RelayCommand(BackToMenu);
        }

        private async Task LoadDataAsync()
        {
            var contractors = await _repository.GetAll();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Contracts.Clear();
                foreach (var e in contractors)
                    Contracts.Add(e);
            });
        }

        private void BackToMenu()
        {
            _navigationService.NavigateTo<MainMenuWindow>();
        }

        private void Delete()
        {
            var contract = SelectedItem;
            _repository.Delete(SelectedItem.Id);
            MessageBox.Show($"{contract.ContractNumber} удален из БД");
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
                var fullEntity = await _repository.GetById(entity.Id);
                if (fullEntity != null)
                    Contracts.Add(entity);
            });

        }
    }
}
