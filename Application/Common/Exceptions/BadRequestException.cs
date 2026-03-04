namespace Wallet.Application.Common.Exceptions;

public class BadRequestException : WalletServiceException
{
    public BadRequestException(string message)
        : base(message)
    {
    }

    public BadRequestException()
    {
    }

    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}