using System;
using System.Collections.Generic;

namespace TheBookNook_WPF.DBModel;

public partial class Country
{
    public int Id { get; set; }

    public string Country1 { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
