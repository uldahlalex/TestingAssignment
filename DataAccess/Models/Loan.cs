using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Loan
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public int UserId { get; set; }

    public DateTime? ReturnDate { get; set; }

    public bool? IsReturned { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Book1 Book { get; set; } = null!;

    public virtual Libraryuser User { get; set; } = null!;
}
