﻿namespace TheBookNook_WPF.Model;

public partial class AuthorBook
{
    public int AuthorId { get; set; }
    public Author Author { get; set; }

    public long BookIsbn { get; set; }
    public Book Book { get; set; }
}
