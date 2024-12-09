using N9.Data.Entities;
using N9.Services.Models;

namespace N9.Services.Mappings;

public static class BookMappings
{
    public static BookModel ToModel(this Book book)
    {
        return new BookModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author?.ToModel()
        };
    }
}