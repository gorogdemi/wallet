using System.ComponentModel.DataAnnotations;

namespace Wallet.Contracts.Requests
{
    public class RegistrationRequest
    {
        [Required(ErrorMessage = "Az e-mail címet kötelező megadni.")]
        [EmailAddress(ErrorMessage = "Érvénytelen e-mail cím.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Az e-mail címet kötelező megadni.")]
        [Compare(nameof(Email), ErrorMessage = "A két e-mail cím nem egyezik.")]
        [EmailAddress(ErrorMessage = "Érvénytelen e-mail cím.")]
        public string EmailConfirm { get; set; }

        [Required(ErrorMessage = "A keresztnevet kötelező megadni.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "A vezetéknevet kötelező megadni.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "A jelszót kötelező megadni.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "A jelszót kötelező megadni.")]
        [Compare(nameof(Password), ErrorMessage = "A két jelszó nem egyezik.")]
        public string PasswordConfirm { get; set; }
    }
}