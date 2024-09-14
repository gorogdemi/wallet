using DevQuarter.Wallet.Application.Common.Exceptions;
using DevQuarter.Wallet.Application.Common.Interfaces;
using DevQuarter.Wallet.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace DevQuarter.Wallet.Application.Common.Services;

public class WalletContextService : IWalletContextService
{
    public WalletContextService(IWalletContext walletContext)
    {
        Context = walletContext;
    }

    public IWalletContext Context { get; }

    public async Task<TDomainType> CreateAsync<TDomainType>(TDomainType domainType, CancellationToken cancellationToken)
        where TDomainType : EntityBase, new()
    {
        Context.Set<TDomainType>().Add(domainType);

        try
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            if (await GetAsync<TDomainType>(domainType.Id, cancellationToken) is not null)
            {
                throw new EntityConflictException("Entity with the same ID already exists in the database.");
            }

            throw;
        }

        return domainType;
    }

    public async Task DeleteAsync<TDomainType>(TDomainType domainType, CancellationToken cancellationToken)
        where TDomainType : EntityBase, new()
    {
        try
        {
            Context.Set<TDomainType>().Remove(domainType);
            await Context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await GetAsync<TDomainType>(domainType.Id, cancellationToken) is not null)
            {
                throw new EntityConflictException("Entity has been modified during deletion.");
            }

            throw;
        }
    }

    public async Task<IEnumerable<TDomainType>> GetAllAsync<TDomainType>(CancellationToken cancellationToken)
        where TDomainType : EntityBase, new() =>
        await Context.Set<TDomainType>().ToListAsync(cancellationToken);

    public async Task<TDomainType> GetAsync<TDomainType>(long id, CancellationToken cancellationToken)
        where TDomainType : EntityBase, new() =>
        await Context.Set<TDomainType>().FindAsync([id], cancellationToken);

    public async Task<TDomainType> UpdateAsync<TDomainType>(TDomainType domainType, CancellationToken cancellationToken)
        where TDomainType : EntityBase, new()
    {
        try
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await GetAsync<TDomainType>(domainType.Id, cancellationToken) is null)
            {
                throw new EntityNotFoundException("Entity has been deleted before updating.");
            }

            throw new EntityConflictException("Entity has been modified before updating.");
        }

        return domainType;
    }
}