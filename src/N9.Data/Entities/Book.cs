namespace N9.Data.Entities;

public class Book : IEntity
{
    public required string Title { get; set; }
    public int AuthorId { get; set; }
    public Author? Author { get; set; } 
    public int Id { get; set; }
}