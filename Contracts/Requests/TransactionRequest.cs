using System;
using System.ComponentModel.DataAnnotations;

namespace Wallet.Contracts.Requests
{
    public class TransactionRequest
    {
        public double BankAmount { get; set; }

        public double CashAmount { get; set; }

        public int? CategoryId { get; set; }

        [MaxLength(255)]
        public string Comment { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int Type { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}