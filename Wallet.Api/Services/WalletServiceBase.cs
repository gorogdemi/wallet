using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wallet.Api.Domain;
using Wallet.Api.Exceptions;

namespace Wallet.Api.Services
{
    public abstract class WalletServiceBase<TDomainType> : IWalletService<TDomainType>
        where TDomainType : DomainBase, new()
    {
        protected WalletServiceBase(WalletContext context)
        {
            WalletContext = context;
        }

        protected WalletContext WalletContext { get; }

        public virtual async Task<TDomainType> CreateAsync(TDomainType domainType)
        {
            WalletContext.Set<TDomainType>().Add(domainType);

            try
            {
                await WalletContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (await GetAsync(domainType.Id) is not null)
                {
                    throw new EntityConflictException("Entity with the same ID already exists in the database.");
                }

                throw;
            }

            return domainType;
        }

        public virtual async Task DeleteAsync(TDomainType domainType)
        {
            try
            {
                WalletContext.Remove(domainType);
                await WalletContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await GetAsync(domainType.Id) is not null)
                {
                    throw new EntityConflictException("Entity has been modified during deletion.");
                }

                throw;
            }
        }

        public virtual async Task<IEnumerable<TDomainType>> GetAllAsync() => await WalletContext.Set<TDomainType>().ToListAsync();

        public virtual async Task<TDomainType> GetAsync(long id) => await WalletContext.Set<TDomainType>().FindAsync(id);

        public virtual async Task<TDomainType> UpdateAsync(TDomainType domainType)
        {
            try
            {
                await WalletContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await GetAsync(domainType.Id) is null)
                {
                    throw new EntityNotFoundException("Entity has been deleted before updating.");
                }

                throw new EntityConflictException("Entity has been modified before updating.");
            }

            return domainType;
        }
    }
}