using System;
using System.Collections.Generic;

namespace Ipz_server.Models.Database;

public partial class Order
{
    public Guid OrderId { get; set; }

    public decimal TotalAmount { get; set; }

    public Guid UserId { get; set; }

    public Guid RestaurantId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<OrderInformation> OrderInformations { get; set; } = new List<OrderInformation>();

    public virtual Restaurant Restaurant { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
