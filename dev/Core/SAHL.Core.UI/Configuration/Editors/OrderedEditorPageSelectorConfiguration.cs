using SAHL.Core.UI.Models;
using System;
using System.Linq;

namespace SAHL.Core.UI.Configuration.Editors
{
    public class OrderedEditorPageSelectorConfiguration<T> : AbstractEditorPageSelectorConfiguration<T>, IOrderedEditorPageSelectorConfiguration<T>
        where T : IEditorConfiguration
    {
        public override Type PageSelectorType
        {
            get
            {
                var editorConfigType = typeof(T).GetInterfaces().Where(x => x.IsGenericType && x.Name.StartsWith("IEditorConfiguration")).SingleOrDefault();
                var editorType = editorConfigType.GetGenericArguments().First();
                if (editorType != null)
                {
                    Type openGenericOrderedPageSelector = typeof(OrderedEditorPageModelSelector<>);
                    Type genericOrderedPageSelector = openGenericOrderedPageSelector.MakeGenericType(editorType);
                    return genericOrderedPageSelector;
                }
                return null;
            }
        }
    }
}