namespace SAHL.Core.UI.Models
{
    public class UIEditorPageModel : IUIEditorPageModel
    {
        public UIEditorPageModel(IEditorPageModel editorPageModel, bool isFirstPage, bool isLastPage)
        {
            this.EditorPageModel = editorPageModel;
            this.IsFirstPage = isFirstPage;
            this.IsLastPage = isLastPage;
        }

        public IEditorPageModel EditorPageModel { get; protected set; }

        public bool IsLastPage { get; protected set; }

        public bool IsFirstPage { get; protected set; }
    }
}