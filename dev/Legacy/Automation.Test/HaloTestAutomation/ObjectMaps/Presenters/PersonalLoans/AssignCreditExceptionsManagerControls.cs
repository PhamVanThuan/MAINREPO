using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.Presenters.PersonalLoans
{
    public abstract class AssignCreditExceptionsManagerControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlExceptionsManager")]
        public SelectList ddlExceptionsManager { get; set; }
    }
}