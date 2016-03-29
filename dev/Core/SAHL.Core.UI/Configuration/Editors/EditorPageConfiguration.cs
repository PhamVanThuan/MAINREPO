using SAHL.Core.UI.Models;
using System;

namespace SAHL.Core.UI.Configuration.Editors
{
    public class EditorPageConfiguration<T> : IEditorPageConfiguration<T>
        where T : IEditorPageModel
    {
        public EditorPageConfiguration()
        {
            this.EditorPageModelType = typeof(T);
        }

        public Type EditorPageModelType
        {
            get;
            protected set;
        }
    }
}