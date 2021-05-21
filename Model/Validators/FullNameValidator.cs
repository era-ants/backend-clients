using FluentValidation;

namespace Clients.Model.Validators
{
    public sealed class FullNameValidator : AbstractValidator<FullName>
    {
        public FullNameValidator()
        {
            ValidateFirstName();
            ValidateLastName();
            ValidateParentName();
        }

        private void ValidateFirstName() => RuleFor(x => x.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("First name must be specified")
            .MaximumLength(500)
            .WithMessage("First name cannot contain over 500 characters")
        ;

        private void ValidateLastName() => RuleFor(x => x.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Last name must be specified")
            .MaximumLength(500)
            .WithMessage("Last name cannot contain over 500 characters")
        ;

        private void ValidateParentName() => RuleFor(x => x.ParentName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .When(name => name.HasParentName)
            .WithMessage("Parent name must be specified because HasParentName is set to true")
            .Empty()
            .When(name => !name.HasParentName)
            .WithMessage("Parent name must be empty because HasParentName is set to false")
        ;
    }
}