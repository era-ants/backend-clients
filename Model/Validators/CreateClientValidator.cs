using Clients.Model.Operations;
using FluentValidation;

namespace Clients.Model.Validators
{
    public sealed class CreateClientValidator : AbstractValidator<CreateClient>
    {
        public CreateClientValidator()
        {
            ValidateFullName();
            ValidateCard();
            ValidateClientSubtype();
            ValidateFullName();
            ValidateClientTypeAndCredentials();
        }

        private void ValidateCard() => RuleFor(x => x.Card)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Client's card must be specified");

        private void ValidateClientSubtype() => RuleFor(x => x.ClientSubtype)
            .NotEmpty()
            .WithMessage("Client's subtype must be specified");

        private void ValidateFullName() => RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Client's full name must be specified");

        private void ValidateClientTypeAndCredentials() => RuleFor(x => x.ClientType)
            .NotEmpty()
            .WithMessage("Client's type must be specified")
            .DependentRules(() =>
            {
                RuleFor(x => x.GuestCredentials)
                    .NotEmpty()
                    .When(x => x.ClientType == ClientType.Guest)
                    .WithMessage("GuestCredentials must be specified for guest")
                    ;
                RuleFor(x => x.CitizenCredentials)
                    .NotEmpty()
                    .When(x => x.ClientType == ClientType.Citizen)
                    .WithMessage("CitizenCredentials must be specified for citizen")
                    ;
            });
    }
}