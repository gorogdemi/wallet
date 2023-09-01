namespace DevQuarter.Wallet.WebUI.Helpers
{
    internal static class UriHelper
    {
        public const string BalanceUri = "api/balance";
        public const string CategoryUri = "api/categories";
        public const string LoginUri = "api/authentication/login";
        public const string RegisterUri = "api/authentication/register";
        public const string RefreshUri = "api/authentication/refresh";
        public const string TransactionUri = "api/transactions";

        public static string GetCategoryUriWith(long id) => $"{CategoryUri}/{id}";

        public static string GetTransactionUriWith(long id) => $"{TransactionUri}/{id}";

        public static string GetTransactionUriWith(string name) => $"{TransactionUri}/search/{name}";
    }
}