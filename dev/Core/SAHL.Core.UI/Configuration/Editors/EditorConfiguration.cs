using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Elements.Editors;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Models;
using System;

namespace SAHL.Core.UI.Configuration.Editors
{
    public class EditorConfiguration<T> : IEditorConfiguration<T> where T : IEditor
    {
        public EditorConfiguration(string editorTitle, EditorAction editorAction)
        {
            this.EditorTitle = editorTitle;
            this.EditorModelType = typeof(T);
            this.EditorAction = editorAction;
        }

        public Type EditorModelType { get; protected set; }

        public string EditorTitle { get; protected set; }

        public EditorAction EditorAction { get; protected set; }

        public EditorElement CreateElement(BusinessContext businessContext)
        {
            return new EditorElement(businessContext, this.EditorModelType, this.GetType(), this.EditorTitle, this.EditorAction);
        }
    }
}