using System;
using System.Collections.Generic;

namespace Ipz_server.Models.Database;

public partial class Restaurant
{
    public Guid RestaurantId { get; set; }

    public string Name { get; set; } = null!;

    public Guid LocationId { get; set; }

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
