using DataAccess.Models;

namespace Service.Models;

public class LoanBookDto
{

    public int BookId { get; set; }

    public int UserId { get; set; }
    



    public Loan ToLoan()
    {
        var loan = new Loan()
        {
           
            BookId = BookId,
            UserId = UserId,
            ReturnDate = DateTime.UtcNow.AddDays(14),
            IsReturned = false,
            CreatedAt = DateTime.UtcNow
        };
        return loan;
    }
}