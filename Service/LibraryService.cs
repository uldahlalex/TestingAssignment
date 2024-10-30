using DataAccess;
using DataAccess.Models;
using Service.Models;

namespace Service;

public interface ILibraryService
{
    Book AddBook(CreateBookDto dto);
    LoanResponse Loan(LoanBookDto dto);
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

    public LoanResponse Loan(LoanBookDto dto)
    {
        var loan = dto.ToLoan();
        
        var isLoaned = context.Loans.Any(l => l.BookId == loan.BookId || l.IsReturned != false);
        if(!isLoaned)
            throw new Exception("Book is already loaned");
        context.Loans.Add(loan);
        context.SaveChanges();
        return new LoanResponse() {Id = loan.Id};
    }
}