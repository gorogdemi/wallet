using System.ComponentModel.DataAnnotations;

namespace Wallet.Contracts.Requests
{
    public class CategoryRequest
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}