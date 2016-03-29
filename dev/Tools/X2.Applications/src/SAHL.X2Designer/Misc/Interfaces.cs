using Northwoods.Go;

namespace SAHL.X2Designer.Misc
{
    public interface ICutCopyPasteTarget
    {
        void DoCut();

        void DoCopy();

        void DoPaste();

        void DoUndo();

        void DoRedo();

        void DoPrint();

        void DoSelectAll();

        void DoDelete();
    }

    public interface IPopupMenu
    {
        void populateMenu(GoContextMenu e);

        void OnMenuClosed(GoContextMenu e);
    }
}