using SAHL.Core.UI.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.UI.Models
{
    public class OrderedEditorPageModelSelector<T> : IOrderedEditorPageModelSelector<T>, IEditorPageModelSelector<T>
        where T : IEditor
    {
        private List<Type> orderedPages;

        public OrderedEditorPageModelSelector(IParentedEditorPageConfiguration<IEditorConfiguration<T>>[] pageConfigurations)
        {
            this.orderedPages = new List<Type>();

            var pages = new List<IEditorPageOrderedConfiguration>(pageConfigurations.OfType<IEditorPageOrderedConfiguration>().OrderBy(x => x.Sequence));

            // for each ordered page get the generic IEditorPageMode<T> and store the type of T
            foreach (var page in pages)
            {
                var editorPageConfigType = page.GetType().GetInterfaces().Where(x => x.IsGenericType && x.Name.StartsWith("IEditorPageConfiguration")).SingleOrDefault();
                var editorPageModelType = editorPageConfigType.GetGenericArguments().First();
                if (editorPageModelType != null)
                {
                    this.orderedPages.Add(editorPageModelType);
                }
            }
        }

        public virtual void Initialise(T editor)
        {
            // No base class implemenation required
        }

        public virtual UIEditorPageTypeModel GetFirstPage()
        {
            return new UIEditorPageTypeModel(orderedPages.FirstOrDefault(), true, this.orderedPages.Count == 1);
        }

        public virtual UIEditorPageTypeModel GetNextPage(IEditorPageModel currentPageModel)
        {
            int index = this.orderedPages.IndexOf(currentPageModel.GetType());
            if (index != -1 && index < this.orderedPages.Count - 1)
            {
                return new UIEditorPageTypeModel(this.orderedPages[++index], false, ++index == this.orderedPages.Count);
            }
            return null;
        }

        public virtual UIEditorPageTypeModel GetPreviousPage(IEditorPageModel currentPageModel)
        {
            int index = this.orderedPages.IndexOf(currentPageModel.GetType());
            if (index > 0 && index < this.orderedPages.Count)
            {
                return new UIEditorPageTypeModel(this.orderedPages[--index], index == 0, false);
            }
            return null;
        }
    }
}