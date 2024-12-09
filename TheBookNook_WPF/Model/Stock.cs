using System;
using System.Collections.Generic;

namespace TheBookNook_WPF.DBModel;

public partial class Stock
{
    public int StoreId { get; set; }

    public long Isbn { get; set; }

    public int Amount { get; set; }

    public virtual Book IsbnNavigation { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
