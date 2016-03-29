using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace BuildingBlocks.Assertions
{
    public static class AssignmentAssertions
    {
        private static IApplicationService applicationService;
        private static IAssignmentService assignmentService;
        private static IX2WorkflowService x2Service;

        static AssignmentAssertions()
        {
            applicationService = ServiceLocator.Instance.GetService<IApplicationService>();
            assignmentService = ServiceLocator.Instance.GetService<IAssignmentService>();
            x2Service = ServiceLocator.Instance.GetService<IX2WorkflowService>();
        }

        /// <summary>
        /// Queries the db for all Workflow Assignment Records for a specified offer and related to
        /// a specified OfferRoleType.  If the Query brings back results, the Assertion equates to true
        /// </summary>
        /// <param name="OfferKey">OfferKey e.g. 767421</param>
        /// <param name="expectedOfferRoleTypeKey">The expected OfferRoleTypeKey e.g. on case creation the 101 = Branch Consultant D OfferRoleType is expected</param>
        public static void AssertThatAWorkFlowAssignmentRecordExists(int OfferKey, OfferRoleTypeEnum expectedOfferRoleTypeKey)
        {
            var results = assignmentService.GetLatestWorkFlowAssignment(OfferKey, expectedOfferRoleTypeKey);
            WatiN.Core.Logging.Logger.LogAction("Asserting that a WorkFlowAssignment record with OfferRoleTypeKey: " + (int)expectedOfferRoleTypeKey + " exists for OfferKey: " + OfferKey);
            Assert.True(results.HasResults, "No WorkFlowAssignment record with OfferRoleTypeKey: " + (int)expectedOfferRoleTypeKey + " exists for OfferKey: " + OfferKey);
        }

        /// <summary>
        /// Queries the db for all Workflow Assignment Records for a specified offer and related to
        /// a specified OfferRoleType, ordered by insert date.  If the 1st/latest record is active, the Assertion equates to true
        /// </summary>
        /// <param name="OfferKey">OfferKey e.g. 767421</param>
        /// <param name="expectedOfferRoleTypeKey">The expected OfferRoleTypeKey e.g. on case creation the 101 = Branch Consultant D OfferRoleType is expected</param>
        public static void AssertThatTheWorkFlowAssignmentRecordIsActive(int OfferKey, OfferRoleTypeEnum expectedOfferRoleTypeKey)
        {
            var results = assignmentService.GetLatestWorkFlowAssignment(OfferKey, expectedOfferRoleTypeKey);
            int GeneralStatusKey = results.Rows(0).Column("GSKey").GetValueAs<int>();
            WatiN.Core.Logging.Logger.LogAction("Asserting that the WorkFlowAssignment record with OfferRoleTypeKey: " + (int)expectedOfferRoleTypeKey + " is Active");
            Assert.AreEqual(1, GeneralStatusKey, "The WorkFlowAssignment record is with OfferRoleTypeKey: " + (int)expectedOfferRoleTypeKey + " not Active");
        }

        /// <summary>
        /// Queries the db for all Workflow Assignment Records for a specified offer and related to
        /// a specified OfferRoleType, ordered by insert date.  If the ADUsername of the 1st/latest record matches the ExpectedADUsername, the Assertion equates to true
        /// </summary>
        /// <param name="OfferKey">OfferKey e.g. 767421</param>
        /// <param name="ExpectedOfferRoleTypeKey">The expected OfferRoleTypeKey e.g. on case creation the 101 = Branch Consultant D OfferRoleType is expected</param>
        /// <param name="ExpectedADUSerName">The expected ADUserName e.g. sahl\bcuser</param>
        public static void AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(int OfferKey, OfferRoleTypeEnum ExpectedOfferRoleTypeKey, string ExpectedADUSerName)
        {
            var results = assignmentService.GetLatestWorkFlowAssignment(OfferKey, ExpectedOfferRoleTypeKey);
            string ADUserName = results.Rows(0).Column("ADUserName").Value;
            WatiN.Core.Logging.Logger.LogAction("Asserting that the WorkFlowAssignment record was assigned to ADUser:" + ExpectedADUSerName);
            StringAssert.AreEqualIgnoringCase(ExpectedADUSerName, ADUserName, "The WorkFlowAssignment record was not assigned to the ADUser " + ExpectedADUSerName);
        }

        /// <summary>
        /// Queries the db for the active OfferRole record for a specified offer and of a specified OfferRoleType.
        /// If the query returns results, the Assertion equates to true
        /// </summary>
        /// <param name="OfferKey">OfferKey e.g. 767421</param>
        /// <param name="expectedOfferRoleTypeKey">The expected OfferRoleTypeKey e.g. on case creation the 101 = Branch Consultant D OfferRoleType is expected</param>
        public static void AssertOfferRoleRecordExists(int OfferKey, OfferRoleTypeEnum expectedOfferRoleTypeKey)
        {
            var results = assignmentService.GetActiveOfferRolesByOfferRoleType(OfferKey, expectedOfferRoleTypeKey);
            WatiN.Core.Logging.Logger.LogAction("Asserting that an OfferRole record with OfferRoleTypeKey: " + expectedOfferRoleTypeKey + " exists for OfferKey: " + OfferKey);
            Assert.True(results.HasResults, "No OfferRole record with OfferRoleTypeKey: " + expectedOfferRoleTypeKey + " exists for OfferKey: " + OfferKey);
        }

        /// <summary>
        /// Queries the db for the active OfferRole record for a specified offer and of a specified OfferRoleType.
        /// Asserts that only one active record of that OfferRoleType is returned
        /// </summary>
        /// <param name="OfferKey">OfferKey e.g. 767421</param>
        /// <param name="expectedOfferRoleTypeKey">The expected OfferRoleTypeKey e.g. on case creation the 101 = Branch Consultant D OfferRoleType is expected</param>
        public static void AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(int OfferKey, OfferRoleTypeEnum expectedOfferRoleTypeKey)
        {
            var results = assignmentService.GetActiveOfferRolesByOfferRoleType(OfferKey, expectedOfferRoleTypeKey);
            WatiN.Core.Logging.Logger.LogAction("Asserting that only 1 Active OfferRole record for OfferRoleTypeKey " + expectedOfferRoleTypeKey + " exists");
            Assert.AreEqual(1, results.RowList.Count, "More than 1 Active OfferRole record for OfferRoleTypeKey " + expectedOfferRoleTypeKey + " exists");
        }

        /// <summary>
        /// Queries the db for the active OfferRole record for a specified offer and of a specified OfferRoleType.
        /// If the ADUsername of the 1st record matches the ExpectedADUsername, the Assertion equates to true
        /// </summary>
        /// <param name="OfferKey">OfferKey e.g. 767421</param>
        /// <param name="expectedOfferRoleTypeKey">The expected OfferRoleTypeKey e.g. on case creation the 101 = Branch Consultant D OfferRoleType is expected</param>
        /// <param name="ExpectedADUSerName">The expected ADUserName e.g. sahl\bcuser</param>
        public static void AssertWhoTheOfferRoleRecordIsAssignedTo(int OfferKey, OfferRoleTypeEnum expectedOfferRoleTypeKey, string ExpectedADUSerName)
        {
            string ADUserName = applicationService.GetADUserNameOfFirstActiveOfferRole(OfferKey, expectedOfferRoleTypeKey);
            WatiN.Core.Logging.Logger.LogAction("Asserting that the OfferRole record was assigned to ADUser:" + ExpectedADUSerName);
            StringAssert.AreEqualIgnoringCase(ExpectedADUSerName, ADUserName, "The WorkFlowAssignment was not assigned to the ADUser" + ExpectedADUSerName);
        }

        /// <summary>
        /// Navigates to the specified Worklist and checks if the specified OfferKey exists in the
        /// grid results.  Has the ability to page through results if necessary
        /// </summary>
        /// <param name="TestBrowser">WatiN.Core.TestBrowser object</param>
        public static void AssertOfferExistsOnWorkList(int offerKey, string workflowState, string aduser, string workflow)
        {
            var worklist = assignmentService.GetWorklistDetails(offerKey, workflowState, workflow);
            string message = String.Format("There are no worklist records for user {0}, offerkey:\"{1}\" at workflowstate:\"{2}\"", aduser, offerKey, workflowState);
            Assert.True(worklist.Count() > 0, message);
        }

        /// <summary>
        /// Queries the db for all Workflow Assignment Records for a specified offer and related to
        /// a specified OfferRoleType, ordered by insert date.  If the 1st/latest record is Inactive, the Assertion equates to true
        /// </summary>
        /// <param name="OfferKey">OfferKey e.g. 767421</param>
        /// <param name="ExpectedOfferRoleTypeKey">The expected OfferRoleTypeKey e.g. on case creation the 101 = Branch Consultant D OfferRoleType is expected</param>
        public static void AssertThatTheWorkFlowAssignmentRecordIsInactive(int OfferKey, OfferRoleTypeEnum ExpectedOfferRoleTypeKey)
        {
            QueryResults Results = assignmentService.GetLatestWorkFlowAssignment(OfferKey, ExpectedOfferRoleTypeKey);
            int GeneralStatusKey = Results.Rows(0).Column("GSKey").GetValueAs<int>();
            WatiN.Core.Logging.Logger.LogAction("Asserting that the WorkFlowAssignment record with OfferRoleTypeKey: " + ExpectedOfferRoleTypeKey + " is Inactive");
            Assert.AreEqual((int)GeneralStatusEnum.Inactive, GeneralStatusKey, "The WorkFlowAssignment record with OfferRoleTypeKey: " + ExpectedOfferRoleTypeKey + " is Active");
        }

        /// <summary>
        /// Asserts that the Offer Role and Workflow Assignment is correct for when a case moves states.
        /// Calls the following Assertions:
        /// AssertOfferRoleRecordExists
        /// AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive
        /// AssertWhoTheOfferRoleRecordIsAssignedTo
        /// AssertThatAWorkFlowAssignmentRecordExists
        /// AssertThatTheWorkFlowAssignmentRecordIsActive
        /// AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo
        /// </summary>
        /// <param name="Username">ADUserName</param>
        /// <param name="OfferRoleTypeKey">OfferRoleType of the User</param>
        /// <param name="OfferKey">OfferKey</param>
        public static void AssertWorkflowAssignment(string Username, int OfferKey, OfferRoleTypeEnum OfferRoleTypeKey)
        {
            //we need to check the Offer Role is setup
            AssertOfferRoleRecordExists(OfferKey, OfferRoleTypeKey);
            AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(OfferKey, OfferRoleTypeKey);
            AssertWhoTheOfferRoleRecordIsAssignedTo(OfferKey, OfferRoleTypeKey, Username);
            //we need to check the WorkflowAssignment exists
            AssertThatAWorkFlowAssignmentRecordExists(OfferKey, OfferRoleTypeKey);
            AssertThatTheWorkFlowAssignmentRecordIsActive(OfferKey, OfferRoleTypeKey);
            AssertWhoTheWorkFlowAssignmentRecordIsAssignedTo(OfferKey, OfferRoleTypeKey, Username);
        }

        /// <summary>
        /// Asserts that that the user workflowrole assignment is reactivated or if already active that it gets round robin assigned.
        /// </summary>
        public static void AssertUserReactivatedOrRoundRobinAssigned(string userName, int genericKey, WorkflowRoleType workflowRoleType)
        {
            var nextUser = assignmentService.GetNextRoundRobinAssignmentByWorkflowRoleTypeKey(WorkflowRoleTypeEnum.PLCreditAnalystD,
                                                                                                        RoundRobinPointerEnum.PLCreditAnalyst);
            var instanceId = x2Service.GetPersonalLoanInstanceId(genericKey);
            var sortedWorkflowRoleAssignment = (from ass in assignmentService.GetWorkflowRoleAssignmentByInstance(instanceId)
                                                select ass).OrderByDescending(x => x.Column("insertdate"));

            var latestWorkflowRoleAssignment = sortedWorkflowRoleAssignment.FirstOrDefault();

            var isActive = latestWorkflowRoleAssignment.Column("generalstatuskey").GetValueAs<int>();
        }

        public static void AssertAllWorkflowAssignmentRecordsForInstanceAreInactive(int instanceID)
        {
            QueryResults results = assignmentService.GetWorkflowAssignmentByInstance(Convert.ToInt64(instanceID));
            bool activeRecordsExists = (from r in results where r.Column("generalstatuskey").GetValueAs<int>() == (int)GeneralStatusEnum.Active select r).Any();
            Assert.That(activeRecordsExists == false);
        }
    }
}