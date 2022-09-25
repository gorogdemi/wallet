using System;
using Wallet.Contracts.Types;

namespace Wallet.UI.Extensions
{
    public static class ConverterExtensions
    {
        public static string ToFormattedDate(this DateTime dateTime) => dateTime.ToString("yyyy.MM.dd");

        public static string ToFormattedDate(this DateOnly dateOnly) => dateOnly.ToString("yyyy.MM.dd");

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