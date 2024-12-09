using N9.Services.Models;

namespace N9.Services;

public interface IBooksService
{
    Task<IEnumerable<BookModel>> GetBooksAsync();
}