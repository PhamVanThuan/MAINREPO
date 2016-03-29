using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.Presenters.PersonalLoans
{
    public abstract class PersonalLoanLeadControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlLegalEntityTypes")]
        public SelectList ddlLeadType { get; set; }

        [FindBy(Id = "ctl00_Main_ddlRoleTypeAdd")]
        public SelectList ddlLeadRole { get; set; }
    }
}