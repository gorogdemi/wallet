using AutoMapper;
using Wallet.Application.Common.Models;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;
using Wallet.Shared.Transactions;

namespace Wallet.Application.Common.Mappings;

public class EntityProfile : Profile
{
    public EntityProfile()
    {
        CreateMap<TransactionRequest, Transaction>();
        CreateMap<Transaction, TransactionViewModel>();

        CreateMap<CategoryRequest, Category>();
        CreateMap<Category, CategoryViewModel>();

        CreateMap<Balance, BalanceViewModel>();
    }
}