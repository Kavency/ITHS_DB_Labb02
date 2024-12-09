namespace TheBookNook_WPF.Model;

public partial class Service
{
    public int Id { get; set; }

    public string Service1 { get; set; } = null!;

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
