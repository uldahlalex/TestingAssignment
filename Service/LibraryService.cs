using DataAccess;
using DataAccess.Models;
using Service.Models;

namespace Service;

public interface ILibraryService
{
    Book AddBook(CreateBookDto dto);
}

public class LibraryService(LibraryContext context) : ILibraryService
{
    public Book AddBook(CreateBookDto dto)
    {
        var book = dto.ToBook();
        context.Books.Add(book);
        context.SaveChanges();
        return book;
    }
}