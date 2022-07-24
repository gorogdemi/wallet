using System;
using AutoMapper;
using Wallet.Contracts.Types;

namespace Wallet.Api.MappingProfiles
{
    public class TypeProfile : Profile
    {
        public TypeProfile()
        {
            CreateMap<DateOnly, DateTime>().ConvertUsing(date => date.ToDateTime(TimeOnly.MinValue));
            CreateMap<DateTime, DateOnly>().ConvertUsing(date => DateOnly.FromDateTime(date));

            CreateMap<TransactionType, Domain.Types.TransactionType>().ReverseMap();
        }
    }
}