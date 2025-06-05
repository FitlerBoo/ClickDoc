using ClickDoc.Database.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickDoc.Validation
{
    internal class EntrepreneurValidator : AbstractValidator<EntrepreneurEntity>
    {
        public EntrepreneurValidator()
        {
            RuleFor(p => p.Surname)
                .NotEmpty()
                    .WithMessage("Введите фамилию")
                .Must(BeAStringWithoutNumbers)
                    .WithMessage("Фамилия не должна содержать цифр")
                .MaximumLength(100)
                    .WithMessage("Превышена максимальная длинна");

            RuleFor(p => p.Name)
                .NotEmpty()
                    .WithMessage("Введите имя")
                .Must(BeAStringWithoutNumbers)
                    .WithMessage("Имя не должно содержать цифр")
                .MaximumLength(100)
                    .WithMessage("Превышена максимальная длинна");

            RuleFor(p => p.Name)
                .Must(BeAStringWithoutNumbers)
                    .WithMessage("Отчество не должно содержать цифр")
                .MaximumLength(100)
                    .WithMessage("Превышена максимальная длинна");

            RuleFor(p => p.OGRNIP)
                .NotNull()
                    .WithMessage("Введите ОГРНИП")
                .Matches(@"^[0-9]+$")
                    .WithMessage("Поле должно содержать только цифры")
                .Length(15)
                    .WithMessage("Неверное количество цифр в ОГРНИП");
        }

        private bool BeAStringWithoutNumbers(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return true;

            return !value.Any(char.IsDigit);
        }
    }
}
