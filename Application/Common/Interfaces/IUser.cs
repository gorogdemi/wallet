namespace Wallet.Application.Common.Interfaces;

public interface IUser
{
    string Email { get; }

    string FullName { get; }

    string Id { get; }

    string UserName { get; }
}