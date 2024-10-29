using DataAccess.Models;

namespace Service.Models;

public class CreateBookDto
{
    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string? Genre { get; set; }


    public Book ToBook()
    {
        var book = new Book
        {
            Title = Title,
            Author = Author,
            Genre = Genre,
            CreatedAt = DateTime.UtcNow,
        };
        return book;
    }
}