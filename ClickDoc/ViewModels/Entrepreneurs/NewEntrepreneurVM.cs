using ClickDoc.Database.Entities;
using ClickDoc.Database.Repositories;
using ClickDoc.Utils;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using System.Windows.Input;

namespace ClickDoc.ViewModels.Entrepreneurs
{
    public class NewEntrepreneurVM : ValidatableObject
    {
        private string _surname = string.Empty;
        private string _name = string.Empty;
        private string _patronymic = string.Empty;
        private string _ogrnip = string.Empty;
        private bool _isButtonEnabled;

        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigation;
        private readonly IRepository<EntrepreneurEntity> _repository;
        private readonly INotificationService _notificationService;

        public ICommand CreateCommand { get; }
        public ICommand CloseCommand { get; }

        public NewEntrepreneurVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigation = _serviceProvider.GetRequiredService<INavigationService>();
            _repository = _serviceProvider.GetRequiredService<IRepository<EntrepreneurEntity>>();
            _notificationService = _serviceProvider.GetRequiredService<INotificationService>();
            Validator = GetValidator();
            PropertyChanged += (s, e) => UpdateButtonState();

            CreateCommand = new AsyncRelayCommand(CreateNew);
            CloseCommand = new RelayCommand(Close);
        }

        private void UpdateButtonState()
        {
            IsButtonEnabled = Validator?.IsValid ?? false;
        }

        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<NewEntrepreneurVM>();

            builder.RuleFor(e => e.Name)
                .NotEmpty()
                    .WithMessage("Заполните поле")
                .MaxLength(100)
                    .WithMessage("Максимальное количество символов 100")
                .Matches(@"\p{IsCyrillic}")
                    .WithMessage("Используйте символы кириллицы");

            builder.RuleFor(e => e.Surname)
                .NotEmpty()
                    .WithMessage("Заполните поле")
                .MaxLength(100)
                    .WithMessage("Максимальное количество символов 100")
                .Matches(@"\p{IsCyrillic}")
                    .WithMessage("Используйте символы кириллицы");

            builder.RuleFor(e => e.Patronymic)
                .MaxLength(100)
                    .WithMessage("Максимальное количество символов 100")
                .Matches(@"\p{IsCyrillic}")
                    .WithMessage("Используйте символы кириллицы");

            builder.RuleFor(e => e.OGRNIP)
                .NotEmpty()
                    .WithMessage("Заполните поле")
                .Length(15)
                    .WithMessage("ОГРНИП состоит из 15 символов")
                .Matches(@"^\d+$")
                    .WithMessage("Используйте только цифры");

            return builder.Build(this);
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
                EntrepreneurEntity entity = new()
                {
                    Name = _name,
                    Surname = _surname,
                    Patronymic = _patronymic,
                    OGRNIP = _ogrnip,
                };
                try
                {
                    await _repository.Add(entity);
                    _notificationService.ShowSuccess($"{entity.Surname} {entity.Name} добавлен(а) в БД");
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
                _notificationService.ShowError("Завершите ввод данных");
        }

        public string Surname
        {
            get => _surname;
            set => this.SetAndRaiseIfChanged(ref _surname, value);
        }

        public string Name
        {
            get => _name;
            set => this.SetAndRaiseIfChanged(ref _name, value);
        }

        public string Patronymic
        {
            get => _patronymic;
            set => this.SetAndRaiseIfChanged(ref _patronymic, value);
        }

        public string OGRNIP
        {
            get => _ogrnip;
            set => SetAndRaiseIfChanged(ref _ogrnip, value);
        }

        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set => this.SetAndRaiseIfChanged(ref _isButtonEnabled, value);
        }
    }
}
