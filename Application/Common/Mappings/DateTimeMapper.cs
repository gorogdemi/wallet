using Riok.Mapperly.Abstractions;

namespace Wallet.Application.Common.Mappings;

[Mapper]
public static partial class DateTimeMapper
{
    public static DateOnly ToDateOnly(DateTime? date) => date.HasValue ? DateOnly.FromDateTime(date.Value) : default;

    public static DateTime ToDateTime(DateOnly date) => date.ToDateTime(TimeOnly.MinValue);
}