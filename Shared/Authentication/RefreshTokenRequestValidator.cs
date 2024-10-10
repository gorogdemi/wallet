using FluentValidation;

namespace Wallet.Shared.Authentication;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(request => request.RefreshToken)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(request => request.AccessToken)
            .NotEmpty()
            .MaximumLength(500);
    }
}