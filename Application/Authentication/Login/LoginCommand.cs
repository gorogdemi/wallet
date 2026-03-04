using FastEndpoints;
using Wallet.Application.Common.Interfaces;
using Wallet.Shared.Authentication;

namespace Wallet.Application.Authentication.Login;

public sealed record LoginCommand(LoginRequest Request) : ICommand<IUser>;