using ClickDoc.Database.Entities;
using ClickDoc.Database.Repositories;
using ClickDoc.Utils;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;
using System.Windows;

namespace ClickDoc.ViewModels.Contractors
{
    public class NewContractorVM : ViewModelBase
    {
        private string _surname = string.Empty;
        private string _name = string.Empty;
        private string _patronymic = string.Empty;
        private string _inn = string.Empty;

        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigation;
        private readonly IRepository<ContractorEntity> _repository;

        public ICommand CreateCommand { get; }
        public ICommand CloseCommand { get; }

        public NewContractorVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigation = _serviceProvider.GetRequiredService<INavigationService>();
            _repository = _serviceProvider.GetRequiredService<IRepository<ContractorEntity>>();
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

            ContractorEntity entity = new()
            {
                Name = this.Name,
                Surname = this.Surname,
                Patronymic = this.Patronymic,
                Inn = this.INN
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

        public string INN
        {
            get => _inn;
            set
            {
                _inn = value;
                OnPropertyChanged(nameof(INN));
            }
        }
    }
}
