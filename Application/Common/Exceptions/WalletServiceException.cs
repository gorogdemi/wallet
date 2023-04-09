namespace DevQuarter.Wallet.Application.Common.Exceptions
{
    public class WalletServiceException : Exception
    {
        public WalletServiceException(string message)
            : base(message)
        {
        }

        public WalletServiceException()
        {
        }

        public WalletServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}