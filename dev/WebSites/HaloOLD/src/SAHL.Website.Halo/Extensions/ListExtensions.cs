using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SAHL.Website.Halo
{
    public static class ListExtensions
    {
        public static IEnumerable<SelectListItem> ToCheckBoxListSource(this IEnumerable<KeyValuePair<int, string>> allCollection, IEnumerable<int> checkedCollection = null)
        {
            var result = new List<SelectListItem>();

            foreach (KeyValuePair<int, string> item in allCollection)
            {
                SelectListItem selectItem = new SelectListItem();
                selectItem.Text = item.Value;
                selectItem.Value = item.Key.ToString();
                selectItem.Selected = checkedCollection != null ? checkedCollection.Contains(item.Key) : false;
                result.Add(selectItem);
            }
            return result;
        }
    }
}