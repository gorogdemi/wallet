using System;
using Wallet.Api.Domain.Types;

namespace Wallet.Api.Domain
{
    public class Transaction : DomainBase
    {
        public double BankAmount { get; set; }

        public double CashAmount { get; set; }

        public virtual Category Category { get; set; }

        public long? CategoryId { get; set; }

        public string Comment { get; set; }

        public DateOnly Date { get; set; }

        public string Name { get; set; }

        public TransactionType Type { get; set; }

        public virtual User User { get; set; }

        public string UserId { get; set; }
    }
}