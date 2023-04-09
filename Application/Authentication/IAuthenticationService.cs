namespace DevQuarter.Wallet.Application.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

        Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);

        Task RegisterAsync(RegistrationRequest request);
    }
}