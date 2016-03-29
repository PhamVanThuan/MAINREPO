using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Automation.DataModels;
using Common.Enums;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// This will return the ADUserKey for a user given the ADUserName WITHOUT THE SAHL\ prefix and their org structure key
        /// </summary>
        /// <param name="adUserName"></param>
        /// <param name="orgStructureKey"></param>
        /// <returns></returns>
        public QueryResults GetADUserKeyByADUserName(string adUserName, int orgStructureKey)
        {
            string query = string.Format(@"select a.aduserkey
                                    from [2am].dbo.aduser a with (nolock)
                                    join [2am].dbo.userorganisationstructure uos with (nolock) on a.aduserkey=uos.aduserkey
                                    where a.adusername like '%{0}'
                                    and uos.OrganisationStructureKey={1}
                                    and a.GeneralStatusKey=1", adUserName, orgStructureKey);
            SQLStatement statement = new SQLStatement { StatementString = query };

            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Gets an ADUser record when supplied with an ADUserName
        /// </summary>
        /// <param name = "adUserName">ADUserName</param>
        /// <returns>ADUser.*</returns>
        public QueryResults GetADUser(string adUserName)
        {
            string query = string.Format(@"select a.* from [2am].dbo.AdUser a with (nolock)
                                           join userorganisationstructure uos with (nolock) on a.aduserkey=uos.aduserkey
                                           where aduserName='{0}'", adUserName);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Runs the test.IsUserInSameBranchAsApp' stored procedure in order to determine if the user and the application are in the same branch.
        /// </summary>
        /// <param name = "adUserName">ADUserName</param>
        /// <param name = "offerKey">OfferKey</param>
        /// <returns>1 if the user is not in the same branch</returns>
        public QueryResults isUserInSameBranchAsApp(string adUserName, string offerKey)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "test.IsUserInSameBranchAsApp" };
            proc.AddParameter(new SqlParameter("@ADUserName", adUserName));
            proc.AddParameter(new SqlParameter("@OfferKey", offerKey));
            return dataContext.ExecuteStoredProcedureWithResults(proc);
        }

        ///<summary>
        /// When provided with an AD User Name this will return the legal entity legal name
        ///</summary>
        ///<param name="adUserName">ADUserName</param>
        ///<param name="isInitials">0 or 1</param>
        ///<param name="gsKey">GeneralStatusKey</param>
        ///<returns></returns>
        public QueryResults GetLegalEntityNameFromADUserName(string adUserName, int isInitials, GeneralStatusEnum gsKey)
        {
            string query = string.Format(@"select top 1 dbo.legalentityLegalName(le.legalEntityKey, {0} )
                                            from [2am].dbo.aduser a
                                            join [2am].dbo.legalentity le on a.legalEntityKey = le.legalEntityKey
                                            join [2am].dbo.userOrganisationStructure uos on a.aduserkey=uos.aduserkey
                                            where adusername like '%{1}' and a.generalStatusKey = {2}", isInitials, adUserName, (int)gsKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Used to update the ADUser.GeneralStatusKey and the userOrganisationStructureRoundRobinStatus.GeneralStatusKey
        /// </summary>
        /// <param name="adUserName">adUserName</param>
        /// <param name="genStatus">New ADUser Status</param>
        /// <param name="rrStatus">New Round Robin Status</param>
        /// <returns></returns>
        public bool UpdateADUserStatus(string adUserName, GeneralStatusEnum genStatus, GeneralStatusEnum rrStatus, GeneralStatusEnum uosStatus)
        {
            string query =
                string.Format(@"update ad
                                set ad.GeneralStatusKey = {0}
                                from [2am].dbo.aduser ad
                                join [2am].dbo.userorganisationstructure uos
                                on ad.aduserkey=uos.aduserkey
                                join [2am].dbo.userOrganisationStructureRoundRobinStatus rr
                                on uos.userOrganisationStructureKey=rr.userOrganisationStructureKey
                                where ad.adUserName = '{1}'

                                update rr
                                set rr.GeneralStatusKey = {2}
                                from [2am].dbo.aduser ad
                                join [2am].dbo.userorganisationstructure uos
                                on ad.aduserkey=uos.aduserkey
                                join [2am].dbo.userOrganisationStructureRoundRobinStatus rr
                                on uos.userOrganisationStructureKey=rr.userOrganisationStructureKey
                                where ad.adUserName = '{3}'

                                update uos
                                set uos.GeneralStatusKey = {4}
                                from [2am].dbo.aduser ad
                                join [2am].dbo.userorganisationstructure uos
                                on ad.aduserkey=uos.aduserkey
                                join [2am].dbo.userOrganisationStructureRoundRobinStatus rr
                                on uos.userOrganisationStructureKey=rr.userOrganisationStructureKey
                                where ad.adUserName = '{5}'", (int)genStatus, adUserName, (int)rrStatus, adUserName, (int)uosStatus, adUserName);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// This will return the ADUserKey for a user given the ADUserName WITHOUT THE SAHL\ prefix and their org structure key
        /// </summary>
        /// <param name="adusername"></param>
        /// <returns></returns>
        public IEnumerable<ADUser> GetADUserKeyByADUserName(params string[] adusername)
        {
            string adusernameStr = Helpers.GetDelimitedString<string>(adusername, "','");
            string query = string.Format(@" select a.AdUserKey, a.adUserName, a.GeneralStatusKey as GeneralStatus, a.legalEntityKey
                                             from [2am].dbo.ADUser a with (nolock)
                                             where adusername in ('{0}') and a.GeneralStatusKey=1", adusernameStr);
            return dataContext.Query<ADUser>(query);
        }

        /// <summary>
        /// Gets a list of debt counselling users who are not one of our test users.
        /// </summary>
        /// <returns></returns>
        public QueryResults GetDebtCounsellingBusinessUsers()
        {
            string query =
                string.Format(@"select adusername
                                from [2am].dbo.userOrganisationStructure u
                                join [2am].dbo.aduser ad on u.aduserKey=ad.adUserKey
                                join [2am].dbo.userOrganisationStructureRoundRobinStatus rr
                                on u.userOrganisationStructureKey=rr.userOrganisationStructureKey
                                where
                                u.genericKey in (1,2,3,4,5,6)
                                and u.genericKeyTypeKey=34
                                and
                                aduserName not like 'SAHL\DCSUser%'
                                and aduserName not like 'SAHL\DCCCUser%'
                                and aduserName not like 'SAHL\DCMUser%'
                                and aduserName not like 'SAHL\DCAUser%'
                                and aduserName not like 'SAHL\LCLCUser%'
                                and aduserName not like 'SAHL\LCDUser%'
                                and aduserName not like 'SAHL\DCCUser%'");
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userLoginName"></param>
        /// <returns></returns>
        public QueryResults RoleSQL(string userLoginName)
        {
            var query
                = String.Format(@" with ROSM (organisationstructurekey, roledescription, aduserkey)
                                    as
                                    (
		                                    select * from (select
						                                    ortosm.organisationstructurekey,
						                                    ort.description as roledescription,
						                                    uos.aduserkey
					                                       from OfferRoleTypeOrganisationStructureMapping as ortosm
				   			                                    join OfferRoleType as ort
								                                    on ort.OfferRoleTypeKey = ortosm.OfferRoleTypeKey
							                                    left join UserOrganisationStructure as uos (nolock)
								                                    on ortosm.OrganisationStructureKey = uos.OrganisationStructureKey) as t1
		                                    union all
		                                    select * from (select
						                                    wrtosm.organisationstructurekey,
						                                    wrt.description as roledescription,
						                                    uos.aduserkey
					                                       from WorkflowRoleTypeOrganisationStructureMapping as wrtosm
							                                    join WorkflowRoleType as wrt (nolock)
								                                    on wrt.workflowroletypekey = wrtosm.workflowroletypekey
							                                    left join UserOrganisationStructure as uos (nolock)
								                                    on wrtosm.OrganisationStructureKey = uos.OrganisationStructureKey)  as t2
                                    )
                                    select
                                          ROSM.RoleDescription as 'OfferRoleType'
                                    from OrganisationStructure mos (nolock)
	                                    join UserOrganisationStructure muos (nolock)
		                                      on mos.OrganisationStructureKey = muos.OrganisationStructureKey
	                                    join ADUser mad (nolock)
		                                      on muos.ADUserKey = mad.ADUserKey
	                                    join OrganisationStructure os (nolock)
		                                      on os.ParentKey = mos.ParentKey
	                                    join ROSM
		                                    on os.organisationstructurekey=ROSM.organisationstructurekey
	                                    left join ADUser ad (nolock)
		                                      on ROSM.ADUserKey = ad.ADUserKey
                                    where
                                          mos.description = 'Manager'
                                                and
                                          mad.GeneralStatusKey = 1
                                                and
                                          mad.adusername = '{0}'
                                    group by
	                                    ROSM.RoleDescription
                                    order by
	                                    ROSM.RoleDescription
                                    ", userLoginName);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userLoginName"></param>
        /// <returns></returns>
        public QueryResults TestAduserSQL(string userLoginName)
        {
            var query
                = String.Format
                    (@" with testSet (ADUsername,OfferRoleType,OSKey)
                    as
                    (
                           select
                                  top 1 mad.adusername,
                                  ort.Description,
                                  uos.OrganisationStructureKey
                           from OrganisationStructure mos (nolock)
                           join UserOrganisationStructure muos (nolock)
                                  on mos.OrganisationStructureKey = muos.OrganisationStructureKey
                           join ADUser mad (nolock)
                                  on muos.ADUserKey = mad.ADUserKey
                           join OrganisationStructure os (nolock)
                                  on os.ParentKey = mos.ParentKey
                           join OfferRoleTypeOrganisationStructureMapping ortosm (nolock)
                                  on ortosm.OrganisationStructureKey = os.OrganisationStructureKey
                           join OfferRoleType ort (nolock)
                                  on ort.OfferRoleTypeKey = ortosm.OfferRoleTypeKey
                           join UserOrganisationStructure uos (nolock)
                                  on uos.OrganisationStructureKey = ortosm.OrganisationStructureKey
                           join UserOrganisationStructureRoundRobinStatus uosrrs (nolock)
                                  on uos.UserOrganisationStructureKey = uosrrs.UserOrganisationStructureKey
                           join ADUser ad (nolock)
                                  on uos.ADUserKey = ad.ADUserKey
                           where
                                  mos.description = 'Manager'
                                         and
                                  mad.adusername = '{0}'
                           group by
                                  mad.adusername, ort.Description, uos.OrganisationStructureKey
                           having count(ad.AduserKey) > 1
                           order by
                                  mad.adusername, count(ad.AduserKey) desc
                    )
                    select
                        top 1
                            ad.ADUserName as 'TestADUser',
                            ts.OfferRoleType as 'OfferRoleType',
                            ts.ADUsername as 'LoginADUser',
                            ts.OSKey as 'OrgStructKey'
                    from testSet ts
                    join UserOrganisationStructure uos (nolock)
                           on uos.OrganisationStructureKey = ts.OSKey
                    join ADUser ad (nolock)
                           on uos.ADUserKey = ad.ADUserKey
                    order by
                           ad.ADUserName", userLoginName);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="adUserStatus"></param>
        /// <param name="adUsername"></param>
        /// <returns></returns>
        public bool UpdateADUserStatusSQL(string adUserStatus, string adUsername)
        {
            var query
                = String.Format(@"
                    update ad
                    set ad.GeneralStatusKey = (select GeneralStatusKey from GeneralStatus (nolock) where description = '{0}')
                    from ADUser ad (nolock)
                    where
                    ad.ADUsername = '{1}'", adUserStatus, adUsername);

            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="adUserStatus"></param>
        /// <param name="adUsername"></param>
        /// <returns></returns>
        public bool UpdateADUserRoundRobinStatusSQL(string adUserStatus, string adUsername)
        {
            var query
                = String.Format(@"
					update uosrrs
					set GeneralStatusKey = (select GeneralStatusKey from GeneralStatus (nolock) where description = '{0}')
					from ADUser ad (nolock)
					join UserOrganisationStructure uos (nolock)
						on ad.ADUserKey = uos.ADUserKey
					join UserOrganisationStructureRoundRobinStatus uosrrs (nolock)
						on uos.UserOrganisationStructureKey = uosrrs.UserOrganisationStructureKey
					where ad.ADUserName = '{1}'", adUserStatus, adUsername);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="adUserName"></param>
        /// <returns></returns>
        public QueryResults ADUserStatusSQL(string adUserName)
        {
            var query =
            String.Format(@"
                select gs.description as 'ADUserStatus'
                from ADUser ad (nolock)
                join GeneralStatus gs (nolock)
	                on gs.GeneralStatusKey = ad.GeneralStatusKey
                where
	                ad.ADUserName = '{0}'", adUserName);

            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="adUserName"></param>
        /// <param name="osKey"></param>
        /// <returns></returns>
        public QueryResults RoundRobinStatusSQL(string adUserName, int osKey)
        {
            var query =
                String.Format(@"
                select
	                gs.description as 'RoundRobinStatus'
                from ADUser ad (nolock)
                join UserOrganisationStructure uos (nolock)
	                on uos.ADUserKey = ad.ADUserKey
                join UserOrganisationStructureRoundRobinStatus uosrrs (nolock)
	                on uos.UserOrganisationStructureKey = uosrrs.UserOrganisationStructureKey
                join GeneralStatus gs (nolock)
	                on gs.GeneralStatusKey = uosrrs.GeneralStatusKey
                where
	                ad.ADUserName = '{0}'
		                and
	                uos.OrganisationStructureKey = {1}", adUserName, osKey);

            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userLoginName"></param>
        /// <returns></returns>
        public QueryResults SetUserInactiveAndGetTestDataSQL(string userLoginName)
        {
            var query =
               String.Format(@"
                    declare @TestADUser varchar(50)
                    declare @OfferRoleType varchar(50)
                    declare @OSKey int
                    declare @LoginADUserName varchar(50)

                    ;with testSet (ADUsername,OfferRoleType,OSKey)
                    as
                    (
	                    select
		                    top 1 mad.adusername,
		                    ort.Description,
		                    uos.OrganisationStructureKey
	                    from OrganisationStructure mos (nolock)
	                    join UserOrganisationStructure muos (nolock)
		                    on mos.OrganisationStructureKey = muos.OrganisationStructureKey
	                    join ADUser mad (nolock)
		                    on muos.ADUserKey = mad.ADUserKey
	                    join OrganisationStructure os (nolock)
		                    on os.ParentKey = mos.ParentKey
	                    join OfferRoleTypeOrganisationStructureMapping ortosm (nolock)
		                    on ortosm.OrganisationStructureKey = os.OrganisationStructureKey
	                    join OfferRoleType ort (nolock)
		                    on ort.OfferRoleTypeKey = ortosm.OfferRoleTypeKey
	                    join UserOrganisationStructure uos (nolock)
		                    on uos.OrganisationStructureKey = ortosm.OrganisationStructureKey
	                    join UserOrganisationStructureRoundRobinStatus uosrrs (nolock)
		                    on uos.UserOrganisationStructureKey = uosrrs.UserOrganisationStructureKey
	                    join ADUser ad (nolock)
		                    on uos.ADUserKey = ad.ADUserKey
	                    where
		                    mos.description = 'Manager'
			                    and
		                    mad.adusername = '{0}'
	                    group by
		                    mad.adusername, ort.Description, uos.OrganisationStructureKey
	                    having count(ad.AduserKey) > 1
	                    order by
		                    mad.adusername, count(ad.AduserKey) desc
                    )
                    select
	                    top 1
		                    @TestADUser = ad.ADUserName,
		                    @OfferRoleType = ts.OfferRoleType,
		                    @OSKey = ts.OSKey,
		                    @LoginADUserName = ts.ADUsername
                    from testSet ts
                    join UserOrganisationStructure uos (nolock)
	                    on uos.OrganisationStructureKey = ts.OSKey
                    join ADUser ad (nolock)
	                    on uos.ADUserKey = ad.ADUserKey
                    order by
	                    ad.ADUserName

                    -- Set the TOP 1 user to active
                    update ad
	                    set ad.GeneralStatusKey = 1
                    from ADUser ad (nolock)
                    where
	                    ad.ADUsername = @TestADUser

                    -- Set all other users to inactive
                    update ad
	                    set ad.GeneralStatusKey = 2
                    from UserOrganisationStructure uos (nolock)
                    join ADUser ad (nolock)
	                    on uos.ADUserKey = ad.ADUserKey
                    where
	                    uos.OrganisationStructureKey = @OSKey
		                    and
	                    ad.ADUsername <> @TestADUser

                    select
	                    @TestADUser as 'TestADUser',
	                    @OfferRoleType as 'OfferRoleType',
	                    @OSKey as 'OrgStructKey',
	                    @LoginADUserName as 'LoginADUser'", userLoginName);

            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="orgStructKey"></param>
        /// <returns></returns>
        public QueryResults CheckAtLeastOneUserStillActiveForOrgStructKeySQL(int orgStructKey)
        {
            var query =
              String.Format(@"
                select count(ad.aduserkey)
                from UserOrganisationStructure uos (nolock)
                join ADUser ad (nolock)
	                on uos.ADUserKey = ad.ADUserKey
                where
	                ad.GeneralStatusKey = 1 and uos.OrganisationStructureKey = {0}", orgStructKey);

            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="orgStructKey"></param>
        /// <returns></returns>
        public void SetAllUsersForOrgStructKeyActive(int orgStructKey)
        {
            var query =
             String.Format(@"
                update ad
	                set ad.GeneralStatusKey = 1
                from UserOrganisationStructure uos (nolock)
                join ADUser ad (nolock)
	                on uos.ADUserKey = ad.ADUserKey
                where
	                uos.OrganisationStructureKey = {0}", orgStructKey);
            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Role SQL Count
        /// </summary>
        /// <param name="userLoginName"></param>
        /// <returns></returns>
        public QueryResults RoleSQLCount(string userLoginName)
        {
            var query = String.Format(@"select
									  --mad.adusername as 'LoginADUserName',
									  ort.Description as 'OfferRoleType',
									  count(mad.adusername) as [Count]
								from OrganisationStructure mos (nolock)
								join UserOrganisationStructure muos (nolock)  on mos.OrganisationStructureKey = muos.OrganisationStructureKey
								join ADUser mad (nolock)  on muos.ADUserKey = mad.ADUserKey
								join OrganisationStructure os (nolock)  on os.ParentKey = mos.ParentKey
								join OfferRoleTypeOrganisationStructureMapping ortosm (nolock)  on ortosm.OrganisationStructureKey = os.OrganisationStructureKey
								join OfferRoleType ort (nolock) on ort.OfferRoleTypeKey = ortosm.OfferRoleTypeKey
								join UserOrganisationStructure uos (nolock)  on uos.OrganisationStructureKey = ortosm.OrganisationStructureKey
								join UserOrganisationStructureRoundRobinStatus uosrrs (nolock) on uos.UserOrganisationStructureKey = uosrrs.UserOrganisationStructureKey
								join ADUser ad (nolock)   on uos.ADUserKey = ad.ADUserKey
								where mos.description = 'Manager' and  mad.GeneralStatusKey = 1	and   mad.adusername = '{0}'
								group by  ort.Description
								order by  ort.Description", userLoginName);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="roleType"></param>
        /// <returns></returns>
        public QueryResults GetBranchUsersForApplication(int offerKey, OfferRoleTypeEnum roleType)
        {
            var query = string.Format(@"declare @key int
                                        --get the parent of the consultant on the application
                                        select @key = organisationStructureParentKey from [2am].dbo.offerRole ofr
                                        join [2am].dbo.adUser ad on ofr.legalEntityKey=ad.legalEntityKey
                                        join [2am].dbo.userOrganisationStructure leos on ad.adUserKey=leos.adUserKey
                                        join [2am].dbo.vorganisationStructure os on leos.organisationStructureKey=os.organisationStructureKey
                                        where offerKey={0}
                                        and ofr.offerRoleTypeKey=101
                                        and ofr.generalStatusKey=1
                                        --the find the admins linked to the same parent
                                        select dbo.legalEntityLegalName(a.legalEntityKey,0), a.aduserName,a.GeneralStatusKey as ADUserStatus from [2am].dbo.vOrganisationStructure vos
                                        join [2am].dbo.userOrganisationStructure uos on vos.organisationStructureKey=uos.organisationStructureKey
                                        join [2am].dbo.offerRoleTypeOrganisationStructureMapping map on uos.organisationStructureKey=map.organisationStructureKey
                                        join [2am].dbo.aduser a on uos.aduserkey=a.aduserkey and a.legalEntityKey not in
                                        (select legalEntityKey from OfferRole ofr where offerRoleTypeKey={1} and generalStatusKey=1 and offerKey={2})
                                        where organisationStructureParentKey = @key
                                        and offerRoleTypeKey = {3}", offerKey, (int)roleType, offerKey, (int)roleType);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        public QueryResults GetAdUserName(int udUserKey)
        {
            var query = string.Format(@"select adusername from dbo.aduser where aduserkey = {0}", udUserKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }
        public QueryResults GetAdUserNameByLegalEntityKey(int legalEntityKey)
        {
            var query = string.Format(@"select adusername from dbo.aduser where legalEntityKey = {0}", legalEntityKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }
        public string GetADUserPlayingWorkflowRole(string role, string userToExclude)
        {
            var query = string.Format(@"select top 1 adusername from [2am].dbo.userOrganisationStructure uos
                    join [2am].dbo.workflowRoleType wrt on uos.genericKey = wrt.workflowRoleTypeKey
                    join [2am].dbo.aduser a on uos.aduserKey = a.aduserKey
                    where generickeytypekey=34
                    and wrt.description = '{0}'
                    and a.adusername <> '{1}'
                    and a.generalstatuskey=1
                    order by newid()", role, userToExclude);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement).Rows(0).Column(0).GetValueAs<string>();
        }

        public string GetADUserPlayingOfferRole(OfferRoleTypeEnum role, string userToExclude)
        {
            var query = string.Format(@"SELECT TOP 1 a.ADUserName FROM [2AM].dbo.UserOrganisationStructure uos
                    INNER JOIN [2AM].dbo.OrganisationStructure os ON uos.OrganisationStructureKey=os.OrganisationStructureKey
                    INNER JOIN [2AM].dbo.OfferRoleTypeOrganisationStructureMapping map ON os.OrganisationStructureKey = map.OrganisationStructureKey
                    INNER JOIN [2AM].dbo.ADUser a ON uos.ADUserkey = a.ADUserkey
                    WHERE map.offerRoleTypeKey = {0} AND a.ADUserName <> '{1}'
                    AND uos.GeneralStatusKey=1 AND os.GeneralStatusKey=1 AND a.GeneralStatusKey=1
                    ORDER BY NewID()", (int)role, userToExclude);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement).Rows(0).Column(0).GetValueAs<string>();
        }
    }
}