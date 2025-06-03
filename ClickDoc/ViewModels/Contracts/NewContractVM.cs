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

        private ObservableCollection<ContractorEntity> _contractors = [];
        private ObservableCollection<EntrepreneurEntity> _entrepreneurs = [];
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

        public NewContractVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigation = _serviceProvider.GetRequiredService<INavigationService>();
            _repository = _serviceProvider.GetRequiredService<IRepository<ContractEntity>>();
            CreateCommand = new RelayCommand(CreateNew);
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
            var entities = await repository.GetAll();
            Application.Current.Dispatcher.Invoke(() =>
            {
                collection.Clear();
                foreach (var e in entities)
                    collection.Add(e);
            });
            return collection;
        }

        private void Close()
        {
            _navigation.CloseCurrentWindow();
        }

        private void CreateNew()
        {
            _navigation.CloseCurrentWindow();

            if (_contractNumber[0] == '№')
                _contractNumber = _contractNumber.Remove(0,1);

            ContractEntity entity = new()
            {
                ContractNumber = _contractNumber,
                EntrepreneurId = _entrepreneur.Id,
                ContractorId = _contractor.Id
            };

            _repository.Add(entity);
            MessageBox.Show($"{entity.ContractNumber} добавлен в БД");
        }
    }
}
