using FluentValidation;

namespace Wallet.Shared.Transactions;

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
            .When(request => request.CashAmount <= 0)
            .WithMessage("The sum of the transaction must greater than zero.")
            .GreaterThanOrEqualTo(0);

        RuleFor(request => request.CashAmount)
            .GreaterThan(0)
            .When(request => request.BankAmount <= 0)
            .WithMessage("The sum of the transaction must greater than zero.")
            .GreaterThanOrEqualTo(0);

        RuleFor(request => request.Type)
            .NotNull();

        RuleFor(request => request.Date)
            .NotNull();
    }
}