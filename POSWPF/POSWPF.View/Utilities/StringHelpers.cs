using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR.WPF.Utilities {
    internal static class StringHelpers {
        public static string? NullIfEmptyAndWhitespace_TrimIfNot(this string? value) {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return value.Trim();
        }
    }
}