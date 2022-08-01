namespace Wallet.UI.Services
{
    public interface IHttpInterceptorService
    {
        void DisposeEvent();

        void RegisterEvent();
    }
}