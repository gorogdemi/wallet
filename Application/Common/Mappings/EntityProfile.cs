using AutoMapper;
using Wallet.Application.Categories;
using Wallet.Application.Common.Models;
using Wallet.Application.Transactions;
using Wallet.Domain.Entities;

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