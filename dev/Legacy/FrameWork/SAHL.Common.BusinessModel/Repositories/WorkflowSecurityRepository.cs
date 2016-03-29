using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Aspects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using offerRole = SAHL.Common.BusinessModel.Interfaces.Repositories.OfferRole;
using workflowRole = SAHL.Common.BusinessModel.Interfaces.Repositories.WorkflowRole;
using System.Linq;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IWorkflowSecurityRepository))]
    public class WorkflowSecurityRepository : AbstractRepositoryBase, IWorkflowSecurityRepository
    {
        private ICastleTransactionsService castleTransactionService;
        private IUIStatementService uiStatementService;

        public WorkflowSecurityRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public WorkflowSecurityRepository(ICastleTransactionsService castleTransactionService, IUIStatementService uiStatementService)
        {
            this.castleTransactionService = castleTransactionService;
            this.uiStatementService = uiStatementService;
        }

        [DSCacheAttribute_OfferRole]
        public offerRole.WorkflowAssignment GetOfferRoleOrganisationStructure()
        {
            string applicationName = "Client.Cache.Loader";
            string statementName = "LoadOrgStructureInfo";

            string uistatementSQL = string.Format(@"select Statement from [2am].dbo.uiStatement where ApplicationName = '{0}' and StatementName = '{1}' order by Version desc", applicationName, statementName);
            //object uiStatement = AbstractRepositoryBase.ExecuteScalarOnCastleTran(uistatementSQL, typeof(GeneralStatus_DAO), null);
            object uiStatement = castleTransactionService.ExecuteScalarOnCastleTran(uistatementSQL, Databases.TwoAM, new ParameterCollection());
            if (null == uiStatement)
                throw new Exception(String.Format("uiStatement:{0}, {1} isnt in the uiStatement Table", applicationName, statementName));

            System.Collections.Specialized.StringCollection TableMappings = new System.Collections.Specialized.StringCollection();
            TableMappings.Add("ADUser");
            TableMappings.Add("OfferRoleType");
            TableMappings.Add("OrganisationStructure");
            TableMappings.Add("UserOrganisationStructure");
            TableMappings.Add("OfferRoleTypeOrganisationStructureMapping");

            offerRole.WorkflowAssignment ds = new offerRole.WorkflowAssignment();
            ds = (offerRole.WorkflowAssignment)castleTransactionService.ExecuteQueryOnCastleTran(uiStatement.ToString(), Databases.TwoAM, new ParameterCollection(), TableMappings, ds);
            //AbstractRepositoryBase.ExecuteQueryOnCastleTran(Convert.ToString(uiStatement), typeof(GeneralStatus_DAO), null, TableMappings, ds);
            //DSCache.Add(KnownGoodKeys.ORGSTRUCTURE_DATASET_DS.ToString(), ds);
            return ds;
        }

        [DSCacheAttribute_WorkflowRole]
        public workflowRole.WorkflowAssignment GetWorkflowRoleOrganisationStructure()
        {
            string applicationName = "Client.Cache.Loader";
            string statementName = "LoadOrgStructureInfo";

            string uistatementSQL = string.Format(@"select Statement from [2am].dbo.uiStatement where ApplicationName = '{0}' and StatementName = '{1}' order by Version desc", applicationName, statementName);
            object uiStatement = castleTransactionService.ExecuteScalarOnCastleTran(uistatementSQL, Databases.TwoAM, new ParameterCollection());
            if (null == uiStatement)
                throw new Exception(String.Format("uiStatement:{0}, {1} isnt in the uiStatement Table", applicationName, statementName));

            System.Collections.Specialized.StringCollection TableMappings = new System.Collections.Specialized.StringCollection();
            TableMappings.Add("ADUser");
            TableMappings.Add("OfferRoleType");
            TableMappings.Add("OrganisationStructure");
            TableMappings.Add("UserOrganisationStructure");
            TableMappings.Add("OfferRoleTypeOrganisationStructureMapping");
            TableMappings.Add("UserOrganisationStructureRoundRobinStatus");
            TableMappings.Add("RoundRobinPointer");
            TableMappings.Add("RoundRobinPointerDefinition");

            //new debt counselling stuff
            TableMappings.Add("WorkflowRoleType");
            TableMappings.Add("WorkflowRoleTypeGroup");
            TableMappings.Add("WorkflowRoleTypeOrganisationStructureMapping");

            workflowRole.WorkflowAssignment ds = new workflowRole.WorkflowAssignment();

            //SAHL.X2.Framework.DataAccess.WorkerHelper.FillFromQuery(ds, TableMappings, Convert.ToString(uiStatement), Tran.Context, new SAHL.X2.Framework.DataAccess.ParameterCollection());
            ds = (workflowRole.WorkflowAssignment)castleTransactionService.ExecuteQueryOnCastleTran(uiStatement.ToString(), Databases.TwoAM, new ParameterCollection(), TableMappings, ds);
            //DSCache.Clear();
            //DSCache.Add(KnownGoodKeys.ORGSTRUCTURE_DATASET_Client.ToString(), ds);

            return ds;
        }

        public offerRole.WorkflowAssignment.ADUserRow GetADUserRowByName(string adUserName)
        {
            var ds = GetOfferRoleOrganisationStructure();
            //object Cache = DSCache.Get(KnownGoodKeys.ORGSTRUCTURE_DATASET_Client.ToString());
            //offerRole.WorkflowAssignment ds = null;
            //if (null == Cache)
            //    ds = LoadOrgStructureInfo(Tran);
            //else
            //    ds = (offerRole.WorkflowAssignment)Cache;

            object o = ds.ADUser.Select(string.Format("Adusername='{0}'", adUserName));
            if (null == o) throw new Exception(string.Format("Cant find username:{0}", adUserName));
            offerRole.WorkflowAssignment.ADUserRow[] arr = (offerRole.WorkflowAssignment.ADUserRow[])o;
            return arr[0];
        }

        public void AssignWorkflowRole(long instanceID, int adUserKey, int blaKey, string stateName)
        {
            string query = string.Format("exec pr_AssignWorkflowRole {0}, {1}, {2}, '{3}'", instanceID, blaKey, adUserKey, stateName);
            castleTransactionService.ExecuteNonQueryOnCastleTran(query, Databases.X2, new ParameterCollection());
        }

        public void Assign2AMOfferRole(int offerKey, int offerRoleTypeKey, int legalEntityKey)
        {
            // The aim here is to get an offerkey and an offerroletypekey and an lekey.
            // First check if the offerrole exists then reactivate it.
            // Once that's done we need to loop through the offerroles and ensure that there is always the latest one active
            // for each offerroletype.

            if (!IfOfferRoleExistsReactivate(offerKey, offerRoleTypeKey, legalEntityKey))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("insert into [2am]..offerrole with(rowlock) values ({0}, {1}, {2}, {3}, getdate())", legalEntityKey, offerKey, offerRoleTypeKey, (int)GeneralStatuses.Active);
                castleTransactionService.ExecuteNonQueryOnCastleTran(sb.ToString(), Databases.TwoAM, new ParameterCollection());
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("update [2am]..OfferRole with(rowlock) set StatusChangeDate = getDate() where OfferKey = {0} and OfferRoleTypeKey = {1} and LegalEntityKey = {2}", offerKey, offerRoleTypeKey, legalEntityKey);
                castleTransactionService.ExecuteNonQueryOnCastleTran(sb.ToString(), Databases.TwoAM, new ParameterCollection());
            }
            ActivateLatestOfferRoleForEachOfferType(offerKey, offerRoleTypeKey);
        }

        public void ActivateLatestOfferRoleForEachOfferType(int offerKey, int offerRoleTypeKey)
        {
            // first deactivate all offerroles then reactivate the latest one for each offerroletype.
            // do a select to find the offerrolekeys that need to be changed. once we have those we'll be able to do the same as below but in a much more efficient manner.
            string selectQuery = String.Format(@"select o.OfferRoleKey from [2am]..offerrole o (nolock)
                                        join [2am]..offerroletype ort (nolock) on ort.OfferRoleTypeKey = o.OfferRoleTypeKey
                                        join [2am]..offerroletypegroup ortg (nolock) on ortg.Offerroletypegroupkey = ort.offerroletypegroupkey
                                        where o.OfferKey={0} and ortg.OfferRoleTypeGroupKey != {1} and o.OfferRoleTypeKey = {2} order by o.StatusChangeDate desc", offerKey, (int)OfferRoleTypeGroups.Client, offerRoleTypeKey);
            DataTable dataTable = new DataTable();
            SAHL.Common.DataAccess.ParameterCollection Params = new SAHL.Common.DataAccess.ParameterCollection();
            var dataSet = castleTransactionService.ExecuteQueryOnCastleTran(selectQuery.ToString(), Databases.TwoAM, new SAHL.Common.DataAccess.ParameterCollection());
            //WorkerHelper.FillFromQuery(DT, selectQuery.ToString(), Params);
            dataTable = dataSet.Tables[0];
            if (dataTable.Rows.Count > 0)
            {
                int latestOfferRoleKey = Convert.ToInt32(dataTable.Rows[0]["OfferRoleKey"]);

                List<string> oRKeys = new List<string>();
                foreach (DataRow dr in dataTable.Rows)
                {
                    oRKeys.Add(dr["OfferRoleKey"].ToString());
                }
                string oRKeysString = string.Join(",", oRKeys.ToArray());

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("update o set GeneralStatusKey={0} from [2am]..offerrole o with(rowlock) ", (int)GeneralStatuses.Inactive);
                sb.AppendFormat("where o.OfferRoleKey in ({0})", oRKeysString);
                sb.AppendLine();
                sb.AppendFormat("update [2am]..OfferRole with(rowlock) set GeneralStatusKey = {0} where OfferRoleKey = {1}", (int)GeneralStatuses.Active, latestOfferRoleKey);
                castleTransactionService.ExecuteNonQueryOnCastleTran(sb.ToString(), Databases.TwoAM, new SAHL.Common.DataAccess.ParameterCollection());

                //StringBuilder query = new StringBuilder();
                //query.AppendFormat("update [2am]..OfferRole with(rowlock) set GeneralStatusKey = {0} where OfferRoleKey = {1}", (int)GeneralStatuses.Active, latestOfferRoleKey);
                //castleTransactionService.ExecuteNonQueryOnCastleTran(query.ToString(), Databases.TwoAM, new SAHL.Common.DataAccess.ParameterCollection());
            }
        }

        public bool IfOfferRoleExistsReactivate(int offerKey, int offerRoleTypeKey, int LEKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select OfferRoleKey from [2am]..offerrole where OfferKey={0} and LegalEntityKey={1} and OfferRoleTypeKey={2}",
                offerKey, LEKey, offerRoleTypeKey);
            DataTable dt = new DataTable();
            var dataSet = castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.TwoAM, new ParameterCollection());
            dt = dataSet.Tables[0]; //the first table
            //WorkerHelper.FillFromQuery(dt, sb.ToString(), Tran.Context, new ParameterCollection());
            if (dt.Rows.Count > 0)
            {
                int offerRoleKey = Convert.ToInt32(dt.Rows[0]["OfferRoleKey"]);
                sb = new StringBuilder();
                sb.AppendFormat("Update [2am]..offerrole with(rowlock) set GeneralStatusKey={0}, StatusChangeDate=getdate() where OfferRoleKey={1}", (int)GeneralStatuses.Active, offerRoleKey);
                //WorkerHelper.ExecuteNonQuery(Tran.Context, sb.ToString(), new ParameterCollection());
                castleTransactionService.ExecuteNonQueryOnCastleTran(sb.ToString(), Databases.TwoAM, new ParameterCollection());
                return true;
            }
            else
            {
                return false;
            }
        }

        public offerRole.WorkflowAssignment.OfferRoleTypeRow GetOfferRoleRow(string dynamicRole)
        {
            //object Cache = DSCache.Get(KnownGoodKeys.ORGSTRUCTURE_DATASET_Client.ToString());
            //offerRole.WorkflowAssignment ds = null;
            //if (null == Cache)
            //    ds = LoadOrgStructureInfo(Tran);
            //else
            //    ds = (offerRole.WorkflowAssignment)Cache;

            object o = GetOfferRoleOrganisationStructure().OfferRoleType.Select(string.Format("Description='{0}'", dynamicRole));
            if (null == o)
                throw new Exception(String.Format("DynamicRole:{0} isnt in the orgstructure", dynamicRole));
            offerRole.WorkflowAssignment.OfferRoleTypeRow[] arr = (offerRole.WorkflowAssignment.OfferRoleTypeRow[])o;
            return arr[0];
        }

        public void DeactivateWorkflowRole(long instanceID)
        {
            string query = string.Format("exec pr_DeactivateWFRolesForInstanceID {0}", instanceID);
            castleTransactionService.ExecuteNonQueryOnCastleTran(query, Databases.X2, new ParameterCollection());
        }

        public offerRole.WorkflowAssignment GetWFAssignment(int genericKey, string dynamicRole, long instanceID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from vw_WFAssignment where IID={0} and ORT='{2}' order by id desc", instanceID, genericKey, dynamicRole);
            offerRole.WorkflowAssignment ds = new offerRole.WorkflowAssignment();
            var tableMappings = new StringCollection();
            tableMappings.Add("WFAssignment");
            ds = (offerRole.WorkflowAssignment)castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection(), tableMappings, ds);
            return ds;
        }

        public bool IsUserActive(GenericKeyTypes userOrganisationStructureGenericType, WorkflowRoleTypes workflowRoleType, int adUserKey)
        {
            offerRole.WorkflowAssignment ds = GetOfferRoleOrganisationStructure();
            offerRole.WorkflowAssignment.ADUserRow ad = ds.ADUser.FindByADUserKey(adUserKey);
            offerRole.WorkflowAssignment.UserOrganisationStructureRow[] userOrganisationStructureRows = (offerRole.WorkflowAssignment.UserOrganisationStructureRow[])ds.UserOrganisationStructure.Select(
                String.Format("GenericKey = {0} and GenericKeyTypeKey = {1} and ADUserKey = {2}",
                (int)workflowRoleType,
                (int)userOrganisationStructureGenericType,
                adUserKey));

            if (userOrganisationStructureRows != null &&
                userOrganisationStructureRows.Length > 0 &&
                userOrganisationStructureRows[0].GeneralStatusKey == (int)GeneralStatuses.Active &&
                ad.GeneralStatusKey == 1)
                return true;
            return false;
        }

        public bool IsUserStillInSameOrgStructureForCaseReassign(int adUserKey, GenericKeyTypes userOrganisationStructureGenericType, WorkflowRoleTypes workflowRoleType, long instanceID)
        {
            // look at the workflow role assignment record.
            // Check the BlaKey
            // Check user still maps to same bla key given this ORT

            StringBuilder sb = new StringBuilder();

            //workflowRole.WorkflowAssignment dsOS = (offerRole.WorkflowAssignment)DSCache.Get(KnownGoodKeys.ORGSTRUCTURE_DATASET_Client.ToString());
            //if (null == dsOS)
            //    dsOS = GetOrgStructureInfo(transaction);

            // Get the WorkflowRoleTypeKey for this Dynamic Role.
            Object o = GetWorkflowRoleOrganisationStructure().WorkflowRoleType.Select(string.Format("WorkflowRoleTypeKey={0}", (int)workflowRoleType));
            if (null == o) return false;
            workflowRole.WorkflowAssignment.WorkflowRoleTypeRow[] arr = (workflowRole.WorkflowAssignment.WorkflowRoleTypeRow[])o;
            int WRTKey = arr[0].WorkflowRoleTypeKey;

            // Get the existing record for this user so we can check if its valid to let him have this case.
            DataTable dt = new DataTable();
            sb.AppendFormat("select top 1 * from x2..vw_WFRAssignment where InstanceID={0} and WorkflowRoleTypeKey={1} and ADUserKey={2} order by id desc", instanceID, WRTKey, adUserKey);
            var dataSet = castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection());
            dt = dataSet.Tables[0];
            //WorkerHelper.FillFromQuery(dt, sb.ToString(), transaction.Context, new ParameterCollection());
            if (dt.Rows.Count == 0)
            {
                // given aduser and wrtkey need to check in the mapping table for an entry. if there is one
                // then return true as this user is still in the right wrtype.
                // if there are no rows then return false.
                StringBuilder blahSQL = new StringBuilder();
                blahSQL.Append("select top 1 * from [2am].dbo.WorkflowRoleTypeOrganisationStructureMapping wrtosm ");
                blahSQL.Append("join [2am].dbo.UserOrganisationStructure uos on uos.OrganisationStructureKey = wrtosm.OrganisationStructureKey ");
                blahSQL.AppendFormat("where wrtosm.WorkflowRoleTypeKey = {0} and uos.ADUserKey = {1} ", WRTKey, adUserKey);
                blahSQL.AppendFormat(" and uos.GenericKey = {0} and uos.GenericKeyTypeKey = {1} ", WRTKey, (int)userOrganisationStructureGenericType);

                dataSet = castleTransactionService.ExecuteQueryOnCastleTran(blahSQL.ToString(), Databases.TwoAM, new ParameterCollection());
                dt = dataSet.Tables[0];

                //WorkerHelper.FillFromQuery(dt, blahSQL.ToString(), transaction.Context, new ParameterCollection());
                if (dt.Rows.Count == 0)
                {
                    // user is no longer part of that org structure.
                    return false;
                }
            }

            // Get the Existing BlaKey for this case assignment. From there Get the ORgStructureKey.
            // This will tell us which OrgStruct the person was part of when the case was assigned.
            // If they are STILL in that org structure return true else they have moved.
            int OrgStructureKeyAtTimeOfAssignmet = Convert.ToInt32(dt.Rows[0]["OrganisationStructureKey"]);

            // Get the ADUser and the OS's that they belong to then see if it overlaps with the BlaKey
            workflowRole.WorkflowAssignment.ADUserRow adRow = GetWorkflowRoleOrganisationStructure().ADUser.FindByADUserKey(adUserKey);
            workflowRole.WorkflowAssignment.UserOrganisationStructureRow[] uos = adRow.GetUserOrganisationStructureRows();
            foreach (workflowRole.WorkflowAssignment.UserOrganisationStructureRow row in uos)
            {
                workflowRole.WorkflowAssignment.WorkflowRoleTypeOrganisationStructureMappingRow[] BlaRows = row.OrganisationStructureRow.GetWorkflowRoleTypeOrganisationStructureMappingRows();
                foreach (workflowRole.WorkflowAssignment.WorkflowRoleTypeOrganisationStructureMappingRow BlaRow in BlaRows)
                {
                    int OSKey = BlaRow.OrganisationStructureKey;
                    if (OrgStructureKeyAtTimeOfAssignmet == OSKey)
                    {
                        // The user is still in an orgstructure that is the same as the one when the case was assigned so we can assign this case to them
                        return true;
                    }
                }
            }
            return false;
        }

        public void CreateWorkflowRoleAssignment(long instanceID, int workflowRoleTypeOrganisationStructureMapping, int adUserKey, string message)
        {
            string sql = string.Format(@"
                            INSERT INTO [X2].[X2].[WorkflowRoleAssignment]  with (rowlock)
                        	        ([InstanceID]
                        	        ,[WorkflowRoleTypeOrganisationStructureMappingKey]
                        	        ,[ADUserKey]
                        	        ,[GeneralStatusKey]
                        	        ,[InsertDate]
                        	        ,[Message])
                            VALUES
                        		    ({0},{1},{2}, {3},getdate(),'{4}')", instanceID, workflowRoleTypeOrganisationStructureMapping, adUserKey, (int)GeneralStatuses.Active, message);

            castleTransactionService.ExecuteNonQueryOnCastleTran(sql, Databases.X2, new ParameterCollection());
        }

        public workflowRole.WorkflowAssignment.WorkflowRoleTypeRow GetWorkflowRoleTypeRow(WorkflowRoleTypes workflowRoleType)
        {
            //object Cache = DSCache.Get(KnownGoodKeys.ORGSTRUCTURE_DATASET_Client.ToString());
            //WorkflowAssignment ds = null;
            //if (null == Cache)
            //    ds = GetOrgStructureInfo(transaction);
            //else
            //    ds = (WorkflowAssignment)Cache;

            object o = GetWorkflowRoleOrganisationStructure().WorkflowRoleType.Select(string.Format("WorkflowRoleTypeKey={0}", (int)workflowRoleType));
            if (null == o)
                throw new Exception(String.Format("WorkflowRoleType:{0} isnt in the orgstructure", workflowRoleType.ToString()));
            workflowRole.WorkflowAssignment.WorkflowRoleTypeRow[] arr = (workflowRole.WorkflowAssignment.WorkflowRoleTypeRow[])o;
            return arr[0];
        }

        public void Create2AMWorkflowRole(int genericKey, int workflowRoleTypeKey, int legalEntityKey)
        {
            // The aim here is to get an offerkey and an offerroletypekey and an lekey.
            // First check if the offerrole exists then reactivate it.
            // Once that's done we need to loop through the offerroles and ensure that there is always the latest one active
            // for each offerroletype.

            if (!ReactivateIfWorkflowRoleExists(genericKey, workflowRoleTypeKey, legalEntityKey))
            {
                CreateWorkflowRole((WorkflowRoleTypes)workflowRoleTypeKey, genericKey, legalEntityKey);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("update [2am]..WorkflowRole with (rowlock)  set StatusChangeDate = getDate() where GenericKey = {0} and WorkflowRoleTypeKey = {1} and LegalEntityKey = {2}", genericKey, workflowRoleTypeKey, legalEntityKey);
                castleTransactionService.ExecuteNonQueryOnCastleTran(sb.ToString(), Databases.TwoAM, new ParameterCollection());
                //WorkerHelper.ExecuteNonQuery(transaction.Context, sb.ToString(), new ParameterCollection());
            }
            ActivateLatestWorkflowRoleForEachWorklflowRoleType(genericKey, workflowRoleTypeKey);
        }

        public void ActivateLatestWorkflowRoleForEachWorklflowRoleType(int genericKey, int workflowRoleTypeKey)
        {
            // first deactivate all workflowroles then reactivate the latest one for each workflowroletype.
            // do a select to find the workflowrolekeys that need to be changed. once we have those we'll be able to do the same as below but in a much more efficient manner.
            string selectQuery = String.Format(@"select wr.WorkflowRoleKey from [2am]..WorkflowRole wr (nolock)
                                        where wr.GenericKey={0} and wr.WorkflowRoleTypeKey = {1} order by wr.StatusChangeDate desc", genericKey, workflowRoleTypeKey);
            DataTable dataTable = new DataTable();
            ParameterCollection Params = new ParameterCollection();
            var dataSet = castleTransactionService.ExecuteQueryOnCastleTran(selectQuery.ToString(), Databases.TwoAM, Params);
            dataTable = dataSet.Tables[0];
            if (dataTable.Rows.Count > 0)
            {
                int latestWorkflowRoleKey = Convert.ToInt32(dataTable.Rows[0]["WorkflowRoleKey"]);

                List<string> wRKeys = new List<string>();
                foreach (DataRow dr in dataTable.Rows)
                {
                    wRKeys.Add(dr["WorkflowRoleKey"].ToString());
                }
                string wRKeysString = string.Join(",", wRKeys.ToArray());

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("update wr set GeneralStatusKey={0} from [2am]..WorkflowRole wr with (rowlock)  ", (int)GeneralStatuses.Inactive);
                sb.AppendFormat("where wr.WorkflowRoleKey in ({0})", wRKeysString);
                sb.AppendLine();
                sb.AppendFormat("update [2am]..WorkflowRole  with (rowlock) set GeneralStatusKey = {0} where WorkflowRoleKey = {1}", (int)GeneralStatuses.Active, latestWorkflowRoleKey);
                castleTransactionService.ExecuteNonQueryOnCastleTran(sb.ToString(), Databases.TwoAM, new ParameterCollection());

                //StringBuilder query = new StringBuilder();
                //query.AppendFormat("update [2am]..WorkflowRole  with (rowlock) set GeneralStatusKey = {0} where WorkflowRoleKey = {1}", (int)GeneralStatuses.Active, latestWorkflowRoleKey);
                //castleTransactionService.ExecuteNonQueryOnCastleTran(query.ToString(), Databases.TwoAM, new ParameterCollection());
            }
        }

        public void CreateWorkflowRole(WorkflowRoleTypes workflowRoleType, int genericKey, int legalEntityKey)
        {
            // check if record alreadu exists
            // if exists and inactive the recaftivate and update change date

            // if not exist then inset
            string sql = string.Format(@"
                        INSERT INTO [2AM].[dbo].[WorkflowRole] with (rowlock)
                           ([LegalEntityKey]
                           ,[GenericKey]
                           ,[WorkflowRoleTypeKey]
                           ,[GeneralStatusKey]
                           ,[StatusChangeDate])
                        VALUES ({0},{1},{2},{3},getdate())", legalEntityKey, genericKey, (int)workflowRoleType, (int)GeneralStatuses.Active);

            castleTransactionService.ExecuteNonQueryOnCastleTran(sql, Databases.TwoAM, new ParameterCollection());
        }

        public bool ReactivateIfWorkflowRoleExists(int genericKey, int workflowRoleTypeKey, int legalEntityKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select WorkflowRoleKey from [2am]..WorkflowRole where GenericKey={0} and LegalEntityKey={1} and WorkflowRoleTypeKey={2}",
                genericKey, legalEntityKey, workflowRoleTypeKey);
            DataTable dt = new DataTable();
            //WorkerHelper.FillFromQuery(dt, sb.ToString(), transaction.Context, new ParameterCollection());
            var dataSet = castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.TwoAM, new ParameterCollection());
            dt = dataSet.Tables[0];
            if (dt.Rows.Count > 0)
            {
                int workflowRoleKey = Convert.ToInt32(dt.Rows[0]["WorkflowRoleKey"]);
                sb = new StringBuilder();
                sb.AppendFormat("Update [2am]..WorkflowRole with (rowlock)  set GeneralStatusKey={0}, StatusChangeDate=getdate() where WorkflowRoleKey={1}", (int)GeneralStatuses.Active, workflowRoleKey);
                castleTransactionService.ExecuteNonQueryOnCastleTran(sb.ToString(), Databases.TwoAM, new ParameterCollection());
                //WorkerHelper.ExecuteNonQuery(transaction.Context, sb.ToString(), new ParameterCollection());
                return true;
            }
            else
            {
                return false;
            }
        }

        public string LoadBalanceAssign(GenericKeyTypes userOrganisationStructureGenericType, WorkflowRoleTypes workflowRoleType, int genericKey, long instanceID, List<string> statesToDetermineLoad, Process process, Workflow workflow, bool includeStates, bool checkRoundRobinStatus)
        {
            string adUserName = string.Empty;
            string workflowName = String.Empty, processName = String.Empty;
            int instanceCount = 0;

            workflowName = GetWorkflowName(workflow);
            processName = GetProcessName(process);

            // get workflow id
            int processID = -1;
            int workflowID = GetWorkFlowIDByName(workflowName, out processID);

            // get a comma delimited list of state ids to determine the load balance
            string stateIDs = "";
            foreach (string sName in statesToDetermineLoad)
            {
                int stateID = GetStateIDByName(workflowName, processName, sName);
                stateIDs += stateID + ",";
            }
            stateIDs = stateIDs.TrimEnd(',');

            // run query to return a sorted list of users belonging to the specified WorkflowRoleType with a  count of instances at the specified states
            string sql = GetUIStatement("Client.Assignment.WorkflowRoleAssignment", "LoadBalanceAssign");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@CheckRoundRobinStatus", checkRoundRobinStatus));
            prms.Add(new SqlParameter("@IncludeStates", includeStates));
            prms.Add(new SqlParameter("@WorkflowID", workflowID));
            prms.Add(new SqlParameter("@GenericKeyTypeKey", (int)userOrganisationStructureGenericType));
            prms.Add(new SqlParameter("@WorkflowRoleTypeKey", (int)workflowRoleType));
            prms.Add(new SqlParameter("@StateIDs", stateIDs));

            DataTable dt = new DataTable();
            //WorkerHelper.FillFromQuery(dt, sql, transaction.Context, prms);
            var dataSet = castleTransactionService.ExecuteQueryOnCastleTran(sql.ToString(), Databases.TwoAM, prms);
            dt = dataSet.Tables[0];

            // use the first row as this will be the guy with the least amount of cases
            if (dt != null && dt.Rows.Count > 0)
            {
                adUserName = Convert.ToString(dt.Rows[0]["ADUserName"]);
                instanceCount = Convert.ToInt32(dt.Rows[0]["InstanceCount"]);

                // go and assign the case
                DeactivateWorkflowRoleForDynamicRole((int)workflowRoleType, genericKey);
                AssignWorkflowRoleForADUser(instanceID, adUserName, workflowRoleType, genericKey);

                DeactivateAllWorkflowRoleAssigmentsForInstance(instanceID);
                AssignWorkflowRoleAssignmentForADUser(instanceID, adUserName, workflowRoleType, "");
            }
            return adUserName;
        }

        public void DeactivateWorkflowRoleForDynamicRole(int workflowRoleTypeKey, int genericKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("update [2am]..WorkflowRole with (rowlock)  set GeneralStatusKey={0}, StatusChangeDate=getdate() where GenericKey={1} and WorkflowRoleTypeKey={2} and GeneralStatusKey={3}", (int)GeneralStatuses.Inactive, genericKey, workflowRoleTypeKey, (int)GeneralStatuses.Active);
            castleTransactionService.ExecuteNonQueryOnCastleTran(sb.ToString(), Databases.TwoAM, new ParameterCollection());
        }

        public void DeactivateAllWorkflowRoleAssigmentsForInstance(long instanceID)
        {
            string query = string.Format("update x2.WorkflowRoleAssignment with (rowlock)  set GeneralStatusKey=2 where InstanceID={0}", instanceID);
            castleTransactionService.ExecuteNonQueryOnCastleTran(query, Databases.X2, new ParameterCollection());
        }

        public string GetUIStatement(string applicationName, string statementName)
        {
            string uistatementSQL = string.Format(@"select Statement from [2am].dbo.uiStatement where ApplicationName = '{0}' and StatementName = '{1}' order by Version desc", applicationName, statementName);
            object statement = castleTransactionService.ExecuteScalarOnCastleTran(uistatementSQL, Databases.TwoAM, new ParameterCollection());
            if (null == statement)
                throw new Exception(String.Format("uiStatement:{0}, {1} isnt in the uiStatement Table", applicationName, statementName));

            return statement.ToString();
        }

        public int GetStateIDByName(string workflow, string process, string state)
        {
            int stateID = -1;

            string query = string.Format(@"SELECT TOP 1 xs.ID
                        FROM x2.x2.state xs (nolock)
                        JOIN x2.x2.workflow xwf (nolock) ON xs.WorkFlowID = xwf.ID
                        JOIN x2.x2.process xp (nolock) ON xwf.ProcessID = xp.ID
                        WHERE xp.name = '{0}'
		                AND xwf.name = '{1}'
		                AND xs.name = '{2}'
                        ORDER BY XS.ID DESC", process, workflow, state);

            DataTable dt = new DataTable();
            //WorkerHelper.FillFromQuery(dt, query, transaction.Context, new ParameterCollection());
            var dataSet = castleTransactionService.ExecuteQueryOnCastleTran(query.ToString(), Databases.X2, new ParameterCollection());
            dt = dataSet.Tables[0];
            if (dt.Rows.Count > 0)
                stateID = Convert.ToInt32(dt.Rows[0]["ID"]);

            return stateID;
        }

        public int GetWorkFlowIDByName(string workflowName, out int processID)
        {
            int workflowId = -1;
            processID = -1;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select top 1 ID, ProcessID from [x2].[x2].WorkFlow (nolock) where [Name] = '{0}' order by 1 desc", workflowName);
            DataTable dt = new DataTable();
            //WorkerHelper.FillFromQuery(dt, sb.ToString(), transaction.Context, new ParameterCollection());
            var dataSet = castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection());
            dt = dataSet.Tables[0];
            if (dt.Rows.Count > 0)
            {
                workflowId = Convert.ToInt32(dt.Rows[0]["ID"]);
                processID = Convert.ToInt32(dt.Rows[0]["ProcessID"]);
            }

            return workflowId;
        }

        public DataRow GetWorkflowRoleAssignment(List<long> instanceIDs, WorkflowRoleTypes workflowRoleType)
        {
            // run query to return a sorted list of users belonging to the specified WorkflowRoleType with a  count of instances at the specified states
            string sql = GetUIStatement("Client.Assignment.WorkflowRoleAssignment", "GetWFRAssignmentByInstIDAndWRTKey");
            ParameterCollection prms = new ParameterCollection();

            string instanceIDStrings = String.Empty;
            foreach (int instanceID in instanceIDs)
            {
                instanceIDStrings += instanceID + ",";
            }
            instanceIDStrings = instanceIDStrings.TrimEnd(',');

            sql = String.Format(sql, instanceIDStrings);

            prms.Add(new SqlParameter("@WorkflowRoleTypeKey", (int)workflowRoleType));

            DataTable workflowRoleAssignmentTable = new DataTable();
            var dataSet = castleTransactionService.ExecuteQueryOnCastleTran(sql, Databases.TwoAM, prms);
            workflowRoleAssignmentTable = dataSet.Tables[0];
            //WorkerHelper.FillFromQuery(workflowRoleAssignmentTable, sql, transaction.Context, prms);

            return workflowRoleAssignmentTable.Rows.Count > 0 ? workflowRoleAssignmentTable.Rows[0] : null;
        }

        public void AssignWorkflowRoleForADUser(long instanceID, string adUserName, WorkflowRoleTypes workflowRoleType, int genericKey)
        {
            DeactivateWorkflowRoleForDynamicRole((int)workflowRoleType, genericKey);
            workflowRole.WorkflowAssignment.ADUserRow adRow = GetADUser(adUserName);
            if (!ReactivateIfWorkflowRoleExists(genericKey, (int)workflowRoleType, adRow.LegalEntityKey))
            {
                CreateWorkflowRole(workflowRoleType, genericKey, adRow.LegalEntityKey);
            }
        }

        public void AssignWorkflowRoleAssignmentForADUser(long instanceID, string adUserName, WorkflowRoleTypes workflowRoleType, string state)
        {
            DeactivateAllWorkflowRoleAssigmentsForInstance(instanceID);
            workflowRole.WorkflowAssignment.ADUserRow adRow = GetADUser(adUserName);
            int wrtosm = GetWorkflowRoleTypeOrgStructMapKey(adUserName, workflowRoleType);
            if (wrtosm != -1)
            {
                CreateWorkflowRoleAssignment(instanceID, wrtosm, adRow.ADUserKey, state);
            }
        }

        public int GetWorkflowRoleTypeOrgStructMapKey(string adUserName, WorkflowRoleTypes workflowRoleType)
        {
            int wrtosm = -1;
            string sql = string.Format(@"
                SELECT TOP 1 wrtosm.WorkflowRoleTypeOrganisationStructureMappingKey
                FROM [2AM].[dbo].[WorkflowRoleTypeOrganisationStructureMapping] wrtosm (nolock)
                JOIN [2AM].[dbo].UserOrganisationStructure uos (nolock)
	                ON uos.OrganisationStructureKey = wrtosm.OrganisationStructureKey
                JOIN [2AM].[dbo].ADUser ad (nolock)
	                ON ad.adUserKey = uos.adUserKey
                WHERE
	                wrtosm.WorkflowRoleTypeKey = {0}
		                AND
	                ad.ADUserName = '{1}'", (int)workflowRoleType, adUserName);

            object o = castleTransactionService.ExecuteScalarOnCastleTran(sql, Databases.TwoAM, new ParameterCollection());
            //object o = WorkerHelper.ExecuteScalar(transaction.Context, sql, new ParameterCollection());
            if (o != null)
                wrtosm = Convert.ToInt32(o);

            return wrtosm;
        }

        public int GetLastAssignedUserForGroupByRole(long instanceID, int debtCounsellingKey, WorkflowRoleTypes workflowRoleType, out DataRow workflowRoleAssignmentRow)
        {
            //Get Related cases for the Current Debt Counselling Case
            List<Int64> instanceIDs = GetRelatedInstances(debtCounsellingKey);

            //Get the Workflow Role Assignment for the Instances that we got
            workflowRoleAssignmentRow = GetWorkflowRoleAssignment(instanceIDs, workflowRoleType);

            if (workflowRoleAssignmentRow != null)
            {
                //Get the AD User
                int adUserKey = Convert.ToInt32(workflowRoleAssignmentRow["ADUserKey"]);

                return adUserKey;
            }
            return 0;
        }

        public List<long> GetRelatedInstances(int debtCounsellingKey)
        {
            // run query to return a sorted list of users belonging to the specified WorkflowRoleType with a  count of instances at the specified states
            string sql = GetUIStatement("Client.Assignment.WorkflowRoleAssignment", "GetRelatedInstancesByDCKey");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@DebtCounsellingKey", debtCounsellingKey));

            DataTable instanceDataTable = new DataTable();
            var dataSet = castleTransactionService.ExecuteQueryOnCastleTran(sql, Databases.X2, prms);
            instanceDataTable = dataSet.Tables[0];
            //WorkerHelper.FillFromQuery(instanceDataTable, sql, transaction.Context, prms);

            List<Int64> instanceIDs = new List<Int64>();

            foreach (DataRow dataRow in instanceDataTable.Rows)
            {
                instanceIDs.Add(Convert.ToInt32(dataRow["InstanceID"]));
            }

            return instanceIDs;
        }

        public workflowRole.WorkflowAssignment.ADUserRow GetADUser(int adUserKey)
        {
            workflowRole.WorkflowAssignment.ADUserRow ad = GetWorkflowRoleOrganisationStructure().ADUser.FindByADUserKey(adUserKey);
            return ad;
        }

        public workflowRole.WorkflowAssignment.ADUserRow GetADUser(string adUserName)
        {
            object o = GetWorkflowRoleOrganisationStructure().ADUser.Select(string.Format("ADUserName='{0}'", adUserName));
            if (null == o) throw new Exception(string.Format("Cant find username:{0}", adUserName));
            workflowRole.WorkflowAssignment.ADUserRow[] arr = (workflowRole.WorkflowAssignment.ADUserRow[])o;
            return arr[0];
        }

        public string GetWorkflowName(Workflow workflow)
        {
            string workflowName = String.Empty;

            switch (workflow)
            {
                case Workflow.ApplicationCapture:
                    workflowName = "Application Capture";
                    break;
                case Workflow.ApplicationManagement:
                    workflowName = "Application Management";
                    break;
                case Workflow.Credit:
                    workflowName = "Credit";
                    break;
                case Workflow.DebtCounselling:
                    workflowName = "Debt Counselling";
                    break;
                case Workflow.DeleteDebitOrder:
                    workflowName = "Delete Debit Order";
                    break;
                case Workflow.HelpDesk:
                    workflowName = "Help Desk";
                    break;
                case Workflow.LifeOrigination:
                    workflowName = "LifeOrigination";
                    break;
                case Workflow.LoanAdjustments:
                    workflowName = "Loan Adjustments";
                    break;
                case Workflow.RCS:
                    workflowName = "RCS";
                    break;
                case Workflow.ReadvancePayments:
                    workflowName = "Readvance Payments";
                    break;
                case Workflow.Valuations:
                    workflowName = "Valuations";
                    break;
                default:
                    break;
            }

            return workflowName;
        }

        public string GetProcessName(Process process)
        {
            string processName = String.Empty;

            switch (process)
            {
                case Process.DebtCounselling:
                    processName = "Debt Counselling";
                    break;
                case Process.DeleteDebitOrder:
                    processName = "Delete Debit Order";
                    break;
                case Process.HelpDesk:
                    processName = "Help Desk";
                    break;
                case Process.Life:
                    processName = "Life";
                    break;
                case Process.Origination:
                    processName = "Origination";
                    break;
                case Process.RCS:
                    processName = "RCS";
                    break;
                default:
                    break;
            }

            return processName;
        }

        public workflowRole.WorkflowAssignment GetWorkflowRoleAssignment(WorkflowRoleTypes workflowRoleType, long instanceID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from vw_WFRAssignment where InstanceID={0} and WorkflowRoleTypeKey='{1}' order by id desc", instanceID, (int)workflowRoleType);
            workflowRole.WorkflowAssignment ds = new workflowRole.WorkflowAssignment();
            //WorkerHelper.FillFromQuery(ds.WFRAssignment, sb.ToString(), transaction.Context, new ParameterCollection());
            var tableMappings = new StringCollection();
            tableMappings.Add("WFRAssignment");
            ds = (workflowRole.WorkflowAssignment)castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection(), tableMappings, ds);
            return ds;
        }

        public workflowRole.WorkflowAssignment GetWorkflowRoleAssignment(List<WorkflowRoleTypes> workflowRoleTypes, long instanceID)
        {
            StringBuilder sb = new StringBuilder();
            string workflowRoleTypesAggregate = string.Join(", ", workflowRoleTypes.Select(x => (int)x).ToArray());
            sb.AppendFormat("select * from vw_WFRAssignment where InstanceID={0} and WorkflowRoleTypeKey in ({1}) order by id desc", instanceID, workflowRoleTypesAggregate);
            workflowRole.WorkflowAssignment ds = new workflowRole.WorkflowAssignment();
            //WorkerHelper.FillFromQuery(ds.WFRAssignment, sb.ToString(), transaction.Context, new ParameterCollection());
            var tableMappings = new StringCollection();
            tableMappings.Add("WFRAssignment");
            ds = (workflowRole.WorkflowAssignment)castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection(), tableMappings, ds);
            return ds;
        }

        public offerRole.WorkflowAssignment GetWFAssignmentAcrossInstances(string dynamicRole, long instanceID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select top 1 wf.* from x2.dbo.vw_WFAssignment wf ");
            sb.AppendFormat("where wf.iid = {0} ", instanceID);
            sb.AppendFormat("and wf.ORT='{0}' ", dynamicRole);
            sb.Append("order by wf.id Desc  ");
            offerRole.WorkflowAssignment ds = new offerRole.WorkflowAssignment();
            var tableMappings = new StringCollection();
            tableMappings.Add("WFAssignment");
            ds = (offerRole.WorkflowAssignment)castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection(), tableMappings, ds);
            return ds;
        }

        public DataTable GetADUsersForRoundRobinByRoundRobinPointerDescription(int roundRobinPointerKey)
        {
            //object Cache = DSCache.Get(KnownGoodKeys.ORGSTRUCTURE_DATASET_Client.ToString());
            //workflowRole.WorkflowAssignment ds = null;
            //if (null == Cache)
            //    ds = LoadOrgStructureInfo(Tran);
            //else
            //    ds = (workflowRole.WorkflowAssignment)Cache;

            object o = GetWorkflowRoleOrganisationStructure().RoundRobinPointerDefinition.Select(string.Format("RoundRobinPointerKey = {0}", roundRobinPointerKey));
            if (null == o)
                throw new Exception(String.Format("RoundRobinPointerKey:{0} isnt in the RoundRobinPointerDefinition", roundRobinPointerKey));
            workflowRole.WorkflowAssignment.RoundRobinPointerDefinitionRow[] roundRobinPointerDefinitionRow = (workflowRole.WorkflowAssignment.RoundRobinPointerDefinitionRow[])o;

            DataTable fullTable = new DataTable();
            foreach (workflowRole.WorkflowAssignment.RoundRobinPointerDefinitionRow row in roundRobinPointerDefinitionRow)
            {
                string applicationName = Convert.ToString(row.ApplicationName);
                string statementName = Convert.ToString(row.StatementName);
                int roundRobinPointerDefinitionKey = Convert.ToInt32(row.RoundRobinPointerDefinitionKey);

                string uistatementSQL = string.Format(@"select Statement from [2am].dbo.uiStatement where ApplicationName = '{0}' and StatementName = '{1}' order by Version desc", applicationName, statementName);
                object uiStatement = castleTransactionService.ExecuteScalarOnCastleTran(uistatementSQL, Databases.TwoAM, new ParameterCollection());
                //object uiStatement = WorkerHelper.ExecuteScalar(Tran.Context, uistatementSQL, new ParameterCollection());
                if (null == uiStatement)
                    throw new Exception(String.Format("uiStatement:{0}, {1} isnt in the uiStatement Table", applicationName, statementName));

                ParameterCollection Params = new ParameterCollection();
                Params.Add(new SqlParameter("@RoundRobinPointerDefinitionKey", roundRobinPointerDefinitionKey));
                DataTable ADUsers = new DataTable("ADUsers");  // ADUsers is the list of ADUserKeys in the group
                DataColumn BlaKeyColumn = new DataColumn();
                BlaKeyColumn.DataType = System.Type.GetType("System.Int32");
                BlaKeyColumn.AllowDBNull = false;
                BlaKeyColumn.ColumnName = "BlaKey";
                BlaKeyColumn.DefaultValue = Convert.ToInt32(row.GenericKey);
                ADUsers.Columns.Add(BlaKeyColumn);
                castleTransactionService.FillDataTableFromQueryOnCastleTran(ADUsers, uiStatement.ToString(), Databases.TwoAM, Params); //WorkerHelper.FillFromQuery(ADUsers, uiStatement.ToString(), Tran.Context, Params);
                fullTable.Merge(ADUsers);
            }

            return fullTable;
        }

        public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();

            //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
            //And add duplicate item value in arraylist.
            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }

            //Removing a list of duplicate items from datatable.
            foreach (DataRow dRow in duplicateList)
                dTable.Rows.Remove(dRow);

            //Datatable which contains unique records will be return as output.
            return dTable;
        }

        public int TrueRoundRobin(DataTable fullTable, int roundRobinPointerKey)
        {
            int ADUserKey = -1, rowCounter = fullTable.Rows.Count;

            while (ADUserKey == -1 || rowCounter >= 0)
            {
                rowCounter--;
                int roundRobinPointerNextID = GetNextRoundRobinPointerIndexID(roundRobinPointerKey, fullTable.Rows.Count);

                // now we need to get the aduser at this index (RoundRobinPointerNextID) and see if available for roundrobin
                int adUserKey = Convert.ToInt32(fullTable.Rows[roundRobinPointerNextID - 1]["ADUserKey"]);

                //Get the BlaKey
                int offerRoleTypeOrganisationStructureMappingKey = Convert.ToInt32(fullTable.Rows[roundRobinPointerNextID - 1]["BlaKey"]);

                //Get the OrganisationStructureKey from the OfferRoleTypeOrganisationStructureMapping table
                workflowRole.WorkflowAssignment ds = GetWorkflowRoleOrganisationStructure();
                var offerRoleTypeOrganisationStructureMappingRows = (workflowRole.WorkflowAssignment.OfferRoleTypeOrganisationStructureMappingRow[])ds.OfferRoleTypeOrganisationStructureMapping.Select(String.Format("OfferRoleTypeOrganisationStructureMappingKey = {0}", offerRoleTypeOrganisationStructureMappingKey));

                //If BlaRows is null or there are no rows, then just continue looping and finding the next person
                if (offerRoleTypeOrganisationStructureMappingRows == null || offerRoleTypeOrganisationStructureMappingRows.Length == 0)
                {
                    continue;
                }

                if (CheckADUserAvaiableForRoundRobin(roundRobinPointerKey, offerRoleTypeOrganisationStructureMappingRows[0].OrganisationStructureKey, adUserKey) && GetADUserRowByKey(adUserKey).GeneralStatusKey == (int)GeneralStatuses.Active)
                {
                    ADUserKey = adUserKey;                   
                }

                if (ADUserKey > 0)
                {
                    break;
                }

                if (rowCounter <= 0)
                {
                    break;
                }
            }
            return ADUserKey;
        }

        public string AssignToWorkflowAssignmentAndOfferRoleTables(int adUserKey, int blaKey, string dynamicRole, int genericKey, Int64 instanceID, string state, Process process)
        {
            // now we need to assign this ADUserKey to WFAssignment and OfferRole (if Origination)
            // we need to get the BlaKey
            offerRole.WorkflowAssignment.OfferRoleTypeRow oRow = GetOfferRoleRow(dynamicRole);

            AssignWorkflowRole(instanceID, adUserKey, blaKey, state);
            if (process == Process.Origination)
            {
                int OfferRoleTypeKey = -1;
                int LEKey = -1;
                // Now as legacy crap we need to go and write offerrole records.
                LEKey = GetOfferRoleOrganisationStructure().ADUser.FindByADUserKey(adUserKey).LegalEntityKey;
                OfferRoleTypeKey = GetOfferRoleOrganisationStructure().OfferRoleTypeOrganisationStructureMapping.FindByOfferRoleTypeOrganisationStructureMappingKey(blaKey).OfferRoleTypeKey;

                Assign2AMOfferRole(genericKey, OfferRoleTypeKey, LEKey);
            }
            string ADUserName = GetADUserRowByKey(adUserKey).ADUserName;
            Console.WriteLine(string.Format("IID:{0} To:{1} Key:{2}", instanceID, ADUserName, adUserKey));
            return ADUserName;
        }

        public bool CheckADUserAvaiableForRoundRobin(int roundRobinPointerKey, int organisationStructureKey, int adUserKey)
        {
            //Get the Cache
            var cachedDataSet = GetWorkflowRoleOrganisationStructure();

            //Get the user organisation structure rows for the organisation structure key and the ad user key
            var userOrganisationStructureRows = (workflowRole.WorkflowAssignment.UserOrganisationStructureRow[])cachedDataSet.UserOrganisationStructure.Select(string.Format("OrganisationStructureKey = {0} and ADUserKey = {1}", organisationStructureKey, adUserKey));
            if (userOrganisationStructureRows != null && userOrganisationStructureRows.Length > 0)
            {
                //Get the Organisation Structure Status for the AD User
                var userOrganisationStructureRoundRobinStatusRows = (workflowRole.WorkflowAssignment.UserOrganisationStructureRoundRobinStatusRow[])cachedDataSet.UserOrganisationStructureRoundRobinStatus.Select(string.Format("UserOrganisationStructureKey = {0}", userOrganisationStructureRows[0].UserOrganisationStructureKey));
                if (userOrganisationStructureRoundRobinStatusRows != null && userOrganisationStructureRoundRobinStatusRows.Length > 0)
                {
                    //Is he/she active?
                    if (roundRobinPointerKey==(int)SAHL.Common.Globals.RoundRobinPointers.CapitecConsultant)
                        return userOrganisationStructureRoundRobinStatusRows[0].CapitecGeneralStatusKey == 1;
                    else
                        return userOrganisationStructureRoundRobinStatusRows[0].GeneralStatusKey == 1;
                }
            }
            return false;
        }

        public workflowRole.WorkflowAssignment.ADUserRow GetADUserRowByKey(int adUserKey)
        {
            object o = GetWorkflowRoleOrganisationStructure().ADUser.Select(string.Format("AduserKey='{0}'", adUserKey));
            if (null == o) throw new Exception(string.Format("Cant find ADUserKey:{0}", adUserKey));
            workflowRole.WorkflowAssignment.ADUserRow[] arr = (workflowRole.WorkflowAssignment.ADUserRow[])o;
            return arr[0];
        }

        public DataTable GetUsersForOrgStructureKeyAndDynamicRole(long instanceID, int organisationStructureKey, string dynamicRole)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select ADUserKey, ADUserName, BlaKey from vw_UsersOfferRoleOrgStruct where ORT='{0}' and OSKey in ({1}) order by OSKey, ADUserKey", dynamicRole, organisationStructureKey);
            DataTable dt = new DataTable();
            var dataSet = castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection());
            dt = dataSet.Tables[0];
            return dt;
        }

        public long GetSourceInstanceFromInstanceID(long instanceID)
        {
            string query = string.Format("select sourceinstanceid from x2.x2.instance where id = {0} ", instanceID);
            object sourceInstanceID = castleTransactionService.ExecuteScalarOnCastleTran(query, Databases.X2, new ParameterCollection());
            if (sourceInstanceID != null)
                return (long)sourceInstanceID;
            else
                return 0;
        }

        public offerRole.WorkflowAssignment.WFAssignmentDataTable GetLastSecurityRecordForApplicationKeyOrgStuctAndOfferRoleType(int ApplicationKey, int OrgStuctKey, int ORTKey, string Map)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select StorageTable from x2.workflow where name='{0}'", Map);
            DataSet ds1 = castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection());

            string MapName = ds1.Tables[0].Rows[0]["StorageTable"].ToString();

            sb = new StringBuilder();
            sb.Append("select wfa.* from  ");
            sb.AppendFormat("x2data.{0} d  ", MapName);
            sb.Append("(nolock) ");
            sb.Append("join x2.instance i (nolock) on d.instanceid=i.id ");
            sb.Append("join vw_WFAssignment wfa (nolock) on i.id=wfa.iid ");
            sb.AppendFormat("where d.ApplicationKey={0} and wfa.OfferRoleTypeKey={1} and OrganisationStructureKey={2} ", ApplicationKey, ORTKey, OrgStuctKey);
            sb.Append("order by id desc ");

            var tableMappings = new StringCollection();
            tableMappings.Add("WFAssignment");

            offerRole.WorkflowAssignment ds = new offerRole.WorkflowAssignment();
            ds = (offerRole.WorkflowAssignment)castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection(), tableMappings, ds);

            return ds.WFAssignment;
        }

        public DataTable GetWorkflowInstanceInPreviousState(long sourceInstanceID, long instanceID, string previousStateName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"select top 1
								instance.*
							  from
								[x2].[x2].instance instance join
								[x2].[x2].state state on
									instance.StateID = state.ID
							  where
								SourceInstanceID = {0} and
								instance.ID <> {1} and
								state.Name = '{2}'
							  order by
								instance.ID desc", sourceInstanceID, instanceID, previousStateName);

            DataSet ds = new DataSet();
            castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, null, new StringCollection { "DATA" }, ds);
            return ds.Tables["DATA"];
        }

        public DataTable GetWorkflowHistoryForActivityByInstance(long instanceID, string activityName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"select top 1
								*
							  from
								[x2].[x2].WorkflowHistory wfh join
								[x2].[x2].Activity activity on
									wfh.activityID = activity.id
							  where
								InstanceID = {0} and
								activity.Name = '{1}'
							  order by
								wfh.id desc", instanceID, activityName);

            DataSet ds = new DataSet();
            castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, null, new StringCollection { "DATA" }, ds);
            return ds.Tables["DATA"];
        }

        public DataTable GetLatestWorkflowAssignmentForADUserKeyAndInstance(long instanceID, int adUserKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"select top 1
								*
							  from
								[x2].[x2].WorkflowAssignment wfa
							  where
								wfa.InstanceID = {0} and
								wfa.ADUserKey = '{1}'
							  order by
								ID desc", instanceID, adUserKey);

            DataSet ds = new DataSet();
            castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, null, new StringCollection { "DATA" }, ds);
            return ds.Tables["DATA"];
        }

        public int TrueRoundRobinForWorkflowRoleAssignment(DataTable userTable, int roundRobinPointerKey)
        {
            int roundRobinPointerNextID = GetNextRoundRobinPointerIndexID(roundRobinPointerKey, userTable.Rows.Count);
            int ADUserKey = Convert.ToInt32(userTable.Rows[roundRobinPointerNextID - 1]["ADUserKey"]);
            return ADUserKey;
        }

        /// <summary>
        /// Specific to WorkFlow Role Assignment
        /// </summary>
        /// <param name="adUserKey"></param>
        /// <param name="userOrganisationStructureGenericType"></param>
        /// <param name="workflowRoleType"></param>
        /// <returns></returns>
        public bool IsUserStillInSameOrgStructureForCaseReassign(int adUserKey, GenericKeyTypes userOrganisationStructureGenericType, WorkflowRoleTypes workflowRoleType)
        {
            DataSet ds = null;

            // given aduser and wrtkey need to check in the mapping table for an entry. if there is one
            // then return true as this user is still in the right wrtype.
            // if there are no rows then return false.
            StringBuilder sb = new StringBuilder();
            sb.Append("select top 1 * from [2am].dbo.WorkflowRoleTypeOrganisationStructureMapping wrtosm ");
            sb.Append("join [2am].dbo.UserOrganisationStructure uos on uos.OrganisationStructureKey = wrtosm.OrganisationStructureKey ");
            sb.AppendFormat("where wrtosm.WorkflowRoleTypeKey = {0} and uos.ADUserKey = {1} ", (int)workflowRoleType, adUserKey);
            sb.AppendFormat(" and uos.GenericKey = {0} and uos.GenericKeyTypeKey = {1} ", (int)workflowRoleType, (int)userOrganisationStructureGenericType);

            //WorkerHelper.FillFromQuery(dt, blahSQL.ToString(), transaction.Context, new ParameterCollection());
            ds = new DataSet();
            ds = castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, null, new StringCollection { "DATA" }, ds);

            if (ds.Tables["DATA"].Rows.Count == 0)
            {
                // user is no longer part of that org structure.
                return false;
            }

            return true;
        }

        /// <summary>
        /// /Specific to WorkFlow Role Assignment
        /// </summary>
        /// <param name="roundRobinPointerKey"></param>
        /// <returns></returns>
        public DataTable GetADUsersForRoundRobinByRoundRobinPointerKey(int roundRobinPointerKey)
        {
            //Get the Cache
            var dsOS = GetWorkflowRoleOrganisationStructure();

            object o = dsOS.RoundRobinPointerDefinition.Select(string.Format("RoundRobinPointerKey = {0}", roundRobinPointerKey));
            if (null == o)
                throw new Exception(String.Format("RoundRobinPointerKey:{0} isnt in the RoundRobinPointerDefinition", roundRobinPointerKey));

            workflowRole.WorkflowAssignment.RoundRobinPointerDefinitionRow[] roundRobinPointerDefinitionRow = (workflowRole.WorkflowAssignment.RoundRobinPointerDefinitionRow[])o;

            DataTable fullTable = new DataTable();
            foreach (workflowRole.WorkflowAssignment.RoundRobinPointerDefinitionRow row in roundRobinPointerDefinitionRow)
            {
                string applicationName = Convert.ToString(row.ApplicationName);
                string statementName = Convert.ToString(row.StatementName);
                int roundRobinPointerDefinitionKey = Convert.ToInt32(row.RoundRobinPointerDefinitionKey);

                string uistatementSQL = uiStatementService.GetStatement(applicationName, statementName);

                if (string.IsNullOrEmpty(uistatementSQL))
                    throw new Exception(String.Format("uiStatement:{0}, {1} isnt in the uiStatement Table", applicationName, statementName));

                ParameterCollection Params = new ParameterCollection();
                Params.Add(new SqlParameter("@RoundRobinPointerDefinitionKey", roundRobinPointerDefinitionKey));
                DataSet ds = new DataSet();
                castleTransactionService.ExecuteQueryOnCastleTran(uistatementSQL, Databases.X2, Params, new StringCollection { "DATA" }, ds);
                fullTable.Merge(ds.Tables["DATA"]);
            }
            return fullTable;
        }

        /// <summary>
        /// /Specific to WorkFlow Role Assignment
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="roundRobinPointer"></param>
        /// <param name="genericKey"></param>
        /// <param name="workflowRoleType"></param>
        /// <returns></returns>
        public string RoundRobinAssignForPointer(long instanceID, RoundRobinPointers roundRobinPointer, int genericKey, WorkflowRoleTypes workflowRoleType)
        {
            string adUserName = string.Empty;
            int adUserKey = -1;

            // Load Cached Data
            workflowRole.WorkflowAssignment ds = GetWorkflowRoleOrganisationStructure();

            // Get list of users for roundrobin pointer - returns unique sorted list
            DataTable userTable = GetADUsersForRoundRobinByRoundRobinPointerKey((int)roundRobinPointer);

            // Perform the round robinning
            adUserKey = TrueRoundRobinForWorkflowRoleAssignment(userTable, (int)roundRobinPointer);

            // need to pass in the ADUserKey, BlaKey, DynamicRole, GenericKey -- returns ADUserName assigned to.
            // if ADUserKey = -1 then no user was found so try returning error to the screen
            if (adUserKey > 0)
            {
                int WorkflowRoleTypeOrganisationStructureMappingKey = -1;
                System.Data.DataRow[] wrtosmRow = userTable.Select(string.Format("ADUserKey = {0}", adUserKey));
                WorkflowRoleTypeOrganisationStructureMappingKey = Convert.ToInt32(wrtosmRow[0]["WorkflowRoleTypeOrganisationStructureMappingKey"]);

                // get the aduser row
                workflowRole.WorkflowAssignment.ADUserRow adUserRow = GetADUser(adUserKey);
                adUserName = adUserRow.ADUserName;

                // this inserts a record into the x2.workflowroleassignment table
                DeactivateAllWorkflowRoleAssigmentsForInstance(instanceID);
                CreateWorkflowRoleAssignment(instanceID, WorkflowRoleTypeOrganisationStructureMappingKey, adUserKey, String.Empty);

                // insert / update the [2am]..WorkflowRole
                DeactivateWorkflowRoleForDynamicRole((int)workflowRoleType, genericKey);
                Create2AMWorkflowRole(genericKey, (int)workflowRoleType, adUserRow.LegalEntityKey);
            }

            return adUserName;
        }

        private int GetNextRoundRobinPointerIndexID(int roundRobinPointerKey, int maxRoundRobinPointerIndexID)
        {
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@RoundRobinPointerKey", roundRobinPointerKey));
            parameters.Add(new SqlParameter("@MaxRoundRobinPointerIndexID", maxRoundRobinPointerIndexID));
            SqlParameter nextRoundRobinPointerIndexID = new SqlParameter("@NextRoundRobinPointerIndexID", SqlDbType.Int);
            nextRoundRobinPointerIndexID.Direction = ParameterDirection.Output;
            parameters.Add(nextRoundRobinPointerIndexID);

            castleTransactionService.ExecuteNonQueryOnCastleTran("exec [dbo].[GetNextRoundRobinPointerIndexID] @RoundRobinPointerKey, @MaxRoundRobinPointerIndexID, @NextRoundRobinPointerIndexID out", Databases.TwoAM, parameters);

            return (int)nextRoundRobinPointerIndexID.Value;
        }
    }
}