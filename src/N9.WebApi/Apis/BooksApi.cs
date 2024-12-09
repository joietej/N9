using N9.Services;

namespace N9.WebApi.Apis;

public static class BooksApi
{
    public static RouteGroupBuilder MapBooksApi(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/books");

        group.MapGet("/", async (IBooksService booksService) => await booksService.GetBooksAsync())
            .WithOpenApi()
            .WithName("GetBooks")
            .WithDescription("Get all books");

        return group;
    }
}