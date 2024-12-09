namespace TheBookNook_WPF.Model;

public partial class Format
{
    public int Id { get; set; }

    public string Format1 { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
