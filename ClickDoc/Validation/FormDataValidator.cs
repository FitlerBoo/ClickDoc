using ClickDoc.Models;
using FluentValidation;

namespace ClickDoc.Validation
{
    class FormDataValidator : AbstractValidator<FormData>
    {
        public FormDataValidator()
        {
            //RuleFor(x => x.ActDate)
            //    .NotEmpty().WithMessage("Дата акта обязательна")
            //    .LessThanOrEqualTo(DateTime.Today).WithMessage("Дата не может быть в будущем");

            //RuleFor(x => x.UnitCost)
            //    .GreaterThan(0).WithMessage("Стоимость должна быть положительной");
        }
    }
}
