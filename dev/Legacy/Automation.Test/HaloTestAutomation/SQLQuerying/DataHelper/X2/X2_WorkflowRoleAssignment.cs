using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Constants;
using Common.Enums;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Returns the WorkflowRoleAssignment details by instance id
        /// </summary>
        /// <param name="instanceID">InstanceID</param>
        /// <returns></returns>
        public QueryResults GetWorkflowRoleAssignmentByInstance(Int64 instanceID)
        {
            string query =
                string.Format(@"select instanceid, ad.aduserkey, ad.adusername, wra.generalstatuskey, insertdate, wrt.description, wrt.workflowRoleTypeKey
                        from x2.x2.workflowRoleAssignment wra (nolock)
                        left join [2am].dbo.aduser ad (nolock) on wra.adUserKey=ad.adUserKey
                        left join [2am].dbo.workflowRoleTypeOrganisationStructureMapping map (nolock)
                        on wra.workflowRoleTypeOrganisationStructureMappingKey=map.workflowRoleTypeOrganisationStructureMappingKey
                        left join [2am]..workflowRoleType wrt (nolock) on map.workflowRoleTypeKey=wrt.workflowRoleTypeKey
                        where wra.instanceid = {0}
                        order by wra.id desc", instanceID);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets an active workflow role when provided with the generic key, the workflow role type key and the adUserName
        /// </summary>
        /// <param name="genericKey">GenericKey</param>
        /// <param name="wfRoleType">WorkflowRoleTypeKey</param>
        /// <param name="adUserName">ADUserName</param>
        /// <returns></returns>
        public QueryResults GetActiveWorkflowRoleByAdUserAndType(int genericKey, WorkflowRoleTypeEnum wfRoleType, string adUserName)
        {
            string query =
                string.Format(@"select * from [2am].dbo.workFlowRole wfr
                                join [2am]..aduser ad on wfr.legalentitykey=ad.legalentitykey
                                where genericKey = {0} and wfr.workflowRoleTypeKey = {1}
                                and adUserName = '{2}' and wfr.generalStatusKey = 1", genericKey, (int)wfRoleType, adUserName);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns the next user due to be assigned a case via a Load Balancing assignment
        /// </summary>
        /// <param name="wrt">WorkflowRoleType</param>
        /// <param name="workflowID">WorkflowID</param>
        /// <param name="states">list of stateID's</param>
        /// <returns></returns>
        public string GetLoadBalanceUserAssign(WorkflowRoleTypeEnum wrt, int workflowID, string states, bool incl)
        {
            string inclusive = incl ? "in" : "not in";
            //Hack: If you need to include all workflow states in your load balance query parse an empty string in the states parameter.
            //This will result in the states condition being excluded
            string concatStates = string.IsNullOrEmpty(states) ? string.Empty : string.Format("and convert(varchar,i.StateID) {0} ({1})", inclusive, states);
            concatStates += wrt == WorkflowRoleTypeEnum.DebtCounsellingConsultantD 
                ? " and i.CreationDate > '2012/09/04 00:00:00.000' and datediff(dd, i.CreationDate, getdate()) < = 7" 
                : string.Empty;
            string query =
                string.Format(@"with Users_CTE (ADUserName)
                                as   (
                                select a.adusername from
                                [2am].dbo.workflowRoleTypeorganisationstructuremapping map
                                join [2am].dbo.userorganisationstructure u
                                on map.organisationstructureKey=u.organisationstructureKey
                                join [2am].dbo.ADUser a
                                on u.aduserkey=a.aduserkey
                                join [2am].dbo.userOrganisationStructureRoundRobinStatus stat
                                on u.userOrganisationStructureKey=stat.userOrganisationStructureKey
                                where workflowRoleTypeKey = {0} and workflowRoleTypeKey=u.generickey
                                and genericKeytypekey=34 and
                                stat.GeneralStatusKey=1 and u.generalStatusKey=1 and a.generalStatusKey=1
                                )
                                select    u.ADUserName , count(i.id) as InstanceCount
                                from    Users_CTE u
                                left join [x2].[x2].WorkList wl (nolock)
                                on wl.ADUserName = u.ADUserName
                                left join [x2].[x2].Instance i (nolock)
                                on i.ID = wl.InstanceID and i.WorkflowID = {1}
                                {2}
                                group by      u.ADUserName
                                order by     InstanceCount asc", (int)wrt, workflowID, concatStates);
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults r = dataContext.ExecuteSQLQuery(statement);
            return r.Rows(0).Column(0).Value;
        }

        /// <summary>
        /// This query will find a user in a role on another one of the grouped cases if it already exists.
        /// It will only return the user if they are still mapped to the correct point in the organisation structure in the workflow
        /// role type provided and if the ADUser.GeneralStatus is Active and the UOS.GeneralStatus is active.
        /// </summary>
        /// <param name="wrt"></param>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        public string GetDebtCounsellingGroupAssign(WorkflowRoleTypeEnum wrt, int debtCounsellingKey)
        {
            string adUserName = string.Empty;
            string query =
                string.Format(@"select top 1 ad.adUserName from [2am].debtcounselling.DebtCounselling dc with (nolock)
                                join [2am].debtcounselling.DebtCounselling dcp with (nolock)
                                on dc.debtCounsellingGroupKey=dcp.debtCounsellingGroupKey
                                join x2.x2data.debt_counselling data with (nolock)
                                on dcp.debtCounsellingKey=data.debtCounsellingKey
                                join x2.x2.WorkflowRoleAssignment wra with (nolock)
                                on data.instanceid=wra.instanceid
                                join [2am].dbo.WorkflowRoleTypeOrganisationStructureMapping map with (nolock)
                                on wra.WorkflowRoleTypeOrganisationStructureMappingKey=map.WorkflowRoleTypeOrganisationStructureMappingKey
                                join [2am].dbo.aduser ad with (nolock)
                                on wra.aduserKey=ad.aduserKey
                                join [2am].dbo.userOrganisationStructure uos with (nolock)
                                on map.OrganisationStructureKey=uos.organisationStructureKey and uos.GenericKey=map.workflowRoleTypeKey and genericKeyTypeKey=34
                                and ad.adUserKey=uos.aduserkey
                                where dc.debtCounsellingKey={0}
                                and workflowRoleTypeKey={1} and uos.generalStatusKey=1
                                and ad.generalStatusKey=1
                                order by wra.id desc", debtCounsellingKey, (int)wrt);
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults r = dataContext.ExecuteSQLQuery(statement);
            if (r.HasResults)
            {
                adUserName = r.Rows(0).Column(0).Value;
            }
            return adUserName;
        }

        /// <summary>
        /// Returns the active WorkflowRoleAssignment details for a case searched for by instance id
        /// </summary>
        /// <param name="instanceID">InstanceID</param>
        /// <returns></returns>
        public QueryResults GetActiveWorkflowRoleAssignmentByInstance(Int64 instanceID)
        {
            string query =
                string.Format(@"select instanceid, ad.aduserkey, ad.adusername, wra.generalstatuskey, insertdate, wrt.description,
                        wrt.workflowRoleTypeKey
                        from x2.x2.workflowRoleAssignment wra (nolock)
                        left join [2am].dbo.aduser ad (nolock) on wra.adUserKey=ad.adUserKey
                        left join [2am].dbo.workflowRoleTypeOrganisationStructureMapping map (nolock)
                        on wra.workflowRoleTypeOrganisationStructureMappingKey=map.workflowRoleTypeOrganisationStructureMappingKey
                        left join [2am]..workflowRoleType wrt (nolock) on map.workflowRoleTypeKey=wrt.workflowRoleTypeKey
                        where wra.instanceid = {0} and wra.generalstatuskey = 1
                        order by wra.id desc", instanceID);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflowRoleType"></param>
        /// <returns>aduser/legalentity pair </returns>
        public Dictionary<int, string> GetActiveWorkflowRoleByTypeAndGenericKey(WorkflowRoleTypeEnum workflowRoleType, int genericKey)
        {
            Dictionary<int, string> results = new Dictionary<int, string>();
            string query =
                string.Format(@"select adUserName, wr.legalEntityKey from [2am]..workflowRole wr
                                join [2am]..aduser a on wr.legalentitykey=a.legalentitykey
                                where wr.workflowroletypekey={0}
                                and wr.generickey={1} and wr.generalStatusKey=1
                                order by wr.WorkflowRoleKey desc", (int)workflowRoleType, genericKey);
            SQLStatement s = new SQLStatement { StatementString = query };
            var q = dataContext.ExecuteSQLQuery(s);
            results.Add(q.Rows(0).Column(1).GetValueAs<int>(), q.Rows(0).Column(0).GetValueAs<string>());
            return results;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflowRoleType"></param>
        /// <returns>aduser/legalentity pair </returns>
        public Dictionary<int, string> GetActiveWorkflowRoleByTypeAndGenericKeyForRoundRobinAssign(WorkflowRoleTypeEnum workflowRoleType, int genericKey)
        {
            Dictionary<int, string> results = new Dictionary<int, string>();
            string query =
                string.Format(@"select adUserName, wr.legalEntityKey
                    from [2am]..workflowRole wr
                    join [2am]..aduser a on wr.legalentitykey=a.legalentitykey
                    join [2am]..userorganisationStructure uos on a.aduserkey = uos.aduserkey
	                    and wr.workflowRoleTypeKey = uos.genericKey
	                    and uos.genericKeyTypeKey = 34
                    where
                    wr.workflowroletypekey={0}
                    and wr.generickey={1}
                    and wr.generalStatusKey=1
                    and a.generalstatuskey = 1
                    order by wr.WorkflowRoleKey desc", (int)workflowRoleType, genericKey);
            SQLStatement s = new SQLStatement { StatementString = query };
            var q = dataContext.ExecuteSQLQuery(s);
            if (!q.HasResults)
            {
                return results;
            }
            results.Add(q.Rows(0).Column(1).GetValueAs<int>(), q.Rows(0).Column(0).GetValueAs<string>());
            return results;
        }

        public QueryResults GetWorkflowAssignmentByInstance(Int64 instanceID)
        {
            string query = string.Format(@"select * from x2.x2.WorkflowAssignment where instanceID = {0}", instanceID);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }
    }
}