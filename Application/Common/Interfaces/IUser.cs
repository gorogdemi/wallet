namespace Wallet.Application.Common.Interfaces;

public interface IUser
{
    string Email { get; set; }

    string FullName { get; set; }

    string Id { get; set; }

    string UserName { get; set; }
}