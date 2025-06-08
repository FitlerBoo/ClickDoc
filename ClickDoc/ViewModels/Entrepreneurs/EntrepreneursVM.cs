using ClickDoc.Database.Entities;
using ClickDoc.Database.Repositories;
using ClickDoc.Utils;
using ClickDoc.Views;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ClickDoc.ViewModels.Entrepreneurs
{
    public class EntrepreneursVM : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigationService;
        private readonly INotificationService _notificationService;
        private readonly IRepository<EntrepreneurEntity> _repository;

        private ObservableCollection<EntrepreneurEntity> _entrepreneurs = [];
        private EntrepreneurEntity _selectedItem;

        public ICommand CreateNewCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ObservableCollection<EntrepreneurEntity> Entrepreneurs => _entrepreneurs;
        public bool IsItemSelected => SelectedItem != null;

        public EntrepreneurEntity SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                OnPropertyChanged(nameof(IsItemSelected));
            }
        }

        public EntrepreneursVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            _notificationService = _serviceProvider.GetRequiredService<INotificationService>();
            _repository = _serviceProvider.GetRequiredService<IRepository<EntrepreneurEntity>>();
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
                var entrepreneurs = await _repository.GetAll();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Entrepreneurs.Clear();
                    foreach (var e in entrepreneurs)
                        Entrepreneurs.Add(e);
                });
            }
            catch (Exception ex)
            {
                _notificationService.ShowError(ex.Message);
            }

        }

        private async Task Delete()
        {
            try
            {
                var entrepreneur = SelectedItem;
                await _repository.Delete(SelectedItem.Id);
                _notificationService.ShowSuccess($"{entrepreneur.Surname} {entrepreneur.Name} удален(а) из БД");
            }
            catch (Exception ex)
            {
                _notificationService.ShowError(ex.Message);
            }
        }

        private void CreateNew()
        {
            _navigationService.NavigateTo<NewEntrepreneurWindow>();
        }

        private void OnItemRemoved(EntrepreneurEntity entity)
        {
            Application.Current.Dispatcher.Invoke(() =>
            Entrepreneurs.Remove(Entrepreneurs.FirstOrDefault(x => x.Id == entity.Id)));
        }

        private void OnItemAdded(EntrepreneurEntity entity)
        {
            Application.Current.Dispatcher.Invoke(() => Entrepreneurs.Add(entity));
        }
    }
}
