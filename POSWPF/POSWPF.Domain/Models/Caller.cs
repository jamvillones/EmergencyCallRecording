using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR.Domain.Models {
    public sealed class Caller {
        public string Name { get; set; } = null!;
        public string ContactDetail { get; set; } = null!;
        public string? Address { get; set; } = null;
        public override string ToString()
            => Name + " ● " + ContactDetail + (string.IsNullOrWhiteSpace(Address) ? "" : " ● " + Address);
    }
}
