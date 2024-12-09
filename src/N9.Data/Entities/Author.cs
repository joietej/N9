namespace N9.Data.Entities;

public class Author : IEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Email { get; set; }
    public ICollection<Book> Books { get; } = [];
    public int Id { get; set; }
}