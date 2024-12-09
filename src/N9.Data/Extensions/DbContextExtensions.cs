using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using N9.Data.Context;
using N9.Data.Entities;

namespace N9.Data.Extensions;

public static class DbContextExtensions
{
    public static async Task EnsureDatabaseAsync(this BooksDbContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken)) await dbCreator.CreateAsync(cancellationToken);
        });
    }

    public static async Task SeedAsync(this BooksDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Books.AnyAsync(cancellationToken))
        {
            return;
        }
        
        var author = new Author { FirstName = "James", LastName = "Bond" };
        var book = new Book { Title = "C#", Author = author };

        await dbContext.Books.AddAsync(book, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
    }
}