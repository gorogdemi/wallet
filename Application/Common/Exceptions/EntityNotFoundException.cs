namespace DevQuarter.Wallet.Application.Common.Exceptions;

public class EntityNotFoundException : WalletServiceException
{
    public EntityNotFoundException(string message)
        : base(message)
    {
    }

    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}