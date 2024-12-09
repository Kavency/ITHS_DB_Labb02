using System;
using System.Collections.Generic;

namespace TheBookNook_WPF.DBModel;

public partial class Book
{
    public long Isbn { get; set; }

    public string Title { get; set; } = null!;

    public int LanguageId { get; set; }

    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public int FormatId { get; set; }

    public int GenreId { get; set; }

    public virtual Format Format { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
}
