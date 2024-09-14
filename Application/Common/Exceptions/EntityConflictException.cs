namespace Wallet.Application.Common.Exceptions;

public class EntityConflictException : WalletServiceException
{
    public EntityConflictException(string message)
        : base(message)
    {
    }

    public EntityConflictException()
    {
    }

    public EntityConflictException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}