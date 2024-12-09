using System;
using System.Collections.Generic;

namespace TheBookNook_WPF.DBModel;

public partial class Language
{
    public int Id { get; set; }

    public string Language1 { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
