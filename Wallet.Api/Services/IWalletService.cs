using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Api.Domain;

namespace Wallet.Api.Services
{
    public interface IWalletService<TDomainType>
        where TDomainType : DomainBase
    {
        Task<TDomainType> CreateAsync(TDomainType domainType, CancellationToken cancellationToken);

        Task DeleteAsync(TDomainType domainType, CancellationToken cancellationToken);

        Task<IEnumerable<TDomainType>> GetAllAsync(CancellationToken cancellationToken);

        Task<TDomainType> GetAsync(long id, CancellationToken cancellationToken);

        Task<TDomainType> UpdateAsync(TDomainType domainType, CancellationToken cancellationToken);
    }
}