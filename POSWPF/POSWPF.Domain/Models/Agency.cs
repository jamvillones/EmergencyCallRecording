using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSWPF.Domain.Models {
    public sealed class Agency {
        public int Id { get; set; }
        /// <summary>
        /// the optional logo of the organization the call is referred
        /// </summary>
        public byte[]? Logo { get; set; } = [];
        public string Name { get; set; } = null!;
        public string ContactInfo { get; set; } = string.Empty;
    }
}
