using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet.Api.Domain
{
    public class RefreshToken
    {
        public DateTime CreationDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsInvalidated { get; set; }

        public bool IsUsed { get; set; }

        [Required]
        [MaxLength(100)]
        public string JwtId { get; set; }

        [Key]
        [MaxLength(100)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Token { get; set; }

        public virtual User User { get; set; }

        [ForeignKey(nameof(User))]
        [Required]
        public string UserId { get; set; }
    }
}