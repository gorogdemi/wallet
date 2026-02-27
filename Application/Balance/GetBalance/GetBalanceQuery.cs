using FastEndpoints;
using Wallet.Shared.Balance;

namespace Wallet.Application.Balance.GetBalance;

public sealed record GetBalanceQuery : ICommand<BalanceDto>;