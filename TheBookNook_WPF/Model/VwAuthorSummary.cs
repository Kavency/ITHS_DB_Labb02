using System;
using System.Collections.Generic;

namespace TheBookNook_WPF.DBModel;

public partial class VwAuthorSummary
{
    public string AuthorName { get; set; } = null!;

    public int? Age { get; set; }

    public int? OfBookTitles { get; set; }

    public double? ValueInStockPerAuthor { get; set; }
}
