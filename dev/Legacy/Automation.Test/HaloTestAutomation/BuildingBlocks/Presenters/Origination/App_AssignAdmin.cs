using BuildingBlocks.Services.Contracts;
using Common.Enums;
using ObjectMaps.Pages;
using System.Linq;

namespace BuildingBlocks.Presenters.Origination
{
    public class App_AssignAdmin : AssignAdminControls
    {
        private readonly IADUserService adUserService;

        public App_AssignAdmin()
        {
            adUserService = ServiceLocator.Instance.GetService<IADUserService>();
        }

        /// <summary>
        /// When provided with an ADUserName (without the SAHL\ domain e.g BAUser), this method will fetch
        /// the Legal Entity Name from the DB and then select that admin user from the dropdown and add them
        /// to the Application/Lead.
        /// </summary>
        /// <param name="b">An IE TestBrowser  object</param>
        /// <param name="adUserName">The AD User Name of the Branch Admin to be added to the Application/Lead</param>
        public void SelectAdminFromDropdownAndCommit(string adUserName)
        {
            //fetch the AD Users full name for the dropdown
            adUserName = (from results in adUserService.GetLegalEntityNameFromADUserName(adUserName, 0, GeneralStatusEnum.Active)
                          select results.Column(0).Value).FirstOrDefault();
            //select the option from the dropdown
            base.ConsultantDropDown.Option(adUserName).Select();
            base.SubmitButton.Click();
        }
    }
}