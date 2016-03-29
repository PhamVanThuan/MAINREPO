using BuildingBlocks.Services.Contracts;
using Common.Enums;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination
{
    public class ReassignOriginatingBranchConsultant : ReassignOriginatingBranchConsultantControls
    {
        private readonly IADUserService adUserService;

        public ReassignOriginatingBranchConsultant()
        {
            adUserService = ServiceLocator.Instance.GetService<IADUserService>();
        }

        /// <summary>
        /// This method will reassign the commission earning consultant on an application/lead. The ReAssignBCRole
        /// indicates whether the test from which this is called is required to change the 101 role as well as the 100
        /// role.
        /// </summary>
        /// <param name="b">The IE TestBrowser Object</param>
        /// <param name="adUserName">The ADUserName to be reassigned the 100/101 role</param>
        /// <param name="reAssignBcRole">True=100 and 101 role, False=100 role only</param>
        public void ReassignCommissionConsultant(string adUserName, bool reAssignBcRole)
        {
            //fetch the AD Users full name
            var results = adUserService.GetLegalEntityNameFromADUserName(adUserName, 0, GeneralStatusEnum.Active);
            adUserName = results.Rows(0).Column(0).Value;
            results.Dispose();
            //select the option from the drop down
            base.ddlConsultant.Option(adUserName).Select();
            //provide a memo comment and select the checkbox  if needed
            if (!reAssignBcRole)
            {
                base.txtMemo.TypeText("Reassign Commission Consultant");
            }
            else
            {
                base.txtMemo.TypeText("Reassign Commission Consultant and Branch Consultant");
                base.chkReassignBC.Click();
            }
            //submit the change
            base.btnSubmit.Click();
        }
    }
}