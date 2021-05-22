using System;
using System.Threading;
using System.Threading.Tasks;
using Clients.Model.Operations;
using FluentValidation;
using Organizations.Model.Validators;

namespace Clients.Model.Validators
{
    public sealed class CreateCitizenCredentialsValidator : AbstractValidator<CreateCitizenCredentials>
    {
        private readonly Func<string, CancellationToken, Task<bool>> _isEmailAlreadyRegistered;

        public CreateCitizenCredentialsValidator(Func<string, CancellationToken, Task<bool>> isEmailAlreadyRegistered)
        {
            _isEmailAlreadyRegistered = isEmailAlreadyRegistered;

            ValidateEmail();
            ValidatePassword();
        }

        private void ValidateEmail() => RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Citizen email must be specified")
            .EmailAddress()
            .WithMessage("Citizen email is invalid")
            .MustAsync(async (email, cancellationToken) => !await _isEmailAlreadyRegistered.Invoke(email, cancellationToken))
            .WithMessage("Citizen with specified email is already registered")
        ;

        private void ValidatePassword() => RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Citizen password must be specified")
            .Length(6, 30)
            .WithMessage("Citizen password length must be between 6 and 30 characters")
            .Password()
            .WithMessage("Citizen password contains invalid symbols")
        ;
    }
}