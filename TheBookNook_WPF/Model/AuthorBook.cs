namespace TheBookNook_WPF.Model
{
    public class AuthorBook
    {
        public long BookIsbn { get; set; }
        public Book Book { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
