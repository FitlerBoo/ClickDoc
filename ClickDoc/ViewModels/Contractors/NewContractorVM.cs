using ClickDoc.Database.Entities;
using ClickDoc.Database.Repositories;
using ClickDoc.Utils;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using System.Windows.Input;

namespace ClickDoc.ViewModels.Contractors
{
    public class NewContractorVM : ValidatableObject
    {
        private string _surname = string.Empty;
        private string _name = string.Empty;
        private string _patronymic = string.Empty;
        private string _inn = string.Empty;
        private bool _isButtonEnabled;
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigation;
        private readonly IRepository<ContractorEntity> _repository;
        private readonly INotificationService _notificationService;

        public ICommand CreateCommand { get; }
        public ICommand CloseCommand { get; }

        public NewContractorVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigation = _serviceProvider.GetRequiredService<INavigationService>();
            _repository = _serviceProvider.GetRequiredService<IRepository<ContractorEntity>>();
            _notificationService = _serviceProvider.GetRequiredService<INotificationService>();
            CreateCommand = new AsyncRelayCommand(CreateNew);
            CloseCommand = new RelayCommand(Close);
            Validator = GetValidator();

            PropertyChanged += (s, e) => UpdateButtonState();
        }

        private IObjectValidator? GetValidator()
        {
            var builder = new ValidationBuilder<NewContractorVM>();

            builder.RuleFor(c => c.Name)
                .NotEmpty()
                    .WithMessage("Заполните поле")
                .MaxLength(100)
                    .WithMessage("Максимальное количество символов 100")
                .Matches(@"\p{IsCyrillic}")
                    .WithMessage("Используйте символы кириллицы");

            builder.RuleFor(c => c.Surname)
                .NotEmpty()
                    .WithMessage("Заполните поле")
                .MaxLength(100)
                    .WithMessage("Максимальное количество символов 100")
                .Matches(@"\p{IsCyrillic}")
                    .WithMessage("Используйте символы кириллицы");

            builder.RuleFor(c => c.Patronymic)
                .MaxLength(100)
                    .WithMessage("Максимальное количество символов 100")
                .Matches(@"\p{IsCyrillic}")
                    .WithMessage("Используйте символы кириллицы");

            builder.RuleFor(c => c.INN)
                .NotEmpty()
                    .WithMessage("Заполните поле")
                .Length(10, 12)
                    .WithMessage("ИНН состоит из 10-12 цифр")
                .Matches(@"^\d+$")
                    .WithMessage("Используйте только цифры");

            return builder.Build(this);
        }

        private void UpdateButtonState()
        {
            IsButtonEnabled = Validator?.IsValid ?? false;
        }

        private void Close()
        {
            _navigation.CloseCurrentWindow();
        }

        private async Task CreateNew()
        {
            if (Validator.IsValid)
            {
                IsButtonEnabled = false;
                ContractorEntity entity = new()
                {
                    Name = this.Name,
                    Surname = this.Surname,
                    Patronymic = this.Patronymic,
                    Inn = this.INN
                };
                try
                {
                    await _repository.Add(entity);
                    _notificationService.ShowSuccess($"{entity.FullName} добавлен в БД");
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
            else
            {
                _notificationService.ShowError("Завершите ввод данных");
            }
        }

        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set => this.SetAndRaiseIfChanged(ref _isButtonEnabled, value);
        }

        public string Surname
        {
            get => _surname;
            set => SetAndRaiseIfChanged(ref _surname, value);
        }

        public string Name
        {
            get => _name;
            set => SetAndRaiseIfChanged(ref _name, value);
        }

        public string Patronymic
        {
            get => _patronymic;
            set => SetAndRaiseIfChanged(ref _patronymic, value);
        }

        public string INN
        {
            get => _inn;
            set => SetAndRaiseIfChanged(ref _inn, value);
        }
    }
}
