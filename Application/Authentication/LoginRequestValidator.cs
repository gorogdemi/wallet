using FluentValidation;

namespace Wallet.Application.Authentication;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.Password)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(request => request.UserName)
            .NotEmpty()
            .MaximumLength(100);
    }
}