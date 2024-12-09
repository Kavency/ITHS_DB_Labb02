namespace TheBookNook_WPF.Model;

public partial class VwTotalInStock
{
    public long Isbn { get; set; }

    public string Title { get; set; } = null!;

    public string Language { get; set; } = null!;

    public string Format { get; set; } = null!;

    public string Genre { get; set; } = null!;

    public decimal Price { get; set; }

    public int? InStock { get; set; }

    public string NeedsRestock { get; set; } = null!;
}
