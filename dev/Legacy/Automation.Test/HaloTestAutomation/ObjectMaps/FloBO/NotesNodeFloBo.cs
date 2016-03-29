using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.FloboControls
{
    public abstract class NotesNodeFloBoControls : BaseNavigation
    {
        [FindBy(Title = "Notes")]
        protected Link Notes { get; set; }

        [FindBy(Title = "Notes Summary")]
        protected Link NotesSummary { get; set; }

        [FindBy(Title = "Maintain Notes")]
        protected Link MaintainNotes { get; set; }
    }
}