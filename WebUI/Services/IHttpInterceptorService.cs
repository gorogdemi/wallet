namespace DevQuarter.Wallet.WebUI.Services
{
    public interface IHttpInterceptorService
    {
        void DisposeEvent();

        void RegisterEvent();
    }
}