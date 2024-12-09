using System;
using System.Collections.Generic;

namespace TheBookNook_WPF.DBModel;

public partial class Genre
{
    public int Id { get; set; }

    public string Genre1 { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
