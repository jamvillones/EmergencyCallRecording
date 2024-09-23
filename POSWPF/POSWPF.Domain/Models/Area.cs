using System;
using System.Collections.Generic;

namespace ECR.Domain.Models;

public partial class Area {
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Caller> Callers { get; set; } = new List<Caller>();
}
