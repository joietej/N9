using HotChocolate.Resolvers;
using N9.Services;
using N9.Services.Mappings;
using N9.Services.Models;

namespace N9.WebApi.GraphQL.Queries;

public class BookQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<BookModel> GetBooks([Service] IBooksService booksService, IResolverContext context) =>
        booksService
            .GetBooksQuery("Author")
            .Project(context)
            .Filter(context)
            .Sort(context)
            .Select(x => x.ToModel());
}