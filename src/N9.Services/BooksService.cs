using N9.Data.Repositories;
using N9.Services.Mappings;
using N9.Services.Models;

namespace N9.Services;

public class BooksService(IBookRepository repository) : IBooksService
{
    public async Task<IEnumerable<BookModel>> GetBooksAsync()
    {
        var books = await repository.GetAllAsync();
        return books
            .Select(b => b.ToModel())
            .ToList();
    }
}