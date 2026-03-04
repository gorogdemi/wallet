using FastEndpoints;
using Wallet.Application.Common.Models;
using Wallet.Shared.Authentication;

namespace Wallet.Application.Authentication.Register;

public sealed record RegisterCommand(RegisterRequest Request) : ICommand<Result>;