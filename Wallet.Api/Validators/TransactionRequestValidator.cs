using FluentValidation;
using Wallet.Contracts.Requests;

namespace Wallet.Api.Validators
{
    public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
    {
        public TransactionRequestValidator()
        {
            RuleFor(request => request.Comment)
                .MaximumLength(255);

            RuleFor(request => request.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}