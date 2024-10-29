using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Book
{
    public int Id { get; set; }

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

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
