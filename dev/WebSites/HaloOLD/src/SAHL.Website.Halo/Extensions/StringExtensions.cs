using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SAHL.Website.Halo
{
    public static class StringExtensions
    {
        public static string PutSpacesInBetweenCapitals(this string text)
        {
            Regex regex = new Regex(@"\p{Lu}\p{Ll}*");
            StringBuilder builder = new StringBuilder();
            return String.Join(" ", regex.Matches(text)
                                         .Cast<Match>()
                                         .Select(x => x.Groups[0].Value)
                                         .ToArray());
        }

        internal static string CreateHtmlCheckBoxItemFromSelectListItem(this string checkboxName, SelectListItem selectListItem)
        {
            var output = new StringBuilder();

            output.Append(@"<input type=""checkbox"" name=""");
            output.Append(checkboxName + "_" + selectListItem.Value);
            output.Append("\" value=\"");
            output.Append(selectListItem.Value);
            output.Append("\"");

            if (selectListItem.Selected)
                output.Append(@" checked=""checked""");

            output.Append(" /> ");
            output.Append(selectListItem.Text);
            output.Append("<br />");

            return output.ToString();
        }
    }
}