using System.ComponentModel.DataAnnotations;

namespace Wallet.Contracts.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Az e-mail címet kötelező megadni.")]
        [EmailAddress(ErrorMessage = "Érvénytelen e-mail cím.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Az jelszót kötelező megadni.")]
        public string Password { get; set; }
    }
}