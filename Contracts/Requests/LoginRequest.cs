using System.ComponentModel.DataAnnotations;

namespace Wallet.Contracts.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Az jelszót kötelező megadni.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "A felhasználónevet kötelező megadni.")]
        public string UserName { get; set; }
    }
}