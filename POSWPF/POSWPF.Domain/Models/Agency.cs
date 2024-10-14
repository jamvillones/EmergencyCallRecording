using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR.Domain.Models {
    [Table(nameof(Agency))]
    public sealed class Agency {
        public int Id { get; set; }
        /// <summary>
        /// the optional logo of the organization the call is referred
        /// </summary>
        public byte[]? Logo { get; set; } = [];
        public string Name { get; set; } = null!;
        public string? Address { get; set; } = string.Empty;
        public List<ContactDetail> ContactDetails { get; set; } = [];
        public List<Record> Records { get; set; } = [];
    }
}
