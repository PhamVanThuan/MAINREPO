using Automation.DataModels;
using Common.Constants;
using Common.Enums;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="offerRoleType"></param>
        /// <returns></returns>
        public QueryResults GetLatestWorkFlowAssignment(int offerKey, OfferRoleTypeEnum offerRoleType)
        {
            string query = String.Format(@"Declare @OfferKey varchar(10),
										   @OfferRoleTypeKey int,
										   @Workflow varchar(50)
										   Set @OfferKey =  '{0}'
										   Set @OfferRoleTypeKey = {1}
										Select max(w.id) as ID , w.IID, w.GSKey, w.ADUserName, w.ORT, w.OfferRoleTypeKey, w.Description, w.StateName
											from x2.x2.Instance i with (nolock)
											join x2.dbo.vw_WFAssignment w with (nolock) on i.id = w.iid and i.name = @OfferKey
											where OfferRoleTypeKey = @OfferRoleTypeKey
											Group By w.ID, w.IID, w.GSKey, w.ADUserName, w.ORT, w.OfferRoleTypeKey, w.Description, w.StateName order by w.id desc",
                                                 offerKey, (int)offerRoleType);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// returns the instance data for an offer in application capture
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <returns></returns>
        public QueryResults GetAppCapInstanceDetails(int offerKey)
        {
            string query =
                string.Format(@"select ac.*, s.name as StateName, w.name as WorkflowName from
								x2.x2data.application_capture ac with (nolock)
								join x2.x2.instance i with (nolock) on ac.instanceid=i.id
								join x2.x2.state s with (nolock) on i.stateid=s.id
								join x2.x2.workflow w with (nolock) on s.workflowid=w.id
								where ac.applicationkey={0} and parentInstanceid is null", offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// returns the instance data for an disability claim
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <returns></returns>
        public QueryResults GetDisabilityClaimInstanceDetails(int disabilityClaimKey)
        {
            string query =
                string.Format(@"SELECT
	                            x2dc.*,
	                            s.name AS StateName,
	                            w.name AS WorkflowName
                            FROM [X2].[X2Data].Disability_Claim x2dc
	                            INNER JOIN [X2].[X2].Instance i ON x2dc.InstanceID = i.Id
	                            INNER JOIN [X2].[X2].State s ON  i.StateID = s.Id
	                            INNER JOIN [X2].[X2].Workflow w ON s.WorkFlowId = w.Id
                            WHERE x2dc.DisabilityClaimKey = {0} ", disabilityClaimKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>

        /// Get the time value for a Scheduled Activity
        /// </summary>
        /// <param name="instanceName">OfferKey</param>
        /// <param name="activityName">The Scheduled Activity Name</param>
        /// <returns></returns>
        public QueryResults GetScheduledActivityTime(string instanceName, string activityName)
        {
            string query =
                string.Format(@"Select sa.time  as SCHEDULE,
				DATEPART(hh,sa.Time) as HOUR,
				DATEPART(mi,sa.Time) as MINUTE,
                sa.instanceID as INSTANCEID
				From X2.X2.Instance i with (nolock)
				LEFT JOIN X2.X2.Instance clone with (nolock)
				on i.id=clone.parentinstanceid
				JOIN X2.X2.ScheduledActivity sa with (nolock)
				On i.ID=sa.InstanceID OR clone.ID=sa.instanceID
				JOIN X2.X2.Activity a with (nolock) On sa.ActivityID=a.ID and a.Name='{0}'
				where i.Name='{1}'", activityName, instanceName);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Identifies and returns the instance data for an offer in Application Management
        /// </summary>
        /// <param name = "offerKey">OfferKey used to identify the relevant Instance records</param>
        /// <returns>Returns the Instance data for an offer in Application Management</returns>
        public QueryResults GetAppManInstanceDetails(int offerKey, bool includeClones = false)
        {
            string parentInstanceIDfilter = includeClones ? string.Empty : "and i.ParentInstanceID is null";
            string query =
                string.Format(@"SELECT i.ID, i.WorkFlowID, i.ParentInstanceID, i.Name, i.Subject, i.WorkFlowProvider, i.StateID, i.CreatorADUserName,
								i.CreationDate, i.StateChangeDate, i.DeadlineDate, i.ActivityDate, i.ActivityADUserName, i.ActivityID, i.Priority,
								i.SourceInstanceID, i.ReturnActivityID, w.ProcessID, w.WorkFlowAncestorID, w.Name AS WorkFlowName, w.CreateDate, w.StorageTable,
								w.StorageKey, w.IconID, w.DefaultSubject, w.GenericKeyTypeKey, s.WorkFlowID AS StateWorkFlowID,s.Name AS StateName, s.Type,
								s.ForwardState, s.Sequence, s.ReturnWorkflowID, s.ReturnActivityID AS StateReturnActivityID, c.InstanceID, c.ApplicationKey,
								c.PreviousState, c.GenericKey, c.CaseOwnerName, c.IsFL, c.EWorkFolderID, c.IsResub, c.OfferTypeKey, c.AppCapIID, c.Requirevaluation,
                                c.AlphaHousingSurveyEmailSent
								FROM
								x2.x2data.application_management c with (nolock)
								join x2.x2.instance i with (nolock) on c.instanceid=i.id
								join x2.x2.state s with (nolock) on i.stateid=s.id
								join x2.x2.workflow w with (nolock) on s.workflowid=w.id
								where c.applicationkey = {0} {1}", offerKey, parentInstanceIDfilter);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Identifies and returns the instance data for an offer in Valuations
        /// </summary>
        /// <param name = "offerKey">OfferKey used to identify the relevant Instance records</param>
        /// <returns>Returns the Instance data for an offer in Valuations</returns>
        public QueryResults GetValuationsInstanceDetails(int offerKey)
        {
            string query =
                string.Format(@"SELECT     TOP 1 i.ID, i.WorkFlowID, i.ParentInstanceID, i.Name, i.Subject, i.WorkFlowProvider,
								i.StateID, i.CreatorADUserName, i.CreationDate, i.StateChangeDate, i.DeadlineDate, i.ActivityDate,
								i.ActivityADUserName, i.ActivityID, i.Priority, i.SourceInstanceID, i.ReturnActivityID, w.ProcessID,
								w.WorkFlowAncestorID, w.Name AS WorkFlowName, w.CreateDate, w.StorageTable, w.StorageKey, w.IconID, w.DefaultSubject,
								w.GenericKeyTypeKey, s.WorkFlowID AS StateWorkFlowID, s.Name AS StateName, s.Type, s.ForwardState, s.Sequence,
								s.ReturnWorkflowID, s.ReturnActivityID AS StateReturnActivityID, c.InstanceID, c.ApplicationKey, c.PropertyKey, c.Withdrawn,
								c.RequestingAdUser, c.AdcheckPropertyID, c.AdcheckValuationID, c.ValuationKey, c.AdCheckValuationIDStatus, c.EntryPath,
								c.OnManagerWorkList, c.nLoops
								FROM
								x2.x2data.valuations c with (nolock)
								join x2.x2.instance i with (nolock) on c.instanceid=i.id
								join x2.x2.state s with (nolock) on i.stateid=s.id
								join x2.x2.workflow w with (nolock) on s.workflowid=w.id
								where c.applicationkey = {0} and i.ParentInstanceid is null
								order by 1 desc", offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Identifies and returns the instance data for an offer in Valuations
        /// </summary>
        /// <param name = "offerKey">OfferKey used to identify the relevant Instance records</param>
        /// <returns>Returns the Instance data for an offer in Valuations</returns>
        public IEnumerable<Automation.DataModels.Valuation> GetValidValuationInstances(string offerKey)
        {
            string query = String.Format(@"SELECT * FROM x2.x2data.valuations");
            return dataContext.Query<Automation.DataModels.Valuation>(query);
        }

        /// <summary>
        ///   Checks if a particular offer is currently at the Application Capture state
        /// </summary>
        /// <param name = "offerKey">OfferKey</param>
        /// <param name="workflowState"></param>
        /// <returns></returns>
        public bool OfferExistsAtState(int offerKey, string workflowState)
        {
            string query = string.Format(@"SELECT *
                           FROM X2.X2.State with (nolock) INNER JOIN
                           X2.X2.Instance with (nolock) ON X2.X2.State.ID = X2.X2.Instance.StateID
                           Where X2.X2.Instance.Name = '{0}' and X2.X2.State.Name = '{1}'", offerKey, workflowState);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.HasResults;
        }

        /// <summary>
        ///   Checks what User State a particular offer is currently at in the Application Management map
        /// </summary>
        /// <param name = "offerKey">OfferKey</param>
        /// <returns>State Name (if Offer exists in Application Management map), false (if Offer NOT in Application Management map)</returns>
        public QueryResults SearchApplicationManagementStates(string offerKey)
        {
            string query =
                @"if exists
										(SELECT *
										FROM X2.X2.State with (nolock)  INNER JOIN
										X2.X2.Instance with (nolock) ON X2.X2.State.ID = X2.X2.Instance.StateID INNER JOIN
										X2.X2DATA.Application_Management with (nolock) ON X2.X2.Instance.ID = X2.X2DATA.Application_Management.InstanceID
										WHERE     (X2.X2DATA.Application_Management.ApplicationKey = '" +
                offerKey +
                @"') and X2.X2.State.Type = 1)
									begin
										(SELECT X2.X2.State.Name
										FROM X2.X2.State with (nolock) INNER JOIN
										X2.X2.Instance with (nolock) ON X2.X2.State.ID = X2.X2.Instance.StateID INNER JOIN
										X2.X2DATA.Application_Management with (nolock) ON X2.X2.Instance.ID = X2.X2DATA.Application_Management.InstanceID
										WHERE     (X2.X2DATA.Application_Management.ApplicationKey = '" +
                offerKey +
                @"') and X2.X2.State.Type = 1)
									end
								else
									begin
										select 'Other'
									end;";
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Retrieves the details for an instance in the Readvance Payments workflow.
        /// </summary>
        /// <param name = "offerKey">OfferKey</param>
        /// <returns></returns>
        public QueryResults GetReadvancePaymentsInstanceDetails(int offerKey)
        {
            string query =
                string.Format(@"select top 1 *,s.name as StateName
                                from x2.x2data.readvance_payments ac
								join x2.x2.instance i with (nolock) on ac.instanceid=i.id
								join x2.x2.state s with (nolock) on i.stateid=s.id
								join x2.x2.workflow w with (nolock) on s.workflowid=w.id
								where ac.applicationkey = {0} and ParentInstanceID is null order by 1 desc", offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Retrieves the details for an instance in the Credit Workflow.
        /// </summary>
        /// <param name = "offerKey">OfferKey</param>
        /// <returns></returns>
        public QueryResults GetCreditInstanceDetails(int offerKey)
        {
            string query =
                string.Format(@"select top 1 i.ID, i.WorkFlowID, i.ParentInstanceID, i.Name, i.Subject, i.WorkFlowProvider, i.StateID,
								i.CreatorADUserName, i.CreationDate, i.StateChangeDate, i.DeadlineDate, i.ActivityDate, i.ActivityADUserName,
								i.ActivityID, i.Priority, i.SourceInstanceID, i.ReturnActivityID, w.ProcessID, w.WorkFlowAncestorID,
								w.Name AS WorkFlowName, w.CreateDate, w.StorageTable, w.StorageKey, w.IconID, w.DefaultSubject, w.GenericKeyTypeKey,
								s.WorkFlowID AS StateWorkFlowID, s.Name AS StateName, s.Type, s.ForwardState, s.Sequence, s.ReturnWorkflowID,
								s.ReturnActivityID AS StateReturnActivityID, c.InstanceID, c.ApplicationKey, c.IsResub, c.ActionSource, c.PreviousState,
								c.ReviewRequired, c.StopRecursing, c.EntryPath, c.ExceptionsDeclineWithOffer, c.PolicyOverride
								from x2.x2data.Credit c (nolock)
								join x2.x2.instance i (nolock) on c.instanceid=i.id
								join x2.x2.state s (nolock) on i.stateid=s.id
								join x2.x2.workflow w (nolock) on s.workflowid=w.id
								where c.applicationkey = {0}
								and ParentInstanceID is null order by i.id desc", offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="state"></param>
        /// <param name="workflow"></param>
        /// <returns></returns>
        public QueryResults GetInstanceCloneByState(string instanceName, string state, string workflow)
        {
            string tableName = string.Empty;
            string matchKey = string.Empty;

            switch (workflow)
            {
                case Workflows.ApplicationCapture:
                    tableName = "x2.x2data.Application_Capture";
                    matchKey = "ApplicationKey";
                    break;

                case Workflows.ApplicationManagement:
                    tableName = "x2.x2data.Application_Management";
                    matchKey = "ApplicationKey";
                    break;

                case Workflows.Credit:
                    tableName = "x2.x2data.Credit";
                    matchKey = "ApplicationKey";
                    break;

                case Workflows.ReadvancePayments:
                    tableName = "x2.x2data.Readvance_Payments";
                    matchKey = "ApplicationKey";
                    break;

                case Workflows.Valuations:
                    tableName = "x2.x2data.Valuations";
                    matchKey = "ApplicationKey";
                    break;

                case Workflows.CAP2Offers:
                    tableName = "x2.x2data.cap2_offers";
                    matchKey = "CapOfferKey";
                    break;

                case Workflows.LifeOrigination:
                    tableName = "x2.x2data.lifeorigination";
                    matchKey = "OfferKey";
                    break;

                case Workflows.DebtCounselling:
                    tableName = "x2.x2data.debt_counselling";
                    matchKey = "DebtCounsellingKey";
                    break;
            }
            
            string query = String.Format(@" SELECT 
	                                            *,
	                                            i.ID AS ClonedInstanceID 
                                            FROM [X2].[X2].Instance i (NOLOCK)
	                                            INNER JOIN [X2].[X2].State s (NOLOCK) ON s.ID = i.StateID
                                            WHERE s.Name = '{0}'
                                            AND i.ParentInstanceID IN 
                                            (
	                                            SELECT
		                                            i.ID 
	                                            FROM {1} data
		                                            INNER JOIN [X2].[X2].Instance i ON i.ID = data.InstanceID
	                                            WHERE data.{2} = {3}
	                                            AND i.ParentInstanceID IS NULL
                                            )", state, tableName, matchKey, instanceName);

            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Clone of the above one, but this one only picks the top 1 and orders by creation date. This is if you have multiple follow up instances so the test always picks the latest ones
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="state"></param>
        /// <param name="workflow"></param>
        /// <returns></returns>
        public QueryResults GetTop1InstanceCloneByState(string instanceName, string state, string workflow)
        {
            string tableName = string.Empty;
            switch (workflow)
            {
                case Workflows.ApplicationCapture:
                    tableName = "x2.x2data.Application_Capture";
                    break;

                case Workflows.ApplicationManagement:
                    tableName = "x2.x2data.Application_Management";
                    break;

                case Workflows.Credit:
                    tableName = "x2.x2data.Credit";
                    break;

                case Workflows.ReadvancePayments:
                    tableName = "x2.x2data.Readvance_Payments";
                    break;

                case Workflows.Valuations:
                    tableName = "x2.x2data.Valuations";
                    break;

                case Workflows.CAP2Offers:
                    tableName = "x2.x2data.cap2_offers";
                    break;

                case Workflows.LifeOrigination:
                    tableName = "x2.x2data.lifeorigination";
                    break;

                case Workflows.DebtCounselling:
                    tableName = "x2.x2data.debt_counselling";
                    break;
            }
            string query =
                String.Format(@"select top 1 *,i.ID as ClonedInstanceID from x2.x2.instance i (nolock)
							join x2.x2.workflow w with (nolock)  on i.workflowid=w.id
							join x2.x2.state s with (nolock)  on i.stateid=s.id
							where s.name = '{0}' and
							ParentInstanceID in (select i.id from x2.x2.instance i (nolock)
							join x2.x2.workflow w with (nolock)  on i.workflowid=w.id
							join x2.x2.state s with (nolock)  on i.stateid=s.id
							join {1} ac (nolock) on i.id=ac.instanceid
							where i.name = '{2}'
						    and ParentInstanceID is null)
                            order by CreationDate desc", state, tableName, instanceName);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Retrieves the Scheduled Activity details of a specific Activity for an instance
        /// </summary>
        /// <param name = "offerKey">OfferKey</param>
        /// <param name = "activityName">Name of the Activity</param>
        /// <returns>ScheduledActivity.Time as SCHEDULE, DATEPART(hh,isnull(sa.Time)) as HOUR and DATEPART(mi,isnull(sa.Time)) as MINUTE</returns>
        public QueryResults GetScheduledActivityTimeForInstance(int offerKey, string activityName)
        {
            string query = string.Format(@"Select sa.Time as SCHEDULE, DATEPART(hh,sa.Time) as HOUR, DATEPART(mi,sa.Time) as MINUTE
						From X2.X2.Instance i with (nolock)
						LEFT JOIN X2.X2.ScheduledActivity sa with (nolock) On i.ID=sa.InstanceID
						LEFT JOIN X2.X2.Activity a with (nolock) On sa.ActivityID=a.ID
						Where i.Name='{0}' AND (a.Name='{1}')", offerKey, activityName);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="activityName"></param>
        /// <returns></returns>
        public QueryResults GetScheduledActivityTimeForCloneInstance(int offerKey, string activityName)
        {
            string query =
                string.Format(@"Select sa_clone.time as SCHEDULE, DATEPART(hh,sa_clone.time) as HOUR, DATEPART(mi,sa_clone.time) as MINUTE
				From X2.X2.Instance i with (nolock)
				INNER JOIN X2.X2.Instance clone with (nolock) on i.id=clone.parentinstanceid
				LEFT JOIN X2.X2.ScheduledActivity sa_clone with (nolock) On clone.ID=sa_clone.InstanceID
				LEFT JOIN X2.X2.Activity a_clone  with (nolock) On sa_clone.ActivityID=a_clone.ID
				Where i.Name='{0}' AND (a_clone.Name='{1}')", offerKey, activityName);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="offerkey"></param>
        /// <param name="stateName"></param>
        /// <returns></returns>
        public QueryResults GetX2WorkFlowAssignment_ByStateName(int offerkey, string stateName)
        {
            string query =
                @"select w.ID, w.InstanceID, w.GeneralStatusKey, a.ADUserName, ort.OfferRoleTypeKey, ort.Description, s.Name, s.type from
								(select * from x2.x2.instance i (nolock)
								where i.name = '" +
                offerkey +
                @"' and i.parentinstanceid is null
								union
								select clone.* from
								x2.x2.instance i (nolock) join
								x2.x2.instance clone (nolock) on i.id = clone.parentinstanceid
								where i.name = '" +
                offerkey +
                @"' and i.parentinstanceid is null) i join
							x2.x2.state s (nolock) on i.stateid = s.id join
							x2.x2.WorkflowAssignment w (nolock) ON i.ID = w.InstanceID join
							[2am].dbo.aduser a (nolock) on w.aduserkey = a.aduserkey join
							[2am].dbo.offerroletypeorganisationstructuremapping blah (nolock) on w.offerroletypeorganisationstructuremappingkey = blah.offerroletypeorganisationstructuremappingkey join
							[2am].dbo.offerroletype ort (nolock) on blah.offerroletypekey = ort.offerroletypekey";

            if (stateName != null)
            {
                query = query + @" Where s.name in ('" + stateName + @"')";
            }
            query = query +
                    @" Group By w.ID, w.InstanceID, w.GeneralStatusKey, a.ADUserName, ort.OfferRoleTypeKey, ort.Description, s.Name, s.type
							order by w.GeneralStatusKey, w.ID desc";
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Identifies and returns the instance data for a Life offer.
        /// </summary>
        /// <param name = "offerKey">OfferKey used to identify the relevant Instance records</param>
        /// <returns>Returns the Instance data for an offer in LifeOrigination</returns>
        public IEnumerable<LifeLead> GetLifeInstanceDetails(int offerKey, int accountkey)
        {
            var query = string.Format(@"
                           select la.Offerkey, s.Name as StateName, i.creationdate, i.statechangedate, i.ParentInstanceID
                           from x2.x2.instance i (nolock)
                                join x2.x2.workflow w with (nolock)  on i.workflowid=w.id
                                join x2.x2.state s with (nolock)  on i.stateid=s.id
                                join x2.x2data.lifeorigination la (nolock) on i.id=la.instanceid
                           where (la.offerkey = '{0}' or la.loannumber = '{1}')
                                and i.ParentInstanceID is null"
                , offerKey, accountkey);
            return dataContext.Query<LifeLead>(query);
        }

        /// <summary>
        ///   Gets the OfferKey of an application in a particular state when provided with the Account Key
        /// </summary>
        /// <param name = "accountKey">Mortgage Loan Account</param>
        /// <param name = "state">Workflow State</param>
        /// <returns></returns>
        public QueryResults GetOfferKeyByAccountKeyAndState(string accountKey, string state)
        {
            string query = string.Format(@"select * from offer o (nolock)
                           join x2.x2data.application_management data (nolock)  on o.offerkey=data.applicationkey
                           join x2.x2.instance i (nolock) on data.instanceid=i.id
                           join x2.x2.state s (nolock) on i.stateid=s.id
                           where accountkey= {0}
                           and offerstatuskey=1
                           and s.name= '{1}'", accountKey, state);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Gets the worklist record for an application management instance at a particular state
        /// </summary>
        /// <param name = "genericKey">OfferKey</param>
        /// <param name="state">Workflow State</param>
        /// <param name="workflow">Workflow Name</param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.X2WorkflowList> GetWorklistDetails(int genericKey, string state, string workflow)
        {
            string table = String.Empty;
            switch (workflow)
            {
                case Workflows.ApplicationManagement:
                    table = "x2.x2data.application_management";
                    break;

                case Workflows.ApplicationCapture:
                    table = "x2.x2data.application_capture";
                    break;

                case Workflows.ReadvancePayments:
                    table = "x2.x2data.readvance_payments";
                    break;

                case Workflows.Credit:
                    table = "x2.x2data.credit";
                    break;

                case Workflows.CAP2Offers:
                    table = "x2.x2data.cap2_offers";
                    break;

                case Workflows.PersonalLoans:
                    table = "x2.X2DATA.Personal_Loans";
                    break;

                case Workflows.DebtCounselling:
                    table = "x2.x2data.debt_counselling";
                    break;

                case Workflows.Valuations:
                    table = "x2.x2data.valuations";
                    break;
            }
            string query = string.Empty;
            if (workflow == Workflows.DebtCounselling)
            {
                query = string.Format(@"select wl.*, s.name as StateName from {0} data (nolock)
                        join x2.x2.instance i (nolock) on data.instanceid=i.id
                        join x2.x2.state s (nolock) on i.stateid=s.id
                        join x2.x2.worklist wl  (nolock) on data.instanceid=wl.instanceid
                        where data.debtCounsellingKey = {1}", table, genericKey);
            }
            else
            {
                query = string.Format(@"select wl.*, s.name as StateName from {0} data (nolock)
                        join x2.x2.instance i (nolock) on data.instanceid=i.id
                        join x2.x2.state s (nolock) on i.stateid=s.id
                        join x2.x2.worklist wl  (nolock) on data.instanceid=wl.instanceid
                        where data.applicationKey = {1}", table, genericKey);
            }
            var worklist = dataContext.Query<Automation.DataModels.X2WorkflowList>(query);
            if (!string.IsNullOrEmpty(state))
            {
                return (from w in worklist where w.StateName == state select w);
            }
            return worklist;
        }

        /// <summary>
        ///   Calls the test.GetCasesInWorklistByStateAndType SP
        /// </summary>
        /// <param name = "state">State.Description</param>
        /// <param name = "workflow">Workflow.Description</param>
        /// <param name = "offerTypeKey">Offer.OfferTypeKey</param>
        /// <param name = "exclusions">List of offers to exclude</param>
        /// <returns>
        ///   Offer.OfferKey, Offer.OfferStartDate, OfferType.Description, Product.Description,
        ///   Category.Description, LTV, LAA, PTI
        /// </returns>
        public QueryResults GetApplicationsByStateAndAppType(string state, string workflow, string exclusions,
                                                             int offerTypeKey)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "[2am].test.GetCasesInWorklistByStateAndType" };
            proc.AddParameter(new SqlParameter("@State", state));
            proc.AddParameter(new SqlParameter("@WorkFlow", workflow));
            proc.AddParameter(new SqlParameter("@OfferTypeKey", offerTypeKey.ToString()));
            proc.AddParameter(new SqlParameter("@Exclusions", exclusions));
            return dataContext.ExecuteStoredProcedureWithResults(proc);
        }

        /// <summary>
        ///   Calls the test.GetCasesInWorklistByStateAndType SP
        /// </summary>
        /// <param name = "state">State.Description</param>
        /// <param name = "workflow">Workflow.Description</param>
        /// <param name = "offerTypeKey">Offer.OfferTypeKey</param>
        /// <param name = "exclusions">List of offers to exclude</param>
        /// <param name="maxLTV">The maximum LTV for this case</param>
        /// <param name="numLE">The maximum number of LE's on the application</param>
        /// <returns>
        ///   Offer.OfferKey, Offer.OfferStartDate, OfferType.Description, Product.Description,
        ///   Category.Description, LTV, LAA, PTI
        /// </returns>
        public QueryResults GetApplicationsByStateAndAppType(string state, string workflow, string exclusions, string offerTypeKey, double maxLTV, int numLE)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "test.GetCasesInWorklistByStateAndTypeAndLTV" };
            proc.AddParameter(new SqlParameter("@State", state));
            proc.AddParameter(new SqlParameter("@WorkFlow", workflow));
            proc.AddParameter(new SqlParameter("@OfferTypeKey", offerTypeKey));
            proc.AddParameter(new SqlParameter("@Exclusions", exclusions));
            proc.AddParameter(new SqlParameter("@maxLTV ", (maxLTV).ToString()));
            proc.AddParameter(new SqlParameter("@maxLECount", numLE.ToString()));
            return dataContext.ExecuteStoredProcedureWithResults(proc);
        }

        /// <summary>
        ///   Get Offers on ADUSer's worklist at the particular State.  Includes the ability to exclude offers in a specified test data table
        ///   and to filter (include/exclude) offers with a clone identified by clone state
        /// </summary>
        /// <param name = "workflow">workflow.name</param>
        /// <param name = "state">state.name</param>
        /// <param name = "adUser">aduser.adusername</param>
        /// <param name = "includeExclude">Include = 0, Exclude = 1 (default)</param>
        /// <param name = "filterByCloneState">state.name</param>
        /// <param name = "excludeOffersInTable">test.[tablename]</param>
        /// <returns>instance.name</returns>
        public QueryResults GetOffers_FilterByClone(string workflow, string state, string adUser, int includeExclude,
                                                    string filterByCloneState, string excludeOffersInTable)
        {
            return GetOffers_FilterByClone(workflow, state, adUser, includeExclude,
                                                    filterByCloneState, excludeOffersInTable, -1);
        }

        /// <summary>
        ///   Get Offers greater than or less than given date
        /// </summary>
        /// <param name = "workflow">workflow.name</param>
        /// <param name = "state">state.name</param>
        /// <param name = "greaterOrLessThanDate">'&gt;' or '&lt;' defaults to '&lt;'</param>
        /// <param name = "date">yyyy-mm-dd</param>
        /// <returns>instance.name, max(valuation.valuationdate)</returns>
        public QueryResults GetOffersByLatestLightstoneValuationsDate(string workflow, string state,
                                                                      char greaterOrLessThanDate, string date)
        {
            if (greaterOrLessThanDate != '>' && greaterOrLessThanDate != '<') greaterOrLessThanDate = '<';

            string query =
                @"Declare @Workflow varchar(50),
							@State varchar(50),
							@ValuationGreaterOrLessThan2Months varchar(1),
							@HasExistingValuation bit,
							@ValuationDate datetime

							Set @Workflow = '" +
                workflow + @"'
							Set @State = '" + state +
                @"'
							Set @ValuationGreaterOrLessThan2Months = '" + greaterOrLessThanDate +
                @"'
							Set @ValuationDate = '" + date +
                @"'

									if @ValuationGreaterOrLessThan2Months = '>'
										begin
											Select i.name, max(v.valuationdate)
																			FROM     [X2].[X2].[Worklist] wl WITH (NOLOCK)
																					 JOIN [X2].[X2].[Instance] i WITH (NOLOCK)
																					   ON wl.instanceid = i.id
																			join x2.x2.workflow w (nolock) on i.workflowid = w.id
																			join x2.x2.state s (nolock) on i.stateid = s.id
																			join [2am]..offer o (nolock) on i.name = convert(varchar(10), o.offerkey)
																			join [2am]..offermortgageloan oml (nolock) on o.offerkey = oml.offerkey
																			join [2am]..property p (nolock) on oml.propertykey = p.propertykey
																			join [2am]..valuation v (nolock) on  p.propertykey = v.propertykey
																			left join propertydata pd (nolock) on pd.propertydataproviderdataservicekey = 1 and p.propertykey = pd.propertykey
																			WHERE
																				w.name = 'Application Management'
																				and s.name = 'Manage Application'
																				and v.valuationdataproviderdataservicekey = 3 --lightstone automated valuation
																				and pd.propertydatakey is not null
																				--and wl.adusername = @ADUser
																			Group By i.name
																			Having max(v.valuationdate) > @ValuationDate
										end
									else
										begin
											Select i.name, max(v.valuationdate)
																			FROM     [X2].[X2].[Worklist] wl WITH (NOLOCK)
																					 JOIN [X2].[X2].[Instance] i WITH (NOLOCK)
																					   ON wl.instanceid = i.id
																			join x2.x2.workflow w (nolock) on i.workflowid = w.id
																			join x2.x2.state s (nolock) on i.stateid = s.id
																			join [2am]..offer o (nolock) on i.name = convert(varchar(10), o.offerkey)
																			join [2am]..offermortgageloan oml (nolock) on o.offerkey = oml.offerkey
																			join [2am]..property p (nolock) on oml.propertykey = p.propertykey
																			join [2am]..valuation v (nolock) on  p.propertykey = v.propertykey
																			left join propertydata pd (nolock) on pd.propertydataproviderdataservicekey = 1 and p.propertykey = pd.propertykey
																			WHERE
																				w.name = 'Application Management'
																				and s.name = 'Manage Application'
																				and v.valuationdataproviderdataservicekey = 3 --lightstone automated valuation
																				and pd.propertydatakey is not null
																				--and wl.adusername = @ADUser
																			Group By i.name
																			Having max(v.valuationdate) < @ValuationDate
										end";

            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Get Offers that dont have a Lightstone PropertyID
        /// </summary>
        /// <param name = "workflow">workflow.name</param>
        /// <param name = "state">state.name</param>
        /// <returns>instance.name</returns>
        public QueryResults GetOffersWithoutLightstonePropertyID(string workflow, string state)
        {
            string query =
                @"Declare @Workflow varchar(50),
							@State varchar(50)

							Set @Workflow = '" +
                workflow + @"'
							Set @State = '" + state +
                @"'

									Select i.name
									FROM     [X2].[X2].[Worklist] wl WITH (NOLOCK)
											 JOIN [X2].[X2].[Instance] i WITH (NOLOCK)
											   ON wl.instanceid = i.id
									join x2.x2.workflow w (nolock) on i.workflowid = w.id
									join x2.x2.state s (nolock) on i.stateid = s.id
									join [2am]..offer o (nolock) on i.name = convert(varchar(10), o.offerkey)
									join [2am]..offermortgageloan oml (nolock) on o.offerkey = oml.offerkey
									join [2am]..property p (nolock) on oml.propertykey = p.propertykey
									left join [2am]..propertydata pd (nolock) on pd.PropertyDataProviderDataserviceKey = 1 and p.propertykey = pd.propertykey
									WHERE
										w.name = @Workflow and
										s.name = @State and
										pd.PropertyID is null
									Group By i.name";

            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Get Offers on ADUSer's worklist at the particular State.  Includes the ability to exclude offers in a specified test data table
        ///   and to filter (include/exclude) offers with a clone identified by clone state
        /// </summary>
        /// <param name = "workflow">workflow.name</param>
        /// <param name = "state">state.name</param>
        /// <param name = "adUser">aduser.adusername</param>
        /// <param name = "includeExclude">Include = 0, Exclude = 1 (default)</param>
        /// <param name = "filterByCloneState">state.name</param>
        /// <param name = "excludeOffersInTable">test.[tablename]</param>
        /// <returns>instance.name</returns>
        public QueryResults GetOffers_FilterByClone(string workflow, string state, string adUser, int includeExclude,
                                                    string excludeOffersInTable, params string[] filterByCloneState)
        {
            string query =
                @"Declare @ADUser varchar(20),
								@Workflow varchar(50),
								@State varchar(50),
								@IncludeExclude bit,
								@FilterByCloneState varchar(50),
								@ExcludeOffersInTable varchar(100),
								@Query varchar(max),
								@Counter int,
								@SCounter varchar(2)

								Set @Workflow = " +
                Helpers.FormatStringForSQL(workflow) + @"
								Set @State = " +
                Helpers.FormatStringForSQL(state) + @"
								Set @ADUser = " +
                Helpers.FormatStringForSQL(adUser) + @"
								Set @IncludeExclude = " +
                includeExclude + @"
								Set @ExcludeOffersInTable = " +
                Helpers.FormatStringForSQL(excludeOffersInTable) +
                @"

								Declare @CloneFilters table (StateName varchar(50))";

            foreach (string CloneState in filterByCloneState)
            {
                query = query + @"Insert Into @CloneFilters (StateName) Values (" + Helpers.FormatStringForSQL(CloneState) +
                        @") ";
            }

            query = query +
                    @"Set @Query = 'Select i.name FROM X2.X2.Worklist wl (NOLOCK) JOIN X2.X2.Instance i (NOLOCK) ON i.parentinstanceid is null and wl.instanceid = i.id '

								If @Workflow is not null
									Begin
										Set @Query = @Query + 'join x2.x2.workflow w (nolock) on w.name = ''' + @Workflow + ''' and i.workflowid = w.id '
									End

								If @State is not null
									Begin
										Set @Query = @Query + 'join x2.x2.state s (nolock) on s.name = ''' + @State + ''' and i.stateid = s.id '
									End

								If Exists (select * from @CloneFilters)
									Begin
										DECLARE CloneFilters CURSOR FOR SELECT * FROM @CloneFilters
										Open CloneFilters
										Fetch CloneFilters into @FilterByCloneState
										Set @Counter = 1
										WHILE @@Fetch_Status = 0
											Begin
												Set @SCounter = Convert(varchar(2), @Counter)
												Set @Query = @Query + 'left outer join x2.x2.instance i_c' + @SCounter + ' join x2.x2.state s_c' + @SCounter + ' on s_c' + @SCounter + '.name like ''' + @FilterByCloneState + ''' and i_c' + @SCounter + '.stateid = s_c' + @SCounter + '.id on i.id = i_c' + @SCounter + '.parentinstanceid '
												Set @Counter = @Counter + 1
												Fetch CloneFilters into @FilterByCloneState
											End
										Close CloneFilters
									End

								if @FilterByCloneState is not null or @ExcludeOffersInTable is not null or @ADUser is not null
									Begin
										Set @Query = @Query + 'Where '
									End

								if @ExcludeOffersInTable is not null
									Begin
										Set @Query = @Query + 'i.name not in (Select OfferKey from ' + @ExcludeOffersInTable + ') '
									End
								--Exclude Offers with clone (default)--
								if Exists (select * from @CloneFilters) and @IncludeExclude = 1
									Begin
										Open CloneFilters
										Fetch CloneFilters into @FilterByCloneState
										Set @Counter = 1
										WHILE @@Fetch_Status = 0
											Begin
												Set @SCounter = Convert(varchar(2), @Counter)

												if @ExcludeOffersInTable is not null and @Counter = 1
													Begin
														Set @Query = @Query + 'and ('
													End
												Else if @Counter = 1
													Begin
														Set @Query = @Query + '('
													End
												if @Counter > 1
													Begin
														Set @Query = @Query + 'and '
													End
												Set @Query = @Query + 'i_c' + @SCounter + '.id is null '

												Set @Counter = @Counter + 1
												Fetch CloneFilters into @FilterByCloneState
												if @@Fetch_Status <> 0
													Begin
														Set @Query = @Query + ') '
													End
											End
										Close CloneFilters
										Deallocate CloneFilters
									End

								--Include Offers with clone--
								if Exists (select * from @CloneFilters) and @IncludeExclude = 0
									Begin
										Open CloneFilters
										Fetch CloneFilters into @FilterByCloneState
										Set @Counter = 1
										WHILE @@Fetch_Status = 0
											Begin
												Set @SCounter = Convert(varchar(2), @Counter)

												if @ExcludeOffersInTable is not null and @Counter = 1
													Begin
														Set @Query = @Query + 'and '
													End
												if @Counter > 1
													Begin
														Set @Query = @Query + 'and '
													End
												Set @Query = @Query + 'i_c' + @SCounter + '.id is not null '

												Set @Counter = @Counter + 1
												Fetch CloneFilters into @FilterByCloneState
											End
										Close CloneFilters
										Deallocate CloneFilters
									End

								If @ADUser is not null
									Begin
										if @ExcludeOffersInTable is not null or @FilterByCloneState is not null
											Begin
												Set @Query = @Query + 'and '
											End
										Set @Query = @Query + 'wl.adusername like ''%' + @ADUser + ''';'
									End

								--print @query

								Execute(@Query)";

            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflowHistoryState"></param>
        /// <param name="stateFilterType"></param>
        /// <param name="valuationFilterType"></param>
        /// <returns></returns>
        public QueryResults GetAppManOffers_FilterByValuationsAndWorkflowHistory(string workflowHistoryState,
                                                                                 int stateFilterType,
                                                                                 int valuationFilterType)
        {
            return GetAppManOffers_FilterByValuationsAndWorkflowHistory(workflowHistoryState, stateFilterType, valuationFilterType, -1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflowHistoryState"></param>
        /// <param name="stateFilterType"></param>
        /// <param name="valuationFilterType"></param>
        /// <param name="requireValuationFlag"></param>
        /// <param name="offerTypeKeys"></param>
        /// <returns></returns>
        public QueryResults GetAppManOffers_FilterByValuationsAndWorkflowHistory(string workflowHistoryState, int stateFilterType, int valuationFilterType,
                                                                                 int requireValuationFlag, params int[] offerTypeKeys)
        {
            string offerTypeKeyString = string.Empty;

            foreach (int offerTypeKey in offerTypeKeys)
                offerTypeKeyString = offerTypeKeyString + offerTypeKey.ToString() + ", ";

            if (!string.IsNullOrEmpty(offerTypeKeyString))
                offerTypeKeyString = offerTypeKeyString.Remove(offerTypeKeyString.LastIndexOf(','));
            else
                offerTypeKeyString = "6,7,8";

            string query =
                @"SELECT	i.Name, i_v1.id as 'ValuationInstance', s_v1.name as 'ValuationState', i_v2.id as 'ValuationHistoryInstance', s_h.name as 'ValuationHistoryState'
							FROM	X2.X2data.Application_Management am WITH (NOLOCK) join
										X2.X2.Instance AS i WITH (NOLOCK) on i.ParentInstanceID IS NULL and am.instanceid = i.id INNER JOIN
											X2.X2.State AS s WITH (NOLOCK) on s.Name = 'Manage Application' AND i.StateID = s.ID join
												[2am]..offer o WITH (NOLOCK) on o.offertypekey in (" + offerTypeKeyString + @") and am.applicationkey = o.offerkey left JOIN
									--check non archived valuations instances
									X2.X2data.valuations v1 WITH (NOLOCK) join
										x2.x2.instance i_v1 WITH (NOLOCK) on v1.instanceid = i_v1.id join
											x2.x2.state s_v1 WITH (NOLOCK) on s_v1.type <> 5 and i_v1.stateid = s_v1.id
												on am.applicationkey  = v1.applicationkey left join
									--check workflow history
										x2.x2.instance i_v2 WITH (NOLOCK) join
											x2.x2.workflowhistory wh WITH (NOLOCK) on i_v2.id = wh.instanceid join
												x2.x2.state s_h WITH (NOLOCK) on s_h.name = '" +
                workflowHistoryState +
                @"' and
												wh.stateid = s_h.id
											on convert(varchar(10), am.applicationkey) = i_v2.name
							where ";

            if (requireValuationFlag != -1) query = query + "am.RequireValuation = " + requireValuationFlag + @" and ";

            if (valuationFilterType == 0) query = query + @"i_v1.id is not null ";
            else query = query + @"i_v1.id is null ";

            if (stateFilterType == 0) query = query + @"and i_v2.id is not null ";
            else query = query + @"and i_v2.id is null ";

            query = query + "Order by	i.name";

            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLQuery(statement);

            return results;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sWorkflowName"></param>
        /// <param name="sStateName"></param>
        /// <returns></returns>
        public QueryResults GetX2DataByWorkflowAndState(string sWorkflowName, string sStateName, int noRows)
        {
            var top = String.Format("+'top {0}'+", noRows.ToString());
            if (noRows == 0)
                top = "++";

            var query =
                @"declare @tableName varchar(50)
				declare	@columnName varchar(50)
				declare	@maxID int;

				With Temp As
				(Select max(ID) as ID
				From x2.x2.Workflow (nolock)
				Where Name = " + Helpers.FormatStringForSQL(sWorkflowName) + @"
				group By Name)
				Select @tableName = StorageTable, @columnName = StorageKey, @maxID = w.ID
				From x2.x2.Workflow w (nolock) join
				Temp t on w.ID = t.ID

				declare @query varchar(max)

				Set @query = 'select '" + top + @"' data.' + @columnName + ' as OfferKey, i.*, w.*, s.* from x2.x2data.' + @tableName + ' as data (nolock) join
				x2.x2.instance as i (nolock) on data.instanceid = i.id join
				x2.x2.workflow w (nolock) on i.workflowid = w.id join
				x2.x2.state as s (nolock) on i.stateid = s.id
				where w.ID = ' + cast(@maxID as varchar(10)) + ' and s.name = '" + Helpers.FormatStringForSQL(sStateName) + @"''

				execute (@query)";

            return dataContext.ExecuteSQLQuery(new SQLStatement() { StatementString = query });
        }

        /// <summary>
        /// Retrieves an instance from the Loan Adjustments workflow when provided with an account number,
        /// loan adjustment type and a workflow state
        /// </summary>
        /// <param name="accountKey">Account Number</param>
        /// <param name="loanAdjustmentType">Loan Adjustment Type</param>
        /// <param name="state">Workflow State</param>
        /// <returns></returns>
        public QueryResults GetLoanAdjustmentInstance(int accountKey, int loanAdjustmentType, string state)
        {
            string query =
                String.Format(
                           @"SELECT la.* FROM X2.X2DATA.Loan_Adjustments la WITH (NOLOCK)
						   JOIN X2.X2.Instance i WITH (NOLOCK) ON la.InstanceId=i.Id
						   JOIN X2.X2.State s WITH (NOLOCK) ON i.StateId=s.Id
						   WHERE la.AccountKey= {0}
						   AND la.LoanAdjustmentType= '{1}'
						   AND s.Name= '{2}'", accountKey, loanAdjustmentType, state);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Determines whether an instance exists in the specified workflow and is on the worklist of the provided aduser at the expected state.
        /// </summary>
        /// <param name="map">Workflow Map</param>
        /// <param name="genericKey">OfferKey/AccountKey</param>
        /// <param name="state">Workflow State</param>
        /// <param name="aduserName">ADUserName</param>
        /// <returns></returns>
        public QueryResults GetWorkflowInstanceForStateAndADUser(string map, int genericKey, string state, string aduserName)
        {
            string tableName = string.Empty;
            string genericColumn = "ApplicationKey";
            switch (map)
            {
                case Workflows.ApplicationCapture:
                    tableName = "X2.X2DATA.Application_Capture";
                    break;

                case Workflows.ApplicationManagement:
                    tableName = "X2.X2DATA.Application_Management";
                    break;

                case Workflows.Credit:
                    tableName = "X2.X2DATA.Credit";
                    break;

                case Workflows.ReadvancePayments:
                    tableName = "X2.X2DATA.Readvance_Payments";
                    break;

                case Workflows.Valuations:
                    tableName = "X2.X2DATA.Valuations";
                    break;

                case Workflows.CAP2Offers:
                    tableName = "X2.X2DATA.CAP2_Offers";
                    genericColumn = "AccountKey";
                    break;

                case Workflows.LoanAdjustments:
                    tableName = "X2.X2DATA.Loan_Adjustments";
                    genericColumn = "AccountKey";
                    break;

                case Workflows.LifeOrigination:
                    tableName = "X2.X2DATA.LifeOrigination";
                    genericColumn = "OfferKey";
                    break;

                case Workflows.PersonalLoans:
                    tableName = "X2.X2DATA.Personal_Loans";
                    genericColumn = "ApplicationKey";
                    break;
            }
            string query =
                String.Format(@"SELECT * FROM {0} data WITH (NOLOCK)
								JOIN X2.X2.Instance i WITH (NOLOCK) ON data.InstanceId=i.Id
								JOIN X2.X2.State s WITH (NOLOCK) ON i.StateId=s.Id
								JOIN X2.X2.Worklist wl WITH (NOLOCK) ON i.Id=wl.InstanceId
								WHERE data.{1}='{2}'
								AND s.Name='{3}'
								AND wl.ADUserName='{4}'", tableName, genericColumn, genericKey, state, aduserName);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="aduserName"></param>
        /// <param name="offerTypeKey"></param>
        /// <returns></returns>
        public QueryResults GetWorkflowInstanceForStateADUserAndOfferType(string stateName, string aduserName, params int[] offerTypeKey)
        {
            string offerTypeKeys = string.Empty;

            for (int i = 0; i < offerTypeKey.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        offerTypeKeys = offerTypeKey[i].ToString();
                        break;

                    default:
                        offerTypeKeys += ", " + offerTypeKey[i].ToString();
                        break;
                }
            }

            string query = String.Format(@"select * from x2.x2.instance i join
							x2.x2.state s on i.stateid = s.id join
							[2am].dbo.offer o on i.name = cast(o.offerkey as varchar(10)) join
							x2.x2.workflowassignment w on i.id = w.instanceid join
							[2am].dbo.aduser a on w.aduserkey = a.aduserkey
							where s.name = '{0}' and w.generalstatuskey = 1 and a.adusername like '%{1}%' and o.offertypekey in ({2})", stateName, aduserName, offerTypeKeys);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="aduserName"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public QueryResults GetOpenApplicationManagementOffers(string stateName, string aduserName, int recordCount)
        {
            string query = String.Format(@" select top {2} xs.name, xwl.adUserName,xam.ApplicationKey
											from x2.X2DATA.Application_Management xam (nolock)
											join x2.X2.Instance xi (nolock) on xi.ID = xam.InstanceID
											join x2.X2.WorkList xwl (nolock) on xwl.InstanceID = xi.ID
											join x2.x2.state xs (nolock)  on xs.ID = xi.StateID
											join [2am].dbo.[Offer] ofr (nolock) on ofr.offerKey = xam.applicationKey
											where xwl.adUserName like '{1}%' and xs.name = '{0}'
											and ofr.OfferStatusKey in (1) and ofr.offerTypeKey in (7)", stateName, aduserName, recordCount);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Get Open Application
        /// </summary>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public QueryResults GetOpenOffersForPreSubmission(int recordCount)
        {
            string query = String.Format(
            @"select distinct top {0} o.OfferKey,	xwl.ADUserName,	le.LegalEntityKey,
            le.IDNumber, le.DateOfBirth
            from
            x2.X2DATA.Application_Capture xam (nolock)
            join x2.X2.Instance xi (nolock) on xi.ID = xam.InstanceID
            join x2.X2.WorkList xwl (nolock) on xwl.InstanceID = xi.ID
            join x2.x2.state xs (nolock) on	xs.ID = xi.StateID
            join [2am].dbo.Offer o on o.OfferKey = xam.applicationKey
            join [2am].dbo.OfferRole ofr on o.OfferKey = ofr.OfferKey
                and o.OfferStatusKey = 1
                and o.OfferTypeKey in (6,7,8)
                and offerroletypekey in (8,10)
            join [2am].dbo.LegalEntity le on ofr.LegalEntityKey = le.LegalEntityKey
            left join [Test].OffersAtApplicationCapture test on test.OfferKey = o.OfferKey
            where
	            xwl.adUserName like '%bcuser%'
                and len(idnumber) = 13 and
	            xs.[Name] = 'Application Capture'
	            and test.OfferKey is null", recordCount);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Calls the test.GetCasesInWorklistByStateAndType SP
        /// </summary>
        /// <param name ="state">State.Description</param>
        /// <param name ="workflow">Workflow.Description</param>
        /// <param name ="offerTypeKey">Offer.OfferTypeKey</param>
        /// <param name ="exclusions">List of offers to exclude</param>
        /// <param name="maxLTV">The maximum LTV for this case</param>
        /// <param name="minLTV">The minimum required LTV</param>
        /// <param name="numLE">The maximum number of LE's on the application</param>
        /// <param name="occupancyType">occupancyType</param>
        /// <returns>
        ///   Offer.OfferKey, Offer.OfferStartDate, OfferType.Description, Product.Description,
        ///   Category.Description, LTV, LAA, PTI
        /// </returns>
        public QueryResults GetApplicationsByStateAndAppType(string state, string workflow, string exclusions, string offerTypeKey, double maxLTV, double minLTV,
            int numLE, OccupancyTypeEnum occupancyType = OccupancyTypeEnum.OwnerOccupied, int employmentType = 0, int category = -1)
        {
            var proc = new SQLStoredProcedure { Name = "test.GetCasesInWorklistByStateAndTypeAndLTV" };
            proc.AddParameter(new SqlParameter("@State", state));
            proc.AddParameter(new SqlParameter("@WorkFlow", workflow));
            proc.AddParameter(new SqlParameter("@OfferTypeKey", offerTypeKey));
            proc.AddParameter(new SqlParameter("@Exclusions", exclusions));
            proc.AddParameter(new SqlParameter("@maxLTV ", (maxLTV).ToString()));
            proc.AddParameter(new SqlParameter("@maxLECount", numLE.ToString()));
            proc.AddParameter(new SqlParameter("@minLTV ", (minLTV).ToString()));
            proc.AddParameter(new SqlParameter("@occupancyTypeKey", (int)occupancyType));
            proc.AddParameter(new SqlParameter("@employmentType ", employmentType));
            proc.AddParameter(new SqlParameter("@categoryKey", category));
            return dataContext.ExecuteStoredProcedureWithResults(proc);
        }

        public QueryResults GetApplicationsByStateAndAppType(string state, string workflow, string exclusions, string offerTypeKey, double maxLTV, double minLTV,
            int numLE, OccupancyTypeEnum occupancyType = OccupancyTypeEnum.OwnerOccupied, int employmentType = 0, int category = -1, double maxIncome = 500000, double minIncome = 0)
        {
            var proc = new SQLStoredProcedure { Name = "test.GetCasesInWorklistByStateAndTypeAndLTV" };
            proc.AddParameter(new SqlParameter("@State", state));
            proc.AddParameter(new SqlParameter("@WorkFlow", workflow));
            proc.AddParameter(new SqlParameter("@OfferTypeKey", offerTypeKey));
            proc.AddParameter(new SqlParameter("@Exclusions", exclusions));
            proc.AddParameter(new SqlParameter("@maxLTV ", (maxLTV).ToString()));
            proc.AddParameter(new SqlParameter("@maxLECount", numLE.ToString()));
            proc.AddParameter(new SqlParameter("@minLTV ", (minLTV).ToString()));
            proc.AddParameter(new SqlParameter("@occupancyTypeKey", (int)occupancyType));
            proc.AddParameter(new SqlParameter("@employmentType ", employmentType));
            proc.AddParameter(new SqlParameter("@categoryKey", category));
            proc.AddParameter(new SqlParameter("@maxIncome", maxIncome));
            proc.AddParameter(new SqlParameter("@minIncome", minIncome));
            return dataContext.ExecuteStoredProcedureWithResults(proc);
        }

        /// <summary>
        /// Check if an Application is in a specific Workflow and State on an ADUser's worklist
        /// </summary>
        /// <param name="state"></param>
        /// <param name="workflow"></param>
        /// <param name="offerKey"></param>
        /// <param name="adUserName"></param>
        /// <returns></returns>
        public QueryResults GetCasesInWorklistByStateAndADUser(string state, string workflow, int offerKey, string adUserName)
        {
            var proc = new SQLStoredProcedure { Name = "test.GetCasesInWorklistByStateAndADUser" };
            proc.AddParameter(new SqlParameter("@State", state));
            proc.AddParameter(new SqlParameter("@WorkFlow", workflow));
            proc.AddParameter(new SqlParameter("@OfferKey", offerKey.ToString()));
            proc.AddParameter(new SqlParameter("@ADUserName", adUserName));
            return dataContext.ExecuteStoredProcedureWithResults(proc);
        }

        /// <summary>
        /// Check if the application has sucessfully moved a specific state
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public QueryResults GetWorklistOfCaseAtState(int applicationKey, string state)
        {
            string query = String.Format(@"select xwl.*
							from x2.X2DATA.Application_Management xam (nolock)
							join x2.X2.Instance xi (nolock)	on xi.ID = xam.InstanceID
							join x2.X2.WorkList xwl (nolock) on xwl.InstanceID = xi.ID
							join x2.x2.state xs (nolock) on xs.ID = xi.StateID
							join [2am].[dbo].offer o (nolock) on o.OfferKey = xam.ApplicationKey
							where o.OfferKey = {0} and	xs.name = '{1}'", applicationKey, state);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Inserts the flag for the Up For Fees action in the Application Management workflow
        /// </summary>
        /// <param name="instanceID">Application Management InstanceID</param>
        public void PipelineUpForFees(Int64 instanceID)
        {
            var proc = new SQLStoredProcedure { Name = "[x2].dbo.pr_PipeLineUpForFees" };
            proc.AddParameter(new SqlParameter("@InstanceID", instanceID));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Inserts the flag for the EXT_Complete action in the Application Management workflow
        /// </summary>
        /// <param name="instanceID">Application Management InstanceID</param>
        public void PipeLineComplete(Int64 instanceID)
        {
            var proc = new SQLStoredProcedure { Name = "[x2].dbo.pr_PipeLineComplete" };
            proc.AddParameter(new SqlParameter("@InstanceID", instanceID));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Inserts the flag for the EXT_HeldOver action in the Application Management workflow
        /// </summary>
        /// <param name="instanceID">Application Management InstanceID</param>
        public void PipeLineHeldOver(Int64 instanceID)
        {
            var proc = new SQLStoredProcedure { Name = "[x2].dbo.pr_PipeLineHeldOver" };
            proc.AddParameter(new SqlParameter("@InstanceID", instanceID));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Inserts the flag for the Pipeline Relodge action in the Application Management workflow
        /// </summary>
        /// <param name="instanceID">Application Management InstanceID</param>
        public void PipeLineRelodge(Int64 instanceID)
        {
            var proc = new SQLStoredProcedure { Name = "[x2].dbo.pr_PipeLineRelodge" };
            proc.AddParameter(new SqlParameter("@InstanceID", instanceID));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Inserts the flag for the Ext NTU action in the Application Management workflow
        /// </summary>
        /// <param name="instanceID">Application Management InstanceID</param>
        public void PipeLineNTU(Int64 instanceID)
        {
            var proc = new SQLStoredProcedure { Name = "[x2].dbo.pr_PipeLineNTU" };
            proc.AddParameter(new SqlParameter("@InstanceID", instanceID));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Inserts the flag for the Ext Reinstate NTU action in the Application Management workflow
        /// </summary>
        /// <param name="instanceID">Application Management InstanceID</param>
        public void PipeLineReinstateNTU(Int64 instanceID)
        {
            var proc = new SQLStoredProcedure { Name = "[x2].dbo.pr_PipeLineReinstateNTU" };
            proc.AddParameter(new SqlParameter("@InstanceID", instanceID));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Inserts the flag for the Ext NTU Final action in the Application Management workflow
        /// </summary>
        /// <param name="instanceID">Application Management InstanceID</param>
        public void PipeLineNTUFinal(Int64 instanceID)
        {
            var proc = new SQLStoredProcedure { Name = "[x2].dbo.pr_PipeLineNTUFinal" };
            proc.AddParameter(new SqlParameter("@InstanceID", instanceID));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Gets the latest record from the Application Management workflow's data table ordered by the instance id
        /// </summary>
        /// <param name="applicationKey">Application Number</param>
        /// <returns></returns>
        public QueryResults GetlatestX2DataApplicationManagementRow(int applicationKey)
        {
            string query = String.Format(@"select   top 1 *
											from    x2.x2data.application_management
											where   ApplicationKey = {0}
											order by InstanceID desc", applicationKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="state"></param>
        /// <param name="adUser"></param>
        /// <param name="includeExclude"></param>
        /// <param name="filterByCloneState"></param>
        /// <param name="excludeOffersInTable"></param>
        /// <param name="requireValuationFlag"></param>
        /// <param name="offerTypeKeys"></param>
        /// <returns></returns>
        public QueryResults GetOffers_FilterByClone(string workflow, string state, string adUser, int includeExclude,
                                                    string filterByCloneState, string excludeOffersInTable, int requireValuationFlag, params int[] offerTypeKeys)
        {
            string offerTypeKeyString = string.Empty;

            foreach (int offerTypeKey in offerTypeKeys)
                offerTypeKeyString = offerTypeKeyString + offerTypeKey.ToString() + ", ";

            offerTypeKeyString = !string.IsNullOrEmpty(offerTypeKeyString) ? offerTypeKeyString.Remove(offerTypeKeyString.LastIndexOf(',')) : "2,3,4,6,7,8";

            string query =
                @"Declare  @ADUser varchar(20),
								@Workflow varchar(50),
								@State varchar(50),
								@IncludeExclude bit,
								@FilterByCloneState varchar(50),
								@ExcludeOffersInTable varchar(100),
								@Query varchar(max),
								@tableName varchar(50),
								@columnName varchar(50),
								@RequireValuationFlag int,
								@OfferTypeKeys varchar(12)

								Set @ADUser = " + Helpers.FormatStringForSQL(adUser) + @"
								Set @Workflow = " + Helpers.FormatStringForSQL(workflow) + @"
								Set @State = " + Helpers.FormatStringForSQL(state) + @"
								Set @IncludeExclude = " + includeExclude + @"
								Set @FilterByCloneState = " + Helpers.FormatStringForSQL(filterByCloneState) + @"
								Set @ExcludeOffersInTable = " + Helpers.FormatStringForSQL(excludeOffersInTable) + @"
								Set @RequireValuationFlag = " + requireValuationFlag + @"
								Set @OfferTypeKeys = " + Helpers.FormatStringForSQL(offerTypeKeyString) + @";

								With Temp As
								(Select max(ID) as ID
								From x2.x2.Workflow (nolock)
								Where Name = @Workflow
								Group By Name)
								Select @tableName = w.StorageTable, @columnName = w.StorageKey
								From x2.x2.Workflow w (nolock) join
								Temp t on w.ID = t.ID

								Set @Query = 'Select top 1 o.offerkey from x2.x2data.' + @tableName + ' as data (nolock) join
												x2.x2.instance as i (nolock) on data.instanceid = i.id join
												[2am].dbo.offer o on i.name = cast(o.offerkey as varchar(10)) and o.offertypekey in (' + @OfferTypeKeys + ') '

								If @State is not null
									Begin
										Set @Query = @Query + 'join x2.x2.state s (nolock) on s.name = ''' + @State + ''' and i.stateid = s.id '
									End

								If @ADUser is not null
									Begin
										Set @Query = @Query + 'join X2.X2.Worklist wl (nolock) on i.parentinstanceid is null and wl.instanceid = i.id '
									End

								If @FilterByCloneState is not null
									Begin
										Set @Query = @Query + 'left outer join x2.x2.instance i_c (nolock) join x2.x2.state s_c (nolock) on s_c.name like ''' + @FilterByCloneState + ''' and i_c.stateid = s_c.id on i.id = i_c.parentinstanceid '
									End

								Set @Query = @Query + 'Where '

								if @ExcludeOffersInTable is not null
									Begin
										Set @Query = @Query + 'i.name not in (Select OfferKey from ' + @ExcludeOffersInTable + ') '
									End

								if @RequireValuationFlag <> -1
									Begin
										if @ExcludeOffersInTable is not null
											Begin
												Set @Query = @Query + 'and '
											End
										Set @Query = @Query + 'Data.RequireValuation = ' + cast(@RequireValuationFlag as varchar(1)) + ' '
									End

								--Exclude Offers with Clone (default)--
								if @FilterByCloneState is not null and cast(@IncludeExclude as varchar(1)) = 1
									Begin
										if @ExcludeOffersInTable is not null or @RequireValuationFlag <> -1
											Begin
												Set @Query = @Query + 'and '
											End
										Set @Query = @Query + 'i_c.id is null '
									End

								--Include Offers with Clone--
								if @FilterByCloneState is not null and cast(@IncludeExclude as varchar(1)) = 0
									Begin
										if @ExcludeOffersInTable is not null or @RequireValuationFlag <> -1
											Begin
												Set @Query = @Query + 'and '
											End
										Set @Query = @Query + 'i_c.id is not null '
									End

								If @ADUser is not null
									Begin
										if @ExcludeOffersInTable is not null or @FilterByCloneState is not null or @RequireValuationFlag <> -1
											Begin
												Set @Query = @Query + 'and '
											End
										Set @Query = @Query + 'wl.adusername like ''%' + @ADUser + ''';'
									End

								Execute(@Query)";

            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns an instance ID when provided with a debtCounsellingKey
        /// </summary>
        /// <param name="debtCounsellingKey">debtcounselling.debtCounsellingKey</param>
        /// <returns>x2.x2.instance.ID</returns>
        public int GetInstanceIDByDebtCounsellingKey(int debtCounsellingKey)
        {
            string query =
                string.Format(@"select top 1 i.id from x2.x2data.debt_counselling data (nolock)
                                join x2.x2.instance i (nolock) on data.instanceid=i.id
                                where debtCounsellingkey = {0}
                                and parentinstanceid is null
                                order by i.id desc", debtCounsellingKey);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column("id").GetValueAs<int>();
        }

        /// <summary>
        /// Gets the instance details linked to a debt counselling case when provided with a debtCounsellingKey
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        /// <returns>i.ID, debtCounsellingKey, AccountKey, i.subject, s.name </returns>
        public QueryResults GetDebtCounsellingInstanceDetails(int debtCounsellingKey = 0, Int64 instanceID = 0)
        {
            string query;
            if (debtCounsellingKey != 0)
            {
                query =
                    string.Format(@"select i.ID, debtCounsellingKey, AccountKey, i.subject, s.name as StateName, CourtCase, PreviousState
								    from x2.x2data.debt_counselling data (nolock)
								    join x2.x2.instance i (nolock) on data.instanceid=i.id
								    join x2.x2.state s (nolock) on i.stateid=s.id
								    where data.debtcounsellingkey = {0} and parentinstanceid is null", debtCounsellingKey);
            }
            else
            {
                query =
                    string.Format(@"select i.ID, debtCounsellingKey, AccountKey, i.subject, s.name as StateName, CourtCase, PreviousState
								    from x2.x2data.debt_counselling data (nolock)
								    join x2.x2.instance i (nolock) on data.instanceid=i.id
								    join x2.x2.state s (nolock) on i.stateid=s.id
								    where i.id = {0} and parentinstanceid is null", instanceID);
            }
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns the latest workflow.ID for a workflow
        /// </summary>
        /// <param name="workflowName">WorkflowName</param>
        /// <returns>int</returns>
        public int GetMaxWorkflowID(string workflowName)
        {
            string query =
                string.Format(@"select max(ID) from x2.x2.workflow where name='{0}'", workflowName);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(0).GetValueAs<int>();
        }

        /// <summary>
        /// gets a users worklist at a given state
        /// </summary>
        /// <param name="stateName">state</param>
        /// <param name="workflowName">workflow</param>
        /// <param name="userName">user</param>
        /// <returns>QueryResults</returns>
        public QueryResults GetCasesInWorklist(string stateName, string workflowName, string userName)
        {
            string query =
                    string.Format(@"select wl.* from x2.x2.instance i with (nolock)
									join x2.x2.state s with (nolock) on i.stateid=s.id
									join x2.x2.workflow w with (nolock) on s.workflowid=w.id
									join x2.x2.worklist wl with (nolock) on i.id=wl.instanceid
									where s.name='{0}'
									and w.name='{1}'
									and wl.adusername='{2}'", stateName, workflowName, userName);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets the worklist record for an instance
        /// </summary>
        /// <param name="instanceid">Instance ID</param>
        /// <returns></returns>
        public QueryResults GetWorklistDetails(Int64 instanceid)
        {
            string query =
                string.Format(@"select * from x2.x2.worklist where instanceid={0}", instanceid);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets the instance details
        /// </summary>
        /// <param name="instanceID">Instance.ID</param>
        /// <returns></returns>
        public QueryResults GetInstanceDetails(Int64 instanceID)
        {
            string query =
                string.Format(@"select i.id, s.name as StateName from x2.x2.instance i
                                join x2.x2.state s on i.stateid=s.id
                                where i.id={0}", instanceID);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets the number of times an Activity has occurred on an Instance
        /// </summary>
        /// <param name="instanceID">Instance ID</param>
        /// <param name="activity">Activity Name</param>
        /// <returns></returns>
        public QueryResults GetWorkflowHistoryActivityCount(Int64 instanceID, string activity, string dateFilter = "")
        {
            var query =
                string.Format(@"SELECT COUNT(*) FROM X2.X2.workflowHistory wh (NOLOCK)
	                            JOIN X2.X2.activity a (NOLOCK)
		                        ON wh.activityID=a.ID
                                WHERE wh.instanceID={0}
                                AND a.name = '{1}' and wh.stateChangeDate >= '{2}'", instanceID, activity, dateFilter);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets the number of times an Activity has occurred on an Instance
        /// </summary>
        /// <param name="instanceID">Instance ID</param>
        /// <param name="activity">Activity Name</param>
        /// <returns></returns>
        public QueryResults GetWorkflowHistoryActivitiesCount(int instanceID, string dateFilter = "", params string[] activity)
        {
            var activities = string.Empty;
            foreach (var value in activity)
            {
                activities += "'" + value + "'" + ",";
            }
            activities = activities.TrimEnd(',');
            var query =
                string.Format(@"SELECT COUNT(*) FROM X2.X2.workflowHistory wh (NOLOCK)
	                            JOIN X2.X2.activity a (NOLOCK)
		                        ON wh.activityID=a.ID
                                WHERE wh.instanceID={0}
                                AND a.name in ({1}) and wh.stateChangeDate >= '{2}'", instanceID, activities, dateFilter);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///  Get x2.x2.workflow records by workflowname
        /// </summary>
        /// <param name="genericKeyType"></param>
        /// <returns></returns>
        public QueryResults GetWorkflowRecordsByGenericKeyType(GenericKeyTypeEnum genericKeyType)
        {
            string query =
                string.Format(@"select top 10 * from x2.x2.workflow with (nolock) where workflow.generickeytypekey = {0}", (int)genericKeyType);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets the first External Activity Instance provided the parameters.
        /// </summary>
        public int? GetActiveExternalActivity(string workflowName, string externalActivity)
        {
            try
            {
                if (workflowName == Workflows.IT)
                {
                    //we need an instance from the IT map
                    string query = @"select top 1 instanceid from x2.x2data.IT itmap
                                            join x2.x2.instance i
                                            on itmap.instanceid=i.id
                                            join x2.x2.state s
                                            on i.stateid=s.id
                                            where s.name='Archive' order by itmap.instanceid desc";
                    var statement = new SQLStatement() { StatementString = query };
                    var results = dataContext.ExecuteSQLScalar(statement);
                    return Int32.Parse(results.SQLScalarValue);
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Raises an External Activity by inserting the record into the X2 database
        /// </summary>
        /// <param name="workflowName">WorkflowName</param>
        /// <param name="externalActivity">External Activity to Raise</param>
        /// <param name="instanceID"></param>
        public void InsertActiveExternalActivity(string workflowName, string externalActivity, Int64? instanceID, string activityXMLData)
        {
            string query =
                String.Format(@"insert into x2.x2.activeExternalActivity
								select max(a.id), max(a.workflowID), {0}, getdate(), {1}, NULL
								from x2.x2.ExternalActivity a
								where a.name ='{2}'
								and workflowid in (
								select max(id) from x2.x2.workflow
								where name = '{3}'
								)", instanceID.Value, "{1}", externalActivity, workflowName);

            if (!String.IsNullOrEmpty(activityXMLData))
            {
                query = query.Replace("{1}", activityXMLData);
            }
            else
            {
                if (activityXMLData == null)
                    query = query.Replace("{1}", "NULL");
                else
                    query = query.Replace("{1}", "''");
            }

            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Gets the instance id for a case in the Readvance Payments workflow at a specific state
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="workflowState"></param>
        /// <returns></returns>
        public int GetReadvancePaymentsInstanceIDByState(int offerKey, string workflowState)
        {
            string sql =
                string.Format(@"select i.id from x2.x2data.readvance_payments rp
                                join x2.x2.instance i on rp.instanceid=i.id
                                join x2.x2.state s on i.stateid=s.id
                                where s.name='{0}' and rp.applicationkey={1}", workflowState, offerKey);
            var statement = new SQLStatement() { StatementString = sql };
            var r = dataContext.ExecuteSQLQuery(statement);
            return r.Rows(0).Column(0).GetValueAs<int>();
        }

        /// <summary>
        /// Check whether an activity has timed out.
        /// </summary>
        /// <param name="accountKey"></param>
        public bool IsActivityTimedOut(int accountKey)
        {
            var query =
                string.Format(@"select * from x2.x2.workflowHistory w (nolock)
                                    join x2.x2.activity a (nolock) on w.activityid=a.id
                                    where w.instanceid = (select max(instanceid) from x2.x2data.loan_adjustments (nolock)
                                    where accountkey={0}) and a.name = 'Term Request Timeout'", accountKey);
            var statement = new SQLStatement() { StatementString = query };
            var r = dataContext.ExecuteSQLQuery(statement);
            return r.HasResults;
        }

        /// <summary>
        /// returns the latest State.ID for a workflow state
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public int GetStateIDByName(string stateName, string workflowName)
        {
            string query =
                string.Format(@"select max(id) from x2.x2.state s
                                        where s.name = '{0}'
                                        and workflowid = (
                                        select max(id) from x2.x2.workflow
                                        where name='{1}')", stateName, workflowName);

            var statement = new SQLStatement() { StatementString = query };
            var r = dataContext.ExecuteSQLQuery(statement);
            return r.Rows(0).Column(0).GetValueAs<int>();
        }

        /// <summary>
        /// Gets the instanceid for a case in the Application Capture workflow at a specific state
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="workflowState"></param>
        /// <returns></returns>
        public int GetAppCapInstanceIDByState(int offerKey, string workflowState)
        {
            string sql =
                 string.Format(@"select i.id from x2.x2data.application_capture ac
                                    join x2.x2.instance i on ac.instanceid=i.id
                                    join x2.x2.state s on i.stateid=s.id
                                    where s.name='{0}' and ac.applicationkey={1}", workflowState, offerKey);

            var statement = new SQLStatement() { StatementString = sql };
            var r = dataContext.ExecuteSQLQuery(statement);
            return r.Rows(0).Column(0).GetValueAs<int>();
        }

        public QueryResults GetITWorkflowInstance()
        {
            //we need an instance from the IT map
            const string query = @"select top 1 instanceid from x2.x2data.IT itmap
                                            join x2.x2.instance i
                                            on itmap.instanceid=i.id
                                            join x2.x2.state s
                                            on i.stateid=s.id
                                            where s.name='Archive' order by itmap.instanceid desc";
            var statement = new SQLStatement() { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        public QueryResults GetCreditInstanceDetailsBySourceInstanceAndState(Int64 sourceInstanceid, int offerKey, string expectedState)
        {
            var query = string.Format(@"select * from x2.x2data.credit c
                join x2.x2.instance i on c.instanceid=i.id
                join x2.x2.state s on i.stateid=s.id
                where applicationkey={0}
                and i.sourceinstanceid={1}
                and s.name='{2}'", offerKey, sourceInstanceid, expectedState);
            var statement = new SQLStatement() { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        public IEnumerable<Automation.DataModels.X2Instance> GetWorkflowInstances(Automation.DataModels.X2Workflow workflow)
        {
            SqlMapper.ClearCache();
            var procParams = new DynamicParameters();
            procParams.Add("@GenericKeyTypeKey", value: (int)workflow.GenericKeyTypeKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            procParams.Add("@WorkflowName", value: workflow.Name, dbType: DbType.String, direction: ParameterDirection.Input);
            var instances = dataContext.Query<Automation.DataModels.X2Instance>("x2.test.GetWorkflowInstances", parameters: procParams, commandtype: CommandType.StoredProcedure);
            return instances;
        }

        /// <summary>
        /// Get all the workflows
        /// </summary>
        public IEnumerable<Automation.DataModels.X2Workflow> GetWorkflows()
        {
            return dataContext.Query<Automation.DataModels.X2Workflow>(@"select * from x2.x2.workflow w where w.id in (select max(id) from x2.x2.workflow group by name)");
        }

        /// <summary>
        /// Get all Activities
        /// </summary>
        public IEnumerable<Automation.DataModels.X2Activity> GetActivities(Automation.DataModels.X2Workflow workflow)
        {
            return dataContext.Query<Automation.DataModels.X2Activity>(
                string.Format(@"select * from x2.x2.activity with (nolock) where workflowid = {0} order by 1 desc", workflow.Id)
                );
        }

        /// <summary>
        /// Get activity by ID
        /// </summary>
        public Automation.DataModels.X2Activity GetActivity(int activityID)
        {
            var activity = dataContext.Query<Automation.DataModels.X2Activity>(
                string.Format(@"select * from x2.x2.activity with (nolock) where id = {0}", activityID)
                );
            return (from a in activity select a).FirstOrDefault();
        }

        /// <summary>
        /// Get all States
        /// </summary>
        public IEnumerable<Automation.DataModels.X2State> GetStates(Automation.DataModels.X2Workflow workflow)
        {
            return dataContext.Query<Automation.DataModels.X2State>(
                    string.Format(@"select * from x2.x2.State with (nolock) where workflowid = {0} order by 1 desc", workflow.Id)
                    );
        }

        public IEnumerable<Automation.DataModels.X2ScheduledActivity> GetScheduledActivities()
        {
            return dataContext.Query<Automation.DataModels.X2ScheduledActivity>("select * from x2.x2.scheduledactivity with (nolock) order by 1 desc");
        }

        /// <summary>
        ///   Gets the worklist record by aduser and workflowmap
        /// </summary>
        /// <param name = "adusername">adusername</param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.X2WorkflowList> GetWorklistDetails(string adusername, string workflowMapName)
        {
            var sql = String.Format(@"with instances as
                                        (
                                        select instanceid from x2.x2.worklist with (nolock)
                                        where adusername='{0}'
                                        )
                                        select worklist.* from x2.x2.worklist with (nolock)
                                        inner join x2.x2.instance with (nolock) on worklist.instanceid = instance.id
                                        inner join x2.x2.workflow with (nolock) on instance.workflowid = workflow.id
                                        inner join x2.x2.state with (nolock) on instance.stateid = state.id
	                                        and state.type = 1
	                                        and state.name not like '%NTU%'
                                        where instance.id in
                                        (
	                                        select
		                                        wra.instanceid
	                                        from x2.x2.workflowRoleassignment wra with (nolock)
	                                        join instances on wra.instanceid=instances.instanceid
		                                        and generalstatuskey = 1
	                                        union
	                                        select
		                                        wra.instanceid
	                                        from x2.x2.workflowAssignment wra with (nolock)
	                                        join instances on wra.instanceid=instances.instanceid
		                                        and generalstatuskey = 1
                                        )
                                        and workflow.name = '{1}'", adusername, workflowMapName);
            return dataContext.Query<Automation.DataModels.X2WorkflowList>(sql);
        }

        /// <summary>
        /// Inserts into the ScheduledActivity table
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="activityName"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public bool InsertScheduledActivity(Int64 instanceID, string activityName, int minutes, int seconds)
        {
            string query = string.Empty;
            if (seconds > 0)
            {
                query =
                    string.Format(@"
                                DECLARE @ActivityID int
                                SELECT @ActivityID = max(id) from x2.x2.Activity
                                where name = '{0}'
                                INSERT INTO x2.x2.ScheduledActivity
                                (InstanceID, Time, ActivityID, Priority, WorkflowProviderName)
                                VALUES
                                ({1},dateadd(ss, {2}, getdate()), @ActivityID, 1, '')", activityName, instanceID, seconds);
            }
            else
            {
                query =
                    string.Format(@"
                                DECLARE @ActivityID int
                                SELECT @ActivityID = max(id) from x2.x2.Activity
                                where name = '{0}'
                                INSERT INTO x2.x2.ScheduledActivity
                                (InstanceID, Time, ActivityID, Priority, WorkflowProviderName)
                                VALUES
                                ({1},dateadd(mi, {2}, getdate()), @ActivityID, 1, '')", activityName, instanceID, minutes);
            }
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Gets a valuations instance by using its expected source instance ID.
        /// </summary>
        /// <param name="sourceInstanceid">Typically the source instance is the one from the application management workflow.</param>
        /// <param name="offerKey">application number</param>
        /// <param name="expectedState">Where we expect the valuations case to be.</param>
        /// <returns></returns>
        public QueryResults GetValuationsInstanceDetailsBySourceInstanceAndState(Int64 sourceInstanceid, int offerKey, string expectedState)
        {
            var query = string.Format(@"select * from x2.x2data.valuations v
                join x2.x2.instance i on v.instanceid=i.id
                join x2.x2.state s on i.stateid=s.id
                where applicationkey={0}
                and i.sourceinstanceid={1}
                and s.name='{2}'", offerKey, sourceInstanceid, expectedState);
            var statement = new SQLStatement() { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// returns the instance data for an offer in application capture
        /// </summary>
        /// <param name="accountKey">OfferKey</param>
        /// <returns></returns>
        public QueryResults GetDeleteDebitOrderInstanceDetails(int accountKey)
        {
            string query =
                string.Format(@"select *,s.name as StateName from
								x2.x2data.Delete_Debit_Order do with (nolock)
								join x2.x2.instance i with (nolock) on do.instanceid=i.id
								join x2.x2.state s with (nolock) on i.stateid=s.id
								join x2.x2.workflow w with (nolock) on s.workflowid=w.id
								where do.accountKey={0} and parentInstanceid is null", accountKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Get the personal loan data from the x2data table;.
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        public Int64 GetPersonalLoanInstanceId(int applicationKey)
        {
            string query =
                string.Format(@"select top 1 instanceid from x2.x2data.personal_loans where applicationkey = {0}", applicationKey);

            SQLStatement statement = new SQLStatement { StatementString = query };
            var queryResults = dataContext.ExecuteSQLQuery(statement);

            return queryResults.FirstOrDefault().Column("instanceid").GetValueAs<Int64>();
        }

        /// <summary>
        /// Check that the role was setup correctly for a particular user
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="userRole"></param>
        /// <param name="isDynamic"></param>
        /// <param name="name"></param>
        /// <param name="isActivity"></param>
        /// <param name="isState"></param>
        /// <returns>return </returns>
        public bool CheckUserRoleSecuritySetup(string workflowName, string userRole, string stateName, string activityName)
        {
            var p = new DynamicParameters();
            p.Add("@activityName", value: activityName, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@stateName", value: stateName, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@roleName", value: userRole, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@workflowName", value: workflowName, dbType: DbType.String, direction: ParameterDirection.Input);
            return dataContext.Query<bool>("test.CheckRoleSecurity", parameters: p, commandtype: CommandType.StoredProcedure).FirstOrDefault();
        }

        public string GetPreviousStateForInstancePriorToActivity(Int64 instanceID, string workflowActivity)
        {
            string query =
                string.Format(@"select top 1 s.name from x2.x2.workflowHistory w
                        join x2.x2.instance i on w.instanceid=i.id
                        join x2.x2.activity a on w.activityid=a.id
                        join x2.x2.state s on a.stateid=s.id
                        where instanceid={0}
                        and a.name = '{1}'
                        order by w.activityDate desc
                        ", instanceID, workflowActivity);

            var statement = new SQLStatement { StatementString = query };
            var queryResults = dataContext.ExecuteSQLQuery(statement);

            return queryResults.Rows(0).Column(0).GetValueAs<string>();
        }

        public Automation.DataModels.X2State GetNextStateForExternalActivity(string activatingExternalActivityName, string activityName, string workflow)
        {
            string activityNameFilter = string.IsNullOrEmpty(activityName) ? string.Empty : string.Format("and a.name = '{0}'", activityName);
            string query = string.Format(@"select s.*
					from x2.x2.ExternalActivity ea
					join x2.x2.activity a on ea.id=a.activatedByExternalActivity
					join x2.x2.state s on a.nextStateID = s.ID
					where ea.name ='{0}'
                    {1}
					and a.workflowid = (select max(id) from x2.x2.workflow where name = '{2}')
					order by 1 desc", activatingExternalActivityName, activityNameFilter, workflow);
            return dataContext.Query<Automation.DataModels.X2State>(query).FirstOrDefault();
        }

        public List<Automation.DataModels.Offer> GetXNumberOfOffersAtQA(int x, bool isFL)
        {
            int FL = isFL ? 1 : 0;
            string query = string.Format(@"select distinct top {0} am.applicationKey as OfferKey from x2.x2.instance i
                    join x2.x2.state s on i.stateid=s.id
                    join x2.x2data.application_management am on i.id=am.instanceid
                    where s.name='QA' and isFL={1}", x, FL);
            return dataContext.Query<Automation.DataModels.Offer>(query).ToList();
        }

        public List<Automation.DataModels.Offer> GetXNumberOfCaseForAppInOrder(int x)
        {
            string query = string.Format(@"select top {0} appMan.ApplicationKey as OfferKey from x2.x2data.application_management appMan
                              join x2.x2.instance i
                                    on appMan.instanceid=i.id
                              join x2.x2.state s on i.stateid=s.id
                              join [2am].dbo.offer o on appMan.applicationkey=o.offerkey
                              left join [2am].dbo.offercondition oc on o.offerkey=oc.offerkey
                        where o.offerstatuskey = 1 and s.name = 'Manage Application'
                        and oc.offerkey is null and o.offertypekey in (6,7,8)", x);
            return dataContext.Query<Automation.DataModels.Offer>(query).ToList();
        }

        public List<Automation.DataModels.Offer> GetAppManOfferWithoutValuation()
        {
            var query = String.Format(@"
                      select  o.* from x2.x2data.application_management as appMan
                            join x2.x2.instance as i
                                on appMan.instanceid=i.id
                            join x2.x2.state as s
                                on i.stateid=s.id
                            join dbo.offer as o
                                on appMan.applicationkey=o.offerkey

                             --No further valuations have been done
                            left join dbo.stagetransition st
		                        on appMan.applicationkey=st.generickey
		                         and st.stagedefinitionstagedefinitiongroupkey in (1889,4584)

	                        left join (select va.applicationkey from x2.x2data.valuations va
				                        join x2.x2.instance ins
					                        on ins.id=va.instanceid
				                        join x2.x2.state s
					                        on ins.stateid=s.id
			                           where s.name <> 'Manual Archive' and s.name <> 'Manager Archive'
				                        ) v
		                        on appMan.Applicationkey=v.Applicationkey

                         where appMan.requirevaluation = 1
                            and o.offerstatuskey = 1
                            and s.name in ('Manage Application')
                            and o.offertypekey in (2,3,4,6,7,8)
                            and st.generickey is null
                            and v.applicationkey is null");
            return dataContext.Query<Automation.DataModels.Offer>(query).ToList();
        }

        public void SetIsValuationRequiredIndicator(bool isRequired, Int64 instanceId)
        {
            var statement = String.Empty;
            if (isRequired)
                statement = String.Format(@"update x2.x2data.application_management set requirevaluation=1 where instanceid={0}", instanceId);
            else
                statement = String.Format(@"update x2.x2data.application_management set requirevaluation=0 where instanceid={0}", instanceId);
            dataContext.Execute(statement);
        }

        public bool IsValuationRequiredIndicator(Int64 instanceId)
        {
            var statement = String.Format(@"select requirevaluation from x2.x2data.application_management where instanceid = {0}", instanceId);
            return dataContext.Query<bool>(statement).FirstOrDefault();
        }

        public bool IsValuationsAtArchiveState(int applicationKey)
        {
            var query = String.Format(@"select case when count(*) > 0 then convert(bit,0) else convert(bit,1) end [count] from x2.x2data.valuations v
	                                            join x2.x2.instance i on v.instanceid=i.id
	                                            join x2.x2.state s on i.stateid=s.id
                                            where v.applicationkey={0} and s.type != 5", applicationKey);
            return dataContext.Query<bool>(query).FirstOrDefault();
        }

        public string GetWorkflowState(int applicationKey)
        {
            var query = String.Format(@"select s.name from x2.x2data.valuations v
	                                            join x2.x2.instance i on v.instanceid=i.id
	                                            join x2.x2.state s on i.stateid=s.id
                                            where v.applicationkey={0} and s.type not in (5,6)", applicationKey);
            return dataContext.Query<string>(query).FirstOrDefault();
        }

        public IEnumerable<Automation.DataModels.Offer> GetValuationOffersAtState(bool amendedValuations, string stateName)
        {
            var p = new DynamicParameters();
            p.Add("@AmendedValuations", value: amendedValuations, dbType: DbType.Boolean, direction: ParameterDirection.Input);
            p.Add("@StateName", value: stateName, dbType: DbType.String, direction: ParameterDirection.Input);
            return dataContext.Query<Automation.DataModels.Offer>("test.GetValuationOffersAtState", parameters: p, commandtype: CommandType.StoredProcedure);
        }

        public Automation.DataModels.Offer GetPersonalLoanOfferAtState(string stateName, bool hasSAHLLife, bool hasExternalLife)
        {
            var query = string.Format(@"select o.* from
                                                [2AM].dbo.offer o
                                                left join (select offerkey, max(offerinformationkey) as offerinformationkey from offerinformation group by offerkey) as maxoi on o.offerkey = maxoi.offerkey
                                                left join [2AM].dbo.offerinformation oi on maxoi.offerinformationkey = oi.offerinformationkey
                                                left join [2AM].dbo.offerinformationpersonalloan oipl on oi.offerinformationkey = oipl.offerinformationkey
                                                left join [2AM].dbo.offerexternallife oel on o.offerkey = oel.offerkey
                                                left join [2AM].dbo.externallifepolicy elp on oel.externallifepolicykey = elp.externallifepolicykey and elp.lifepolicystatuskey = 3 and elp.policyceded = 1
                                                join x2.x2data.personal_loans p on o.offerkey = p.applicationkey
                                                join x2.x2.instance i on p.instanceid = i.id
                                                join x2.x2.state s on i.stateid = s.id
                                                join x2.x2.worklist w on i.id = w.instanceid
                                                join [2AM].dbo.aduser a on w.adusername = a.adusername and a.generalstatuskey = 1
                                            where
                                                case when isnull(oipl.lifepremium, 0) > 0 then 'true' else 'false' end = '{0}'
                                                and case when isnull(elp.externallifepolicykey, 0) > 0 then 'true' else 'false' end = '{1}'
                                                and s.name = '{2}'
                                            order by newid()", hasSAHLLife, hasExternalLife, stateName);
            return dataContext.Query<Automation.DataModels.Offer>(query).FirstOrDefault();
        }

        public Automation.DataModels.Offer GetOfferAtCreditWithUserConfirmedApplicationEmployment(bool isUserConfirmedApplicationEmployment)
        {
            var query = string.Format(@"select o.* from
                                                [2AM].dbo.offer o
                                                left join [2AM].dbo.offerattribute ot on o.offerkey = ot.offerkey
                                                join x2.x2data.credit d on o.offerkey = d.applicationkey
                                                join x2.x2.instance i on d.instanceid = i.id
                                                join x2.x2.state s on i.stateid = s.id
                                            where
                                                o.offerstatuskey = 1
                                                and s.name = 'Credit'
                                                and case when o.offerkey is null then 'False' else 'True' end = '{0}'
                                            order by newid()", isUserConfirmedApplicationEmployment);
            return dataContext.Query<Automation.DataModels.Offer>(query).FirstOrDefault();
        }

        public void UpdateAlphaHousingSurveyEmailSent(int instanceId, bool alphaHousingSurveyEmailSent)
        {
            int emailSentIndicator = Convert.ToInt32(alphaHousingSurveyEmailSent);

            var query = String.Format(@"UPDATE x2.x2data.application_management
                                        SET alphaHousingSurveyEmailSent = {0}
                                        WHERE instanceID = {1}", emailSentIndicator, instanceId);
            dataContext.Execute(query);
        }

        public IEnumerable<Automation.DataModels.Offer> GetOfferWithOfferAttributeAtState(WorkflowEnum workflow, string state, OfferAttributeTypeEnum offerAttributeTypeKey)
        {
            string dataTable = string.Empty;
            if (workflow == WorkflowEnum.ApplicationCapture)
                dataTable = "x2.x2data.Application_Capture";
            if (workflow == WorkflowEnum.ApplicationManagement)
                dataTable = "x2.x2data.Application_Management";
            var query = string.Format(@"SELECT o.*, oa.*
                FROM {0} d
	                JOIN X2.X2.Instance i ON d.InstanceID = i.ID
	                JOIN X2.X2.State s ON i.StateID = s.ID
	                JOIN [2AM].dbo.Offer o ON d.ApplicationKey = o.OfferKey and o.OfferTypeKey in (6,7,8)
	                LEFT JOIN [2AM].dbo.OfferAttribute oa ON O.OfferKey = oa.OfferKey AND oa.OfferAttributeTypeKey = {1}
                WHERE s.Name = '{2}' order by newid()", dataTable, (int)offerAttributeTypeKey, state);
            return dataContext.Query<DataModels.Offer, DataModels.OfferAttribute, DataModels.Offer>(query, (o, oa) => { o.OfferAttribute = oa; return o; }, splitOn: "OfferKey", commandtype: System.Data.CommandType.Text);
        }

        public IEnumerable<Automation.DataModels.X2Request> GetLastestX2Requests()
        {
            var query = @"select top 100 * from x2.x2.Request with (nolock)  order by RequestDate desc";
            return dataContext.Query<Automation.DataModels.X2Request>(query);
        }

        public IEnumerable<Automation.DataModels.DisabilityClaim> GetDisabilityClaimsAtState(string state)
        {
            var query = string.Format(@"select
                                        loan.AccountKey as LoanAccountKey,
                                        dc.*
                                        from x2.X2DATA.Disability_Claim data
                                        join DisabilityClaim dc on data.DisabilityClaimKey = dc.DisabilityClaimKey
                                        join Account life on dc.AccountKey = life.AccountKey
                                        join Account loan on life.ParentAccountKey = loan.AccountKey
                                        join x2.x2.Instance i on data.InstanceID = i.ID
                                        join x2.x2.State s on i.StateID = s.ID
                                        where s.Name='{0}'
                                        order by newid()", state);
            return dataContext.Query<Automation.DataModels.DisabilityClaim>(query);
        }
    }
}