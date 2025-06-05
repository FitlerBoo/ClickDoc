using ClickDoc.Database.Entities;
using ClickDoc.Database.Repositories;
using ClickDoc.Generators;
using ClickDoc.Models;
using ClickDoc.Utils;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ReactiveValidation;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClickDoc.ViewModels
{
    public class AcceptanceTransferActVM : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigationService;
        private readonly IRepository<ContractEntity> _repository;
        private readonly INotificationService _notificationService;
        private IGeneratorFactory _generatorFactory;
        private ObservableCollection<ContractEntity> _contracts = [];
        private bool _isButtonEnabled = true;
        private DocumentGeneratorType _selectedGeneratorType = DocumentGeneratorType.Word;
        public ObservableCollection<ContractEntity> Contracts
        {
            get => _contracts;
            set
            {
                _contracts = value;
                OnPropertyChanged(nameof(Contracts));
            }
        }
        private ContractEntity _selectedItem;

        private FormData _formData;
        private string _filename = "Акт приема-передачи оказанных услуг";
        public ICommand CreateCommand { get; }
        public AcceptanceTransferActVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigationService = serviceProvider.GetRequiredService<INavigationService>();
            _repository = serviceProvider.GetRequiredService<IRepository<ContractEntity>>();
            _notificationService = serviceProvider.GetRequiredService<INotificationService>();
            _generatorFactory = serviceProvider.GetRequiredService<IGeneratorFactory>();
            _repository.ItemAdded += OnItemAdded;
            _repository.ItemRemoved += OnItemRemoved;
            _formData = _serviceProvider.GetRequiredService<FormData>();
            CreateCommand = new AsyncRelayCommand(CreateDocument);

            LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var entities = await _repository.GetAll();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Contracts.Clear();
                    foreach (var entity in entities)
                        Contracts.Add(entity);
                });
            }
            catch (Exception ex)
            {
                _notificationService.ShowError(
                    $"Ошибка загрузки элементов из базы данных\n{ex.Message}");
            }
        }

        #region Properties
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set
            {
                _isButtonEnabled = value;
                OnPropertyChanged(nameof(IsButtonEnabled));
            }
        }
        public ContractEntity SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                UpdateFormData();
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private void UpdateFormData()
        {
            if (_selectedItem != null)
            {
                _formData.EntrepreneurFullName = _selectedItem.Entrepreneur?.FullName ?? string.Empty;
                _formData.OGRNIP = _selectedItem.Entrepreneur?.OGRNIP ?? string.Empty;
                _formData.ContractorFullName = _selectedItem.Contractor?.FullName ?? string.Empty;
                _formData.ContractorINN = _selectedItem.Contractor?.Inn ?? string.Empty;
                _formData.ContractNumber = _selectedItem.ContractNumber;
            }
        }

        #region Generator
        public DocumentGeneratorType SelectedGeneratorType
        {
            get => _selectedGeneratorType;
            set
            {
                _selectedGeneratorType = value;
                OnPropertyChanged(nameof(SelectedGeneratorType));
                OnPropertyChanged(nameof(Generator));
            }
        }

        public Dictionary<DocumentGeneratorType, string> AvailableGeneratorTypes
            => GeneratorFactory.GeneratorTypeDisplayNames;

        public IDocumentGenerator Generator
            => _generatorFactory.GetGenerator(SelectedGeneratorType);

        #endregion

        public decimal UnitCost
        {
            get => _formData.UnitCost;
            set
            {
                _formData.UnitCost = value;
                OnPropertyChanged(nameof(UnitCost));
            }
        }

        public decimal UnitCount
        {
            get => _formData.UnitCount;
            set
            {
                _formData.UnitCount = value;
                OnPropertyChanged(nameof(UnitCount));
            }
        }

        public string ActNumber
        {
            get => _formData.ActNumber;
            set
            {
                _formData.ActNumber = value;
                OnPropertyChanged(nameof(ActNumber));
            }
        }

        public string ServiceTypeDescription
        {
            get => _formData.ServiceTypeDescription;
            set
            {
                _formData.ServiceTypeDescription = value;
                OnPropertyChanged(nameof(ServiceTypeDescription));
            }
        }

        public string InvoiceNumber
        {
            get => _formData.InvoiceNumber;
            set
            {
                _formData.InvoiceNumber = value;
                OnPropertyChanged(nameof(InvoiceNumber));
            }
        }

        public DateTime ActDate
        {
            get => _formData.ActDate;
            set
            {
                _formData.ActDate = value;
                OnPropertyChanged(nameof(ActDate));
            }
        }

        public DateTime ContractDate
        {
            get => _formData.ContractDate;
            set
            {
                _formData.ContractDate = value;
                OnPropertyChanged(nameof(ContractDate));
            }
        }

        public DateTime PeriodStart
        {
            get => _formData.PeriodStart;
            set
            {
                _formData.PeriodStart = value;
                OnPropertyChanged(nameof(PeriodStart));
            }
        }

        public DateTime PeriodEnd
        {
            get => _formData.PeriodEnd;
            set
            {
                _formData.PeriodEnd = value;
                OnPropertyChanged(nameof(PeriodEnd));
            }
        }

        public DateTime InvoiceDate
        {
            get => _formData.InvoiceDate;
            set
            {
                _formData.InvoiceDate = value;
                OnPropertyChanged(nameof(InvoiceDate));
            }
        }

        public DateTime LastDate
        {
            get => _formData.LastDate;
            set
            {
                _formData.LastDate = value;
                OnPropertyChanged(nameof(LastDate));
            }
        }

        public string FileName
        {
            get => _filename;
            set
            {
                _filename = value;
                OnPropertyChanged(nameof(FileName));
            }
        }
        #endregion

        private async Task CreateDocument()
        {
            var contractData = new AcceptanceTransferActContractData(_formData);
            try
            {
                IsButtonEnabled = false;
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var generator = Generator;
                var templatePath = Path.Combine(appDirectory, "Templates", "AcceptanceTransferAct.docx");
                await generator.GenerateAsync(contractData,
                templatePath, _filename);
                _notificationService.ShowSuccess($"Файл {FileName} успешно создан на рабочем столе");
                IsButtonEnabled = true;
                _navigationService.CloseCurrentWindow();
            }
            catch (Exception ex)
            {
                IsButtonEnabled = true;
                _notificationService.ShowError($"Ошибка пути к шаблону:\n{ex.Message}");
            }
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
                    _notificationService.ShowError($"Ошибка загрузки элемента из базы данных\n{ex.Message}");
                }
            });

        }
    }
}
