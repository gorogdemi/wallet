using System;

namespace Wallet.Contracts.Responses
{
    public class TransactionResponse
    {
        public double BankAmount { get; set; }

        public double CashAmount { get; set; }

        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public double SumAmount { get; set; }

        public int Type { get; set; }
    }
}