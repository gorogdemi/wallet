using FluentValidation;
using Wallet.Contracts.Requests;

namespace Wallet.Api.Validators
{
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
                .Equal(request => request.Email);

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
                .Equal(request => request.Password);

            RuleFor(request => request.UserName)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}