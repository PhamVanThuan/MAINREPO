using ObjectMaps.FloboControls;

namespace BuildingBlocks.Navigation
{
    public class NotesNode : NotesNodeFloBoControls
    {
        public void NotesSummary()
        {
            base.Notes.Click();
            base.NotesSummary.Click();
        }

        public void MaintainNotes()
        {
            base.Notes.Click();
            base.MaintainNotes.Click();
        }
    }
}