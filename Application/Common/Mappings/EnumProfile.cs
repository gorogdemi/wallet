using AutoMapper;
using Wallet.Domain.Enums;

namespace Wallet.Application.Common.Mappings;

public class EnumProfile : Profile
{
    public EnumProfile()
    {
        CreateMap<TransactionType, Shared.Common.Enums.TransactionType>();
    }
}