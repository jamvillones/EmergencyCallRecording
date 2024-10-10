using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR.Domain.Models {
    [Table(nameof(Caller))]
    public sealed class Caller {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ContactDetail { get; set; } = null!;
        public string? Address { get; set; } = null;
    }
}
