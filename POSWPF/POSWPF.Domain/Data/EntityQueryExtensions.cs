using ECR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ECR.Domain.Data {
    public static class EntityQueryExtensions {
        public static IQueryable<Record> FilterRecord(this IQueryable<Record> records, string keyword) {
            if (string.IsNullOrWhiteSpace(keyword)) return records;

            return records.Where(r => r.Call.Name.Contains(keyword) ||
            r.Call.ContactDetail.Contains(keyword) ||
            r.Call.Address != null && r.Call.Address.Contains(keyword));
        }

        public static IQueryable<Agency> FilterAgency(this IQueryable<Agency> agencies, string keyword) {
            if (string.IsNullOrWhiteSpace(keyword)) return agencies;

            return agencies.Where(a => a.Name.Contains(keyword) ||
            (a.Address != null && a.Address.Contains(keyword)) || 
            a.ContactDetails.Any(c => c.Value.Contains(keyword)));
        }
    }
}
