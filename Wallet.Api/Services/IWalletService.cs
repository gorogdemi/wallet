using System.Collections.Generic;
using System.Threading.Tasks;
using Wallet.Api.Domain;

namespace Wallet.Api.Services
{
    public interface IWalletService<TDomainType>
        where TDomainType : DomainBase
    {
        Task<TDomainType> CreateAsync(TDomainType domainType);

        Task DeleteAsync(TDomainType domainType);

        Task<IEnumerable<TDomainType>> GetAllAsync();

        Task<TDomainType> GetAsync(long id);

        Task<TDomainType> UpdateAsync(TDomainType domainType);
    }
}