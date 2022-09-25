using FluentValidation;
using Wallet.Contracts.Requests;

namespace Wallet.Validation
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

            RuleFor(request => request.BankAmount)
                .GreaterThan(0)
                .When(request => request.CashAmount == 0)
                .WithMessage("A tranzakció nem lehet nulla összegű.")
                .GreaterThanOrEqualTo(0);

            RuleFor(request => request.CashAmount)
                .GreaterThan(0)
                .When(request => request.BankAmount == 0)
                .WithMessage("A tranzakció nem lehet nulla összegű.")
                .GreaterThanOrEqualTo(0);
        }
    }
}