using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet.Api.Domain
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual User User { get; set; }

        [ForeignKey(nameof(User))]
        [Required]
        public string UserId { get; set; }
    }
}