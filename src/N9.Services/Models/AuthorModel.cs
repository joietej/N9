using N9.Data.Entities;

namespace N9.Services.Models;

public record AuthorModel
{
    public required int Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }

    public string? Email { get; init; }

    public ICollection<Book> Books { get; init; } = [];
}