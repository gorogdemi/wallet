using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Wallet.Infrastructure.Persistence;

public class WalletContextInitializer
{
    private readonly WalletContext _context;
    private readonly ILogger<WalletContextInitializer> _logger;

    public WalletContextInitializer(WalletContext context, ILogger<WalletContextInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }
}