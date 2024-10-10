using FluentValidation;

namespace Wallet.Shared.Authentication;

public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
{
    public RegistrationRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .MaximumLength(100)
            .EmailAddress();

        RuleFor(request => request.EmailConfirm)
            .NotEmpty()
            .MaximumLength(100)
            .EmailAddress()
            .Equal(request => request.Email)
            .WithMessage("Confirm Email address field does not match with Email address field.");

        RuleFor(request => request.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(request => request.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(request => request.Password)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(request => request.PasswordConfirm)
            .NotEmpty()
            .MaximumLength(100)
            .Equal(request => request.Password)
            .WithMessage("Confirm Password field does not match with Password field.");

        RuleFor(request => request.UserName)
            .NotEmpty()
            .MaximumLength(100);
    }
}