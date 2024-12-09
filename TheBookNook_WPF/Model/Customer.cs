namespace TheBookNook_WPF.Model;

public partial class Customer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? StreetName { get; set; }

    public string? PostalCode { get; set; }

    public string? City { get; set; }

    public int? CountryId { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
