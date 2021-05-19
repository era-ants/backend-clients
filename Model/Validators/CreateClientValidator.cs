using Clients.Model.Operations;
using FluentValidation;

namespace Clients.Model.Validators
{
    public sealed class CreateClientValidator : AbstractValidator<CreateClient>
    {
        public CreateClientValidator()
        {
            RuleFor(x => x.Card)
                .NotEmpty()
                .WithMessage("Client's card must be specified");
            RuleFor(x => x.ClientType)
                .NotEmpty()
                .WithMessage("Client's type must be specified");
            RuleFor(x => x.ClientSubtype)
                .NotEmpty()
                .WithMessage("Client's subtype must be specified");
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Client's full name must be specified");
            RuleFor(x => x.PassportData)
                .NotEmpty()
                .WithMessage("Client's passport data must be specified");
            RuleFor(x => x.RegisteredClients)
                .NotNull()
                .WithMessage("Registered clients must be provided");
        }
    }
}