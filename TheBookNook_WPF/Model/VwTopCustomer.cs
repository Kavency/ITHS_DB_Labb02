namespace TheBookNook_WPF.Model;

public partial class VwTopCustomer
{
    public int? CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public decimal? TotalSpent { get; set; }

    public long? CustomerRank { get; set; }

    public int Discount { get; set; }
}
