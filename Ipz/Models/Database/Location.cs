using System;
using System.Collections.Generic;

namespace Ipz.Models.Database;

public partial class Location
{
    public Guid LocationId { get; set; }

    public string Country { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
