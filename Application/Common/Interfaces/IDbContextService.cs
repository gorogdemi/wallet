using Wallet.Domain.Common;

namespace Wallet.Application.Common.Interfaces;

public interface IDbContextService
{
    Task<TDomainType> CreateAsync<TDomainType>(TDomainType domainType, CancellationToken cancellationToken)
        where TDomainType : EntityBase, new();

    Task DeleteAsync<TDomainType>(TDomainType domainType, CancellationToken cancellationToken)
        where TDomainType : EntityBase, new();

    Task<TDomainType> GetAsync<TDomainType>(string id, CancellationToken cancellationToken)
        where TDomainType : EntityBase, new();

    IQueryable<TDomainType> GetQueryableAsNoTracking<TDomainType>()
        where TDomainType : EntityBase, new();

    Task<TDomainType> UpdateAsync<TDomainType>(TDomainType domainType, CancellationToken cancellationToken)
        where TDomainType : EntityBase, new();
}