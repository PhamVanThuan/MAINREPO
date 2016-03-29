using ObjectMaps;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_NTU : LifeNTUControls
    {
        public void NTULifePolicy(string Reason)
        {
            base.ddlNTUReason.Option(Reason).Select();
            CommonIEDialogHandler.SelectOK(base.btnSubmit);
            base.Document.DomContainer.WaitForComplete();
        }
    }
}