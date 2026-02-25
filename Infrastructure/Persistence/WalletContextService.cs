using Microsoft.EntityFrameworkCore;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Common;

namespace Wallet.Infrastructure.Persistence;

public class WalletContextService : IDbContextService
{
    private readonly WalletContext _context;

    public WalletContextService(WalletContext walletContext)
    {
        _context = walletContext;
    }

    public async Task<TDomainType> CreateAsync<TDomainType>(TDomainType domainType, CancellationToken cancellationToken)
        where TDomainType : EntityBase, new()
    {
        _context.Set<TDomainType>().Add(domainType);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
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
            _context.Set<TDomainType>().Remove(domainType);
            await _context.SaveChangesAsync(cancellationToken);
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

    public async Task<TDomainType> GetAsync<TDomainType>(string id, CancellationToken cancellationToken)
        where TDomainType : EntityBase, new() =>
        await _context.Set<TDomainType>().FindAsync([id], cancellationToken);

    public IQueryable<TDomainType> GetQueryableAsNoTracking<TDomainType>()
        where TDomainType : EntityBase, new() =>
        _context.Set<TDomainType>().AsNoTracking();

    public async Task<TDomainType> UpdateAsync<TDomainType>(TDomainType domainType, CancellationToken cancellationToken)
        where TDomainType : EntityBase, new()
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
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