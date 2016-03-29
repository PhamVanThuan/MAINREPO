namespace SAHL.Core.UI.Elements
{
    public interface ISelectionContext
    {
        ISelectable CurrentSelection { get; }

        bool IsItemSelected(ISelectable selectableItem);

        void Select(ISelectable itemToSelect);
    }
}