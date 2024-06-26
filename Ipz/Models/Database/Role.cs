﻿using System;
using System.Collections.Generic;

namespace Ipz_server.Models.Database;

public partial class Role
{
    public Guid RoleId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
