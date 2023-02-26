using AutoMapper;
using Wallet.Api.Domain;
using Wallet.Api.Models;
using Wallet.Contracts.Requests;
using Wallet.Contracts.ViewModels;

namespace Wallet.Api.MappingProfiles
{
    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            CreateMap<TransactionRequest, Transaction>();
            CreateMap<Transaction, TransactionViewModel>();

            CreateMap<CategoryRequest, Category>();
            CreateMap<Category, CategoryViewModel>();

            CreateMap<BalanceModel, BalanceViewModel>();
        }
    }
}