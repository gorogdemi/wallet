namespace Wallet.UI.Helpers
{
    public static class UrlHelper
    {
        public const string BalanceUrl = "api/balance";
        public const string CategoryUrl = "api/categories";
        public const string LoginUrl = "api/authentication/login";
        public const string RegisterUrl = "api/authentication/register";
        public const string TransactionUrl = "api/transactions";

        public static string GetCategoryUrlWith(int id) => $"{CategoryUrl}/{id}";

        public static string GetTransactionUrlWith(int id) => $"{TransactionUrl}/{id}";

        public static string GetTransactionUrlWith(string name) => $"{TransactionUrl}/search/{name}";
    }
}