using System;
using System.Collections.Generic;

namespace POSWPF.Domain.Models;

public partial class Street
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Caller> Callers { get; set; } = new List<Caller>();
}
