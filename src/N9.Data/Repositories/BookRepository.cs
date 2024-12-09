using N9.Data.Context;
using N9.Data.Entities;

namespace N9.Data.Repositories;

public interface IBookRepository : IRepositoryBase<Book>
{
}

public class BookRepository(BooksDbContext context) : RepositoryBase<Book>(context), IBookRepository
{
}