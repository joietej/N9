using N9.Data.Context;
using N9.Data.Entities;

namespace N9.Data.Repositories;

public interface IAuthorRepository : IRepositoryBase<Author>
{
}

public class AuthorRepository(BooksDbContext context) : RepositoryBase<Author>(context), IAuthorRepository
{
}