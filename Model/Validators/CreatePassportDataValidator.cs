using System;
using System.Threading.Tasks;
using Clients.Model.Operations;
using FluentValidation;

namespace Clients.Model.Validators
{
    public sealed class CreatePassportDataValidator : AbstractValidator<CreatePassportData>
    {
        private readonly Func<short, int, Task<bool>> _isPassportWithSeriesAndNumberRegistered;

        public CreatePassportDataValidator(
            Func<short, int, Task<bool>> isPassportWithSeriesAndNumberRegistered)
        {
            _isPassportWithSeriesAndNumberRegistered = isPassportWithSeriesAndNumberRegistered;
        }

        private void ValidateSeries() => RuleFor(x => x.Series)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
        ;

        private void ValidateNumber() => RuleFor(x => x.Number)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();

        private void ValidateSeriesAndNumberUniqueness() => RuleFor(x => x.Number)
            .MustAsync(async (createPassportData, _, _) => await
                _isPassportWithSeriesAndNumberRegistered.Invoke(createPassportData.Series, createPassportData.Number))
            .WithMessage("Passport with the same series and number is already registered")
        ;

        private void ValidateDateOfIssue() => RuleFor(x => x.DateOfIssue)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Passport's date of issue must be specified")
            .LessThan(DateTimeOffset.Now)
            .WithMessage("Passport's date of issue must be less then now")
        ;

        private void ValidateDepartmentName() => RuleFor(x => x.DepartmentName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Passport's department name must be specified")
        ;

        private void ValidateDepartmentCode() => RuleFor(x => x.DepartmentCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Passport's department code must be specified")
        ;
    }
}