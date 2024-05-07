using System;
using System.Collections.Generic;

namespace Ipz_server.Models.Database;

public partial class Location
{
    public Guid LocationId { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public virtual Restaurant? Restaurant { get; set; }

    public virtual User? User { get; set; }
}
