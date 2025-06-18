using ClickDoc.Database.Entities;
using ClickDoc.Database.Repositories;
using ClickDoc.Generators;
using ClickDoc.Models;
using ClickDoc.Utils;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ClickDoc.ViewModels
{
    public class AcceptanceTransferActVM : ValidatableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IRepository<ContractEntity> _repository;
        private readonly INotificationService _notificationService;
        private IGeneratorFactory _generatorFactory;

        private ObservableCollection<ContractEntity> _contracts = [];
        public ObservableCollection<ContractEntity> Contracts
        {
            get => _contracts;
            set => SetAndRaiseIfChanged(ref _contracts, value);
        }

        public ICommand CreateCommand { get; }

        public AcceptanceTransferActVM(IServiceProvider serviceProvider)
        {
            _navigationService = serviceProvider.GetRequiredService<INavigationService>();
            _repository = serviceProvider.GetRequiredService<IRepository<ContractEntity>>();
            _notificationService = serviceProvider.GetRequiredService<INotificationService>();
            _generatorFactory = serviceProvider.GetRequiredService<IGeneratorFactory>();

            _repository.ItemAdded += OnItemAdded;
            _repository.ItemRemoved += OnItemRemoved;

            Validator = GetValidator();
            PropertyChanged += (s, e) => UpdateButtonState();

            LoadDataAsync();

            CreateCommand = new AsyncRelayCommand(Create);
        }

        private void UpdateButtonState()
        {
            IsButtonEnabled = Validator?.IsValid ?? false;
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
        private async Task Create()
        {
            if (!Validator.IsValid)
            {
                _notificationService.ShowError("Заполните форму");
                return;
            }

            FormData formData;
            try
            {
                formData = GetFormData();
            }
            catch
            {
                _notificationService.ShowError("Заполните форму правильно");
                return;
            }

            try
            {
                IsButtonEnabled = false;
                var contractData = new AcceptanceTransferActContractData(formData);
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var templatePath = Path.Combine(appDirectory, "Templates", "AcceptanceTransferAct.docx");
                var generator = Generator;

                await generator.GenerateAsync(contractData, templatePath, FileName);
                IsButtonEnabled = true;
                _navigationService.CloseCurrentWindow();
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Ошибка: {ex.Message}");
            }
            finally
            {
                IsButtonEnabled = true;
            }
        }

        private FormData GetFormData()
            => new()
            {
                ActNumber = ActNumber,
                ActDate = ActDate,
                EntrepreneurFullName = SelectedItem.Entrepreneur.FullName,
                OGRNIP = SelectedItem.Entrepreneur.OGRNIP,
                ContractorFullName = SelectedItem.Contractor.FullName,
                ContractorINN = SelectedItem.Contractor.Inn,
                ContractNumber = SelectedItem.ContractNumber,
                ContractDate = DateTime.Parse(SelectedItem.ContractDate),
                PeriodStart = PeriodStart,
                PeriodEnd = PeriodEnd,
                ServiceTypeDescription = ServiceTypeDescription,
                UnitCost = decimal.Parse(UnitCost),
                UnitCount = int.Parse(UnitCount),
                InvoiceNumber = InvoiceNumber,
                InvoiceDate = InvoiceDate,
                SingingDate = SingingDate
            };

        #region Validation
        private IObjectValidator? GetValidator()
        {
            var builder = new ValidationBuilder<AcceptanceTransferActVM>();

            builder.RuleFor(a => a.SelectedItem)
                .NotNull()
                    .WithMessage("Выберите или создайте договор");

            builder.RuleFor(a => a.ActNumber)
                .NotEmpty()
                    .WithMessage("Заполните поле")
                .MaxLength(100)
                    .WithMessage("Максимальное количество символов 100");

            builder.RuleFor(a => a.ActDate)
                .NotNull()
                    .WithMessage("Выберите дату");

            builder.RuleFor(a => a.PeriodStart)
                .NotNull()
                    .WithMessage("Выберите дату");

            builder.RuleFor(a => a.PeriodEnd)
                .NotNull()
                    .WithMessage("Выберите дату");

            builder.RuleFor(a => a.ServiceTypeDescription)
                .NotEmpty()
                    .WithMessage("Заполните поле");

            builder.RuleFor(a => a.UnitCost)
                .NotNull()
                    .WithMessage("Заполните поле")
                .Matches(@"^[0-9]+([.,][0-9]+)?$")
                    .WithMessage("Допустимы только цифры, точка или запятая")
                .Must(BeGreaterThanZero)
                    .WithMessage("Число должно быть больше 0");

            builder.RuleFor(a => a.UnitCount)
                .NotEmpty()
                    .WithMessage("Заполните поле")
                .Matches(@"^\d+$")
                    .WithMessage("Используйте только цифры")
                .Must(BeGreaterThanZero)
                    .WithMessage("Число должно быть больше 0");

            builder.RuleFor(a => a.InvoiceNumber)
                .NotEmpty()
                    .WithMessage("Заполните поле")
                .MaxLength(100)
                    .WithMessage("Максимальное количество символов 100")
                .Matches(@"^\d+$")
                    .WithMessage("Используйте только цифры");

            builder.RuleFor(a => a.InvoiceDate)
                .NotNull()
                    .WithMessage("Выберите дату");

            builder.RuleFor(a => a.SingingDate)
                .NotNull()
                    .WithMessage("Выберите дату");

            builder.RuleFor(a => a.FileName)
                .NotEmpty()
                    .WithMessage("Заполните поле")
                .MaxLength(100)
                    .WithMessage("Максимальное количество символов 100");

            return builder.Build(this);
        }

        private bool BeGreaterThanZero(string value)
        {
            var normalizedValue = value.Replace(',', '.');

            return decimal.TryParse(normalizedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result)
                   && result >= 0;
        }

        #endregion

        #region Generator

        private DocumentGeneratorType _selectedGeneratorType = DocumentGeneratorType.Word;
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

        #region Properties

        private bool _isButtonEnabled;
        private string _actNumber = string.Empty;
        private ContractEntity _selectedItem;
        private DateTime _actDate = DateTime.Now;
        private DateTime _periodStart = DateTime.Now;
        private DateTime _periodEnd = DateTime.Now;
        private string _serviceTypeDescription = string.Empty;
        private string _unitCost = string.Empty;
        private string _unitCount = "1";
        private string _invoiceNumber = string.Empty;
        private DateTime _invoiceDate = DateTime.Now;
        private DateTime _singingDate = DateTime.Now;
        private string _baseFilename = "Акт приема-передачи оказанных услуг";
        private string _customFilename;
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set => SetAndRaiseIfChanged(ref _isButtonEnabled, value);
        }

        public string ActNumber
        {
            get => _actNumber;
            set
            {
                SetAndRaiseIfChanged(ref _actNumber, value);
                _customFilename = null;
                OnPropertyChanged(nameof(FileName));
            }
        }

        public ContractEntity SelectedItem
        {
            get => _selectedItem;
            set => SetAndRaiseIfChanged(ref _selectedItem, value);
        }

        public DateTime ActDate
        {
            get => _actDate;
            set
            {
                SetAndRaiseIfChanged(ref _actDate, value);
                _customFilename = null;
                OnPropertyChanged(nameof(FileName));
            }
        }

        public DateTime PeriodStart
        {
            get => _periodStart;
            set => SetAndRaiseIfChanged(ref _periodStart, value);
        }

        public DateTime PeriodEnd
        {
            get => _periodEnd;
            set => SetAndRaiseIfChanged(ref _periodEnd, value);
        }

        public string ServiceTypeDescription
        {
            get => _serviceTypeDescription;
            set => SetAndRaiseIfChanged(ref _serviceTypeDescription, value);
        }

        public string UnitCost
        {
            get => _unitCost;
            set => SetAndRaiseIfChanged(ref _unitCost, value);
        }

        public string UnitCount
        {
            get => _unitCount;
            set => SetAndRaiseIfChanged(ref _unitCount, value);
        }

        public string InvoiceNumber
        {
            get => _invoiceNumber;
            set => SetAndRaiseIfChanged(ref _invoiceNumber, value);
        }

        public DateTime InvoiceDate
        {
            get => _invoiceDate;
            set => SetAndRaiseIfChanged(ref _invoiceDate, value);
        }

        public DateTime SingingDate
        {
            get => _singingDate;
            set => SetAndRaiseIfChanged(ref _singingDate, value);
        }

        public string FileName
        {
            get
            {
                if (!string.IsNullOrEmpty(_customFilename))
                {
                    // Проверяем, не совпадает ли текущее значение с автоматическим
                    var autoName = GenerateFilename();
                    return _customFilename == autoName ? autoName : _customFilename;
                }

                return GenerateFilename();
            }
            set
            {
                // Сохраняем пользовательское значение, только если оно отличается от автоматического
                var autoName = GenerateFilename();
                _customFilename = value == autoName ? null : value;
                OnPropertyChanged(nameof(FileName));
            }
        }

        private string GenerateFilename()
        {
            var filename = _baseFilename;

            if (!string.IsNullOrEmpty(ActNumber))
                filename += $" №{ActNumber}";

            if (ActDate != default)
                filename += $" от {ActDate:dd.MM.yyyy}";

            return filename;
        }


        #endregion

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
