using N9.Data.Entities;
using N9.Services.Models;

namespace N9.Services.Mappings;

public static class AuthorMappings
{
    public static AuthorModel ToModel(this Author author)
    {
        return new AuthorModel
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            Email = author.Email
        };
    }
}