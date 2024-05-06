using System;
using System.Collections.Generic;

namespace Ipz.Models.Database;

public partial class Dish
{
    public Guid DishId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public Guid RestaurantId { get; set; }

    public virtual ICollection<OrderInformation> OrderInformations { get; set; } = new List<OrderInformation>();

    public virtual Restaurant Restaurant { get; set; } = null!;
}
