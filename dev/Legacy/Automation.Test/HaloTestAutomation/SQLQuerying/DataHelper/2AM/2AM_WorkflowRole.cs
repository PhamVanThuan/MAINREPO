using Common.Enums;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Sets the general status in the UserOrganisationStructure, ADUser and UserOrganisationStructureRoundRobinStatus tables for all
        /// users that are mapped to a workflow role type
        /// <param name="wrt">Workflow Role Type</param>
        /// <param name="gs">General Status to set</param>
        /// <param name="UOS">TRUE = update UserOrganisationStructure table</param>
        /// <param name="ADUser">TRUE = update ADUser table</param>
        /// <param name="UOSRR">TRUE = update UserOrganisationStructureRoundRobinStatus table</param>
        /// </summary>
        public void UpdateStatusOfAllUsersMappedToWorkflowRoleType(WorkflowRoleTypeEnum wrt, GeneralStatusEnum gs, bool UOS, bool ADUser, bool UOSRR)
        {
            if (UOS)
            {
                string uosQuery =
                    string.Format(@"update u
                                set generalStatusKey={0}
                                from userOrganisationStructure u
                                where genericKey={1}
                                and genericKeyTypeKey=34", (int)gs, (int)wrt);
                SQLStatement statement = new SQLStatement { StatementString = uosQuery };
                dataContext.ExecuteNonSQLQuery(statement);
            }
            if (ADUser)
            {
                string adQuery =
                    string.Format(@"update ad
                                set generalStatusKey={0}
                                from dbo.userOrganisationStructure u
                                join dbo.aduser ad on u.aduserKey=ad.adUserKey
                                where genericKey={1}
                                and genericKeyTypeKey=34", (int)gs, (int)wrt);
                SQLStatement statement = new SQLStatement { StatementString = adQuery };
                dataContext.ExecuteNonSQLQuery(statement);
            }
            if (UOSRR)
            {
                string rrQuery =
                    string.Format(@"
                                update rr
                                set generalStatusKey={0}
                                from dbo.userOrganisationStructure u
                                join dbo.userOrganisationStructureRoundRobinStatus rr
                                on u.userOrganisationStructureKey=rr.userOrganisationStructureKey
                                where genericKey={1}
                                and genericKeyTypeKey=34", (int)gs, (int)wrt);
                SQLStatement statement = new SQLStatement { StatementString = rrQuery };
                dataContext.ExecuteNonSQLQuery(statement);
            }
        }

        public QueryResults GetActiveWorkflowRolesByADUser(string aduser)
        {
            string query = string.Format(@"select * from dbo.workflowrole as wr
	                                        inner join dbo.legalentity as le
		                                        on wr.legalentitykey = le.legalentitykey
	                                        inner join dbo.aduser as ad
		                                        on le.legalentitykey = ad.legalentitykey
                                        where ad.adusername = '{0}' and wr.generalstatuskey = 1", aduser);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets the users mapped to a workflow role type
        /// </summary>
        /// <param name="adUserName"></param>
        /// <returns>au.ADUserKey, au.ADUsername</returns>
        public QueryResults GetAdUsersForWorkflowRoleType(string adUserName)
        {
            var query = string.Format(@"select au.ADUserKey, au.ADUsername,
							le.FirstNames + ' ' + le.Surname + ' ( ' + wrt.Description + ')' as Description,
							sm.WorkflowRoleTypeKey
							from [2am].[dbo].[WorkflowRoleTypeOrganisationStructureMapping] sm (nolock)
							join [2am].[dbo].[WorkflowRoleType] wrt (nolock) on wrt.WorkflowRoleTypeKey = sm.WorkflowRoleTypeKey
							join [2am].[dbo].[UserOrganisationStructure] uos (nolock) on uos.OrganisationStructureKey = sm.OrganisationStructureKey
							join [2am].[dbo].[ADUser] au (nolock) on au.ADUserKey = uos.ADUserKey
							join [2am].[dbo].[LegalEntity] le (nolock) on le.LegalEntityKey = au.LegalEntityKey
							where au.ADUserName = '{0}'", adUserName);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets the users mapped to a workflow role type
        /// </summary>
        /// <param name="adUserName"></param>
        /// <returns>au.ADUserKey, au.ADUsername</returns>
        public QueryResults GetAdUsersForWorkflowRoleType(WorkflowRoleTypeEnum workflowRoleType)
        {
            var query = string.Format(@"select au.ADUserKey, au.ADUsername,
							le.FirstNames + ' ' + le.Surname + ' ( ' + wrt.Description + ')' as Description,
							sm.WorkflowRoleTypeKey, au.generalstatuskey
							from [2am].[dbo].[WorkflowRoleTypeOrganisationStructureMapping] sm (nolock)
							join [2am].[dbo].[WorkflowRoleType] wrt (nolock) on wrt.WorkflowRoleTypeKey = sm.WorkflowRoleTypeKey
							join [2am].[dbo].[UserOrganisationStructure] uos (nolock) on uos.OrganisationStructureKey = sm.OrganisationStructureKey
							join [2am].[dbo].[ADUser] au (nolock) on au.ADUserKey = uos.ADUserKey
							join [2am].[dbo].[LegalEntity] le (nolock) on le.LegalEntityKey = au.LegalEntityKey
							where wrt.WorkflowRoleTypeKey = {0}", (int)workflowRoleType);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }
    }
}