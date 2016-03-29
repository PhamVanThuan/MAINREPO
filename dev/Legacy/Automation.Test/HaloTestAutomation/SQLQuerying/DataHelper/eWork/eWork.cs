using Automation.DataAccess.Interfaces;
using Common.Constants;
using Common.Enums;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace Automation.DataAccess.DataHelper
{
    public class EWorkDataHelper
    {
        private IDataContext dataContext;
        public EWorkDataHelper()
        {
            dataContext = new DataContext();
        }

        public QueryResultsRow GeteFolderCase(int accountKey, string eStage, string eMap)
        {
            try
            {
                var query =
                    string.Format(@"select * from [e-work]..efolder where eStageName='{0}'
                                        and eMapName='{1}' and eFolderName='{2}'", eStage, eMap, accountKey);
                var statement = new SQLStatement() { StatementString = query };
                var r = dataContext.ExecuteSQLQuery(statement);
                if (!r.HasResults)
                    return null;
                return r.Rows(0);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the eFolderId when provided with a folder name, stage name and a map name
        /// </summary>
        /// <param name="eFolderName">eFolder.eFolderName</param>
        /// <param name="eStageName">eFolder.eStageName</param>
        /// <param name="eMapName">eFolder.eMapName</param>
        /// <returns></returns>
        public QueryResults GetEFolderID(string eFolderName, string eStageName, string eMapName)
        {
            string query =
                string.Format(@"select efolderid from [e-work]..efolder
                                where efoldername like '{0}%'
                                and eStageName like ('%{1}%')
                                and eMapName = '{2}' ", eFolderName, eStageName, eMapName);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns the eFolderId when provided with a folder name, stage name and a map name
        /// </summary>
        /// <param name="eFolderName">eFolder.eFolderName</param>
        /// <param name="eStageName">eFolder.eStageName/param>
        /// <param name="eMapName">eFolder.eMapName</param>
        /// <returns></returns>
        public QueryResults GetEFolderID(string eFolderName, string eStageName)
        {
            string query =
                string.Format(@"select efolderid from [e-work]..efolder
                                where efoldername like '{0}%'
                                and eStageName like ('{1}')
                                and eMapName in
                                (
									select emapname from [e-work].dbo.estage
									where estage.estagename = efolder.estagename
                                )", eFolderName, eStageName);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// This statement will update the stage for an Ework workflow case when provided with its current map,
        /// current workflow stage and the folder name.
        /// </summary>
        /// <param name="newStage">Ework stage to be updated to</param>
        /// <param name="currentStage">Current Ework Stage</param>
        /// <param name="currentMap">Current Ework Map</param>
        /// <param name="folderName">Folder Name (usually AccountKey)</param>
        public void UpdateEworkStage(string newStage, string currentStage, string currentMap, string folderName)
        {
            string query =
                string.Format(@"update [e-work]..eFolder
                                set eStageName = '{0}'
                                from [e-work]..eFolder
                                join [e-work]..eMap eM on eFolder.eMapName=eM.eMapName
                                join [e-work]..eStage eS on eFolder.eStageName=eS.eStageName
                                where eFolder.eStageName = '{1}'
                                and eFolder.eMapName = '{2}'
                                and eFolder.eFolderName = '{3}'", newStage, currentStage, currentMap, folderName);
            SQLStatement s = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(s);
        }

        /// <summary>
        /// This statement will return an application that is in a particular X2 application management state where
        /// a certain Ework action cannot be performed on the related ework folder. An example of where this is used
        /// where we check that we can raise the X2 NTU Advise flag in ework before we can complete the NTU Pipeline
        /// activity in X2.
        /// </summary>
        /// <param name="eWorkAction">eWork action that cannot be performed</param>
        /// <param name="x2State">Current X2 state of the application</param>
        /// <param name="isFL">0 = New Business, 1 = Further Loan</param>
        /// <returns></returns>
        public int GetPipelineCaseWhereActionNotApplied(string eWorkAction, string x2State, int isFL)
        {
            string query =
                string.Format(@"select i.id, a.applicationKey, eWorkFolderid, act.eActionName
                                from x2.x2data.application_management a
                                join x2.x2.instance i on a.instanceid=i.id
                                join x2.x2.state s on i.stateid=s.id
                                join [e-work]..efolder e on eWorkFolderid=e.eFolderID
                                left join [e-work]..eAction act on e.eStageName=act.eStageName and act.eActionName='{0}'
                                where s.name = '{1}'
                                and act.eActionName is null and isFL = {2}", eWorkAction, x2State, isFL);
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults r = dataContext.ExecuteSQLQuery(statement);
            return r.Rows(0).Column("applicationKey").GetValueAs<int>();
        }

        /// <summary>
        /// This query will return the offerKey of an application management workflow
        /// case that is at a particular state in X2 and in the Ework maps.
        /// </summary>
        /// <param name="x2State"></param>
        /// <param name="eworkStage"></param>
        /// <param name="isFL"></param>
        /// <returns></returns>
        public int GetPipelineCaseInEworkStage(string x2State, string eworkStage, int isFL)
        {
            string query =
                string.Format(@"select i.id, a.applicationKey, eWorkFolderid from x2.x2data.application_management a
                                join x2.x2.instance i on a.instanceid=i.id
                                join x2.x2.state s on i.stateid=s.id
                                join [e-work]..efolder e on eWorkFolderid=e.eFolderID
                                join [e-work]..eStage eS on e.eStageName=eS.eStageName
                                where isFL = {0}
                                and s.name = '{1}'
                                and eS.eStageName='{2}'", isFL, x2State, eworkStage);
            SQLStatement s = new SQLStatement { StatementString = query };
            QueryResults r = dataContext.ExecuteSQLQuery(s);
            return r.Rows(0).Column("applicationKey").GetValueAs<int>();
        }

        /// <summary>
        /// This method will return the offerkey of an application management workflow case that is at a
        /// particular state in X2 and in Ework pipelein, where a specific action can be performed in Ework. This is required
        /// in order for the x2/ework interactions to complete successfully as we need to be able raise the flags correctly.
        /// </summary>
        /// <param name="eworkAction"></param>
        /// <param name="x2State"></param>
        /// <param name="isFL"></param>
        /// <returns></returns>
        public QueryResults GetPipelineCaseWhereActionIsApplied(string eworkAction, string x2State, int isFL)
        {
            string query =
                string.Format(@"select i.id, a.applicationKey, eWorkFolderid, act.eActionName, o.accountKey, e.eStageName
                                from x2.x2data.application_management a
                                join [2am].dbo.offer o on a.applicationkey=o.offerKey
                                join x2.x2.instance i on a.instanceid=i.id
                                join x2.x2.state s on i.stateid=s.id
                                join [e-work]..efolder e on eWorkFolderid=e.eFolderID
                                join [e-work]..eAction act on e.eStageName=act.eStageName and act.eActionName='{0}'
                                where s.name = '{1}'
                                and isFL = {2}", eworkAction, x2State, isFL);
            SQLStatement s = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(s);
        }

        /// <summary>
        /// Returns the eFolder.* when provided with a folder name, stage name and a map name
        /// </summary>
        /// <param name="eFolderName">eFolder.eFolderName</param>
        /// <param name="eStageName">eFolder.eStageName</param>
        /// <param name="eMapName">eFolder.eMapName</param>
        /// <returns></returns>
        public QueryResults GetEFolderRow(string eFolderName, string eStageName, string eMapName)
        {
            string query =
                string.Format(@"select * from [e-work]..efolder
                                where efoldername = '{0}'
                                and eStageName = '{1}'
                                and eMapName = '{2}' ", eFolderName, eStageName, eMapName);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns the [e-work]..LossControl data linked to an eFolder when provided with the eFolderName
        /// </summary>
        /// <param name="eFolderName">eFolderName</param>
        /// <returns></returns>
        public QueryResults GetLossControlByeFolder(string eFolderName)
        {
            string query =
                string.Format(@"select * from [e-work]..eFolder e
                                join [e-work]..LossControl lc on e.eFolderId=lc.eFolderId
                                where e.eFolderName like '%{0}%'", eFolderName);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns the usertodo and the [e-work]..ealert datafor an efolder of a specific account, in a specific Map and at a specific Stage.
        /// </summary>
        /// <param name="eFolderName">accountkey</param>
        /// <param name="eMapName">ework map name</param>
        /// <param name="eStageName">ework stage name</param>
        /// <returns></returns>
        public QueryResults GetEWorkEFolderAssignmentData(string eFolderName, string eMapName, string eStageName)
        {
            string query =
                string.Format(@"select	f.efolderid, f.emapname, f.estagename, isnull(lc.UserToDo,'') as UserToDo, a.*
                                from	[e-work]..efolder f
		                                join [e-work]..losscontrol lc on f.efolderid = lc.efolderid
		                                left join [e-work]..ealert a on f.efolderid = a.efolderid
                                where f.efoldername = '{0}'
                                        and f.emapname = '{1}'
                                        and f.estagename like '{2}'", eFolderName, eMapName, eStageName);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Fetches an ework event for a specified action.
        /// </summary>
        /// <param name="eFolderId">eFolderID</param>
        /// <param name="eActionName">eActionName</param>
        /// <param name="eventTime">eventTime</param>
        /// <returns></returns>
        public QueryResults GetEworkEvent(string eFolderId, string eActionName, string eventTime)
        {
            string query =
                string.Format(@"select * from [e-work]..eEvent where eFolderID='{0}' and eActionName='{1}' and eEventTime>='{2}'", eFolderId, eActionName, eventTime);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Leave accountkey = 0 to bring back a collection of random ework accounts
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="eMapName"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.eWorkCase> GetEWorkCases(string eMapName, string eWorkStageName, int accountkey = 0)
        {
            var p = new DynamicParameters();
            p.Add("@eMapName", value: eMapName, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@eStageName", value: eWorkStageName, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@accountkey", value: accountkey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            var eWorkLossControlRecords = dataContext.Query<Automation.DataModels.eWorkCase, Automation.DataModels.Account, Automation.DataModels.eWorkCase>
                    ("[2am].test.GetEWorkRecords", (eworkCase, account) =>
                    {
                        eworkCase.Account = account;
                        return eworkCase;
                    }, parameters: p, commandtype: System.Data.CommandType.StoredProcedure, splitOn: "accountkey");
            return eWorkLossControlRecords;
        }

        /// <summary>
        /// Updates the ework assigned user to a specific user.
        /// </summary>
        /// <param name="newUserToDo">User to update to</param>
        /// <param name="accountKey">accountKey</param>
        /// <param name="eStageName">Ework stage</param>
        public void UpdateEworkAssignedUser(int accountKey, string newUserToDo, string eStageName)
        {
            newUserToDo = newUserToDo.Replace(@"SAHL\", string.Empty);
            string query =
                string.Format(@"update loss
								set loss.userToDo = '{0}'
								from
								[e-work]..efolder e
								join [e-work]..lossControl loss on e.efolderid=loss.efolderid
								and e.eStageName='{1}'
								where eFolderName='{2}'", newUserToDo, eStageName, accountKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        public IEnumerable<Automation.DataModels.TokenAssignment> GetTokenAssignments()
        {
            return dataContext.Query<Automation.DataModels.TokenAssignment>(@"select * from sahldb..TokenAssignment order by 1 desc");
        }
    }
}