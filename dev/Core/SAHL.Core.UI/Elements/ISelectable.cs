namespace SAHL.Core.UI.Elements
{
    public interface ISelectable
    {
        bool Selected { get; }

        ISelectionContext SelectionContext { get; set; }
    }
}