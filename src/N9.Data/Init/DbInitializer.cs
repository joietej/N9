using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using N9.Data.Context;
using N9.Data.Extensions;

namespace N9.Data.Init;

public class DbInitializer(BooksDbContext dbContext, ILogger<DbInitializer> logger) : IDbInitializer
{
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Initializing DB");
        
        await dbContext.EnsureDatabaseAsync(cancellationToken);
        logger.LogInformation("DB Initialized");
        
        logger.LogInformation("Applying migrations");
        await dbContext.Database.MigrateAsync(cancellationToken);
        logger.LogInformation("Migrations Applied");
        
        await dbContext.SeedAsync(cancellationToken);
        logger.LogInformation("Database seeded");
    }
}