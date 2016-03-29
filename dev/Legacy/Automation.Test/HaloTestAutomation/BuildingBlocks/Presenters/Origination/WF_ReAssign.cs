using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using ObjectMaps.Pages;
using WatiN.Core;

namespace BuildingBlocks.Presenters.Origination
{
    /// <summary>
    /// Contains building blocks for the Workflow Reassign Screen.
    /// </summary>
    public class WF_ReAssign : WF_ReAssignControls
    {
        private readonly IADUserService adUserService;

        public WF_ReAssign()
        {
            adUserService = ServiceLocator.Instance.GetService<IADUserService>();
        }

        /// <summary>
        /// Reassigns a case between AD Users by providing the currently assigned AD User and a new AD User.
        /// </summary>
        /// <param name="adUserNameAssigned">Currently assigned ADUser</param>
        /// <param name="adUserNameReAssign">ADUser to be reassigned the case</param>
        public void SelectUserRoleAndConsultantFromDropdownAndCommit(string adUserNameAssigned, string adUserNameReAssign)
        {
            QueryResults results = adUserService.GetLegalEntityNameFromADUserName(adUserNameAssigned, 0, GeneralStatusEnum.Active);
            if (!results.HasResults)
            {
                results = adUserService.GetLegalEntityNameFromADUserName(adUserNameAssigned, 0, GeneralStatusEnum.Inactive);
            }
            adUserNameAssigned = results.Rows(0).Column(0).Value;
            results.Dispose();
            //get the user we are assigning to
            results = adUserService.GetLegalEntityNameFromADUserName(adUserNameReAssign, 0, GeneralStatusEnum.Active);
            adUserNameReAssign = results.Rows(0).Column(0).Value;
            results.Dispose();

            adUserNameAssigned = adUserNameAssigned.ToLower();
            adUserNameReAssign = adUserNameReAssign.ToLower();
            base.ddlRole.FireEvent("onkeypress");
            //select the role option from the dropdown
            string option;
            //Loop through every option in the dropdown
            foreach (Option o in base.ddlRole.Options)
            {
                //Set everything to lowercase for string matches
                option = o.Text.ToLower();
                //if option contains the adusername somewhere in the string enter
                if (option.Contains(adUserNameAssigned))
                {
                    o.Select();
                    break;
                }
            }
            base.ddlConsultant.WaitUntilExists();
            //select the consultant option that belongs to the role from the dropdown
            //Loop through every option in the dropdown
            foreach (Option o2 in base.ddlConsultant.Options)
            {
                //Set everything to lowercase for string matches
                option = o2.Text.ToLower();
                //if option contains the adusername to reassign to then enter enter
                if (option.Contains(adUserNameReAssign))
                {
                    o2.Select();
                    break;
                }
            }
            //Continue with ReAssign
            base.btnSubmit.Click();
        }

        /// <summary>
        /// An overload that for reassign presenters that require the user to select the Role Type the dropdown
        /// </summary>
        /// <param name="reassignToUser">ADuser to be reassigned the case</param>
        /// <param name="roleType">Currently assigned Role Type</param>
        public void SelectRoleAndConsultantFromDropdownAndCommit(string reassignToUser, string roleType)
        {
            var results = adUserService.GetLegalEntityNameFromADUserName(reassignToUser, 0, GeneralStatusEnum.Active);
            reassignToUser = results.Rows(0).Column(0).Value;
            results.Dispose();
            reassignToUser = reassignToUser.ToLower();
            roleType = roleType.ToLower();
            base.ddlRole.FireEvent("onkeypress");
            //select the role option from the dropdown
            string option;
            //Loop through every option in the dropdown
            foreach (Option o in base.ddlRole.Options)
            {
                //Set everything to lowercase for string matches
                option = o.Text.ToLower();
                //if option contains the adusername somewhere in the string enter
                if (option.Contains(roleType))
                {
                    o.Select();
                    break;
                }
            }
            base.ddlConsultant.WaitUntilExists();
            //select the consultant option that belongs to the role from the dropdown
            //Loop through every option in the dropdown
            foreach (Option o2 in base.ddlConsultant.Options)
            {
                //Set everything to lowercase for string matches
                option = o2.Text.ToLower();
                //if option contains the adusername to reassign to then enter enter
                if (option.Contains(reassignToUser))
                {
                    o2.Select();
                    break;
                }
            }
            //Continue with ReAssign
            base.btnSubmit.Click();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="indexReassignToUser"></param>
        /// <param name="indexRoleType"></param>
        public void SelectRoleAndConsultantFromDropdownAndCommit(int indexReassignToUser, int indexRoleType)
        {
            base.ddlRole.FireEvent("onkeypress");
            //select the role option from the dropdown
            base.ddlRole.Options[indexRoleType].Select();
            base.ddlConsultant.WaitUntilExists();
            //select the consultant option that belongs to the role from the dropdown
            base.ddlConsultant.Options[indexReassignToUser].Select();
            //Continue with ReAssign
            base.btnSubmit.Click();
        }

        /// <summary>
        /// Reassigns a case between AD Users by providing the new AD User.
        /// </summary>
        /// <param name="reassignToUser">ADuser to be reassigned the case</param>
        public void SelectConsultantFromDropdownAndCommit(string reassignToUser)
        {
            var results = adUserService.GetLegalEntityNameFromADUserName(reassignToUser, 0, GeneralStatusEnum.Active);
            reassignToUser = results.Rows(0).Column(0).Value;
            results.Dispose();
            reassignToUser = reassignToUser.ToLower();
            string option;
            foreach (Option o2 in base.ddlConsultant.Options)
            {
                //Set everything to lowercase for string matches
                option = o2.Text.ToLower();
                //if option contains the adusername to reassign to then enter enter
                if (option.Contains(reassignToUser))
                {
                    o2.Select();
                    break;
                }
            }
            //Continue with ReAssign
            base.btnSubmit.Click();
        }
    }
}