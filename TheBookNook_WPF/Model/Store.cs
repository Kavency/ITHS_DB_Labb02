namespace TheBookNook_WPF.Model;

public partial class Store
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? StreetAddress { get; set; }

    public string? PostalCode { get; set; }

    public string? City { get; set; }

    public int CountryId { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    public virtual ICollection<StoreBook> StoreBooks { get; set; }

}
