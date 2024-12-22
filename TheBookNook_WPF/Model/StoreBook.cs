namespace TheBookNook_WPF.Model;

internal class StoreBook
{
    public int StoreId { get; set; }
    public Store Store { get; set; }

    public long Isbn { get; set; }
    public Book Book { get; set; }
}
