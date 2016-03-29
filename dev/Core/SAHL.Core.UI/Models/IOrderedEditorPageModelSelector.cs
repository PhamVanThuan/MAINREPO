namespace SAHL.Core.UI.Models
{
    public interface IOrderedEditorPageModelSelector<T> : IEditorPageModelSelector<T>
        where T : IEditor
    {
    }
}