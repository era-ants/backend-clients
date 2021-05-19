using Clients.Model.Operations;
using FluentValidation;

namespace Clients.Model.Validators
{
    public sealed class CreateCardValidator : AbstractValidator<CreateCard>
    {
        public CreateCardValidator()
        {
            RuleFor(x => x.Guid)
                .NotEmpty()
                .WithMessage("Card's unique identifier must be specified");
            RuleFor(x => x.ValidFrom)
                .NotEmpty()
                .LessThan(x => x.ValidUntil);
            RuleFor(x => x.ValidUntil)
                .NotEmpty()
                .GreaterThan(x => x.ValidFrom);
        }
    }
}