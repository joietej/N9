using N9.Data.Init;
using N9.Data.Repositories;
using N9.Services;
using N9.WebApi.Services;

namespace N9.WebApi.Extensions;

public static class Dependencies
{
    public static WebApplicationBuilder AddConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDbInitializer, DbInitializer>();
        builder.Services.AddScoped<IBookRepository, BookRepository>();
        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

        builder.Services.AddScoped<IBooksService, BooksService>();

        builder.Services.AddScoped<IBooksApiService, BooksApiService>();

        return builder;
    }
}