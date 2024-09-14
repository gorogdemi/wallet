using DevQuarter.Wallet.Domain.Common;

namespace DevQuarter.Wallet.Application.Common.Interfaces;

public interface IWalletContextService
{
    IWalletContext Context { get; }

    Task<TDomainType> CreateAsync<TDomainType>(TDomainType domainType, CancellationToken cancellationToken)
        where TDomainType : EntityBase, new();

    Task DeleteAsync<TDomainType>(TDomainType domainType, CancellationToken cancellationToken)
        where TDomainType : EntityBase, new();

    Task<IEnumerable<TDomainType>> GetAllAsync<TDomainType>(CancellationToken cancellationToken)
        where TDomainType : EntityBase, new();

    Task<TDomainType> GetAsync<TDomainType>(long id, CancellationToken cancellationToken)
        where TDomainType : EntityBase, new();

    Task<TDomainType> UpdateAsync<TDomainType>(TDomainType domainType, CancellationToken cancellationToken)
        where TDomainType : EntityBase, new();
}