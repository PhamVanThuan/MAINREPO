using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core.Exceptions;

namespace BuildingBlocks.Services
{
    public class AssignmentService : _2AMDataHelper, IAssignmentService
    {
        /// <summary>
        /// Gets the next user due to be assigned a case using the Round Robin assignment
        /// </summary>
        /// <param name="offerRoleTypeKey">OfferRoleType of the User being assigned the case</param>
        /// <returns></returns>
        public string GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum offerRoleTypeKey, RoundRobinPointerEnum roundRobinPointer)
        {
            var results = base.GetNextRoundRobinAssignment(offerRoleTypeKey, roundRobinPointer);
            string nextUser = results.Rows(0).Column("username").Value;
            if (nextUser.Length == 0 || nextUser == String.Empty)
                throw new WatiNException("No user found in the Round Robin Assignment");
            results.Dispose();
            return nextUser;
        }

        /// <summary>
        /// Gets a list of round robin users for a given offer and then selects the appropriate next user
        /// </summary>
        /// <param name="offerRoleTypeKey"></param>
        /// <param name="roundRobinPointer"></param>
        /// <returns></returns>
        public string GetNextRoundRobinUser(OfferRoleTypeEnum offerRoleTypeKey, RoundRobinPointerEnum roundRobinPointer)
        {
            var results = base.GetRoundRobinConfiguration(offerRoleTypeKey, roundRobinPointer);
            var maxRRNumber = results.Max(x => x.Column("RRNumber").GetValueAs<int>());
            var isRoundRobinPointerIndexIncrementedCorrect = false;
            var activeRoundRobinUsers = results.Where(x => x.Column("GeneralStatusKey").GetValueAs<int>() == 1 &&
                x.Column("CapitecGeneralStatusKey").GetValueAs<int>() == 1).OrderBy(x => x.Column("RRNumber").GetValueAs<int>()).ToArray();
            int currentRoundRobinPointerIndex = activeRoundRobinUsers.Select(x => x.Column("RoundRobinPointerIndexID").GetValueAs<int>()).First();
            int roundRobinPointerIndexIncremented = currentRoundRobinPointerIndex;

            //this part determines the next round robin pointer index.
            while (!isRoundRobinPointerIndexIncrementedCorrect)
            {
                roundRobinPointerIndexIncremented++;
                isRoundRobinPointerIndexIncrementedCorrect = activeRoundRobinUsers.Where(x => x.Column("RRNumber").GetValueAs<int>() == roundRobinPointerIndexIncremented).Count() > 0;

                if (roundRobinPointerIndexIncremented > maxRRNumber)
                {
                    roundRobinPointerIndexIncremented = activeRoundRobinUsers.FirstOrDefault().Column("RRNumber").GetValueAs<int>();
                    break;
                }
            }
          
            string nextUser = (from r in activeRoundRobinUsers
                               where (Convert.ToInt32(r.Column(0).Value) == roundRobinPointerIndexIncremented)
                               select r.Column("ADUserName").Value).Single();

            if (nextUser.Length == 0 || nextUser == String.Empty)
                throw new WatiNException("No user found in the Round Robin Assignment");
            results.Dispose();
            return nextUser;
        }

        /// <summary>
        /// This method will first check if we can assign a user by performing the group check. If no user can be found for the group
        /// then we will get the next user from the load balance assignment.
        /// </summary>
        /// <param name="wrt">WorkflowRoleType that is being assigned</param>
        /// <param name="workflowName">Workflow Name</param>
        /// <param name="states">Workflow States for the load balance</param>
        /// <param name="incl">TRUE = list of states to include, FALSE = list of states to exclude</param>
        /// <param name="debtCounsellingKey">debtCounsellingKey of the case we are currently assigning</param>
        /// <returns></returns>
        public string GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum wrt, string workflowName, IList<string> states, bool incl, int debtCounsellingKey)
        {
            //first run the group assignment check
            string adUserName = GetDebtCounsellingGroupAssign(wrt, debtCounsellingKey);
            if (string.IsNullOrEmpty(adUserName))
            {
                //we didnt find anyone in the group check.
                adUserName = GetNextLoadBalanceAssignment(wrt, workflowName, states, incl);
            }

            return adUserName;
        }

        /// <summary>
        /// This method will return the next user due to receive a case via a load balance assignment
        /// </summary>
        /// <param name="wrt">WorkflowRoleType</param>
        /// <param name="workflowName">WorkflowName</param>
        /// <param name="states">list of strings containing the states for the load balance</param>
        /// <returns>ADUser to be assigned the case</returns>
        public string GetNextLoadBalanceAssignment(WorkflowRoleTypeEnum wrt, string workflowName, IList<string> states, bool incl)
        {
            //build up the list of state IDs
            string stateIDs = string.Empty;
            //Hack: For if you need to include all workflow states in your load balance query...parse an empty list and set incl to false.
            //This will result in a where clause of "not in (null)" thus including all states in your load balance query
            if (states.Count > 0)
            {
                foreach (string stateName in states)
                {
                    int stateID = base.GetStateIDByName(stateName, workflowName);
                    stateIDs += stateID + ",";
                }

                stateIDs = stateIDs.TrimEnd(',');
            }
            return base.GetLoadBalanceUserAssign(wrt, base.GetMaxWorkflowID(workflowName), stateIDs, incl);
        }

        /// <summary>
        /// Returns the Username of the active WorkflowRoleAssignment record, identified by InstanceID
        /// </summary>
        /// <param name="instanceID">InstanceID</param>
        /// <returns></returns>
        public string GetADUserNameOfActiveWorkflowRoleAssignment(Int64 instanceID)
        {
            return base.GetActiveWorkflowRoleAssignmentByInstance(instanceID).Rows(0).Column(2).Value;
        }

        /// <summary>
        /// Checks if an OfferRoleType has been assigned a WorkflowAssignment for an Offer at a specific State
        /// </summary>
        /// <param name="offerKey">Offer.OfferKey</param>
        /// <param name="stateName">State.Name</param>
        /// <param name="offerRoleTypeKey">OfferRoleType.OfferRoleTypeKey</param>
        /// <returns>true/false</returns>
        public bool OfferRoleTypesAssignedInX2WorkFlowAssignment(int offerKey, string stateName, string offerRoleTypeKey)
        {
            WatiN.Core.Logging.Logger.LogAction(@"Checking if Offer: {0} has a WorkflowAssignment record at State: {1} with an OfferRoleType: {2}", offerKey, stateName, offerRoleTypeKey);
            QueryResults results = base.GetX2WorkFlowAssignment_ByStateName(offerKey, stateName);

            var offerRoleTypes = new string[results.RowList.Count];
            for (int i = 0; i < results.RowList.Count; i++)
            {
                offerRoleTypes[i] = results.Rows(i).Column("OfferRoleTypeKey").Value;
            }
            results.Dispose();
            return offerRoleTypes.Contains<string>(offerRoleTypeKey);
        }

        /// <summary>
        /// Gets the next user due to be assigned a case using the Round Robin assignment
        /// </summary>
        /// <param name="offerRoleTypeKey">OfferRoleType of the User being assigned the case</param>
        /// <returns></returns>
        public string GetNextRoundRobinAssignmentByWorkflowRoleTypeKey(WorkflowRoleTypeEnum workflowRoleTypeKey, RoundRobinPointerEnum roundRobinPointer)
        {
            var results = base.GetNextRoundRobinAssignmentForWorkflowRole(workflowRoleTypeKey, roundRobinPointer);
            string nextUser = results.Rows(0).Column("username").Value;
            if (nextUser.Length == 0 || nextUser == String.Empty)
                throw new WatiNException("No user found in the Round Robin Assignment");
            results.Dispose();
            return nextUser;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="generickey"></param>
        /// <param name="workflowRoleType"></param>
        /// <param name="roundRobinPointer"></param>
        public string GetUserForReactivateOrRoundRobinAssignment(int generickey, WorkflowRoleTypeEnum workflowRoleType, RoundRobinPointerEnum roundRobinPointer)
        {
            var workflowRoles = base.GetActiveWorkflowRoleByTypeAndGenericKeyForRoundRobinAssign(workflowRoleType, generickey);

            if (workflowRoles.Count > 0)
            {
                var aduserName = (from val in workflowRoles.Values
                                  select val).FirstOrDefault();
                return aduserName;
            }
            else
            {
                return this.GetNextRoundRobinAssignmentByWorkflowRoleTypeKey(workflowRoleType, roundRobinPointer);
            }
        }
    }
}