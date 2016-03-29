using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;
using WatiN.Core.Logging;
using WatiN.Core.UtilityClasses;

namespace BuildingBlocks.Assertions
{
    /// <summary>
    /// contains assertions for the WorkflowRole and WorkflowRoleAssignments
    /// </summary>
    public static class WorkflowRoleAssignmentAssertions
    {
        private static readonly IAssignmentService assignmentService;
        private static readonly IX2WorkflowService x2Service;
        private static readonly IApplicationService applicationService;
        private static readonly IADUserService aduserService;

        static WorkflowRoleAssignmentAssertions()
        {
            assignmentService = ServiceLocator.Instance.GetService<IAssignmentService>();
            x2Service = ServiceLocator.Instance.GetService<IX2WorkflowService>();
            applicationService = ServiceLocator.Instance.GetService<IApplicationService>();
            aduserService = ServiceLocator.Instance.GetService<IADUserService>();
        }

        /// <summary>
        /// Ensures only one active WorkflowRole record exists for an ADUser on a case
        /// </summary>
        /// <param name="genericKey">GenericKey</param>
        /// <param name="wfRoleType">WorkflowRoleType</param>
        /// <param name="adUserName">ADUserName</param>
        public static void AssertActiveWorkflowRoleExists(int genericKey, WorkflowRoleTypeEnum wfRoleType, string adUserName)
        {
            var results = assignmentService.GetActiveWorkflowRoleByAdUserAndType(genericKey, wfRoleType, adUserName);
            //if there are no results then Fail
            Logger.LogAction(string.Format("Asserting that an single active workflow role of type {0} exists for {1} against ADUser {2}", wfRoleType.ToString(),
                    genericKey, adUserName));

            Assert.True(results.HasResults, string.Format(@"There are no active workflow role records for {0} against generic key {1}", adUserName, genericKey));
            Assert.True(results.RowList.Count == 1, "More then one active WorkflowRole exists");
        }

        /// <summary>
        /// Assert that the latest workflow role assignment record has been correctly written for an instance
        /// </summary>
        /// <param name="instanceid">Instance ID</param>
        /// <param name="adUserName">Expected ADUserName of latest record</param>
        /// <param name="workflowRoleType">Expected Workflow Role Type</param>
        /// <param name="checkAllOtherRecordsAreInactive">TRUE = all other workflow role assignment records are expected to be INACTIVE</param>
        public static void AssertLatestDebtCounsellingAssignment(Int64 instanceid, string adUserName, WorkflowRoleTypeEnum workflowRoleType, bool checkAllOtherRecordsAreInactive, bool checkForWorklistEntry)
        {
            //we need to check if its a court case
            var rs = x2Service.GetDebtCounsellingInstanceDetails(instanceID: instanceid);
            if (rs.Rows(0).Column("CourtCase").GetValueAs<string>().Equals("True") && workflowRoleType == WorkflowRoleTypeEnum.DebtCounsellingConsultantD)
            {
                workflowRoleType = WorkflowRoleTypeEnum.DebtCounsellingCourtConsultantD;
            }
            rs.Dispose();
            var results = assignmentService.GetWorkflowRoleAssignmentByInstance(instanceid);
            //check we have results
            Logger.LogAction(@"Asserting WorkflowAssignment records for instance " + instanceid);
            if (!results.HasResults)
            {
                Assert.Fail("No Workflow Role Assignment records found. WorkflowRoleType: {0}. InstanceID: {1}", workflowRoleType, instanceid);
            }
            for (int i = 0; i < 1; i++)
            {
                Logger.LogAction(@"Asserting the latest WorkflowAssignment record is assigned to " + adUserName);
                //check latest record assigned to correct user and WorkflowRoleType
                StringAssert.AreEqualIgnoringCase(adUserName, results.Rows(i).Column("adusername").Value,
                string.Format(@"Latest Workflow Role Assignment record is not assigned to the correct user. Expected {0} but was {1}. WorkflowRoleType: {2}. InstanceID: {3}", adUserName,
                results.Rows(i).Column("adusername").Value, workflowRoleType, instanceid));

                Logger.LogAction(@"Asserting the latest WorkflowAssignment is assigned to WorkflowRoleTypeKey " + workflowRoleType.ToString());
                if (results.Rows(i).Column("workflowRoleTypeKey").GetValueAs<int>() != (int)workflowRoleType)
                {
                    Assert.Fail("Latest Workflow Role Assignment record is not assigned to the correct workflow role type. WorkflowRoleType: {0}. InstanceID: {1}", workflowRoleType, instanceid);
                }
                //check that the latest WorkflowRoleAssignment record is active
                Logger.LogAction(@"Asserting the latest WorkflowAssignment record is Active");
                if (results.Rows(i).Column("GeneralStatusKey").GetValueAs<int>() != (int)GeneralStatusEnum.Active)
                {
                    Assert.Fail("Latest Workflow Assignment Record is not set to Active. InstanceID: {0}", instanceid);
                }
            }

            if (checkAllOtherRecordsAreInactive)
            {
                //check all other records are inactive
                for (int i = 1; i < results.RowList.Count; i++)
                {
                    if (results.Rows(i).Column("GeneralStatusKey").GetValueAs<int>() != (int)GeneralStatusEnum.Inactive)
                    {
                        Assert.Fail("Another workflow role assignment record other than the latest is set to active. InstanceID: {0}", instanceid);
                    }
                }
            }
            if (checkForWorklistEntry)
            {
                string worklistAdUserName = string.Empty;
                bool b = false;
                QueryResults r;
                var timer = new SimpleTimer(TimeSpan.FromSeconds(5));
                while (!timer.Elapsed)
                {
                    r = x2Service.GetWorklistDetails(instanceid);
                    if (r.HasResults)
                    {
                        worklistAdUserName = r.Rows(0).Column("ADUserName").Value;
                        b = true;
                        break;
                    }
                }
                //lastly we need to get the worklist entry and check first that it exists. if it does exist then we need to check the correct ADUser has been assigned.
                if (!b)
                {
                    Assert.Fail(string.Format(@"No worklist entry exists for instance {0}", instanceid));
                }
                else
                {
                    StringAssert.AreEqualIgnoringCase(worklistAdUserName, adUserName,
                    string.Format(@"Worklist entry for instance {0} is not assigned to {1}", instanceid, adUserName));
                }
                results.Dispose();
            }
        }

        /// <summary>
        /// This method will check if the offer has a user that plays an active offer role of the type provided. If that user is still active,
        /// has an active ADUser status and is still actively mapped to the organisation structure that the offer role is mapped to then we should
        /// be reactivating that user. Failing this, we will then go and find the next user to be assigned from the round robin assignment.
        /// </summary>
        /// <param name="offerKey">offerKey</param>
        /// <param name="offerRoleType">OfferRoleType being assigned/reactivated</param>
        /// <param name="roundRobinPointer">RoundRobinPointer being used.</param>
        public static void AssertUserReactivatedOrRoundRobinAssigned(int offerKey, OfferRoleTypeEnum offerRoleType, RoundRobinPointerEnum roundRobinPointer)
        {
            var adUser = (from o in applicationService.GetActiveOfferRolesByOfferRoleType(offerKey, offerRoleType)
                          where o.Column("OfferRoleStatus").GetValueAs<int>() == (int)GeneralStatusEnum.Active
                          && o.Column("ADUserStatus").GetValueAs<int>() == (int)GeneralStatusEnum.Active
                          && o.Column("UOSStatus").GetValueAs<int>() == (int)GeneralStatusEnum.Active
                          select o).FirstOrDefault();
            //if we have found an active offer role
            string adUserToAssignRoleTo = adUser != null ? adUser.Column("ADUserName").GetValueAs<string>()
                                                : assignmentService.GetNextRoundRobinAssignmentByOfferRoleTypeKey(offerRoleType, roundRobinPointer);
            //check the workflow assignment
            AssignmentAssertions.AssertWorkflowAssignment(adUserToAssignRoleTo, offerKey, offerRoleType);
        }

        /// <summary>
        /// Asserts that the WorkflowAssignment record of an instance, identified by the State of a particular Offer,  is
        /// Active/Inactive and assigned to the correct ADUser for the correct OfferRoleType.
        /// </summary>
        /// <param name="offerKey">Offer.OfferKey</param>
        /// <param name="stateName">State.Name(nullable)  If Null all WorkflowAssignment records for this offer will be checked</param>
        /// <param name="adUserName">ADUser.ADUserName(nullable) If Null the ADUserName will not be validated</param>
        /// <param name="generalStatus">Expected WorkflowAssignment.GeneralStatusKey of the latest WorkflowAssignment records for an instance.  GeneralStatus = 1
        /// validates that for each specified OfferRoleTypeKey an Active record exists and that no other Active Workflow records exist. GeneralStatus = 2
        /// validates that all Workflow records are InActive</param>
        /// <param name="offerRoleTypeKeys">OfferRoleType.OfferRoleTypeKey(nullable)If you need to check that more than one WorkflowAssignment record was
        /// inserted (e.g. On Create Followup) specify both/all new OfferRoleTypeKeys that you expect to be inserted.  When checking more than one
        /// OfferRoleTypeKey, the ADUserName is not checked.</param>
        public static void AssertWorkFlowAssignmentRecordOfferRoleAssignment(int offerKey, string stateName, string adUserName, int generalStatus,
            params string[] offerRoleTypeKeys)
        {
            List<string> offerRoleTypeKeysList = new List<string>();
            offerRoleTypeKeysList.AddRange(offerRoleTypeKeys);
            QueryResults dbResults = assignmentService.GetX2WorkFlowAssignment_ByStateName(offerKey, stateName);
            bool testResult = true;
            string errorMessage = "Fail";

            Logger.LogAction(@"Asserting WorkflowAssignment rocords exist for OfferKey: {0}, StateName: {1}", offerKey, stateName);
            if (!dbResults.HasResults)
            {
                testResult = false;
                errorMessage = "No WorkflowAssignment rocords exist for OfferKey: " + offerKey + " StateName: " + stateName;
            }

            for (int i = 0; i < dbResults.RowList.Count; i++)
            {
                if (i < offerRoleTypeKeys.Length && generalStatus == 1)
                {
                    Logger.LogAction(@"Asserting the latest WorkflowAssignment record for OfferKey: {0}, StateName: {1}, has a GeneralStatusKey = {2}", offerKey, stateName,
                        generalStatus);
                    if (dbResults.Rows(i).Column("GeneralStatusKey").Value != generalStatus.ToString())
                    {
                        testResult = false;
                        errorMessage =
                            "The latest WorkflowAssignment record for OfferKey: " + offerKey + @", StateName: " + stateName + @", does not have a GeneralStatusKey = " + generalStatus;
                        break;
                    }
                    if (offerRoleTypeKeys != null)
                    {
                        string offerRoleTypesString = "";
                        for (int j = 0; j < offerRoleTypeKeys.Length; j++)
                        {
                            if (j == 0) offerRoleTypesString = offerRoleTypesString + offerRoleTypeKeys[j];
                            else offerRoleTypesString = offerRoleTypesString + ", " + offerRoleTypeKeys[j];
                        }
                        Logger.LogAction(
                        @"Asserting that an Active WorkflowAssignment record for OfferKey: {0}, StateName: {1}, is assigned to OfferRoleType/s: {2}",
                        offerKey, stateName, offerRoleTypesString);
                        if (offerRoleTypeKeysList.Contains(dbResults.Rows(i).Column("OfferRoleTypeKey").Value))
                        {
                            // Removing the OfferRoleTypeKey once it has been validated against, to ensure only one Workflow record is validated as Active
                            // for an OfferRoleType and any subsequent active Workflow records of this OfferRoleType will cause the test to fail
                            for (int k = 0; k < offerRoleTypeKeys.Length; k++)
                            {
                                if (offerRoleTypeKeys[k] == dbResults.Rows(i).Column("OfferRoleTypeKey").Value) offerRoleTypeKeys[k].Remove(0);
                            }
                        }
                        else
                        {
                            testResult = false;
                            errorMessage = "An Active WorkflowAssignment record does not exist for or has not been assigned to OfferRoleType/s: " + offerRoleTypesString;
                            break;
                        }
                    }
                    // Only validate ADUserName when only 1 OfferRoleTypeKey is specified
                    if (adUserName != null && offerRoleTypeKeys.Length == 1)
                    {
                        Logger.LogAction(@"Asserting the latest WorkflowAssignment record for OfferKey: {0}, StateName: {1}, is assigned to ADUser: {2}",
                            offerKey, stateName, adUserName);
                        if (dbResults.Rows(i).Column("ADUserName").Value != adUserName)
                        {
                            testResult = false;
                            errorMessage = "The latest WorkflowAssignment record was not assigned to ADUser: " + adUserName;
                            break;
                        }
                    }
                }
                else
                {
                    Logger.LogAction(
                    @"Asserting that historical WorkflowAssignment records for OfferKey: {0}, StateName: {1}, have a GeneralStatusKey = 2 (InActive)", offerKey, stateName);
                    if (dbResults.Rows(i).Column("GeneralStatusKey").Value != "2")
                    {
                        testResult = false;
                        errorMessage =
                        @"A Historical WorkflowAssignment record for OfferKey: " + offerKey + @", StateName: " + stateName + @", has a GeneralStatusKey = 1 (Active)";
                        break;
                    }
                }
            }
            Assert.True(testResult, "{0}", errorMessage);
        }

        /// <summary>
        /// Asserts workflow role assignment
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="genericKey"></param>
        /// <param name="workflowRoleType"></param>
        /// <param name="expectedUser"></param>
        /// <param name="workflowState"></param>
        /// <param name="workflow"></param>
        public static void AssertWorkflowRoleAssignment(Int64 instanceID, int genericKey, WorkflowRoleTypeEnum workflowRoleType, string expectedUser,
            string workflowState, string workflow)
        {
            //check workflow role assignment in X2
            var results = x2Service.GetActiveWorkflowRoleAssignmentByInstance(instanceID);
            var exists = (from r in results
                          where r.Column("adusername").GetValueAs<string>() == expectedUser
                              && r.Column("workflowroletypekey").GetValueAs<int>() == (int)workflowRoleType
                          select r).FirstOrDefault();
            Assert.That(exists != null, "Expected workflow case generickey:{0} to be assigned to user:{1} that belongs to workflow role:{2}", genericKey, expectedUser, workflowRoleType);
            //check worklist assigmnent
            X2Assertions.AssertWorklistOwner(genericKey, workflowState, workflow, expectedUser);
            //check workflow role table in 2am
            var workflowRole = assignmentService.GetActiveWorkflowRoleByTypeAndGenericKey(workflowRoleType, genericKey);
            //get user
            var aduser = (from w in workflowRole select w.Value).FirstOrDefault();
            Assert.AreEqual(expectedUser, aduser);
        }
    }
}