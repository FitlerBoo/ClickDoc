using ClickDoc.Database.Entities;
using ClickDoc.Database.Repositories;
using ClickDoc.Utils;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using ClickDoc.Views.Contractors;

namespace ClickDoc.ViewModels.Contractors
{
    public class ContractorsVM : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigationService;
        private readonly IRepository<ContractorEntity> _repository;
        private ObservableCollection<ContractorEntity> _contractors = [];
        private ContractorEntity _selectedItem;

        public ICommand CreateNewCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ObservableCollection<ContractorEntity> Contractors => _contractors;
        public bool IsItemSelected => SelectedItem != null;

        public ContractorEntity SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                OnPropertyChanged(nameof(IsItemSelected));
            }
        }

        public ContractorsVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            _repository = _serviceProvider.GetRequiredService<IRepository<ContractorEntity>>();
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
                Contractors.Clear();
                foreach (var e in contractors)
                    Contractors.Add(e);
            });
        }

        private void BackToMenu()
        {
            _navigationService.NavigateTo<MainMenuWindow>();
        }

        private void Delete()
        {
            var contractor = SelectedItem;
            _repository.Delete(SelectedItem.Id);
            MessageBox.Show($"{contractor.Surname} {contractor.Name} удален(а) из БД");
        }

        private void CreateNew()
        {
            _navigationService.NavigateTo<NewContractorWindow>();
        }

        private void OnItemRemoved(ContractorEntity entity)
        {
            Application.Current.Dispatcher.Invoke(() =>
            Contractors.Remove(Contractors.FirstOrDefault(x => x.Id == entity.Id)));
        }

        private void OnItemAdded(ContractorEntity entity)
        {
            Application.Current.Dispatcher.Invoke(() => Contractors.Add(entity));
        }
    }
}
