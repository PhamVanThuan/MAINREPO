using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WatiN.Core.Exceptions;
using WatiN.Core.Logging;
using WatiN.Core.UtilityClasses;
using WorkflowAutomation.Harness;

namespace BuildingBlocks.Services
{
    public class X2WorkflowService : _2AMDataHelper, IX2WorkflowService
    {
        private readonly IStageTransitionService stageTransitionService;
        private readonly IX2ScriptEngine scriptEngine;

        public X2WorkflowService()
        {
            stageTransitionService = ServiceLocator.Instance.GetService<IStageTransitionService>();
            scriptEngine = new X2ScriptEngine();
        }

        /// <summary>
        /// Inserts an Active External Activity for an Instance
        /// </summary>
        public void InsertActiveExternalActivity(string workflowName, string externalActivity, Int64 instanceID, int genericKey, string activityName = "", string activityXMLData = null)
        {
            base.InsertActiveExternalActivity(workflowName, externalActivity, instanceID, activityXMLData);
            var state = base.GetNextStateForExternalActivity(externalActivity, activityName, workflowName);
            if (genericKey > -1)
                this.WaitForX2State(genericKey, workflowName, state.Name, 10);
        }

        /// <summary>
        /// This method will wait the provided number of seconds for a given activity to exist against an instance in the WorkflowHistory table. If is not
        /// found after the timer has expired, the method will throw an exception.
        /// </summary>
        /// <param name="activityName">Activity Name</param>
        /// <param name="instanceID">Instance ID</param>
        /// <param name="seconds">No. of seconds to wait</param>
        /// <param name="count">No of occurences expected for the activity. e.g Timers can fire twice on a case</param>
        public void WaitForX2WorkflowHistoryActivity(string activityName, Int64 instanceID, int count)
        {
            var dateFilter = new DateTime(1900, 1, 1).ToString(Formats.DateTimeFormatSQL);
            Logger.LogAction(string.Format(@"Waiting 90 Seconds for Activity to Complete: IID={0}, Activity={1}, Date {2}", instanceID, activityName, dateFilter));
            var timer = new SimpleTimer(TimeSpan.FromSeconds(90));
            bool b = false;
            while (!timer.Elapsed)
            {
                //get the workflow history
                var r = GetWorkflowHistoryActivityCount(instanceID, activityName, dateFilter);
                //does the count equal to expected count
                if (r.Rows(0).Column(0).GetValueAs<int>() >= count)
                {
                    b = true;
                    //we have found it
                    break;
                }
            }
            if (!b)
            {
                //throw exception if it isnt found.
                throw new WatiNException(string.Format(@"Activity {0} did not get run for instance {1} after waiting 90 seconds. Expected to
                    occur after date: {2}", activityName, instanceID, dateFilter));
            }
        }

        /// <summary>
        /// This method will wait the provided number of seconds for a given activity to exist against an instance in the WorkflowHistory table. If is not
        /// found after the timer has expired, the method will throw an exception.
        /// </summary>
        /// <param name="activityName">Activity Name</param>
        /// <param name="instanceID">Instance ID</param>
        /// <param name="count">No of occurences expected for the activity. e.g Timers can fire twice on a case</param>
        public void WaitForX2WorkflowHistoryActivity(int instanceID, int count, string dateFilter = "", params string[] activityName)
        {
            dateFilter = string.IsNullOrEmpty(dateFilter) ? dateFilter = new DateTime(1900, 1, 1).ToString(Formats.DateTimeFormatSQL) : dateFilter;
            Logger.LogAction(string.Format(@"Waiting 90 Seconds for Activity to Complete: IID={0}, Activity={1}, Date {2}", instanceID, activityName, dateFilter));
            var timer = new SimpleTimer(TimeSpan.FromSeconds(90));
            bool b = false;
            while (!timer.Elapsed)
            {
                //get the workflow history
                var r = GetWorkflowHistoryActivitiesCount(instanceID, dateFilter, activityName);
                //does the count equal to expected count
                if (r.Rows(0).Column(0).GetValueAs<int>() >= count)
                {
                    b = true;
                    //we have found it
                    break;
                }
            }
            if (!b)
            {
                //throw exception if it isnt found.
                throw new WatiNException(string.Format(@"Activities ( {0} ) did not get run for instance {1} after waiting 90 seconds. Expected to occur after date: {2}", String.Join(",", activityName), instanceID, dateFilter));
            }
        }

        /// <summary>
        /// Provided with an offerKey this method will try for 30 seconds to retrieve the Application Management instance. Once it finds the instance it will
        /// then query the x2.x2.workflowHistory table, waiting for the Assigned QA record to be written, this means that the case should now be at the QA state.
        /// </summary>
        /// <param name="offerKey"></param>
        public void WaitForAppManCaseCreate(int offerKey)
        {
            var timer = new SimpleTimer(TimeSpan.FromSeconds(30));
            bool b = false;
            int id = 0;
            while (!timer.Elapsed)
            {
                var r = base.GetAppManInstanceDetails(offerKey);
                if (r.HasResults)
                {
                    b = true;
                    id = r.Rows(0).Column("ID").GetValueAs<int>();
                    break;
                }
            }
            if (b)
            {
                //we have an app man instance, wait for the assigned QA to be written
                WaitForX2WorkflowHistoryActivity(ConditionalActivities.ApplicationManagement.AssignedQA, id, 1);
            }
        }

        /// <summary>
        /// Executes an X2 stored procedure to fired the Ext NTU external activity
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        public void PipeLineNTU(int offerKey)
        {
            Int64 instanceID = -1;
            instanceID = GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.RegistrationPipeline, offerKey);
            if (instanceID <= 0)
            {
                throw new WatiNException("No Instance found at Registration Pipeline");
            }
            base.PipeLineNTU(instanceID);
        }

        /// <summary>
        /// Executes an X2 stored procedure to fire the Ext Reinstate NTU external activity
        /// </summary>
        /// <param name="offerKey"></param>
        public void PipeLineReinstateNTU(int offerKey)
        {
            Int64 instanceID = -1;
            instanceID = GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.NTU, offerKey);
            if (instanceID <= 0)
            {
                throw new WatiNException("No Instance found at the NTU State");
            }
            string date = DateTime.Now.ToString(Formats.DateTimeFormatSQL);
            base.PipeLineReinstateNTU(instanceID);
            WaitForX2WorkflowHistoryActivity(ExternalActivities.ApplicationManagement.EXTReinstate, instanceID, 1);
        }

        /// <summary>
        /// Returns the InstanceID for an application's instance at a certain workflow state in the application management workflow.
        /// </summary>
        /// <param name="workflowState"></param>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public Int64 GetAppManInstanceIDByState(string workflowState, int offerKey, bool includeClones = false)
        {
            var r = GetAppManInstanceDetails(offerKey, includeClones);
            Int64 instanceID = -1;
            //need to loop through the result set
            foreach (var row in r.RowList)
            {
                if (row.Column("StateName").Value == workflowState)
                {
                    instanceID = row.Column("ID").GetValueAs<int>();
                    return instanceID;
                }
            }
            return instanceID;
        }

        /// <summary>
        /// Executes an X2 stored procedure to fire the Up For Fees external activity
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        public void PipelineUpForFees(int offerKey)
        {
            Int64 instanceID = -1;
            instanceID = GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.RegistrationPipeline, offerKey);
            if (instanceID <= 0)
            {
                throw new WatiNException("No Instance found at Registration Pipeline");
            }
            base.PipelineUpForFees(instanceID);
        }

        /// <summary>
        /// Executes an X2 stored procedure to fire the Ext Complete external activity
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        public void PipelineComplete(int offerKey)
        {
            Int64 instanceID = -1;
            instanceID = GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.Disbursement, offerKey);
            if (instanceID <= 0)
            {
                throw new WatiNException("No Instance found at Disbursement");
            }
            base.PipeLineComplete(instanceID);
        }

        /// <summary>
        /// Executes an X2 stored procedure to fire the Ext Held Over external activity
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        public void PipeLineHeldOver(int offerKey)
        {
            Int64 instanceID = -1;
            instanceID = GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.DisbursementReview, offerKey);
            if (instanceID <= 0)
            {
                throw new WatiNException("No Instance found at Disbursement Review");
            }
            base.PipeLineHeldOver(instanceID);
        }

        /// <summary>
        /// This Helper method will provide a test with an application key for an application of a given type at the state
        /// provided. It allows the user to supply exclusions, which are basically offers that you do not wish to include in
        /// the results. ie. FLAutomation excludes offers in the test.AutomationFLTestCases table. Further exclusions can be
        /// built in the SP.
        /// </summary>
        /// <param name="state">State.Description</param>
        /// <param name="workflow">Workflow.Description</param>
        /// <param name="offerTypeKey">Offer.OfferTypeKey</param>
        /// <param name="exclusions">List of offers to exclude</param>
        /// <returns>
        /// Offer.OfferKey, Offer.OfferStartDate, OfferType.Description, Product.Description,
        /// Category.Description, LTV, LAA, PTI
        /// </returns>
        public int GetOfferKeyAtStateByType(string state, string workflow, OfferTypeEnum offerTypeKey, string exclusions)
        {
            var results = GetApplicationsByStateAndAppType(state, workflow, exclusions, (int)offerTypeKey);
            if (!results.HasResults)
                return 0;
            var key = results.Rows(0).Column(0).GetValueAs<int>();
            results.Dispose();
            return key;
        }

        public int GetOfferKeyAtStateByType(string state, string workflow, OfferTypeEnum offerTypeKey, string exclusions, int worklistADUserStatus)
        {
            var results = from r in GetApplicationsByStateAndAppType(state, workflow, exclusions, (int)offerTypeKey)
                          where r.Column("GeneralStatusKey").Value.Equals(worklistADUserStatus.ToString())
                          select r.Column("offerkey").GetValueAs<int>();

            return results.FirstOrDefault();
        }

        public int GetOfferKeyAtStateByTypeAndLoanAmount(string state, string workflow, OfferTypeEnum offerTypeKey, string exclusions, int LoanAmount)
        {
            var results = from r in GetApplicationsByStateAndAppType(state, workflow, exclusions, (int)offerTypeKey)
                          where r.Column("LAA").GetValueAs<int>() > LoanAmount
                          select r.Column("offerkey").GetValueAs<int>();

            return results.FirstOrDefault();
        }

        /// <summary>
        /// This Helper method will provide a test with an application key for an application of a given type at the state
        /// provided. It allows the user to supply exclusions, which are basically offers that you do not wish to include in
        /// the results. ie. FLAutomation excludes offers in the test.AutomationFLTestCases table. Further exclusions can be
        /// built in the SP.
        /// </summary>
        /// <param name="state">State.Description</param>
        /// <param name="workflow">Workflow.Description</param>
        /// <param name="offerTypeKey">Offer.OfferTypeKey</param>
        /// <param name="exclusions">List of offers to exclude</param>
        /// <returns>
        /// Offer.OfferKey, Offer.OfferStartDate, OfferType.Description, Product.Description,
        /// Category.Description, LTV, LAA, PTI
        /// </returns>
        public QueryResults GetOfferKeysAtStateByType(string state, string workflow, OfferTypeEnum offerTypeKey, string exclusions)
        {
            var results = GetApplicationsByStateAndAppType(state, workflow, exclusions, (int)offerTypeKey);
            if (!results.HasResults)
                throw new WatiNException(String.Format(@"No applications found at {0} in {1}", state, workflow));
            return results;
        }

        /// <summary>
        ///   This Helper method will provide a test with an application key for an application of a given type at the state
        /// provided. It allows the user to supply exclusions, which are basically offers that you do not wish to include in
        /// the results. ie. FLAutomation excludes offers in the test.AutomationFLTestCases table. Further exclusions can be
        /// built in the SP.
        /// </summary>
        /// <param name = "state">State.Description</param>
        /// <param name = "workflow">Workflow.Description</param>
        /// <param name = "offerType">Offer.OfferTypeKey</param>
        /// <param name = "exclusions">List of offers to exclude</param>
        /// <param name="maxLTV">The maximum LTV for this case</param>
        /// <param name="minLTV">The minimum LTV for this case</param>
        /// <param name="numLE">The maximum number of LE's on the application</param>
        /// <returns>
        ///   Offer.OfferKey, Offer.OfferStartDate, OfferType.Description, Product.Description,
        ///   Category.Description, LTV, LAA, PTI
        /// </returns>
        public QueryResults GetOfferKeysAtStateByType(string state, string workflow, string exclusions, OfferTypeEnum offerType, double maxLTV, double minLTV,
            int numLE, OccupancyTypeEnum occupancyType = OccupancyTypeEnum.OwnerOccupied, int employmentType = 0, int category = -1)
        {
            QueryResults results = GetApplicationsByStateAndAppType(state, workflow, exclusions, ((int)offerType).ToString(), maxLTV, minLTV, numLE, occupancyType, employmentType, category);
            if (!results.HasResults)
                throw new WatiNException(String.Format(@"No applications found at {0} in {1}", state, workflow));
            return results;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="workflow"></param>
        /// <param name="exclusions"></param>
        /// <returns></returns>
        public List<int> GetOffersAtState(string state, string workflow, string exclusions)
        {
            List<OfferTypeEnum> offerTypes = new List<OfferTypeEnum>() { OfferTypeEnum.NewPurchase, OfferTypeEnum.SwitchLoan, OfferTypeEnum.Refinance, OfferTypeEnum.FurtherLoan, OfferTypeEnum.FurtherAdvance, OfferTypeEnum.Readvance };
            List<int> offerKeys = new List<int>();
            foreach (var type in offerTypes)
            {
                QueryResults results = GetApplicationsByStateAndAppType(state, workflow, exclusions, (int)type);
                if (results.HasResults)
                {
                    var keys = (from r in results select r.Column(0).GetValueAs<int>()).AsEnumerable();
                    offerKeys.AddRange(keys);
                }
            }
            offerKeys.Sort();
            offerKeys.Reverse();
            return offerKeys;
        }

        /// <summary>
        /// Waits 45 seconds for a case to be created in the given credit state where the instance is linked to the sourceInstanceID provided to the method.
        /// </summary>
        /// <param name="sourceInstanceID">The Application Management source instance</param>
        /// <param name="expectedCreditState">The state you are expecting your case to be at</param>
        /// <param name="offerKey">OfferKey</param>
        public void WaitForCreditCaseCreate(Int64 sourceInstanceID, int offerKey, string expectedCreditState)
        {
            var timer = new SimpleTimer(TimeSpan.FromSeconds(45));
            bool creditCaseExists = false;
            while (!timer.Elapsed)
            {
                var r = GetCreditInstanceDetailsBySourceInstanceAndState(sourceInstanceID, offerKey, expectedCreditState);
                if (r.HasResults)
                {
                    creditCaseExists = true;
                    break;
                }
            }
            Assert.That(creditCaseExists, "Credit case not created after waiting 45 seconds");
        }

        /// <summary>
        /// Wait until an x2 case exist at the provided state. Interval of 12 seconds between every try i.e. about a minute for the case to move.
        /// </summary>
        /// <param name="generickey"></param>
        /// <param name="workflowName"></param>
        /// <param name="workflowstate"></param>
        /// <param name="noTries"></param>
        public void WaitForX2State(int generickey, string workflowName, string workflowstate, int noTries = 12)
        {
            var tryCount = 0;
            var intervals = TimeSpan.FromSeconds(5);
            while (tryCount < noTries)
            {
                var instance = (from i in GetManyWorkflowInstances(workflowName, workflowstate)
                                where i.GenericKey == generickey
                                select i).FirstOrDefault();

                if (instance != null && instance.InstanceId > 0)
                    return;
                SpinWait.SpinUntil(() => { return false; }, intervals);
                tryCount++;
            }
            Assert.Fail(String.Format("Waited for X2 to move case (generickey:{0}) in the {1} workflow to the {2} state, but exceeded the number of tries.", generickey, workflowName, workflowstate));
        }

        public IEnumerable<Automation.DataModels.X2Instance> GetManyWorkflowInstances(string workflowName, params string[] workflowStateNames)
        {
            var workflow = (from w in GetWorkflows() where w.Name == workflowName select w).FirstOrDefault();

            var workflowStates = GetStates(workflow);
            Assert.That(workflow != null, "Workflow not found");
            //we need the instances
            var instances = GetWorkflowInstances(workflow);
            var activities = GetActivities(workflow);
            //Set all to the current workflow map and state
            foreach (var i in instances)
            {
                i.Workflow = workflow;
                var state = (from s in workflowStates
                             where i.StateID == s.ID
                             select s).FirstOrDefault();
                i.State = state;
                activities = from activity in activities
                             where activity.State != null && activity.State.Name.Equals(i.State.Name)
                             select activity;
                i.State.Activities = activities;
            }
            if (workflowStateNames.Length > 0)
            {
                instances = (from i in instances
                             join stateName in workflowStateNames
                                 on i.State.Name equals stateName
                             select i).ToList();
            }
            return instances;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflowState"></param>
        /// <param name="workflow"></param>
        /// <param name="offerType"></param>
        /// <param name="businessEvent"></param>
        /// <returns></returns>
        public int GetWorkflowCaseWithoutBusinessEvent(string workflowState, string workflow, OfferTypeEnum offerType,
            StageDefinitionStageDefinitionGroupEnum businessEvent)
        {
            var r = GetOfferKeysAtStateByType(workflowState, workflow, offerType, "");
            int offerKey = 0;
            foreach (QueryResultsRow row in r.RowList)
            {
                bool transitionExists = false;
                offerKey = row.Column("offerkey").GetValueAs<int>();
                transitionExists = stageTransitionService.CheckIfTransitionExists(offerKey, (int)businessEvent);
                if (!transitionExists)
                {
                    return offerKey;
                }
            }
            if (offerKey == 0)
                throw new WatiNException("No offer found");
            return offerKey;
        }

        /// <summary>
        /// Returns the InstanceID for an application's instance at a certain workflow state in the credit workflow.
        /// </summary>
        /// <param name="workflowState"></param>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public int GetCreditInstanceIDByState(string workflowState, int offerKey, bool includeClones = false)
        {
            QueryResults r = GetCreditInstanceDetails(offerKey);
            int instanceID = -1;
            //need to loop through the result set
            foreach (QueryResultsRow row in r.RowList)
            {
                if (row.Column("StateName").Value == workflowState)
                {
                    instanceID = row.Column("ID").GetValueAs<int>();
                    return instanceID;
                }
            }
            return instanceID;
        }

        /// <summary>
        /// Check if the application has sucessfully moved a specific state
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool DoesApplicationExistAtAppManageState(int applicationKey, string state)
        {
            QueryResults results = base.GetWorklistOfCaseAtState(applicationKey, state);
            if (results.HasResults)
                return true;
            return false;
        }

        /// <summary>
        /// Returns the count of cases for a user
        /// </summary>
        /// <param name="stateName">stateName</param>
        /// <param name="workflowName">workflowName</param>
        /// <param name="userName">userName</param>
        /// <returns>Count of cases in their worklist</returns>
        public int GetCountofCasesForUser(string stateName, string workflowName, string userName)
        {
            var r = base.GetCasesInWorklist(stateName, workflowName, userName);
            return r.RowList.Count;
        }

        /// <summary>
        /// This method will wait the provided number of seconds for a given activity to exist in the scheduled actviity table. If is not
        /// found after the timer has expired, the method will throw an exception.
        /// </summary>
        /// <param name="activityName">Activity Name</param>
        /// <param name="instanceID">Instance ID</param>
        public bool WaitForX2ScheduledActivity(string activityName, Int64 instanceID)
        {
            bool activityFound = false;
            for (int tryCount = 0; tryCount < 10; tryCount++)
            {
                SpinWait.SpinUntil(() => { return false; }, 2000);
                //get the workflow history
                var scheduledActivities = from scheduledActivity in GetScheduledActivities(instanceID)
                                          where scheduledActivity.Activity.Name == activityName
                                                  && scheduledActivity.InstanceID == instanceID
                                          select scheduledActivity;
                if (scheduledActivities.Count() > 0)
                {
                    activityFound = true;
                    break;
                }
            }
            return activityFound;
        }

        /// <summary>
        /// Gets the scheduled activities for an instanc
        /// </summary>
        /// <param name="instanceid"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.X2ScheduledActivity> GetScheduledActivities(Int64 instanceid)
        {
            var scheduledActivities = from scheduledActivity in GetScheduledActivities()
                                      where scheduledActivity.InstanceID == instanceid
                                      select scheduledActivity;

            foreach (var scheduledActivity in scheduledActivities)
            {
                scheduledActivity.Activity = GetActivity(scheduledActivity.ActivityID);
            }
            return scheduledActivities;
        }

        /// <summary>
        /// Gets the latest workflows
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.X2Workflow> GetLatestWorkflows()
        {
            return (from workflow in GetWorkflows() select workflow);
        }

        public IEnumerable<Automation.DataModels.X2Activity> GetManyActivitiesByManyLatestWorkflows()
        {
            var workflows = (from w in GetLatestWorkflows()
                             select w);

            var activities = new List<Automation.DataModels.X2Activity>();
            foreach (var workflow in workflows)
                activities.AddRange(GetManyActivitiesBySingleLatestWorkflow(workflow.Name));

            return activities;
        }

        public List<Automation.DataModels.X2Activity> GetManyActivitiesBySingleLatestWorkflow(string workflowName)
        {
            var workflow = (from w in GetLatestWorkflows()
                            where w.Name.Equals(workflowName)
                            select w).FirstOrDefault();

            var activities = base.GetActivities(workflow).ToList();

            return (from a in activities
                    join s in base.GetStates(workflow)
                        on a.StateID equals s.ID
                    where s.WorkFlowID == workflow.Id
                    select new Automation.DataModels.X2Activity(a, workflow, s)).ToList();
        }

        /// <summary>
        /// Waits 30 seconds for a case to be created in the given valuations state where the instance is linked to the sourceInstanceID provided to the method.
        /// </summary>
        /// <param name="sourceInstanceID">The Application Management source instance</param>
        /// <param name="expectedValuationState">The state you are expecting your case to be at</param>
        /// <param name="offerKey">OfferKey</param>
        public void WaitForValuationsCaseCreate(Int64 sourceInstanceID, int offerKey, string expectedValuationState)
        {
            var timer = new SimpleTimer(TimeSpan.FromSeconds(90));
            bool b = false;
            while (!timer.Elapsed)
            {
                var r = GetValuationsInstanceDetailsBySourceInstanceAndState(sourceInstanceID, offerKey, expectedValuationState);
                if (r.HasResults)
                {
                    b = true;
                    break;
                }
            }
            if (!b)
            {
                throw new WatiNException("Valuations case not created after waiting 90 seconds");
            }
        }

        /// <summary>
        /// Checks if a Valuation Clone exists at the Valuation Hold state for an application in the Application Management workflow.
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="states">A list of states to check at for the clone</param>
        /// <param name="workflow"></param>
        /// <returns>True = Val. Clone Exists, False = Val. Clone does not exist</returns>
        public bool CloneExists(int offerKey, string[] states, string workflow)
        {
            bool exists = false;
            var results = new QueryResults();
            foreach (string state in states)
            {
                results = base.GetInstanceCloneByState(offerKey.ToString(), state, workflow);
                if (results.RowList.Count() > 0)
                {
                    exists = true;
                    break;
                }
            }
            results.Dispose();
            return exists;
        }

        ///<summary>
        ///</summary>
        ///<param name="records"></param>
        ///<returns></returns>
        ///<exception cref="WatiNException"></exception>
        public QueryResults GetOpenApplicationManagementOffers(int records)
        {
            return base.GetOpenApplicationManagementOffers(WorkflowStates.ApplicationManagementWF.ManageApplication, TestUsers.NewBusinessProcessor, records);
        }

        public int GetAppManOffers_FilterByValuationsAndWorkflowHistory(string workflowHistoryState, int stateFilterType, int valuationFilterType,
                    int requiresValuationFlag, params int[] offerTypeKeys)
        {
            var results = base.GetAppManOffers_FilterByValuationsAndWorkflowHistory(workflowHistoryState, stateFilterType, valuationFilterType,
                requiresValuationFlag, offerTypeKeys);
            var offerKey = (from r in results select r.Column(0).GetValueAs<int>()).FirstOrDefault();
            return offerKey;
        }

        /// <summary>
        /// Get Offers on sahl\nbpuser's worklist at 'Manage Application' state which do not have a FollowUp clone and do not exist in the
        /// test.OffersAtApplicationCapture table
        /// </summary>
        /// <returns>The first qualifying OfferKey</returns>
        public int GetAppManOfferKey_FilterByClone(string state, string adUser, int includeExclude, string filterByCloneState)
        {
            var offerKey = (from r in base.GetOffers_FilterByClone(Workflows.ApplicationManagement, state, adUser, includeExclude, filterByCloneState,
                "test.OffersAtApplicationCapture")
                            select r.Column(0).GetValueAs<int>()).FirstOrDefault();
            return offerKey;
        }

        public int GetAppManOfferKey_FilterByClone(string state, string adUser, int includeExclude, string filterByCloneState, int requireValuationFlag,
            params int[] offerTypeKeys)
        {
            var offerKey = (from r in base.GetOffers_FilterByClone(Workflows.ApplicationManagement, state, adUser, includeExclude, filterByCloneState,
                "[2am].test.OffersAtApplicationCapture", requireValuationFlag, offerTypeKeys)
                            select r.Column(0).GetValueAs<int>()).FirstOrDefault();
            return offerKey;
        }

        /// <summary>
        /// Get Offers that dont have a Lightstone PropertyID
        /// </summary>
        /// <param name="workflow">CommonData.Constants.WorkflowName...</param>
        /// <param name="state">CommonData.Constants.WorkflowStates...</param>
        /// <returns>First qualifying OfferKey</returns>
        public int GetOffersWithoutLightstonePropertyID(string workflow, string state)
        {
            int offerKey = (from r in base.GetOffersWithoutLightstonePropertyID(workflow, state)
                            select r.Column(0).GetValueAs<int>()).FirstOrDefault();
            return offerKey;
        }

        public int GetAppManInstanceIDByOfferKey(int offerKey)
        {
            int instanceID = (from r in base.GetAppManInstanceDetails(offerKey, false)
                              select r.Column("InstanceID").GetValueAs<int>()).FirstOrDefault();
            return instanceID;
        }

        public IEnumerable<Automation.DataModels.Offer> GetOffersAtStateWithAmendedValuation(string state, string workflow)
        {
            return base.GetValuationOffersAtState(true, state);
        }

        public IEnumerable<Automation.DataModels.Offer> GetOffersAtStateWithoutAmendedValuation(string state, string workflow)
        {
            return base.GetValuationOffersAtState(false, state);
        }

        public Automation.DataModels.Offer GetPersonalLoanOfferAtState(string stateName, bool hasSAHLLife, bool hasExternalLife)
        {
            return base.GetPersonalLoanOfferAtState(stateName, hasSAHLLife, hasExternalLife);
        }

        public void UpdateAlphaHousingSurveyEmailSent(int offerKey, bool alphaHousingSurveyEmailSent)
        {
            int instanceID = (from r in base.GetAppManInstanceDetails(offerKey)
                              select r.Column("InstanceID").GetValueAs<int>()).FirstOrDefault();

            base.UpdateAlphaHousingSurveyEmailSent(instanceID, alphaHousingSurveyEmailSent);
        }

        public Automation.DataModels.Offer GetOfferWithOfferAttributeAtState(WorkflowEnum workflow, string applicationManagementStateName, OfferAttributeTypeEnum offerAttributeTypeKey, bool offerAttributeExists)
        {
            var result = (from offer in GetOfferWithOfferAttributeAtState(workflow, applicationManagementStateName, offerAttributeTypeKey)
                          where offerAttributeExists ? offer.OfferAttribute != null : offer.OfferAttribute == null
                          select offer).SelectRandom();
            return result;
        }

        public Automation.DataModels.DisabilityClaim GetDisabilityClaimAtState(string state)
        {
            var disabilityClaim = (from dc in base.GetDisabilityClaimsAtState(state) select dc).SelectRandom();
            return disabilityClaim;
        }
    }
}