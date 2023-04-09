using AutoMapper;

namespace DevQuarter.Wallet.Application.Common.Mappings
{
    public class TypeProfile : Profile
    {
        public TypeProfile()
        {
            CreateMap<DateOnly, DateTime>().ConvertUsing(date => date.ToDateTime(TimeOnly.MinValue));
            CreateMap<DateTime, DateOnly>().ConvertUsing(date => DateOnly.FromDateTime(date));
        }
    }
}