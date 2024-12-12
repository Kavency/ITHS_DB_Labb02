namespace TheBookNook_WPF.Model;

public partial class Author
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public virtual ICollection<Book> BookIsbns { get; set; } = new List<Book>();
    //public virtual ICollection<AuthorBook> BookAuthors { get; set; } = new List<AuthorBook>();
}
