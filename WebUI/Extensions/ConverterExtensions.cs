using System;
using System.Globalization;
using DevQuarter.Wallet.Domain.Enums;

namespace DevQuarter.Wallet.WebUI.Extensions
{
    public static class ConverterExtensions
    {
        public static string ToFormattedDate(this DateTime dateTime) => dateTime.ToString("yyyy.MM.dd", CultureInfo.InvariantCulture);

        public static string ToFormattedDate(this DateOnly dateOnly) => dateOnly.ToString("yyyy.MM.dd", CultureInfo.InvariantCulture);

        public static string ToTransactionTypeText(this TransactionType transactionType)
        {
            return transactionType switch
            {
                TransactionType.Expense => "Kiadás",
                TransactionType.Income => "Bevétel",
                _ => null,
            };
        }
    }
}