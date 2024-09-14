using FluentValidation;

namespace Wallet.Application.Authentication;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(request => request.RefreshToken)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(request => request.Token)
            .NotEmpty()
            .MaximumLength(500);
    }
}