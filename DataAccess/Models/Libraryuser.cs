using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Libraryuser
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
