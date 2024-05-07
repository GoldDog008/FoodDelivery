using System;
using System.Collections.Generic;

namespace Ipz_server.Models.Database;

public partial class Dish
{
    public Guid DishId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<OrderInformation> OrderInformations { get; set; } = new List<OrderInformation>();

    public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
}
