using System;
using System.Collections.Generic;

namespace TheBookNook_WPF.DBModel;

public partial class Service
{
    public int Id { get; set; }

    public string Service1 { get; set; } = null!;

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
