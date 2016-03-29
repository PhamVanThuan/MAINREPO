using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Core.Strings
{
    public class StringReplacer : IStringReplacer
    {
        public string Replace(string source, IEnumerable<KeyValuePair<string, string>> replacements,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(source) || replacements == null || !replacements.Any())
            {
                return source;
            }

            var builder = new StringBuilder();
            var previousIndex = 0;
            //have to use a loop rather than builder.Replace because it has no case-insensitive overload
            foreach (var item in replacements)
            {
                if (string.IsNullOrEmpty(item.Key))
                {
                    continue;
                }
                var oldValue = item.Key;
                var newValue = item.Value ?? string.Empty;

                var index = source.IndexOf(oldValue, comparison);
                if (index == -1)
                {
                    continue;
                }
                if (index < previousIndex)
                {
                    throw new InvalidOperationException(
                        "Replacements must be in the order that they appear in the string. Use a regex if you need to replace elements within a string out-of-linear-order.");
                }
                while (index != -1)
                {
                    builder.Append(source.Substring(previousIndex, index - previousIndex));
                    builder.Append(newValue);
                    index += item.Key.Length;

                    previousIndex = index;
                    index = source.IndexOf(oldValue, index, comparison);
                }
            }
            builder.Append(source.Substring(previousIndex));
            return builder.ToString();
        }
    }
}
