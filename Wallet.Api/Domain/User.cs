using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Wallet.Api.Domain
{
    public class User : IdentityUser
    {
        public virtual ICollection<Category> Categories { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}