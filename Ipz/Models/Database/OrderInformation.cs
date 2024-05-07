using System;
using System.Collections.Generic;

namespace Ipz_server.Models.Database;

public partial class OrderInformation
{
    public Guid OrderInformationsId { get; set; }

    public int Quantity { get; set; }

    public Guid OrderId { get; set; }

    public Guid DishId { get; set; }

    public virtual Dish Dish { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
