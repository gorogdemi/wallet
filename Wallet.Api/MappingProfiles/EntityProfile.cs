using AutoMapper;
using Wallet.Api.Domain;
using Wallet.Api.Models;
using Wallet.Contracts.Requests;
using Wallet.Contracts.Responses;

namespace Wallet.Api.MappingProfiles
{
    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            CreateMap<TransactionRequest, Transaction>();
            CreateMap<Transaction, TransactionResponse>();

            CreateMap<CategoryRequest, Category>();
            CreateMap<Category, CategoryResponse>();

            CreateMap<BalanceModel, BalanceResponse>();
        }
    }
}