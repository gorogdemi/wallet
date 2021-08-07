namespace Wallet.Contracts.Responses
{
    public class BalanceResponse
    {
        public double BankAccount { get; set; }

        public double Cash { get; set; }

        public double Full { get; set; }
    }
}