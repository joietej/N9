using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using N9.Data.Context;

namespace N9.WebApi.Extensions;

public static class DbExtensions
{
    public static WebApplicationBuilder AddDbContextWithSqlConnection<T>(this WebApplicationBuilder builder)
        where T : DbContext
    {
        var connectionString = builder.Configuration.GetConnectionString("Sql");

        if (connectionString == null) return builder;
        
        builder.Services
            .AddDbContext<T>(options =>
                options
                    .UseSqlServer(connectionString, p => { p.EnableRetryOnFailure(); })
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        return builder;
    }
}