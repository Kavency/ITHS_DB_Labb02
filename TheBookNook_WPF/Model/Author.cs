using System.ComponentModel.DataAnnotations.Schema;

namespace TheBookNook_WPF.Model;

public partial class Author
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    [NotMapped]
    public string FullName { get; set; }

    public virtual ICollection<Book> BookIsbns { get; set; } = new List<Book>();

}
