﻿namespace TheBookNook_WPF.Model;

public class StoreBook
{
    public int StoreId { get; set; }
    public Store Store { get; set; }

    public long BookIsbn { get; set; }
    public Book Book { get; set; }
}
