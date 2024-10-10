using System.Globalization;
using Wallet.Shared.Common.Enums;

namespace Wallet.WebUI.Extensions;

public static class ConverterExtensions
{
    public static string ToCategoryText(this string category) => category ?? "None";

    public static string ToFormattedDate(this DateTime dateTime) => dateTime.ToString("yyyy.MM.dd", CultureInfo.InvariantCulture);

    public static string ToFormattedDate(this DateOnly dateOnly) => dateOnly.ToString("yyyy.MM.dd", CultureInfo.InvariantCulture);

    public static string ToTransactionTypeText(this TransactionType transactionType) =>
        transactionType switch
        {
            TransactionType.Expense => "Expense",
            TransactionType.Income => "Income",
            _ => null,
        };

    public static string ToTransactionTypeText(this TransactionType? transactionType) => ToTransactionTypeText(transactionType ?? default);
}