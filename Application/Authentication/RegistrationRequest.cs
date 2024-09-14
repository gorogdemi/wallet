namespace DevQuarter.Wallet.Application.Authentication;

public class RegistrationRequest
{
    public string Email { get; set; }

    public string EmailConfirm { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; }

    public string PasswordConfirm { get; set; }

    public string UserName { get; set; }
}