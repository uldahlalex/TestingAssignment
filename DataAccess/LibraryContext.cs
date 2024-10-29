using System;
using System.Collections.Generic;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public partial class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Book1> Books1 { get; set; }

    public virtual DbSet<Libraryuser> Libraryusers { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("books_pkey");

            entity.ToTable("books");

            entity.HasIndex(e => e.Isbn, "books_isbn_key").IsUnique();

            entity.HasIndex(e => e.Author, "idx_books_author");

            entity.HasIndex(e => e.Isbn, "idx_books_isbn");

            entity.HasIndex(e => e.Title, "idx_books_title");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Author)
                .HasMaxLength(255)
                .HasColumnName("author");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Format)
                .HasMaxLength(50)
                .HasColumnName("format");
            entity.Property(e => e.Genre)
                .HasMaxLength(50)
                .HasColumnName("genre");
            entity.Property(e => e.IsAvailable)
                .HasDefaultValue(true)
                .HasColumnName("is_available");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("isbn");
            entity.Property(e => e.Language)
                .HasMaxLength(50)
                .HasDefaultValueSql("'English'::character varying")
                .HasColumnName("language");
            entity.Property(e => e.PageCount).HasColumnName("page_count");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.PublicationDate).HasColumnName("publication_date");
            entity.Property(e => e.Publisher)
                .HasMaxLength(255)
                .HasColumnName("publisher");
            entity.Property(e => e.StockQuantity)
                .HasDefaultValue(0)
                .HasColumnName("stock_quantity");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Book1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("books_pkey");

            entity.ToTable("books", "library");

            entity.HasIndex(e => e.Isbn, "books_isbn_key").IsUnique();

            entity.HasIndex(e => e.Author, "idx_books_author");

            entity.HasIndex(e => e.Isbn, "idx_books_isbn");

            entity.HasIndex(e => e.Title, "idx_books_title");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Author)
                .HasMaxLength(255)
                .HasColumnName("author");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Format)
                .HasMaxLength(50)
                .HasColumnName("format");
            entity.Property(e => e.Genre)
                .HasMaxLength(50)
                .HasColumnName("genre");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("isbn");
            entity.Property(e => e.Language)
                .HasMaxLength(50)
                .HasDefaultValueSql("'English'::character varying")
                .HasColumnName("language");
            entity.Property(e => e.Publisher)
                .HasMaxLength(255)
                .HasColumnName("publisher");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Libraryuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("libraryuser_pkey");

            entity.ToTable("libraryuser", "library");

            entity.HasIndex(e => e.Email, "idx_libraryuser_email");

            entity.HasIndex(e => e.Email, "libraryuser_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("book_loans_pkey");

            entity.ToTable("loans", "library");

            entity.HasIndex(e => e.BookId, "idx_book_loans_book_id");

            entity.HasIndex(e => e.ReturnDate, "idx_book_loans_return_date");

            entity.HasIndex(e => e.UserId, "idx_book_loans_user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.IsReturned)
                .HasDefaultValue(false)
                .HasColumnName("is_returned");
            entity.Property(e => e.ReturnDate).HasColumnName("return_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Loans)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_book");

            entity.HasOne(d => d.User).WithMany(p => p.Loans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
