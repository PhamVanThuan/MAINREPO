namespace SAHL.Core.UI.Models
{
    public interface IEditorPageModelSelector
    {
        UIEditorPageTypeModel GetFirstPage();

        UIEditorPageTypeModel GetNextPage(IEditorPageModel currentPageModel);

        UIEditorPageTypeModel GetPreviousPage(IEditorPageModel currentPageModel);
    }

    public interface IEditorPageModelSelector<T> : IEditorPageModelSelector where T : IEditor
    {
        void Initialise(T editor);
    }
}