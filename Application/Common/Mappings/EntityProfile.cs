using AutoMapper;
using DevQuarter.Wallet.Application.Categories;
using DevQuarter.Wallet.Application.Common.Models;
using DevQuarter.Wallet.Application.Transactions;
using DevQuarter.Wallet.Domain.Entities;

namespace DevQuarter.Wallet.Application.Common.Mappings
{
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
}