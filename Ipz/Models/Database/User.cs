using System;
using System.Collections.Generic;

namespace Ipz_server.Models.Database;

public partial class User
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public Guid RoleId { get; set; }

    public Guid? LocationId { get; set; }

    public string Password { get; set; } = null!;

    public virtual Location? Location { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;
}
