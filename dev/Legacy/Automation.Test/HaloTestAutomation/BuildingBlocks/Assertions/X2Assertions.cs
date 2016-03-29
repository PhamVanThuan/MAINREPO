using Automation.DataAccess;
using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using System;
using System.Linq;

using WatiN.Core.Exceptions;
using WatiN.Core.Logging;
using Automation.DataModels;
using System.Collections.Generic;

namespace BuildingBlocks.Assertions
{
    public static class X2Assertions
    {
        private static IX2WorkflowService x2Service;
        private static ICommonService commonService;

        static X2Assertions()
        {
            x2Service = ServiceLocator.Instance.GetService<IX2WorkflowService>();
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        /// <summary>
        /// This assertion checks for the existence of a cloned workflow instance at a particular state in the Application Management workflow.
        /// </summary>
        /// <param name="instanceName">OfferKey</param>
        /// <param name="expectedState">Expected State of the cloned instance</param>
        /// <param name="workflow">Workflow Map</param>
        public static Int64 AssertX2CloneExists(string instanceName, string expectedState, string workflow)
        {
            QueryResults results = x2Service.GetInstanceCloneByState(instanceName, expectedState, workflow);
            //the current state should match the expected state
            Logger.LogAction("Asserting that an Instance clone was created for Instance.Name: " + instanceName + " at the " + expectedState + " workflow state");
            Assert.True(results.HasResults, "No Instance clone was created for Instance.Name: {0} at the {1} workflow state", instanceName, expectedState);
            var instanceid = results.Rows(0).Column("ClonedInstanceID").GetValueAs<Int64>();
            results.Dispose();
            return instanceid;
        }

        /// <summary>
        /// Clone of the above to only check for the top 1 instance.
        /// </summary>
        /// <param name="instanceName">OfferKey</param>
        /// <param name="expectedState">Expected State of the cloned instance</param>
        /// <param name="workflow">Workflow Map</param>
        public static Int64 AssertTop1X2CloneExists(string instanceName, string expectedState, string workflow)
        {
            QueryResults results = x2Service.GetTop1InstanceCloneByState(instanceName, expectedState, workflow);
            //the current state should match the expected state
            Logger.LogAction("Asserting that an Instance clone was created for Instance.Name: " + instanceName + " at the " + expectedState + " workflow state");
            Assert.True(results.HasResults, "No Instance clone was created for Instance.Name: {0} at the {1} workflow state", instanceName, expectedState);
            var instanceid = results.Rows(0).Column("ClonedInstanceID").GetValueAs<Int64>();
            results.Dispose();
            return instanceid;
        }

        /// <summary>
        /// This assertion will check that the Scheduled Activity has been set to the correct time.
        /// </summary>
        /// <param name="offerkey">OfferKey</param>
        /// <param name="activityName">Name of the Activity</param>
        /// <param name="expHours">The expected hour value for the scheduled activity</param>
        /// <param name="expMinutes">The expected minute value for the scheduled activity</param>
        public static void AssertScheduleActivitySetupForMainInstance(string offerkey, string activityName, int expHours, int expMinutes)
        {
            QueryResults results = x2Service.GetScheduledActivityTime(offerkey, activityName);

            int actHours = results.Rows(0).Column("HOUR").GetValueAs<int>();
            int actMinutes = results.Rows(0).Column("MINUTE").GetValueAs<int>();
            results.Dispose();

            Logger.LogAction("Assert that the correct hour value has been setup for the scheduled activity");
            Assert.AreEqual(actHours, expHours);
            Logger.LogAction("Assert that the correct minute value has been setup for the scheduled activity");
            Assert.AreEqual(actMinutes, expMinutes);
        }

        /// <summary>
        /// This assertion will check the worklist owner for an instance in a particular state of a Workflow
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="state">Workflow State</param>
        /// <param name="workflow">Workflow</param>
        /// <param name="expectedAdUserName">Expected ADUser (can also be a static role)</param>
        public static void AssertWorklistOwner(int offerKey, string state, string workflow, string expectedAdUserName)
        {
            var worklist = x2Service.GetWorklistDetails(offerKey, state, workflow);
            var exists = (from w in worklist where w.ADUserName == expectedAdUserName select w).FirstOrDefault();
            Assert.True(exists != null, "Worklist did not belong to expected AD User");
        }

        /// <summary>
        /// This assertion will check that the instance is in the expected State
        /// </summary>
        /// <param name="instanceID">Instance ID</param>
        /// <param name="expectedState">The Expected State</param>
        public static void AssertCurrentX2State(Int64 instanceID, string expectedState)
        {
            var results = x2Service.GetInstanceDetails(instanceID);
            string currentState = results.Rows(0).Column("StateName").Value;
            //the current state should match the expected state
            Logger.LogAction(string.Format("Asserting that Instance {0} is at the {1} workflow state", instanceID, expectedState));
            StringAssert.AreEqualIgnoringCase(expectedState, currentState,
                string.Format(@"Expected InstanceID {0} to be at {1} but it is at {2}.", instanceID, expectedState, currentState));
            results.Dispose();
        }

        /// <summary>
        /// This assertion checks for the existence of a cloned workflow instance at a particular state in the Application Management workflow.
        /// The Assertion passes if no clone was created and fails if a clone was created
        /// </summary>
        /// <param name="offerkey">OfferKey</param>
        /// <param name="expectedState">Expected State of the cloned instance</param>
        /// <param name="workflowName"></param>
        public static void AssertX2CloneDoesNotExist(int offerkey, string expectedState, string workflowName)
        {
            QueryResults results = x2Service.GetInstanceCloneByState(offerkey.ToString(), expectedState, workflowName);
            //the current state should match the expected state
            Logger.LogAction(string.Format(@"Asserting that an Instance clone was NOT created for offer: {0} at the {1} workflow state",
                offerkey, expectedState));
            Assert.False(results.HasResults, string.Format("An Instance clone was created for offer: {0} at the {1} workflow state", offerkey, expectedState));
            results.Dispose();
        }

        /// <summary>
        /// This asserts that an instance exists in the given workflow and is on the worklist of the provided aduser at the expected state.
        /// </summary>
        /// <param name="map">Workflow Map</param>
        /// <param name="genericKey">OfferKey/AccountKey</param>
        /// <param name="state">Workflow State</param>
        /// <param name="aduserName">ADUserName</param>
        public static int AssertWorkflowInstanceExistsForStateAndADUser(string map, int genericKey, string state, string aduserName)
        {
            QueryResults results = x2Service.GetWorkflowInstanceForStateAndADUser(map, genericKey, state, aduserName);
            Logger.LogAction(string.Format(@"Asserting that a workflow instance exists on {0}'s worklist at the {1} state for Application/Account: {2}",
                aduserName, state, genericKey));
            Assert.True(results.HasResults,
                string.Format(@"No workflow instance exists on {0}'s worklist at the {1} for Application/Account: {2}", aduserName,
                    state, genericKey));
            return results.Rows(0).Column("InstanceID").GetValueAs<int>();
        }

        /// <summary>
        /// This asserts that an instance exists in the given workflow exists at a specific state.
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static void AssertWorkflowInstanceExistsForState(string workflowMap, int genericKey, string state)
        {
            Assert.True(x2Service.OfferExistsAtState(genericKey, state),
                "Workflow case:{0} in workflow map:{1} does not exist at workflow state:{2}", genericKey, workflowMap, state);
        }

        /// <summary>
        /// This assertion will fetch the current state of an application capture instance and then compare the current
        /// state with an expected state provided by the test
        /// </summary>
        /// <param name="offerkey">OfferKey</param>
        /// <param name="expectedState">The Expected State of the Application in Application Capture</param>
        public static void AssertCurrentAppCapX2State(int offerkey, string expectedState)
        {
            QueryResults results = x2Service.GetAppCapInstanceDetails(offerkey);
            string currentState = results.Rows(0).Column("StateName").Value;
            //the current state should match the expected state
            Logger.LogAction(string.Format("Asserting that Application {0} is at the {1} workflow state", offerkey, expectedState));
            StringAssert.AreEqualIgnoringCase(expectedState, currentState,
                string.Format("Expected Application {0} to be at {1} but it is at {2}.", offerkey, expectedState, currentState));
            results.Dispose();
        }

        /// <summary>
        /// This assertion will check that the Scheduled Activity has been set to the correct number of hours or days.
        /// </summary>
        /// <param name="instanceName">OfferKey</param>
        /// <param name="activityName">Name of the Activity</param>
        /// <param name="daysFromNow">Number of days from now the timer is expected to fire. Set to ZERO if not needed</param>
        /// <param name="hoursFromNow">Number of hours from now the timer is expected to fire. Set to ZERO if not needed.</param>
        /// <param name="monthsFromNow">Number of months from now the timer is expected to fire. Set to ZERO if not needed</param>
        /// <param name="customScheduledActivityDate">Allows for a test to provide the expected scheduled activity date</param>
        public static int AssertScheduleActivitySetup(string instanceName, string activityName, int daysFromNow, int hoursFromNow, int monthsFromNow,
            string customScheduledActivityDate = "")
        {
            QueryResults results = x2Service.GetScheduledActivityTime(instanceName.ToString(), activityName);
            string schedule = results.Rows(0).Column("SCHEDULE").Value;
            DateTime scheduledActivityDate = Convert.ToDateTime(schedule);
            int instanceid = (from r in results select r.Column("INSTANCEID").GetValueAs<int>()).FirstOrDefault();
            results.Dispose();
            //this is used if we have set a timer to a specific date and havent added days/months to the current date
            if (!string.IsNullOrEmpty(customScheduledActivityDate))
            {
                StringAssert.AreEqualIgnoringCase(customScheduledActivityDate, schedule);
                return instanceid;
            }
            else
            {
                DateTime date = DateTime.Now;
                if (daysFromNow > 0)
                {
                    TimeSpan diff = scheduledActivityDate - date;
                    int daysDiff = Convert.ToInt32(Math.Round(diff.TotalDays, 0));
                    Logger.LogAction("Assert that the scheduled activity has been setup for the for the correct number of days");
                    Assert.AreEqual(daysFromNow, daysDiff, "Scheduled activity is not correctly setup");
                    return instanceid;
                }
                else if (hoursFromNow > 0)
                {
                    TimeSpan diff = scheduledActivityDate - date;
                    int hoursDiff = Convert.ToInt32(Math.Round(diff.TotalHours, 0));
                    Logger.LogAction("Assert that the scheduled activity has been setup for the for the correct number of hours");
                    Assert.AreEqual(hoursDiff, hoursFromNow, "Scheduled activity is not correctly setup");
                    return instanceid;
                }
                else if (monthsFromNow > 0)
                {
                    int monthsDiff = commonService.MonthDifference(DateTime.Now, scheduledActivityDate);
                    Logger.LogAction("Asserting that the scheduled activity is " + monthsFromNow + " in the future. ");
                    Assert.AreEqual(monthsFromNow, monthsDiff, "Scheduled activity is not correctly setup");
                    return instanceid;
                }
            }
            return instanceid;
        }

        /// <summary>
        /// This assertion will check that the Scheduled Activity has been set to the correct time.
        /// </summary>
        /// <param name="offerkey">OfferKey</param>
        /// <param name="activityName">Name of the Activity</param>
        /// <param name="expHours">The expected hour value for the scheduled activity</param>
        /// <param name="expMinutes">The expected minute value for the scheduled activity</param>
        public static void AssertScheduleActivitySetup(int offerkey, string activityName, string expHours, string expMinutes)
        {
            QueryResults results = x2Service.GetScheduledActivityTime(offerkey.ToString(), activityName);

            int actHours = results.Rows(0).Column("HOUR").GetValueAs<int>();
            int actMinutes = results.Rows(0).Column("MINUTE").GetValueAs<int>();
            results.Dispose();

            Logger.LogAction("Assert that the correct hour value has been setup for the scheduled activity");
            Assert.AreEqual(actHours, Convert.ToInt32(expHours));
            Logger.LogAction("Assert that the correct minute value has been setup for the scheduled activity");
            Assert.AreEqual(actMinutes, Convert.ToInt32(expMinutes));
        }

        /// <summary>
        /// This assertion will fetch the current state of an Application Management instance and then compare the current
        /// state with an expected state provided by the test
        /// </summary>
        /// <param name="offerkey">OfferKey</param>
        /// <param name="expectedState">The Expected State of the Application in Application Management</param>
        public static int AssertCurrentAppManX2State(int offerkey, string expectedState)
        {
            QueryResults results = x2Service.GetAppManInstanceDetails(offerkey);
            string currentState = results.Rows(0).Column("StateName").Value;
            int instanceID = results.Rows(0).Column("ID").GetValueAs<int>();
            //the current state should match the expected state
            Logger.LogAction("Asserting that Application " + offerkey + " is at the " + expectedState + " workflow state");
            StringAssert.AreEqualIgnoringCase(expectedState, currentState, string.Format(@"Application {0} was not at expected state: {1}, it is at the {2} state",
                offerkey, expectedState, currentState));
            results.Dispose();
            return instanceID;
        }

        /// <summary>
        /// This assertion will fetch the current state of an Valuations instance and then compare the current
        /// state with an expected state provided by the test
        /// </summary>
        /// <param name="offerkey">OfferKey</param>
        /// <param name="expectedState">The Expected State of the Application in Valuations</param>
        public static void AssertCurrentValuationsX2State(int offerkey, string expectedState)
        {
            QueryResults results = x2Service.GetValuationsInstanceDetails(offerkey);
            Assert.True(results.HasResults, "Offer {0} is not at the '{1}' state", offerkey, expectedState);
            string currentState = results.Rows(0).Column("StateName").Value;
            //the current state should match the expected state
            Logger.LogAction("Asserting that Application " + offerkey + " is at the " + expectedState + " workflow state");
            StringAssert.AreEqualIgnoringCase(expectedState, currentState);
            results.Dispose();
        }

        /// <summary>
        /// This assertion will fetch the current state of an Readvance Payments instance and then compare the current
        /// state with an expected state provided by the test
        /// </summary>
        /// <param name="offerkey">OfferKey</param>
        /// <param name="expectedState">The Expected State in Readvance Payments</param>
        public static int AssertCurrentReadvPaymentsX2State(int offerkey, string expectedState)
        {
            var results = x2Service.GetReadvancePaymentsInstanceDetails(offerkey);
            if (!results.HasResults)
                throw new WatiNException("No Readvance Payments Instance Exists");
            string currentState = results.Rows(0).Column("StateName").Value;
            //the current state should match the expected state
            Logger.LogAction("Asserting that Application " + offerkey + " is at the " + expectedState + " workflow state");
            StringAssert.AreEqualIgnoringCase(expectedState, currentState, "Case is not at expected state or in the the expected workflow");
            return results.Rows(0).Column("InstanceID").GetValueAs<int>();
        }

        /// <summary>
        /// This assertion will fetch the current state of a Credit instance and then compare the current
        /// state with an expected state provided by the test
        /// </summary>
        /// <param name="offerkey">OfferKey</param>
        /// <param name="expectedState">The Expected State in the Credit workflow</param>
        public static void AssertCurrentCreditX2State(int offerkey, params string[] expectedStates)
        {
            QueryResults results = x2Service.GetCreditInstanceDetails(offerkey);
            var expectedState = String.Empty;
            try
            {
                Logger.LogAction("Asserting that Application " + offerkey + " is at the " + String.Join(",", expectedStates) + " workflow state");
                Assert.True(results.HasResults, "It is possible that the application " + offerkey + " is not in the Credit workflow");
                string currentState = results.Rows(0).Column("StateName").Value;
                var isAtCorrectState = expectedStates.Where(x => x == currentState).Count() > 0;

                //the current state should match the expected state
                Assert.True(isAtCorrectState, "Application " + offerkey + " is not at one of the following workflow states: " + String.Join(",", expectedStates));
            }
            finally
            {
                results.Dispose();
            }
        }
        /// <summary>
        /// This Assertion will check that the Scheduled Actity Timer for the specified Activity is set correctly.
        /// The assertion calculates the expected date n business\consecutive days from today and compares it to the
        /// ScheduledActivity.Time value
        /// </summary>
        /// <param name="instanceName">Instance.Name</param>
        /// <param name="activityName">Activity.Name e.g. "NTU Timeout"</param>
        /// <param name="days">Days</param>
        /// <param name="businessDays">true = business days, false = consecutive days</param>
        public static void AssertScheduledActivityTimer(string instanceName, string activityName, int days, bool businessDays, DateTime? timerBaseDate = null)
        {
            string assertType = businessDays ? "Business Day(s)" : "Consecutive Day(s)";
            DateTime expectedDate = businessDays ? x2Service.CalculateFutureDateInBusinessDays(days, timerBaseValue: timerBaseDate) : DateTime.Today.AddDays(Convert.ToDouble(days));
            DateTime actualDate = x2Service.GetScheduledActivityTime(instanceName, activityName).Rows(0).Column(0).GetValueAs<DateTime>();
            Logger.LogAction(@"Asserting that the Scheduled Activity Timer for Activity: {0}, is set {1} {2} day\s from current date", activityName, days, assertType);
            //get the difference
            var diff = expectedDate - actualDate;
            Assert.That(diff.Days == 0, @"The Scheduled Activity Timer was not set {0} {1} from current date", days, assertType);
        }

        /// <summary>
        /// This assertion will check that the Scheduled Activity has been set to the correct time.  This override allows you to
        /// specify whether to get the Sceduled Activity of a main instance or a clone instance.
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="activityName">Name of the Activity</param>
        /// <param name="days">The expected day value or the number of days in the future</param>
        /// <param name="hours">The expected hour value or the number of hours in the future</param>
        /// <param name="minutes">The expected minute value or the number of minutes in the future</param>
        /// <param name="assertType">1 = Specified time variables evaluated as an exact datetime, 2 = Specified time variables added to current datetime</param>
        /// <param name="clone">true = clone instance, false = main instance</param>
        public static void AssertScheduleActivitySetup(int offerKey, string activityName, int days, int hours, int minutes, int assertType, bool clone)
        {
            QueryResults results;
            if (clone) results = x2Service.GetScheduledActivityTimeForCloneInstance(offerKey, activityName);
            else results = x2Service.GetScheduledActivityTimeForInstance(offerKey, activityName);

            switch (assertType)
            {
                // Specified time variables evaluated as an exact datetime
                case 1:
                    int actHours = results.Rows(0).Column("HOUR").GetValueAs<int>();
                    int actMinutes = results.Rows(0).Column("MINUTE").GetValueAs<int>();
                    results.Dispose();

                    Logger.LogAction("Assert that the correct hour value has been setup for the scheduled activity");
                    Assert.AreEqual(actHours, hours);
                    Logger.LogAction("Assert that the correct minute value has been setup for the scheduled activity");
                    Assert.AreEqual(actMinutes, minutes);
                    break;
                // Specified time variables added to current datetime
                case 2:
                    string schedule = results.Rows(0).Column("SCHEDULE").Value;
                    DateTime scheduledActivityDate = Convert.ToDateTime(schedule);
                    results.Dispose();

                    DateTime date = DateTime.Now;

                    if (days > 0)
                    {
                        TimeSpan diff = scheduledActivityDate - date;
                        int daysDiff = Convert.ToInt32(Math.Round(diff.TotalDays, 0));
                        Logger.LogAction("Assert that the scheduled activity has been setup for the for the correct number of days");
                        Assert.AreEqual(daysDiff, days);
                    }
                    else if (hours > 0)
                    {
                        TimeSpan diff = scheduledActivityDate - date;
                        int hoursDiff = Convert.ToInt32(Math.Round(diff.TotalHours, 0));
                        Logger.LogAction("Assert that the scheduled activity has been setup for the for the correct number of hours");
                        Assert.AreEqual(hoursDiff, hours);
                    }
                    break;
            }
        }

        /// <summary>
        /// This checks that the expected count of an Activity on an Instance equals the actual count.
        /// </summary>
        /// <param name="instanceID">Instance ID</param>
        /// <param name="activity">Activity Name</param>
        /// <param name="expectedCount">Expected count of the Activity</param>
        public static void AssertWorkflowHistoryActivityCount(Int64 instanceID, string activity, int expectedCount, DateTime dateFilter)
        {
            QueryResults results = x2Service.GetWorkflowHistoryActivityCount(instanceID, activity, dateFilter.ToString(Formats.DateTimeFormatSQL));
            int actualCount;
            actualCount = results.Rows(0).Column(0).GetValueAs<int>();
            results.Dispose();
            Logger.LogAction("Asserts that the expected count for the " + activity + " activity is equal to the actual count of the activity on the instance");
            Assert.AreEqual(expectedCount, actualCount);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="expectedState"></param>
        /// <returns></returns>
        public static int AssertCurrentDeleteDebitOrderX2State(int accountKey, string expectedState)
        {
            QueryResults results = x2Service.GetDeleteDebitOrderInstanceDetails(accountKey);
            var row = (from r in results
                       where r.Column("StateName").Value == expectedState
                       select r).FirstOrDefault();
            //the current state should match the expected state
            Logger.LogAction("Asserting that Application " + accountKey + " is at the " + expectedState + " workflow state");
            Assert.That(row != null, string.Format(@"Application {0} was not at expected state: {1}",
                accountKey, expectedState));
            int instanceID = row.Column("ID").GetValueAs<int>();
            results.Dispose();
            return instanceID;
        }

        /// <summary>
        /// Asserts whether or not an application exists at a certain Application Management state
        /// </summary>
        /// <param name="applicationKey">OfferKey</param>
        /// <param name="state">Application Management state</param>
        public static void AssertApplicationExistAtAppManageState(int applicationKey, string state)
        {
            Logger.LogAction(String.Format("Asserting that an application {0} does exist at state {1}", applicationKey, state));
            Assert.IsTrue(x2Service.DoesApplicationExistAtAppManageState(applicationKey, state), "Cannot find Application {0} at state {1}",
                applicationKey, state);
        }

        /// <summary>
        /// Asserts that an activity and/or state is setup correctly for user role
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="actionName"></param>
        /// <param name="stateName"></param>
        /// <param name="userRole"></param>
        public static void AssertUserRoleSecurity(string workflowName, string actionName, string stateName, string userRole)
        {
            var isCorrect = x2Service.CheckUserRoleSecuritySetup(workflowName, userRole, stateName, actionName);
            Assert.True(isCorrect, "Users that belong to the '{0}' role does not have access to the '{1}' action at workflow state '{2}'"
                                            , userRole, actionName, stateName);
        }

        public static void AssertAlphaHousingSurveyEmailSentIsTrue(int offerKey)
        {
            var alphaHousingSurveyEmailSent = (from s in x2Service.GetAppManInstanceDetails(offerKey)
                                               where s.Column("AlphaHousingSurveyEmailSent").GetValueAs<bool>() == true
                                               select s).FirstOrDefault();
            Assert.That(alphaHousingSurveyEmailSent != null, string.Format(@"AlphaHousingSurveyEmailSent was not set to true."));
        }

        public static void AssertAlphaHousingSurveyEmailSentIsFalse(int offerKey)
        {
            var alphaHousingSurveyEmailSent = (from s in x2Service.GetAppManInstanceDetails(offerKey)
                                               where s.Column("AlphaHousingSurveyEmailSent").GetValueAs<bool>() == false
                                               select s).FirstOrDefault();
            Assert.That(alphaHousingSurveyEmailSent != null, string.Format(@"AlphaHousingSurveyEmailSent was set to true."));
        }

        public static void AssertX2RequestProcessed(int applicationKey)
        {
            var contentLookup = String.Format(@"""ApplicationKey"":""{0}""",applicationKey);
            var x2Requests = x2Service.GetLastestX2Requests();

            BuildingBlocks.Timers.GeneralTimer.Wait(3000);

            var requestExists = x2Requests.Where(x => x.Contents.Contains(contentLookup)).Count() > 0;

            Assert.False(requestExists, "X2 Request failed for application: {0}", applicationKey);
        }

    }
}