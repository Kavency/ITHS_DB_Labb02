namespace TheBookNook_WPF.Model;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public long? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public virtual Order? Order { get; set; }
}
