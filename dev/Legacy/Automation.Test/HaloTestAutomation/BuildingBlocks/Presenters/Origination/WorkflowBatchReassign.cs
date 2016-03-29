using BuildingBlocks.Services.Contracts;
using Common.Extensions;
using ObjectMaps.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;

namespace BuildingBlocks.Presenters.Origination
{
    public class WorkflowBatchReassign : WorkflowBatchReassignControls
    {
        private readonly IAssignmentService assignmentService;
        private readonly ILegalEntityService legalEntityService;
        private readonly IADUserService adUserService;

        public WorkflowBatchReassign()
        {
            assignmentService = ServiceLocator.Instance.GetService<IAssignmentService>();
            legalEntityService = ServiceLocator.Instance.GetService<ILegalEntityService>();
            adUserService = ServiceLocator.Instance.GetService<IADUserService>();
        }

        /// <summary>
        /// Batch Reassign Offers
        /// </summary>
        /// <param name="roleType">Option in "Role Type" select list</param>
        /// <param name="reassignUser">Option in "Reassign to User" select list</param>
        /// <param name="numberOfOffers">Number of Offers to reassign</param>
        /// <param name="workflowMapName"></param>
        /// <param name="excludeOffers">Offers not to be reassigned</param>
        public List<int> BatchReassign(string roleType, string reassignUser, int numberOfOffers, string workflowMapName, params string[] excludeOffers)
        {
            List<int> keys = new List<int>();
            roleType = roleType.TrimEnd('D').Trim();

            string usernameToExclude = reassignUser;

            var enabledRoleTypeOption = (from roleOption in base.ddlRoleType.Options
                                         where roleOption.Enabled
                                            && roleOption.Text.Equals(roleType)
                                         select roleOption).First();
            enabledRoleTypeOption.Select();

            //Only want the enabled assign user options  and where the users is not the reassign user
            var filteredOptions = from option in base.ddlSearchUser.Options
                                  where !String.IsNullOrEmpty(option.Value)
                                            && !option.Value.Contains("select")
                                  select option;
            //can we find someone with all the required number of cases?
            var enumerable = filteredOptions as IList<Option> ?? filteredOptions.ToList();
            var enabledAssignUserOption = (from option in enumerable
                                           where option.Enabled
                                             && !option.Value.Equals(usernameToExclude)
                                             && assignmentService.GetWorklistDetails(option.Value, workflowMapName).Count() >= numberOfOffers
                                           select option).First();
            //find someone with a case in their worklist to use
            if (enabledAssignUserOption == null)
            {
                enabledAssignUserOption = (from option in enumerable
                                           where option.Enabled
                                             && !option.Value.Equals(usernameToExclude)
                                             && assignmentService.GetWorklistDetails(option.Value, workflowMapName).Count() > 0
                                           select option).First();
            }

            reassignUser = reassignUser.RemoveDomainPrefix();
            enabledAssignUserOption.Select();
            base.btnSearch.Click();

            if (base.SearchGrid.Exists)
            {
                for (int i = 1; i < base.SearchGridRows.Count; i++)
                {
                    if (!excludeOffers.Contains(base.SearchGridRowCells(i)[2].Text.Trim()))
                    {
                        //first check if the select box is enabled. NTU/Decline applications cannot be reassigned so the checkbox might be disabled.
                        if (base.chkSelect(i).Enabled)
                        {
                            base.chkSelect(i).Checked = true;
                            keys.Add(Convert.ToInt32(base.SearchGridRowCells(i)[2].Text.Trim()));
                            if (keys.Count() == numberOfOffers)
                            {
                                break;
                            }
                        }
                    }
                }
                var option = (from o in base.ddlReassignUser.Options where o.Enabled && o.Text.Contains(reassignUser) select o).First();
                option.Select();
                base.btnReassignLeads.Click();
            }
            return keys;
        }

        /// <summary>
        /// This method will batch reassign a specific case
        /// </summary>
        /// <param name="roleType"></param>
        /// <param name="key"></param>
        /// <param name="currentOwner"></param>
        /// <param name="workflowMapName"></param>
        /// <param name="newOwner"></param>
        /// <returns></returns>
        public bool BatchReassignSpecificCase(string roleType, int key, string currentOwner, string workflowMapName, string newOwner)
        {
            roleType = roleType.TrimEnd('D').Trim();
            base.ddlRoleType.Option(roleType).Select();

            var enabledRoleTypeOption = (from roleOption in base.ddlRoleType.Options
                                         where roleOption.Enabled && roleOption.Text.Equals(roleType)
                                         select roleOption).FirstOrDefault();
            enabledRoleTypeOption.Select();

            //Only want the enabled assign user options  and where the users is not the reassign user
            var filteredOptions = from option in base.ddlSearchUser.Options
                                  where !String.IsNullOrEmpty(option.Value)
                                            && !option.Value.Contains("select")
                                  select option;
            var enabledAssignUserOption = (from option in filteredOptions
                                           where option.Enabled && option.Value.Equals(currentOwner)
                                           select option).FirstOrDefault();
            enabledAssignUserOption.Select();

            base.btnSearch.Click();

            if (base.SearchGrid.Exists)
            {
                for (int i = 1; i < base.SearchGridRows.Count; i++)
                {
                    if (Convert.ToInt32(base.SearchGridRowCells(i)[2].Text.Trim()) == key)
                    {
                        //first check if the select box is enabled. NTU/Decline applications cannot be reassigned so the checkbox might be disabled.
                        if (base.chkSelect(i).Enabled)
                            base.chkSelect(i).Checked = true;
                    }
                }
                newOwner = newOwner.RemoveDomainPrefix();
                var option = (from o in base.ddlReassignUser.Options where o.Enabled && o.Text.Contains(newOwner) select o).FirstOrDefault();
                option.Select();
                base.btnReassignLeads.Click();
                return true;
            }
            return false;
        }
    }
}