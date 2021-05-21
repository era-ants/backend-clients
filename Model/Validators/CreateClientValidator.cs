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
           ValidateClientType();
           ValidateClientSubtype();
           ValidateFullName();
           ValidatePassportData();
        }

        private void ValidateCard() => RuleFor(x => x.Card)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Client's card must be specified")
        ;

        private void ValidateClientType() => RuleFor(x => x.ClientType)
            .NotEmpty()
            .WithMessage("Client's type must be specified")
        ;
        
        private void ValidateClientSubtype() => RuleFor(x => x.ClientSubtype)
            .NotEmpty()
            .WithMessage("Client's subtype must be specified")
        ;

        private void ValidateFullName() => RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Client's full name must be specified")
        ;

        private void ValidatePassportData() => RuleFor(x => x.PassportData)
            .NotEmpty()
            .WithMessage("Client's passport data must be specified")
        ;
    }
}