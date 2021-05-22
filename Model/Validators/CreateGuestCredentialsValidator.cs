using System;
using System.Threading;
using System.Threading.Tasks;
using Clients.Model.Operations;
using FluentValidation;
using Organizations.Model.Validators;

namespace Clients.Model.Validators
{
    public sealed class CreateGuestCredentialsValidator : AbstractValidator<CreateGuestCredentials>
    {
        private readonly Func<string, CancellationToken, Task<bool>> _isPhoneNumberAlreadyRegistered;

        public CreateGuestCredentialsValidator(Func<string, CancellationToken, Task<bool>> isPhoneNumberAlreadyRegistered)
        {
            _isPhoneNumberAlreadyRegistered = isPhoneNumberAlreadyRegistered;
            
            ValidatePhoneNumber();
            ValidatePassword();
        }

        private void ValidatePhoneNumber() => RuleFor(x => x.PhoneNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Guest phone number must be specified")
            .PhoneNumber()
            .WithMessage("Guest phone number is invalid")
            .MustAsync(async (number, cancellationToken) => !await _isPhoneNumberAlreadyRegistered.Invoke(number, cancellationToken))
            .WithMessage("Guest phone number is already registered")
        ;

        private void ValidatePassword() => RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Guest password must be specified")
            .Length(6, 30)
            .WithMessage("Guest password length must be between 6 and 30 characters")
            .Password()
            .WithMessage("Guest password is invalid")
        ;
    }
}