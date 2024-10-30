using System.ComponentModel.DataAnnotations;
using DataAccess.Models;

namespace Service.Models;

public class CreateBookDto
{
    [MinLength(1)]
    public string Title { get; set; } = null!;

    [MinLength(1)]
    public string Author { get; set; } = null!;

    [MaxLength(1)]
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