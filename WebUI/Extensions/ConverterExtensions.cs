using Wallet.Shared.Common.Enums;

namespace Wallet.WebUI.Extensions;

internal static class ConverterExtensions
{
    public static string ToCategoryText(this string category) => category ?? "None";

    public static string ToTransactionTypeText(this TransactionType transactionType) =>
        transactionType switch
        {
            TransactionType.Expense => "Expense",
            TransactionType.Income => "Income",
            _ => null,
        };

    public static string ToTransactionTypeText(this TransactionType? transactionType) => (transactionType ?? default).ToTransactionTypeText();
}