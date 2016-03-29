using System;

namespace SAHL.Core.UI.Models
{
    public class UIEditorPageTypeModel
    {
        public UIEditorPageTypeModel(Type editorPageModelType, bool isFirstPage, bool isLastPage)
        {
            this.EditorPageModelType = editorPageModelType;
            this.IsFirstPage = isFirstPage;
            this.IsLastPage = isLastPage;
        }

        public Type EditorPageModelType { get; protected set; }

        public bool IsLastPage { get; protected set; }

        public bool IsFirstPage { get; protected set; }
    }
}