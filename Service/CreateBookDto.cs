using DataAccess.Models;

namespace Service;

public class CreateBookDto
{

    public string Isbn { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string? Publisher { get; set; }

    public DateOnly? PublicationDate { get; set; }

    public string? Genre { get; set; }

    public string? Description { get; set; }

    public int? PageCount { get; set; }

    public string? Language { get; set; }

    public string? Format { get; set; }

    public decimal? Price { get; set; }

    public int? StockQuantity { get; set; }

    public bool? IsAvailable { get; set; }

    public Book ToBook()
    {
        var book = new Book()
        {
            Isbn = Isbn,
            Title = Title,
            Author = Author,
            Publisher = Publisher,
            PublicationDate = PublicationDate,
            Genre = Genre,
            Description = Description,
            PageCount = PageCount,
            Language = Language,
            Format = Format,
            Price = Price,
            StockQuantity = StockQuantity,
            IsAvailable = IsAvailable,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        return book;
    }
}