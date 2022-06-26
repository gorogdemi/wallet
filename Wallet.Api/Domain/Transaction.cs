using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet.Api.Domain
{
    public class Transaction
    {
        public double BankAmount { get; set; }

        public double CashAmount { get; set; }

        public virtual Category Category { get; set; }

        public int? CategoryId { get; set; }

        [MaxLength(255)]
        public string Comment { get; set; }

        public DateTime Date { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public TransactionType Type { get; set; }

        public virtual User User { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}