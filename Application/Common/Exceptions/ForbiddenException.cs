namespace DevQuarter.Wallet.Application.Common.Exceptions;

public class ForbiddenException : WalletServiceException
{
    public ForbiddenException(string message)
        : base(message)
    {
    }

    public ForbiddenException()
    {
    }

    public ForbiddenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}