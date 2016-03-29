using System.Collections.Generic;
using System.Web.UI;

namespace SAHL.Internet.Components
{
    public static class ControlCollectionExtensions
    {
        /// <summary>
        /// Appends all children within the specified control collection to the specified control collection result.
        /// </summary>
        /// <param name="controls">The <see cref="ControlCollection"/> to retrieve children from.</param>
        /// <param name="result">The <see cref="ICollection{Control}"/> to append the children to.</param>
        private static void AppendChildren(ControlCollection controls, ICollection<Control> result)
        {
            foreach (Control child in controls)
            {
                controls.Add(child);
                AppendChildren(child.Controls, result);
            }
        }

        /// <summary>
        /// Retrieves a flattened version of the control heirarchy.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerable{Control}"/> containing all the controls within the heirarchy.
        /// </returns>
        public static IEnumerable<Control> Flatten(this ControlCollection controls)
        {
            var result = new List<Control>();
            AppendChildren(controls, result);
            return result;
        }
    }
}
