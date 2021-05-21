using System;
using System.Threading.Tasks;
using Clients.Model.Operations;
using FluentValidation;

namespace Clients.Model.Validators
{
    public sealed class CreateCardValidator : AbstractValidator<CreateCard>
    {
        private readonly Func<Guid, Task<bool>> _cardWithGuidExists;

        public CreateCardValidator(
            Func<Guid, Task<bool>> cardWithGuidExists)
        {
            _cardWithGuidExists = cardWithGuidExists;
            ValidateGuid();
            ValidateDates();
        }

        private void ValidateGuid()
        {
            RuleFor(x => x.Guid)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Card's unique identifier must be specified")
                .MustAsync(async (guid, _) => !await _cardWithGuidExists.Invoke(guid))
                .WithMessage(x => $"Card with the identifier {x.Guid} is already registered")
                ;
        }

        private void ValidateDates()
        {
            RuleFor(x => x.ValidFrom)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage($"Card's {nameof(Card.ValidFrom)} property must be specified")
                .LessThan(x => x.ValidUntil)
                .WithMessage(
                    $"Card's {nameof(Card.ValidFrom)} property must be greater than {nameof(Card.ValidUntil)} property")
                .LessThan(DateTimeOffset.UtcNow)
                .WithMessage($"Card's {nameof(Card.ValidFrom)} property must be less than now")
                ;
            RuleFor(x => x.ValidUntil)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage($"Card's {nameof(Card.ValidUntil)} property must be specified")
                .GreaterThan(DateTimeOffset.UtcNow)
                .WithMessage($"Card's {nameof(Card.ValidUntil)} property must be greater than now")
                ;
        }
    }
}