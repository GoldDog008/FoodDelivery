using System;
using System.Collections.Generic;

namespace Ipz_server.Models.Database;

public partial class OrderStatus
{
    public Guid OrderStatuseId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
