using N9.Data.Entities;
using N9.Services.Models;

namespace N9.Services;

public interface IBooksService
{
    Task<IEnumerable<BookModel>> GetBooksAsync();
    IQueryable<Book> GetBooksQuery(string? include = null);
}