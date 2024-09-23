using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR.Domain.Models {
    public sealed class Caller {
        public int Id { get; set; }
        public string? ContactDetails { get; set; }
        public Name Name { get; set; } = null!;
        public Address Address { get; set; } = null!;
    }
}
