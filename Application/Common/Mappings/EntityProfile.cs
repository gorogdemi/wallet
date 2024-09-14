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
        CreateMap<Transaction, TransactionDto>();

        CreateMap<CategoryRequest, Category>();
        CreateMap<Category, CategoryDto>();

        CreateMap<Balance, BalanceViewModel>();
    }
}