namespace SAHL.Core.UI.Models
{
    public interface IUIEditorPageModel
    {
        IEditorPageModel EditorPageModel { get; }

        bool IsLastPage { get; }

        bool IsFirstPage { get; }
    }
}