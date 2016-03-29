using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Logging;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.X2.BusinessModel.DAO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using offerRole = SAHL.Common.BusinessModel.Interfaces.Repositories.OfferRole;
using workflowRole = SAHL.Common.BusinessModel.Interfaces.Repositories.WorkflowRole;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    ///
    /// </summary>
    [FactoryType(typeof(IWorkflowAssignmentRepository))]
    public class WorkflowAssignmentRepository : AbstractRepositoryBase, IWorkflowAssignmentRepository
    {
        private IWorkflowSecurityRepository workflowSecurityRepository;
        private ICastleTransactionsService castleTransactionService;
        private IApplicationRepository applicationRepo;

        public WorkflowAssignmentRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
            if (workflowSecurityRepository == null)
            {
                workflowSecurityRepository = RepositoryFactory.GetRepository<IWorkflowSecurityRepository>();
            }
        }

        public WorkflowAssignmentRepository(ICastleTransactionsService castleTransactionService, IWorkflowSecurityRepository workflowSecurityRepository, IApplicationRepository applicationRepo)
        {
            this.workflowSecurityRepository = workflowSecurityRepository;
            this.castleTransactionService = castleTransactionService;
            this.applicationRepo = applicationRepo;
        }

        public bool IsUserActive(string adUserName)
        {
            offerRole.WorkflowAssignment ds = workflowSecurityRepository.GetOfferRoleOrganisationStructure();
            offerRole.WorkflowAssignment.ADUserRow[] arr = (offerRole.WorkflowAssignment.ADUserRow[])ds.ADUser.Select(string.Format("ADUserName='{0}'", adUserName));
            if (arr[0].GeneralStatusKey == 1)
                return true;
            return false;
        }

        public bool IsUserActive(int adUserKey)
        {
            offerRole.WorkflowAssignment ds = workflowSecurityRepository.GetOfferRoleOrganisationStructure();
            offerRole.WorkflowAssignment.ADUserRow ad = ds.ADUser.FindByADUserKey(adUserKey);
            if (ad.GeneralStatusKey == 1)
                return true;
            return false;
        }

        public bool IsUserStillInSameOrgStructureForCaseReassign(int adUserKey, string dynamicRole, long instanceID)
        {
            // look at the assignment record.
            // Check the BlaKey
            // Check user still maps to same bla key given this ORT

            StringBuilder sb = new StringBuilder();

            // Get the OfferRoleKey for this Dynamic Role.
            Object o = workflowSecurityRepository.GetOfferRoleOrganisationStructure().OfferRoleType.Select(string.Format("Description='{0}'", dynamicRole));
            if (null == o) return false;
            offerRole.WorkflowAssignment.OfferRoleTypeRow[] arr = (offerRole.WorkflowAssignment.OfferRoleTypeRow[])o;
            int ORTKey = arr[0].OfferRoleTypeKey;

            // Get the existing record for this user so we can check if its valid to let him have this case.
            DataSet ds = new DataSet();

            sb.AppendFormat("select top 1 * from vw_WFAssignment where IID={0} and OfferRoleTypeKey={1} and ADUserKey={2} order by id desc", instanceID, ORTKey, adUserKey);
            castleTransactionService.FillDataSetFromQueryOnCastleTran(ds, "WFAssignment", sb.ToString(), Databases.X2, null);
            DataTable dt = ds.Tables["WFAssignment"];
            if (dt.Rows.Count == 0)
            {
                // We cant get an existing assignment for this user. Just go ahead and assign to him. Not necessarily correct but I cant
                // See how we'd get here (Assuming we migrate existing offerroles into x2WofklowAssignment
                //LogPlugin.Logger.LogFormattedWarning("Unable to get existing WFAssignment IID:{0}, DynamicRole:{1}", IID, DynamicRole);
                return true;
            }

            // Get the Existing BlaKey for this case assignment. From there Get the ORgStructureKey.
            // This will tell us which OrgStruct the person was part of when the case was assigned.
            // If they are STILL in that org structure return true else they have moved.
            int OrgStructureKeyAtTimeOfAssignmet = Convert.ToInt32(dt.Rows[0]["OrganisationStructureKey"]);

            // Get the ADUser and the OS's that they belong to then see if it overlaps with the BlaKey
            offerRole.WorkflowAssignment.ADUserRow adRow = workflowSecurityRepository.GetOfferRoleOrganisationStructure().ADUser.FindByADUserKey(adUserKey);
            offerRole.WorkflowAssignment.UserOrganisationStructureRow[] uos = adRow.GetUserOrganisationStructureRows();
            foreach (offerRole.WorkflowAssignment.UserOrganisationStructureRow row in uos)
            {
                offerRole.WorkflowAssignment.OfferRoleTypeOrganisationStructureMappingRow[] BlaRows = row.OrganisationStructureRow.GetOfferRoleTypeOrganisationStructureMappingRows();
                foreach (offerRole.WorkflowAssignment.OfferRoleTypeOrganisationStructureMappingRow BlaRow in BlaRows)
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

        public void AssignWorkflowRole(long instanceID, int adUserKey, int offerRoleTypeOrganisationStructureMappingKey, string stateName)
        {
            string query = string.Format("exec X2..pr_AssignWorkflowRole @IID, @BLA, @ADUserKey, @State");

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@IID", instanceID));
            parameters.Add(new SqlParameter("@BLA", offerRoleTypeOrganisationStructureMappingKey));
            parameters.Add(new SqlParameter("@ADUserKey", adUserKey));
            parameters.Add(new SqlParameter("@State", stateName));

            // Execute Query
            castleTransactionService.ExecuteNonQueryOnCastleTran(query, Databases.X2, parameters);
        }

        public string GetConsultantForInstanceAndRole(long instanceID, string dynamicRole)
        {
            string query = string.Format("exec X2..pr_ResolveWorkflowRole {0}, '{1}'", instanceID, dynamicRole);
            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, Databases.TwoAM, new ParameterCollection());
            if (o != null)
                return o.ToString();

            return "";
        }

        public bool AssignCreateorAsDynamicRole(long instanceID, string dynamicRole, out string assignedTo, int genericKey, string stateName)
        {
            assignedTo = string.Empty;

            // Get the CreatorADUserName from teh x2.instance table.
            // Write a 101 ORT key for their branch into x2.wfassign
            StringBuilder sb = new StringBuilder();
            offerRole.WorkflowAssignment ds = workflowSecurityRepository.GetOfferRoleOrganisationStructure();
            sb.AppendFormat("select CreatorADUserName from x2.instance (nolock) where id={0}", instanceID);
            object o = castleTransactionService.ExecuteScalarOnCastleTran(sb.ToString(), Databases.X2, null);
            string ADUserName = o.ToString();
            offerRole.WorkflowAssignment.ADUserRow adRow = workflowSecurityRepository.GetADUserRowByName(ADUserName);
            int ADUserKey = adRow.ADUserKey;

            int BlaKey = -1;
            offerRole.WorkflowAssignment.OfferRoleTypeRow oRow = workflowSecurityRepository.GetOfferRoleRow(dynamicRole);
            foreach (offerRole.WorkflowAssignment.OfferRoleTypeOrganisationStructureMappingRow blaRow in oRow.GetOfferRoleTypeOrganisationStructureMappingRows())
            {
                foreach (offerRole.WorkflowAssignment.UserOrganisationStructureRow uosRow in blaRow.OrganisationStructureRow.GetUserOrganisationStructureRows())
                {
                    if (uosRow.ADUserKey == ADUserKey)
                    {
                        // we have found the link
                        BlaKey = blaRow.OfferRoleTypeOrganisationStructureMappingKey;
                        break;
                    }
                }
                if (BlaKey != -1) break;
            }
            if (BlaKey != -1)
            {
                workflowSecurityRepository.AssignWorkflowRole(instanceID, ADUserKey, BlaKey, stateName);
                workflowSecurityRepository.Assign2AMOfferRole(genericKey, oRow.OfferRoleTypeKey, adRow.LegalEntityKey);
                assignedTo = ADUserName;
                return true;
            }
            return false;
        }

        public void ReassignCaseToUser(long instanceID, int genericKey, string adUserName, int organisationStructureKey, int offerRoleTypeKey, string stateName)
        {
            offerRole.WorkflowAssignment ds = workflowSecurityRepository.GetOfferRoleOrganisationStructure();
            int ADKey = workflowSecurityRepository.GetADUserRowByName(adUserName).ADUserKey;
            int LEKey = workflowSecurityRepository.GetADUserRowByName(adUserName).LegalEntityKey;
            string where = string.Format("OrganisationStructureKey={0} and OfferRoleTypeKey={1}", organisationStructureKey, offerRoleTypeKey);
            object o = ds.OfferRoleTypeOrganisationStructureMapping.Select(where);
            if (null == o) throw new Exception(string.Format("Cant find BlaKey for OSKey:{0} ORTKey:{1}", organisationStructureKey, offerRoleTypeKey));
            offerRole.WorkflowAssignment.OfferRoleTypeOrganisationStructureMappingRow[] arr = (offerRole.WorkflowAssignment.OfferRoleTypeOrganisationStructureMappingRow[])o;
            int BlaKey = arr[0].OfferRoleTypeOrganisationStructureMappingKey;
            workflowSecurityRepository.AssignWorkflowRole(instanceID, ADKey, BlaKey, stateName);

            workflowSecurityRepository.Assign2AMOfferRole(genericKey, offerRoleTypeKey, LEKey);
        }

        public bool DeActiveUsersForInstance(long instanceID, int genericKey, List<string> dynamicRoles)
        {
            try
            {
                workflowSecurityRepository.DeactivateWorkflowRole(instanceID);
                for (int i = 0; i < dynamicRoles.Count; i++)
                {
                    int ORTKey = workflowSecurityRepository.GetOfferRoleRow(dynamicRoles[i]).OfferRoleTypeKey;
                }
                return true;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (ActiveRecordException)
            {
                throw;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ReactiveUserOrRoundRobin(string dynamicRole, int genericKey, int organisationStructureKey, long instanceID, string stateName)
        {
            List<int> OSkeys = new List<int>();
            OSkeys.Add(organisationStructureKey);
            return ReactiveUserOrRoundRobin(dynamicRole, genericKey, OSkeys, instanceID, stateName);
        }

        public bool ReactiveUserOrRoundRobin(string dynamicRole, int genericKey, List<int> organisationStructureKeys, long instanceID, string stateName)
        {
            string AssignedUser = string.Empty; ;
            try
            {
                // Get the existing record for this case.
                offerRole.WorkflowAssignment ds = workflowSecurityRepository.GetWFAssignment(genericKey, dynamicRole, instanceID);
                if (ds.WFAssignment.Rows.Count > 0)
                {
                    offerRole.WorkflowAssignment.WFAssignmentRow row = (offerRole.WorkflowAssignment.WFAssignmentRow)ds.WFAssignment.Rows[0];
                    if (IsUserStillInSameOrgStructureForCaseReassign(row.ADUserKey, dynamicRole, instanceID))
                    {
                        if (IsUserActive(row.ADUserKey))
                        {
                            AssignedUser = row.ADUserName;

                            // reactivate also make sure the offerrole record is reactivated
                            workflowSecurityRepository.AssignWorkflowRole(instanceID, row.ADUserKey, row.BlaKey, stateName);
                            int LEKey = workflowSecurityRepository.GetOfferRoleOrganisationStructure().ADUser.FindByADUserKey(row.ADUserKey).LegalEntityKey;
                            int ORTKey = workflowSecurityRepository.GetOfferRoleRow(dynamicRole).OfferRoleTypeKey;

                            //for Offerrole we need LEKey
                            workflowSecurityRepository.Assign2AMOfferRole(genericKey, ORTKey, LEKey);
                        }
                        else
                        {
                            // RR
                            AssignedUser = X2RoundRobinForGivenOSKeys(dynamicRole, genericKey, organisationStructureKeys, instanceID, stateName);
                        }
                    }
                    else
                    {
                        // RR
                        AssignedUser = X2RoundRobinForGivenOSKeys(dynamicRole, genericKey, organisationStructureKeys, instanceID, stateName);
                    }
                }
                else
                {
                    // Case has no assigment rows so just RR
                    AssignedUser = X2RoundRobinForGivenOSKeys(dynamicRole, genericKey, organisationStructureKeys, instanceID, stateName);
                }
                return true;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (ActiveRecordException)
            {
                throw;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string X2RoundRobinForGivenOSKeys(string dynamicRole, int genericKey, int organisationStructureKey, long instanceID, string stateName)
        {
            List<int> OSKeys = new List<int>();
            OSKeys.Add(organisationStructureKey);
            return X2RoundRobinForGivenOSKeys(dynamicRole, genericKey, OSKeys, instanceID, stateName);
        }

        public string X2RoundRobinForGivenOSKeys(string dynamicRole, int genericKey, List<int> organisationStructureKeys, long instanceID, string stateName)
        {
            try
            {
                // Get a list of active users to RR the case to
                // Find the last assigned case for this group (Assuming there is one)
                // Get the next Key or loop back to start of list and assign
                // return

                // For the moment we have to post offerrolerecords as well until the security is reworked.
                // So these vars are needed.
                int OfferRoleTypeKey = -1;
                int LEKey = -1;

                string sOSKeys = "";
                for (int i = 0; i < organisationStructureKeys.Count; i++)
                {
                    sOSKeys = string.Format("{0},{1}", sOSKeys, organisationStructureKeys[i]);
                }
                sOSKeys = sOSKeys.Remove(0, 1);
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select * from vw_UsersOfferRoleOrgStruct where ORT='{0}' and OSKey in ({1}) order by OSKey, ADUserKey",
                    dynamicRole, sOSKeys);
                DataSet ds = new DataSet();

                //DBMan.X2Execute(sb.ToString(), ds, "DATA");
                ds = castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, null, new StringCollection() { "DATA" }, ds);

                //WorkerHelper.FillFromQuery(dtUsersToAllocateTo, sb.ToString(), new ParameterCollection());
                DataTable dtUsersToAllocateTo = ds.Tables["DATA"];
                if (dtUsersToAllocateTo.Rows.Count == 0)
                    throw new Exception(string.Format("No users found for dynamic role:{0}", dynamicRole));

                List<int> BlaKeys = new List<int>();
                string BlaKeysWhere = string.Empty;
                foreach (DataRow dr in dtUsersToAllocateTo.Rows)
                {
                    int BlaKey = Convert.ToInt32(dr["BlaKey"]);
                    if (!BlaKeys.Contains(BlaKey))
                    {
                        BlaKeys.Add(BlaKey);
                        BlaKeysWhere = string.Format("{0},{1}", BlaKeysWhere, BlaKey);
                    }
                }
                BlaKeysWhere = BlaKeysWhere.Remove(0, 1);

                int LastUserAssignedID = 0;
                int ADUSerKeyToAssignTo = -1;
                int BlaKeyToAssignTo = -1;
                string AssignedTo = string.Empty;
                sb = new StringBuilder();
                sb.AppendFormat("select top 1 * from vw_WFAssignment where BlaKey in ({0}) order by id desc", BlaKeysWhere);
                DataTable dt = new DataTable();

                //WorkerHelper.FillFromQuery(dt, sb.ToString(), new ParameterCollection());
                ds = new DataSet();

                //DBMan.X2Execute(sb.ToString(), ds, "DATA");
                ds = castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, null, new StringCollection() { "DATA" }, ds);
                dt = ds.Tables["DATA"];
                if (dt.Rows.Count > 0)
                {
                    LastUserAssignedID = Convert.ToInt32(dt.Rows[0]["ADUserKey"]);
                    int idx = -1;

                    for (int i = 0; i < dtUsersToAllocateTo.Rows.Count; i++)
                    {
                        int ADKey = Convert.ToInt32(dtUsersToAllocateTo.Rows[i]["ADUserKey"]);
                        if (ADKey == LastUserAssignedID)
                        {
                            idx = i;
                            break;
                        }
                    }
                    if ((idx == -1) || (idx == (dtUsersToAllocateTo.Rows.Count - 1)))
                    {
                        // use the first one
                        ADUSerKeyToAssignTo = Convert.ToInt32(dtUsersToAllocateTo.Rows[0]["ADUserKey"]);
                        BlaKeyToAssignTo = Convert.ToInt32(dtUsersToAllocateTo.Rows[0]["BlaKey"]);
                        AssignedTo = dtUsersToAllocateTo.Rows[0]["ADUserName"].ToString();
                    }
                    else
                    {
                        ADUSerKeyToAssignTo = Convert.ToInt32(dtUsersToAllocateTo.Rows[(idx + 1)]["ADUserKey"]);
                        BlaKeyToAssignTo = Convert.ToInt32(dtUsersToAllocateTo.Rows[(idx + 1)]["BlaKey"]);
                        AssignedTo = dtUsersToAllocateTo.Rows[(idx + 1)]["ADUserName"].ToString();
                    }
                }
                else
                {
                    // use the first one.
                    ADUSerKeyToAssignTo = Convert.ToInt32(dtUsersToAllocateTo.Rows[0]["ADUserKey"]);
                    BlaKeyToAssignTo = Convert.ToInt32(dtUsersToAllocateTo.Rows[0]["BlaKey"]);
                    AssignedTo = dtUsersToAllocateTo.Rows[0]["ADUserName"].ToString();
                }

                workflowSecurityRepository.AssignWorkflowRole(instanceID, ADUSerKeyToAssignTo, BlaKeyToAssignTo, stateName);

                // Now as legacy crap we need to go and write offerrole records.
                //offerRole.WorkflowAssignment dsOS = (WorkflowAssignment)DSGet(KnownGoodKeys.ORGSTRUCTURE_DATASET_DS.ToString());
                offerRole.WorkflowAssignment dsOS = workflowSecurityRepository.GetOfferRoleOrganisationStructure();
                if (null == dsOS)
                    dsOS = workflowSecurityRepository.GetOfferRoleOrganisationStructure();
                LEKey = dsOS.ADUser.FindByADUserKey(ADUSerKeyToAssignTo).LegalEntityKey;
                OfferRoleTypeKey = dsOS.OfferRoleTypeOrganisationStructureMapping.FindByOfferRoleTypeOrganisationStructureMappingKey(BlaKeyToAssignTo).OfferRoleTypeKey;

                sb = new StringBuilder();

                // mark the existing offerrole for this ORT as inactive
                //sb.AppendFormat("update [2am]..offerrole with(rowlock) set GeneralStatusKey=2 where OfferKey={0} and OfferRoleTypeKey={1}", genericKey, OfferRoleTypeKey);
                //castleTransactionService.ExecuteNonQueryOnCastleTran(sb.ToString(), Databases.X2, new SAHL.Common.DataAccess.ParameterCollection());

                workflowSecurityRepository.Assign2AMOfferRole(genericKey, OfferRoleTypeKey, LEKey);
                return AssignedTo;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (ActiveRecordException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string BP = ex.ToString();
                LogPlugin.Logger.LogErrorMessageWithException("", "X2RoundRobinForGivenOSKeys Failed", ex);
            }
            return string.Empty;
        }

        public bool InsertCommissionableConsultant(long instanceID, string adUserName, int genericKey, string stateName)
        {
            string Role = "Commissionable Consultant";
            try
            {
                workflowSecurityRepository.GetOfferRoleOrganisationStructure();
                offerRole.WorkflowAssignment.ADUserRow adRow = workflowSecurityRepository.GetADUserRowByName(adUserName);
                int ADUserKey = adRow.ADUserKey;

                int BlaKey = -1;
                offerRole.WorkflowAssignment.OfferRoleTypeRow oRow = workflowSecurityRepository.GetOfferRoleRow(Role);
                foreach (offerRole.WorkflowAssignment.OfferRoleTypeOrganisationStructureMappingRow blaRow in oRow.GetOfferRoleTypeOrganisationStructureMappingRows())
                {
                    foreach (offerRole.WorkflowAssignment.UserOrganisationStructureRow uosRow in blaRow.OrganisationStructureRow.GetUserOrganisationStructureRows())
                    {
                        if (uosRow.ADUserKey == ADUserKey)
                        {
                            // we have found the link
                            BlaKey = blaRow.OfferRoleTypeOrganisationStructureMappingKey;
                            break;
                        }
                    }
                    if (BlaKey != -1) break;
                }

                if (BlaKey != -1)
                {
                    workflowSecurityRepository.AssignWorkflowRole(instanceID, ADUserKey, BlaKey, stateName);
                    workflowSecurityRepository.Assign2AMOfferRole(genericKey, oRow.OfferRoleTypeKey, adRow.LegalEntityKey);
                }
                else
                {
                    //LogPlugin.Logger.LogFormattedError("Cant get BlaKey for ORT:{0} CaseOwner:{1} IID:{2}", Role, ADUserName, InstanceID);
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (ActiveRecordException)
            {
                throw;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool CloneActiveSecurityFromInstanceForInstance(long parentInstanceID, long instanceID)
        {
            try
            {
                // Go look at the vw_WFAssignment and look for all roles with GSKey of 1 and then clone them for the new IID
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select * from x2.WorkflowAssignment where InstanceID={0} and GeneralStatusKey=1", parentInstanceID);
                offerRole.WorkflowAssignment ds = new offerRole.WorkflowAssignment();
                castleTransactionService.FillDataSetFromQueryOnCastleTran(ds, "WorkflowAssignment", sb.ToString(), Databases.X2, null);
                foreach (offerRole.WorkflowAssignment.WorkflowAssignmentRow row in ds._WorkflowAssignment.Rows)
                {
                    sb = new StringBuilder();
                    sb.AppendFormat("insert into x2.WorkflowAssignment with(rowlock) values ({0}, {1}, {2}, 1, getdate(),'{3}')", instanceID, row.OfferRoleTypeOrganisationStructureMappingKey, row.ADUserKey, row.State);
                    castleTransactionService.ExecuteNonQueryOnCastleTran(sb.ToString(), Databases.X2, new SAHL.Common.DataAccess.ParameterCollection());
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (ActiveRecordException)
            {
                throw;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public DataTable GetCurrentConsultantAndAdmin(long instanceID)
        {
            // Go look at the x2app_man for this offerkey
            // Get the instnace
            // look for the latest Branch consultant D, Branch Admin D and FL Processor D
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" select top 1 LE.EMailAddress ");
            sb.AppendFormat("from vw_WFAssignment wfa ");
            sb.AppendFormat("join [2am]..aduser a on wfa.aduserkey=a.aduserkey ");
            sb.AppendFormat("join [2am]..LegalEntity LE on a.legalentitykey=le.legalentitykey ");
            sb.AppendFormat("where IID={0} and OfferRoleTypeKey =101 ", instanceID);
            sb.AppendFormat("union ");
            sb.AppendFormat("select top 1 LE.EMailAddress ");
            sb.AppendFormat("from vw_WFAssignment wfa ");
            sb.AppendFormat("join [2am]..aduser a on wfa.aduserkey=a.aduserkey ");
            sb.AppendFormat("join [2am]..LegalEntity LE on a.legalentitykey=le.legalentitykey ");
            sb.AppendFormat("where IID={0} and OfferRoleTypeKey =102 ", instanceID);
            sb.AppendFormat("union ");
            sb.AppendFormat("select top 1 LE.EMailAddress ");
            sb.AppendFormat("from vw_WFAssignment wfa ");
            sb.AppendFormat("join [2am]..aduser a on wfa.aduserkey=a.aduserkey ");
            sb.AppendFormat("join [2am]..LegalEntity LE on a.legalentitykey=le.legalentitykey ");
            sb.AppendFormat("where IID={0} and OfferRoleTypeKey =857 ", instanceID);

            DataSet ds = castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, null);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        public void GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStruct(long sourceInstanceID, string dynamicRole, int organisationStructureKey, out string assignedUser)
        {
            assignedUser = string.Empty;
            offerRole.WorkflowAssignment.OfferRoleTypeRow ortRow = workflowSecurityRepository.GetOfferRoleRow(dynamicRole);
            DataTable dt = new DataTable();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from vw_WFAssignment wfa where wfa.IID={0} and wfa.OfferRoleTypeKey={1} order by wfa.id desc", sourceInstanceID, ortRow.OfferRoleTypeKey);
            var dataSet = castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection());
            dt = dataSet.Tables[0];

            //WorkerHelper.FillFromQuery(dt, sb.ToString(), Tran.Context, new ParameterCollection());
            //DBMan.X2Execute(sb.ToString(), ds, "WFAssignment");
            long instanceID;
            int adUserKey;
            int blaKey;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //offerRole.WorkflowAssignment.WFAssignmentRow row = (offerRole.WorkflowAssignment.WFAssignmentRow)dt.Rows[i];
                adUserKey = Convert.ToInt32(dt.Rows[0]["ADUserKey"]);
                blaKey = Convert.ToInt32(dt.Rows[0]["BlaKey"]);
                instanceID = Convert.ToInt32(dt.Rows[0]["IID"]);
                if (IsUserStillInSameOrgStructureForCaseReassign(adUserKey, dynamicRole, instanceID) && IsUserActive(adUserKey))
                {
                    assignedUser = Convert.ToString(dt.Rows[0]["ADUserName"]);
                    break;
                }
            }
        }

        public void DeActiveUsersForInstance(long instanceID, int genericKey, List<string> dynamicRoles, Process process)
        {
            workflowSecurityRepository.DeactivateWorkflowRole(instanceID);
        }

        public void ReassignParentMapToCurrentUser(long instanceID, long sourceInstanceID, int applicationKey, string state, Process process)
        {
            // sending it back to FL Processor D
            string dynamicRole = "FL Processor D";

            // get user with this case assigned on this instance (readvance)
            offerRole.WorkflowAssignment ds = workflowSecurityRepository.GetWFAssignmentAcrossInstances(dynamicRole, sourceInstanceID);
            if (ds.WFAssignment.Rows.Count > 0)
            {
                int OSKey = ((offerRole.WorkflowAssignment.WFAssignmentRow)ds.WFAssignment.Rows[0]).OrganisationStructureKey;

                // we have a role so assign it to the sourceid as that's where this case is going - to AppMan.
                ReactiveUserOrRoundRobin(dynamicRole, applicationKey, OSKey, sourceInstanceID, state, process, (int)RoundRobinPointers.FLProcessor);
            }
            else
            {
                // before round robining  we need to check if the  sourceid  has a user of this type.

                // RR
                //X2RoundRobinForGivenOSKeys(DynamicRole, ApplicationKey, 157, SourceInstanceID, State, pName);
                X2RoundRobinForPointerDescription(instanceID, (int)RoundRobinPointers.FLProcessor, applicationKey, dynamicRole, state, process);
            }

            //return new X2ReturnData(null, null);
        }

        public string ReactiveUserOrRoundRobin(string dynamicRole, int genericKey, int organisationStructureKey, long instanceID, string state, Process process, int roundRobinPointerKey)
        {
            List<int> organisationStructureKeys = new List<int>();
            organisationStructureKeys.Add(organisationStructureKey);
            return ReactiveUserOrRoundRobin(dynamicRole, genericKey, organisationStructureKeys, instanceID, state, process, roundRobinPointerKey);
        }

        public string ReactiveUserOrRoundRobin(string dynamicRole, int genericKey, List<int> organisationStructureKeys, Int64 instanceID, string state, Process process, int roundRobinPointerKey)
        {
            string assignedUser = string.Empty;

            // Get the existing record for this case.
            offerRole.WorkflowAssignment ds = workflowSecurityRepository.GetWFAssignment(genericKey, dynamicRole, instanceID);
            if (ds.WFAssignment.Rows.Count > 0)
            {
                offerRole.WorkflowAssignment.WFAssignmentRow row = (offerRole.WorkflowAssignment.WFAssignmentRow)ds.WFAssignment.Rows[0];
                if (IsUserStillInSameOrgStructureForCaseReassign(row.ADUserKey, dynamicRole, instanceID))
                {
                    if (IsUserActive(row.ADUserKey))
                    {
                        assignedUser = row.ADUserName;

                        // reactivate also make sure the offerrole record is reactivated
                        workflowSecurityRepository.AssignWorkflowRole(instanceID, row.ADUserKey, row.BlaKey, state);
                        if (process == Process.Origination)
                        {
                            int LEKey = workflowSecurityRepository.GetOfferRoleOrganisationStructure().ADUser.FindByADUserKey(row.ADUserKey).LegalEntityKey;
                            int ORTKey = workflowSecurityRepository.GetOfferRoleRow(dynamicRole).OfferRoleTypeKey;

                            //for Offerrole we need LEKey
                            workflowSecurityRepository.Assign2AMOfferRole(genericKey, ORTKey, LEKey);
                        }
                    }
                    else
                    {
                        //if (RoundRobinPointerDescription == "NONE")
                        //    AssignedUser = X2RoundRobinForGivenOSKeys(DynamicRole, GenericKey, OSKeys, InstanceID, State, pName);
                        //else
                        assignedUser = X2RoundRobinForPointerDescription(instanceID, roundRobinPointerKey, genericKey, dynamicRole, state, process);
                    }
                }
                else
                {
                    // RR
                    //if (RoundRobinPointerDescription == "NONE")
                    //    AssignedUser = X2RoundRobinForGivenOSKeys(DynamicRole, GenericKey, OSKeys, InstanceID, State, pName);
                    //else
                    assignedUser = X2RoundRobinForPointerDescription(instanceID, roundRobinPointerKey, genericKey, dynamicRole, state, process);
                }
            }
            else
            {
                // Case has no assigment rows so just RR
                //if (RoundRobinPointerDescription == "NONE")
                //    AssignedUser = X2RoundRobinForGivenOSKeys(DynamicRole, GenericKey, OSKeys, InstanceID, State, pName);
                //else
                assignedUser = X2RoundRobinForPointerDescription(instanceID, roundRobinPointerKey, genericKey, dynamicRole, state, process);
            }
            return assignedUser;

            //return new X2ReturnData(null, assignedUser);
        }

        public string X2RoundRobinForPointerDescription(long instanceID, int roundRobinPointerKey, int genericKey, string dynamicRole, string state, Process process)
        {
            string adUserName = string.Empty;
            int adUserKey = -1;

            DataTable fullTable = workflowSecurityRepository.GetADUsersForRoundRobinByRoundRobinPointerDescription(roundRobinPointerKey);

            // Remove Duplicates and Sort list of ADUsers - won't change.
            fullTable = workflowSecurityRepository.RemoveDuplicateRows(fullTable, "ADUserKey");

            DataView v = fullTable.DefaultView;
            v.Sort = "[ADUserKey] asc";
            fullTable = v.ToTable();

            adUserKey = workflowSecurityRepository.TrueRoundRobin(fullTable, roundRobinPointerKey);

            int blaKey = -1;
            System.Data.DataRow[] BlaRow = fullTable.Select(string.Format("ADUserKey = {0}", adUserKey));
            blaKey = Convert.ToInt32(BlaRow[0].ItemArray[0]);

            // need to pass in the ADUserKey, BlaKey, DynamicRole, GenericKey -- returns ADUserName assigned to.
            // if ADUserKey = -1 then no user was found so try returning error to the screen
            if (adUserKey > 0)
            {
                adUserName = workflowSecurityRepository.AssignToWorkflowAssignmentAndOfferRoleTables(adUserKey, blaKey, dynamicRole, genericKey, instanceID, state, process);
            }
            return adUserName;
        }

        public bool ReactivateUserOrLoadBalanceAssign(GenericKeyTypes userOrganisationStructureGenericType, WorkflowRoleTypes workflowRoleType, int genericKey, Int64 instanceID, List<string> statesToDetermineLoad, Process processName, Workflow workflowName)
        {
            return ReactivateUserOrLoadBalanceAssign(userOrganisationStructureGenericType, workflowRoleType, genericKey, instanceID, statesToDetermineLoad, processName, workflowName, true);
        }

        public bool ReactivateUserOrLoadBalanceAssign(GenericKeyTypes userOrganisationStructureGenericType, WorkflowRoleTypes workflowRoleType, int genericKey, Int64 instanceID, List<string> statesToDetermineLoad, Process processName, Workflow workflowName, bool includeStates)
        {
            //LogPlugin.Logger.LogInfo(String.Format("Attempting to Reactivate User or Load Balance Assign for case : {0} and Workflow Role Type : {1}", genericKey, workflowRoleType.ToString()));
            string assignedUser = string.Empty;

            // Get the existing record for this case.
            workflowRole.WorkflowAssignment ds = workflowSecurityRepository.GetWorkflowRoleAssignment(workflowRoleType, instanceID);
            if (ds.WFRAssignment.Rows.Count > 0)
            {
                workflowRole.WorkflowAssignment.WFRAssignmentRow row = (workflowRole.WorkflowAssignment.WFRAssignmentRow)ds.WFRAssignment.Rows[0];
                if (workflowSecurityRepository.IsUserStillInSameOrgStructureForCaseReassign(row.ADUserKey, userOrganisationStructureGenericType, workflowRoleType, instanceID))
                {
                    if (workflowSecurityRepository.IsUserActive(userOrganisationStructureGenericType, workflowRoleType, row.ADUserKey))
                    {
                        assignedUser = row.ADUserName;

                        // reactivate also make sure the workflowrole record is reactivated
                        // this inserts a record into the x2.workflowroleassignment table
                        workflowSecurityRepository.CreateWorkflowRoleAssignment(instanceID, row.BlaKey, row.ADUserKey, String.Empty);
                        if (processName == Process.DebtCounselling)
                        {
                            int legalEntityKey = workflowSecurityRepository.GetWorkflowRoleOrganisationStructure().ADUser.FindByADUserKey(row.ADUserKey).LegalEntityKey;
                            int workflowRoleTypeKey = workflowSecurityRepository.GetWorkflowRoleTypeRow(workflowRoleType).WorkflowRoleTypeKey;

                            // insert / update the [2am]..WorkflowRole
                            workflowSecurityRepository.Create2AMWorkflowRole(genericKey, workflowRoleTypeKey, legalEntityKey);
                        }
                    }
                    else
                    {
                        // user is incative - load balance assign here
                        assignedUser = workflowSecurityRepository.LoadBalanceAssign(userOrganisationStructureGenericType, workflowRoleType, genericKey, instanceID, statesToDetermineLoad, processName, workflowName, includeStates, false);
                    }
                }
                else
                {
                    // user has moved org structure - load balance assign here
                    assignedUser = workflowSecurityRepository.LoadBalanceAssign(userOrganisationStructureGenericType, workflowRoleType, genericKey, instanceID, statesToDetermineLoad, processName, workflowName, includeStates, false);
                }
            }
            else
            {
                // never been assigned  - therefore load balance assign here
                assignedUser = workflowSecurityRepository.LoadBalanceAssign(userOrganisationStructureGenericType, workflowRoleType, genericKey, instanceID, statesToDetermineLoad, processName, workflowName, includeStates, false);
            }

            //If we could not assign the case to a user, return true
            return String.IsNullOrEmpty(assignedUser) ? false : true;
        }

        public string X2LoadBalanceAssign(GenericKeyTypes userOrganisationStructureGenericKeyType, WorkflowRoleTypes workflowRoleType, int genericKey, Int64 instanceID, List<string> statesToDetermineLoad, Process process, Workflow workflow, bool checkRoundRobinStatus)
        {
            return workflowSecurityRepository.LoadBalanceAssign(userOrganisationStructureGenericKeyType, workflowRoleType, genericKey, instanceID, statesToDetermineLoad, process, workflow, true, checkRoundRobinStatus);
        }

        public void AssignWorkflowRoleForADUser(long instanceID, string adUserName, WorkflowRoleTypes workflowRoleType, int genericKey, string state)
        {
            workflowSecurityRepository.AssignWorkflowRoleForADUser(instanceID, adUserName, workflowRoleType, genericKey);
            workflowSecurityRepository.AssignWorkflowRoleAssignmentForADUser(instanceID, adUserName, workflowRoleType, state);
        }

        /// <summary>
        /// This method controls the assignment logic for a debt counselling cases.
        /// The court case flag is passed in (not retrieved from the db).
        /// If the court case flag = true, then workflow role type is set to debt counselling court consultant (dynamic).
        /// This overwrites the passed in workflow role type value.
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="workflowRoleType"></param>
        /// <param name="state"></param>
        /// <param name="states"></param>
        /// <param name="includeStates"></param>
        /// <param name="courtCase"></param>
        /// <returns>bool indicating success or failure</returns>
        public bool AssignDebtCounsellingCaseForGroupOrLoadBalance(long instanceID,
            int debtCounsellingKey,
            WorkflowRoleTypes workflowRoleType,
            string state,
            List<string> states,
            bool includeStates,
            bool courtCase)
        {
            if (courtCase && workflowRoleType == WorkflowRoleTypes.DebtCounsellingConsultantD)
                workflowRoleType = WorkflowRoleTypes.DebtCounsellingCourtConsultantD;

            //Get AD User Name from AD User Key
            DataRow workflowRoleAssignmentRow = null;

            //Get the AD User Key to assign the case to
            //Additionally, we want to get the Workflow Role Type Organisation Structure Mapping Row to get the Organisation Structure Mapping Key
            int adUserKey = workflowSecurityRepository.GetLastAssignedUserForGroupByRole(instanceID, debtCounsellingKey, workflowRoleType, out workflowRoleAssignmentRow);

            //If the user is Active,
            //If we need to check the Round Robin Status, then check it
            if (workflowRoleAssignmentRow != null &&
                workflowSecurityRepository.IsUserActive(GenericKeyTypes.WorkflowRoleType, workflowRoleType, adUserKey) &&
                workflowSecurityRepository.IsUserStillInSameOrgStructureForCaseReassign(adUserKey, GenericKeyTypes.WorkflowRoleType, workflowRoleType, instanceID)
                )
            {
                //Assign
                string adUserName = workflowSecurityRepository.GetADUser(adUserKey).ADUserName;
                AssignWorkflowRoleForADUser(instanceID, adUserName, workflowRoleType, debtCounsellingKey, state);

                //We have done the assignment without an issue
                return true;
            }

            //Attempt to perform a Load Balance assign for the Workflow Role
            if (!String.IsNullOrEmpty(workflowSecurityRepository.LoadBalanceAssign(GenericKeyTypes.WorkflowRoleType, workflowRoleType, debtCounsellingKey, instanceID, states, Process.DebtCounselling, Workflow.DebtCounselling, includeStates, true)))
            {
                return true;
            }
            else
            {
                //Everything else has failed,
                //return an error message to the user and roll everything back
                //LogPlugin.Logger.LogError(String.Format("Could not find a user to assign to the case: InstanceID : {0}", instanceID));
                return false;
            }
        }

        public int GetBranchManagerOrgStructureKey(long instanceID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select OrganisationStructureKey ");
            sb.AppendFormat("from [2am]..organisationstructure os  ");
            sb.AppendFormat("where ParentKey=(select top 1 parentkey from vw_WFAssignment where OfferRoleTypeKey=101   ");//and GSKey=1
            sb.AppendFormat("and IID={0} order by id desc) ", instanceID);
            sb.AppendFormat("and Description='Manager' ");
            object o = castleTransactionService.ExecuteScalarOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection());
            return Convert.ToInt32(o);
        }

        public string AssignBranchManagerForOrgStrucKey(long instanceID, string dynamicRole, int organisationStructureKey, int genericKey, string state, SAHL.Common.Globals.Process process)
        {
            string adUserName = string.Empty;

            DataTable dt = workflowSecurityRepository.GetUsersForOrgStructureKeyAndDynamicRole(instanceID, organisationStructureKey, dynamicRole);
            int ADUserKey = Convert.ToInt32(dt.Rows[0]["ADUserKey"]); // only interested in first entry.
            int BlaKey = Convert.ToInt32(dt.Rows[0]["BlaKey"]);
            if (ADUserKey > 0)
            {
                adUserName = workflowSecurityRepository.AssignToWorkflowAssignmentAndOfferRoleTables(ADUserKey, BlaKey, dynamicRole, genericKey, instanceID, state, process);
            }
            return adUserName;
        }

        public string GetLatestUserAcrossInstances(long InstanceID, int ApplicationKey, int OSKey, string DynamicRole, string State, SAHL.Common.Globals.Process pName)
        {
            string ADUserName = string.Empty;

            // Given InstanceID we need to see if it has a SourceInstanceID (in other words does it have a parent map?)
            offerRole.WorkflowAssignment dsFLProcInst = workflowSecurityRepository.GetWFAssignmentAcrossInstances(DynamicRole, InstanceID);
            if (dsFLProcInst.WFAssignment.Rows.Count > 0)
            {
                offerRole.WorkflowAssignment.WFAssignmentRow row = (offerRole.WorkflowAssignment.WFAssignmentRow)dsFLProcInst.WFAssignment.Rows[0];
                OSKey = row.OrganisationStructureKey;
                int ORTKey = row.OfferRoleTypeKey;
                string ADUser = ((offerRole.WorkflowAssignment.WFAssignmentRow)dsFLProcInst.WFAssignment.Rows[0]).ADUserName;

                // we have a user that was in the correct OrgStruc - now must check if they are still in the same orgstruc and are still active
                // reassign this user - this checks validity of user
                if ((IsUserStillInSameOrgStructureForCaseReassign(row.ADUserKey, DynamicRole, InstanceID))
                    && IsUserActive(row.ADUserKey))// note I pass in appcapiid for the check!
                {
                    ADUserName = ADUser;
                }
            }
            else
            {
                // user is not in parent's children so check the parent.
                // if user is in sourceinstanceid then assign to him
                long SourceInstanceID = workflowSecurityRepository.GetSourceInstanceFromInstanceID(InstanceID);
                if (SourceInstanceID > 0)
                {
                    offerRole.WorkflowAssignment dsFLProcSourceInst = workflowSecurityRepository.GetWFAssignment(ApplicationKey, DynamicRole, SourceInstanceID);
                    if (dsFLProcSourceInst.WFAssignment.Rows.Count > 0)
                    {
                        offerRole.WorkflowAssignment.WFAssignmentRow row = (offerRole.WorkflowAssignment.WFAssignmentRow)dsFLProcSourceInst.WFAssignment.Rows[0];
                        OSKey = row.OrganisationStructureKey;
                        int ORTKey = row.OfferRoleTypeKey;
                        string ADUser = ((offerRole.WorkflowAssignment.WFAssignmentRow)dsFLProcSourceInst.WFAssignment.Rows[0]).ADUserName;

                        // we have a user that was in the correct OrgStruc - now must check if they are still in the same orgstruc and are still active
                        // reassign this user - this checks validity of user
                        if ((IsUserStillInSameOrgStructureForCaseReassign(row.ADUserKey, DynamicRole, InstanceID))
                            && IsUserActive(row.ADUserKey))// note I pass in appcapiid for the check!
                        {
                            ADUserName = ADUser;
                        }
                    }
                    else
                    {
                        // not in parent or children so roundrobins
                        ADUserName = ""; // this means we'll need to do a x2roundrobin from the map.
                    }
                }
            }
            return ADUserName;
        }

        public string ResolveDynamicRoleToUserName(string DynamicRole, long InstanceID)
        {
            string SQL = string.Format("exec pr_ResolveWorkflowRole {0}, '{1}'", InstanceID, DynamicRole);
            object o = castleTransactionService.ExecuteScalarOnCastleTran(SQL, Databases.X2, new ParameterCollection());
            if (null != o)
                return o.ToString();
            return "";
        }

        public void ReassignCaseToUser(long InstanceID, int GenercKey, string ADUser, int OSKey, int ORTKey, string State, SAHL.Common.Globals.Process pName)
        {
            offerRole.WorkflowAssignment ds = workflowSecurityRepository.GetOfferRoleOrganisationStructure();
            int ADKey = workflowSecurityRepository.GetADUserRowByName(ADUser).ADUserKey;
            int LEKey = workflowSecurityRepository.GetADUserRowByName(ADUser).LegalEntityKey;
            string where = string.Format("OrganisationStructureKey={0} and OfferRoleTypeKey={1}", OSKey, ORTKey);
            object o = ds.OfferRoleTypeOrganisationStructureMapping.Select(where);
            if (null == o) throw new Exception(string.Format("Cant find BlaKey for OSKey:{0} ORTKey:{1}", OSKey, ORTKey));
            offerRole.WorkflowAssignment.OfferRoleTypeOrganisationStructureMappingRow[] arr = (offerRole.WorkflowAssignment.OfferRoleTypeOrganisationStructureMappingRow[])o;
            int BlaKey = arr[0].OfferRoleTypeOrganisationStructureMappingKey;
            workflowSecurityRepository.AssignWorkflowRole(InstanceID, ADKey, BlaKey, State);
            if (SAHL.Common.Globals.Process.Origination == pName)
                workflowSecurityRepository.Assign2AMOfferRole(GenercKey, ORTKey, LEKey);
        }

        public string GetLastUserToWorkOnCaseAcrossInstances(long InstanceID, long SourceInstanceID, int ORTKey, string DynamicRole, string MapName)
        {
            string ADUserName = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select top 1 wfa.ADUserName from x2.instance i ");
            sb.AppendFormat("join x2.workflow w on i.WorkflowID=w.id ");
            sb.AppendFormat("join vw_WFAssignment wfa on i.id=wfa.IID ");
            sb.AppendFormat("where w.name='{0}' and i.sourceinstanceid={1} and wfa.OfferRoleTypeKey = {2} order by wfa.id desc", MapName, SourceInstanceID, ORTKey);

            object o = castleTransactionService.ExecuteScalarOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection());
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                offerRole.WorkflowAssignment.ADUserRow adRow = workflowSecurityRepository.GetADUserRowByName(o.ToString());
                int ADUserKey = adRow.ADUserKey;
                if ((IsUserStillInSameOrgStructureForCaseReassign(ADUserKey, DynamicRole, InstanceID))
                                && IsUserActive(ADUserKey))// note I pass in appcapiid for the check!
                {
                    ADUserName = o.ToString();
                }
            }
            return ADUserName;
        }

        public string InsertInternetLeadWorkflowAssignment(long InstanceID, int ApplicationKey, string State)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.ExclusionSets.Add(RuleExclusionSets.WebLeadExclusionSet);
            spc.ExclusionSets.Add(RuleExclusionSets.LegalEntityExcludeAll);

            try
            {
                // Set up the Correct Ad User
                IInternetRepository internetRepository = RepositoryFactory.GetRepository<IInternetRepository>();
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                IADUser iaduser = internetRepository.AssignInternetLeadUser(ApplicationKey);

                // Assign the offerole
                IApplication app = appRepo.GetApplicationByKey(ApplicationKey);

                // Add 2 Roles here:
                app.AddRole((int)OfferRoleTypes.BranchConsultantD, iaduser.LegalEntity);

                //TODO add second Offer Role Type here
                app.AddRole((int)OfferRoleTypes.CommissionableConsultant, iaduser.LegalEntity);

                appRepo.SaveApplication(app);
            }
            catch (SqlException)
            {
                throw;
            }
            catch (ActiveRecordException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                spc.ExclusionSets.Remove(RuleExclusionSets.WebLeadExclusionSet);
                spc.ExclusionSets.Remove(RuleExclusionSets.LegalEntityExcludeAll);
            }

            int BlaKey = 22;  // Branch Consultant D

            // first get the case row from the offerrole table with the offerkey
            // with that we can get the blahkey = 22 (Branch Consultant D) and the aduserkey

            StringBuilder sb = new StringBuilder();
            sb.Append("select adu.ADUserName from [2am]..OfferRole o (nolock) ");
            sb.Append("join [2am]..ADUser adu (nolock) on adu.LegalEntityKey = o.LegalEntityKey ");
            sb.AppendFormat("where o.OfferKey = {0} and o.OfferRoleTypeKey = 101", ApplicationKey);
            object o = castleTransactionService.ExecuteScalarOnCastleTran(sb.ToString(), Databases.TwoAM, new ParameterCollection());
            string ADUserName = o.ToString();
            offerRole.WorkflowAssignment.ADUserRow adRow = workflowSecurityRepository.GetADUserRowByName(ADUserName);
            int ADUserKey = adRow.ADUserKey;
            workflowSecurityRepository.AssignWorkflowRole(InstanceID, ADUserKey, BlaKey, State);
            return ADUserName;
        }

        /// <summary>
        /// Much horrible badness lives here that must be exorcised
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="InstanceID"></param>
        /// <returns></returns>
        [Obsolete("DO NOT USE THIS, NEEDS REFACTORING!")]
        public bool CreditDecisionCheckAuthorisationRules(int ApplicationKey, long InstanceID)
        {
            bool b = false;

            try
            {
                List<string> DynamicRoles = new List<string>();
                DynamicRoles.Add("Credit Underwriter D");
                DynamicRoles.Add("Credit Supervisor D");
                DynamicRoles.Add("Credit Manager D");
                DynamicRoles.Add("Credit Exceptions D");
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHL.Common.Security.SAHLPrincipal.GetCurrent());
                IDomainMessageCollection Messages = spc.DomainMessages;
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                ILookupRepository l = RepositoryFactory.GetRepository<ILookupRepository>();
                IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IWorkflowSecurityRepository workflowSecurityRepository = RepositoryFactory.GetRepository<IWorkflowSecurityRepository>();
                IApplication app = appRepo.GetApplicationByKey(ApplicationKey);

                // Look at the vw_WFAssign and look at who has done what. Group by the ORT keys and check sigs as usual.

                #region Get the number of signatures for the case (Based on IID)

                StringBuilder sb = new StringBuilder();
                sb.Append("select wfh.ADUserName, a.Name from x2.workflowhistory wfh ");
                sb.Append("join x2.Activity a on wfh.Activityid=a.id ");
                sb.AppendFormat("where wfh.instanceid={0} ", InstanceID);
                sb.Append("and a.Name in ('Approve Application', 'Agree with decision', 'Refer Senior Analyst', 'Escalate to Mgr', ");
                sb.Append("'Escalate To Exceptions Mgr', 'Approve with Pricing Changes', 'Decline with Offer'); ");
                DataSet ds = new DataSet("DATA");
                castleTransactionService.FillDataSetFromQueryOnCastleTran(ds, "", sb.ToString(), Databases.X2, null);

                //ExecuteQueryOnCastleTran(sb.ToString(), typeof(State_DAO), null);

                Dictionary<int, int> Sigs = new Dictionary<int, int>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string ADUser = dr["ADUserName"].ToString();
                    offerRole.WorkflowAssignment.ADUserRow row = workflowSecurityRepository.GetADUserRowByName(ADUser);
                    foreach (offerRole.WorkflowAssignment.UserOrganisationStructureRow uosRow in row.GetUserOrganisationStructureRows())
                    {
                        switch (uosRow.OrganisationStructureKey)
                        {
                            case 581://EXMGR
                                {
                                    if (Sigs.ContainsKey(581))
                                    {
                                        int nSig = Sigs[581];
                                        Sigs[581] = (nSig + 1);
                                    }
                                    else
                                    {
                                        Sigs.Add(581, 1);
                                    }
                                    break;
                                }
                            case 580://Mgr
                                {
                                    if (Sigs.ContainsKey(580))
                                    {
                                        int nSig = Sigs[580];
                                        Sigs[580] = (nSig + 1);
                                    }
                                    else
                                    {
                                        Sigs.Add(580, 1);
                                    }
                                    break;
                                }
                            case 1008://Supervisor
                                {
                                    if (Sigs.ContainsKey(1008))
                                    {
                                        int nSig = Sigs[1008];
                                        Sigs[1008] = (nSig + 1);
                                    }
                                    else
                                    {
                                        Sigs.Add(1008, 1);
                                    }
                                    break;
                                }
                            case 1007:// underwriter
                                {
                                    if (Sigs.ContainsKey(1007))
                                    {
                                        int nSig = Sigs[1007];
                                        Sigs[1007] = (nSig + 1);
                                    }
                                    else
                                    {
                                        Sigs.Add(1007, 1);
                                    }
                                    break;
                                }
                        }
                    }
                }

                #endregion Get the number of signatures for the case (Based on IID)

                ISupportsVariableLoanApplicationInformation VLI = app.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                IEmploymentType et = VLI.VariableLoanInformation.EmploymentType;

                #region Self Employed status

                if (null != VLI)
                {
                    double? d = VLI.VariableLoanInformation.PTI * 100;
                    if ((et.Key == 2 && d > 25))
                    {
                        int nExcSigs = 0;
                        if (Sigs.ContainsKey(581))
                            nExcSigs = Sigs[581];
                        if (nExcSigs <= 0)
                        {
                            bool hasAppRole = GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyFromWorkflowAssignment(InstanceID, 805);
                            if (!hasAppRole)
                            {
                                DeActiveUsersForInstance(InstanceID, ApplicationKey, DynamicRoles);

                                //MarkCreditUsersAsInactive(ApplicationKey);
                                //string s = RoundRobinAssignForGivenOrgStructure("Credit Exceptions D", ApplicationKey, 581);
                                string s = X2RoundRobinForGivenOSKeys("Credit Exceptions D", ApplicationKey, 581, InstanceID, "");

                                //SPC.DomainMessages.Add(new Error(string.Format("Application :{0} is EmploymentType of Self and PTI > 25% ", app.Key), ""));
                                //LogPlugin.Logger.LogInfo(" *** Application :{0} is EmploymentType:Self and PTI>25% giving to EXCEP MGR {1} *** ", app.Key, s);
                            }
                            b = true;
                        }
                    }
                }

                #endregion Self Employed status

                #region Subsidied / employed status

                if (null != VLI)
                {
                    double? d = VLI.VariableLoanInformation.PTI * 100;
                    if ((et.Key == 3 || et.Key == 1) && d > 30)
                    {
                        int nExcSigs = 0;
                        if (Sigs.ContainsKey(581))
                            nExcSigs = Sigs[581];
                        if (nExcSigs <= 0)
                        {
                            bool hasAppRole = GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyFromWorkflowAssignment(InstanceID, 805);
                            if (!hasAppRole)
                            {
                                DeActiveUsersForInstance(InstanceID, ApplicationKey, DynamicRoles);

                                //this.MarkCreditUsersAsInactive(ApplicationKey);
                                //string s = RoundRobinAssignForGivenOrgStructure("Credit Exceptions D", ApplicationKey, 581);
                                string s = X2RoundRobinForGivenOSKeys("Credit Exceptions D", ApplicationKey, 581, InstanceID, "");

                                //SPC.DomainMessages.Add(new Error(string.Format("Application :{0} is EmploymentType of Subsidy/Employed and PTI > 25% ", app.Key), ""));
                                //LogPlugin.Logger.LogInfo(" *** Application :{0} is EmploymentType:Subsidy/Employed and PTI>30% giving to EXCEP MGR {1} *** ", app.Key, s);
                            }
                            b = true;
                        }
                    }
                }

                #endregion Subsidied / employed status

                IRuleService rules = ServiceFactory.GetService<IRuleService>();

                if (!b)
                {
                    #region Application Over 3 million

                    // 3 million must be signed off by the exceptions manager
                    // Jumbo 2.5 Appro
                    int Result = rules.ExecuteRule(Messages, "JumboApprove", new object[] { app, (double)3000000 });
                    if (Result > 0)
                    {
                        //LogPlugin.Logger.LogInfo(" *** Application :{0} > 3 mill *** ", app.Key);
                        //LogPlugin.Logger.LogInfo("Application:{0} needs Exceptions appro", ApplicationKey);
                        //Dictionary<string, int> Sigs = ActionPerformedBy(InstanceID, "Credit", new string[] { "Exceptions Manager" }, ActionsToCheck);
                        int nExcSigs = 0;
                        if (Sigs.ContainsKey(581))
                            nExcSigs = Sigs[581];
                        if (nExcSigs <= 0)
                        {
                            // assign to exceptions manager
                            bool hasAppRole = GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyFromWorkflowAssignment(InstanceID, 805);
                            if (!hasAppRole)
                            {
                                DeActiveUsersForInstance(InstanceID, ApplicationKey, DynamicRoles);

                                //this.MarkCreditUsersAsInactive(ApplicationKey);
                                //string s = RoundRobinAssignForGivenOrgStructure("Credit Exceptions D", ApplicationKey, 581);
                                string s = X2RoundRobinForGivenOSKeys("Credit Exceptions D", ApplicationKey, 581, InstanceID, "");

                                //LogPlugin.Logger.LogInfo(" *** Application :{0} needs Exp givig to {1} *** ", app.Key, s);
                            }
                            b = true;
                        }
                        else
                        {
                            b = false;
                        }
                    }

                    #endregion Application Over 3 million

                    else
                    {
                        #region > 2.5 Million

                        Result = rules.ExecuteRule(Messages, "JumboApprove", new object[] { app, (double)2500000 });
                        if (Result > 0)
                        {
                            //LogPlugin.Logger.LogInfo(" *** Application :{0} > 2.5 Mill *** ", app.Key);
                            // is greater than 2.5 mill must be approved by  Manager/Exc Mgr so go look in WFHist
                            int nMgrExpSigs = 0;
                            if (Sigs.ContainsKey(580))
                                nMgrExpSigs += Sigs[580];
                            if (Sigs.ContainsKey(581))
                                nMgrExpSigs += Sigs[581];
                            if (nMgrExpSigs > 0)
                            {
                                // needs at least one SA / A as well
                                int nTotalNumSigs = nMgrExpSigs;
                                if (Sigs.ContainsKey(1008))
                                    nTotalNumSigs += Sigs[1008];
                                if (Sigs.ContainsKey(1007))
                                    nTotalNumSigs += Sigs[1007];
                                if (nTotalNumSigs <= 1)
                                {
                                    // we need 2 sigs so assign it to an SA
                                    bool hasAppRole = GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyFromWorkflowAssignment(InstanceID, 807);
                                    if (!hasAppRole)
                                    {
                                        DeActiveUsersForInstance(InstanceID, ApplicationKey, DynamicRoles);

                                        //this.MarkCreditUsersAsInactive(ApplicationKey);
                                        //string s = RoundRobinAssignForGivenOrgStructure("Credit Supervisor D", ApplicationKey, 1008);
                                        string s = X2RoundRobinForGivenOSKeys("Credit Supervisor D", ApplicationKey, 1008, InstanceID, "");

                                        //LogPlugin.Logger.LogInfo(" *** Application :{0} needs 2 sigs givig to {1} *** ", app.Key, s);
                                    }
                                    b = true;
                                }
                            }
                            else
                            {
                                // must have an exc or mgr sig give to mrg they can escalate if need be
                                // Manager
                                //IApplicationRole role = osRepo.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(ApplicationKey, l.ApplicationRoleTypes.ObjectDictionary["806"].Key);
                                bool hasAppRole = GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyFromWorkflowAssignment(InstanceID, 806);
                                if (!hasAppRole)
                                {
                                    DeActiveUsersForInstance(InstanceID, ApplicationKey, DynamicRoles);

                                    //MarkCreditUsersAsInactive(ApplicationKey);
                                    //string s = RoundRobinAssignForGivenOrgStructure("Credit Manager D", ApplicationKey, 580);
                                    string s = X2RoundRobinForGivenOSKeys("Credit Manager D", ApplicationKey, 580, InstanceID, "");

                                    //LogPlugin.Logger.LogInfo(" *** Application :{0} needs Mgr/Exp givig to {1} *** ", app.Key, s);
                                }
                                b = true;
                            }
                        }

                        #endregion > 2.5 Million

                        else
                        {
                            #region > 1.5 Mill < 2.5 Mill

                            // check for > 1.5
                            Result = rules.ExecuteRule(Messages, "JumboApprove", new object[] { app, (double)1500000 });
                            if (Result > 0)
                            {
                                // check that its NOT > 2.5
                                Result = rules.ExecuteRule(Messages, "JumboApprove", new object[] { app, (double)2500000 });
                                if (Result <= 0)
                                {
                                    //LogPlugin.Logger.LogInfo("Application :{0} between 1.5 and 2.5", app.Key);
                                    // need an SA, Mgr or Exp
                                    //Dictionary<string, int> Sigs = ActionPerformedBy(InstanceID, "Credit", new string[] { "Supervisor", "Manager", "Exceptions Manager" }, ActionsToCheck);
                                    int nSASigs = 0;
                                    if (Sigs.ContainsKey(1008))
                                        nSASigs += Sigs[1008];
                                    if (Sigs.ContainsKey(580))
                                        nSASigs += Sigs[580];
                                    if (Sigs.ContainsKey(581))
                                        nSASigs += Sigs[581];
                                    if (nSASigs > 0)
                                    {
                                        int TotalSigs = nSASigs;
                                        if (Sigs.ContainsKey(1007))
                                            TotalSigs += Sigs[1007];
                                        if (TotalSigs <= 1)
                                        {
                                            // we need at least 2 sigs, we have the seniority sigs but not the kippee
                                            //IApplicationRole role = osRepo.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(ApplicationKey, l.ApplicationRoleTypes.ObjectDictionary["808"].Key);
                                            bool hasAppRole = GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyFromWorkflowAssignment(InstanceID, 808);
                                            if (!hasAppRole)
                                            {
                                                DeActiveUsersForInstance(InstanceID, ApplicationKey, DynamicRoles);

                                                //this.MarkCreditUsersAsInactive(ApplicationKey);
                                                //string s = RoundRobinAssignForGivenOrgStructure("Credit Underwriter D", ApplicationKey, 1007);
                                                string s = X2RoundRobinForGivenOSKeys("Credit Underwriter D", ApplicationKey, 1007, InstanceID, "");

                                                //LogPlugin.Logger.LogInfo("Application :{0} needs 2 sigs, have senior ones assigning to {1}", app.Key, s);
                                            }
                                            b = true;
                                        }
                                    }
                                    else
                                    {
                                        // we need at least one sig from an exc, mgr or sa give it to the SA they can escalate if need be.
                                        //IApplicationRole role = osRepo.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(ApplicationKey, l.ApplicationRoleTypes.ObjectDictionary["807"].Key);
                                        bool hasAppRole = GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyFromWorkflowAssignment(InstanceID, 807);
                                        if (!hasAppRole)
                                        {
                                            DeActiveUsersForInstance(InstanceID, ApplicationKey, DynamicRoles);

                                            //this.MarkCreditUsersAsInactive(ApplicationKey);
                                            //string s = RoundRobinAssignForGivenOrgStructure("Credit Supervisor D", ApplicationKey, 1008);
                                            string s = X2RoundRobinForGivenOSKeys("Credit Supervisor D", ApplicationKey, 1008, InstanceID, "");

                                            //LogPlugin.Logger.LogInfo("Application :{0} needs an exp,mgr,supervisor sig assigning to {1}", app.Key, s);
                                        }
                                        b = true;
                                    }
                                }
                            }

                            #endregion > 1.5 Mill < 2.5 Mill
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (ActiveRecordException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // odd but we want to let the transaction live on so that the system activity completes
                // on the other side.
                b = true;
                LogPlugin.Logger.LogFormattedErrorWithException("CreditDecisionCheckAuthorisationRules", "Unable to CreditDecisionCheckAuthorisationRules({0}, {1})  AID:{0} IID:{1}", ex,
                    new Dictionary<string, object>() { { "MethodParameters", MethodBase.GetCurrentMethod().GetParameters() } }, ApplicationKey, InstanceID);
            }

            return b;
        }

        /// <summary>
        /// Looks at the workflowassignment records for a given instance id and offerroletypekey where the records are ACTIVE and returns the latest
        /// entry
        /// </summary>
        /// <param name="InstanceID"></param>
        /// <param name="ApplicationRoleTypeKey"></param>
        /// <returns></returns>
        [Obsolete("SEE CreditDecisionCheckAuthorisationRules")]
        private static bool GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyFromWorkflowAssignment(long InstanceID, int ApplicationRoleTypeKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select top 1 id from x2.dbo.vw_WFAssignment where IID = {0} and OfferRoleTypeKey = {1} and GSKey = 1 order by id desc", InstanceID, ApplicationRoleTypeKey);
            object o = AbstractRepositoryBase.ExecuteScalarOnCastleTran(sb.ToString(), typeof(Instance_DAO), null);
            bool ret = Convert.ToInt32(o) > 0 ? true : false;
            return ret;
        }

        #region Mandates

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="InstanceID"></param>
        /// <param name="loadBalanceStates"></param>
        /// <param name="loadBalanceIncludeStates"></param>
        /// <param name="loadBalance1stPass"></param>
        /// <param name="loadBalance2ndPass"></param>
        [Obsolete("DO NOT USE THIS, NEEDS REFACTORING!")]
        public void PerformCreditMandateCheck(int ApplicationKey, Int64 InstanceID, List<string> loadBalanceStates, bool loadBalanceIncludeStates, bool loadBalance1stPass, bool loadBalance2ndPass)
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

            IApplication app = appRepo.GetApplicationByKey(ApplicationKey);
            IApplicationMortgageLoan iaml = app as IApplicationMortgageLoan;
            if (null == iaml)
            {
                Type T = app.GetType();
                throw new Exception(string.Format("Application:{0} is not a Mortgageloan Application, is : {1}", ApplicationKey, T.FullName));
            }

            // get a list of users from the mandates
            List<IADUser> uniqueADUserList = new List<IADUser>();
            List<MandateUser> mandateUsers = GetUsersFromMandate(iaml, "Credit", out uniqueADUserList);

            if (uniqueADUserList.Count > 0)
            {
                #region get the load of the users

                string aduserNameFromLoadBalance = string.Empty;
                int instanceCount = 0;
                int orgStructureKey = -1;
                int offerRoleTypeKey = -1;

                // get a comma delimited list of state ids to determine the load balance
                string stateIDs = "";
                foreach (string sName in loadBalanceStates)
                {
                    SAHL.Common.X2.BusinessModel.Interfaces.IState state = x2Repo.GetStateByName(SAHL.Common.Constants.WorkFlowName.Credit, SAHL.Common.Constants.WorkFlowProcessName.Origination, sName);
                    if (state != null)
                        stateIDs += state.ID + ",";
                }
                stateIDs = stateIDs.TrimEnd(',');

                // get a comma delimited list of aduser ids to determine the load balance
                string userIDs = "";
                foreach (var adUser in uniqueADUserList)
                {
                    userIDs += adUser.Key + ",";
                }
                userIDs = userIDs.TrimEnd(',');

                // run query to return load - returns a datatable of the users and their instance count sorted in ascending order
                // the query will return every user in the list, even if their load is zero
                string sql = SAHL.Common.DataAccess.UIStatementRepository.GetStatement("DomainService.Credit", "CreditLoadBalance");
                ParameterCollection prms = new ParameterCollection();
                prms.Add(new SqlParameter("@IncludeStates", loadBalanceIncludeStates));
                prms.Add(new SqlParameter("@StateIDs", stateIDs));
                prms.Add(new SqlParameter("@FirstPass", loadBalance1stPass));
                prms.Add(new SqlParameter("@SecondPass", loadBalance2ndPass));
                prms.Add(new SqlParameter("@UserIDs", userIDs));

                DataSet dsUserLoad = AbstractRepositoryBase.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), prms);

                #endregion get the load of the users

                // we now have a datatable of manadated users and their load
                if (dsUserLoad != null && dsUserLoad.Tables.Count > 0 && dsUserLoad.Tables[0].Rows.Count > 0)
                {
                    // use the first row as this will be the guy with the least amount of cases
                    aduserNameFromLoadBalance = Convert.ToString(dsUserLoad.Tables[0].Rows[0]["ADUserName"]);
                    instanceCount = Convert.ToInt32(dsUserLoad.Tables[0].Rows[0]["InstanceCount"]);

                    // now that we have the user to assign to, lets go get his org structure and offerroletype to use in assignment
                    // we get this info from our <MandateUser> list
                    // we may have multiple entries for the same user in the mandate list, becuase they are in diff org structures
                    // sort this list by user --> org structure so that we get more junior roles for the user first
                    // we will use the first (lowest in org structure heirarchy) to get our offertroletype
                    MandateUser firstMandatedUser = (from mu in mandateUsers
                                                     where mu.ADUserName.ToLower() == aduserNameFromLoadBalance.ToLower()
                                                     orderby mu.OrganisationStructureKey ascending
                                                     select mu).First();

                    IOrganisationStructure os = osRepo.GetOrganisationStructureForKey(firstMandatedUser.OrganisationStructureKey);

                    // get the offerroletype via the organisationstructure offerroletype mapping
                    // again, if there is more than one then take the most junior
                    var offerRoleType = (from r in os.ApplicationRoleTypes
                                         where r.Key == (int)SAHL.Common.Globals.OfferRoleTypes.CreditExceptionsD // 805
                                         || r.Key == (int)SAHL.Common.Globals.OfferRoleTypes.CreditManagerD // 806
                                         || r.Key == (int)SAHL.Common.Globals.OfferRoleTypes.CreditSupervisorD // 807
                                         || r.Key == (int)SAHL.Common.Globals.OfferRoleTypes.CreditUnderwriterD // 808
                                         orderby r.Key descending
                                         select r).First();

                    orgStructureKey = firstMandatedUser.OrganisationStructureKey;
                    offerRoleTypeKey = offerRoleType.Key;

                    // assign the case to the user
                    ReassignCaseToUser(InstanceID, ApplicationKey, aduserNameFromLoadBalance, orgStructureKey, offerRoleTypeKey, "Assign User");
                }
            }
        }

        /// <summary>
        /// Returns a list of users (and their orgstructurekey) who fulfil a mandate set.
        /// orgstructurekey is required so that we can detetmone the users offerroletype (via the uos) for use in assignment
        /// </summary>
        /// <param name="iaml"></param>
        /// <param name="Dept"></param>
        /// <param name="uniqueADUserList"></param>
        /// <returns></returns>
        [Obsolete("SEE PerformCreditMandateCheck")]
        private List<MandateUser> GetUsersFromMandate(IApplicationMortgageLoan iaml, string Dept, out List<IADUser> uniqueADUserList)
        {
            uniqueADUserList = new List<IADUser>();
            List<MandateUser> mandateUsers = new List<MandateUser>();
            IMandateService mandateService = ServiceFactory.GetService<IMandateService>();

            IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IOrganisationStructure osCredit = osRepo.GetOrganisationStructureForDescription(Dept);

            List<int> osKeys = new List<int>();
            foreach (IOrganisationStructure os in osCredit.ChildOrganisationStructures)
            {
                osKeys.Add(os.Key);
            }

            IReadOnlyEventList<IAllocationMandateSetGroup> mandateGroupList = GetAllocationMandatesForOrgStructureKeys(osKeys);

            foreach (var mandateGroup in mandateGroupList)
            {
                IList<IADUser> users = mandateService.ExecuteMandateSet(mandateGroup.AllocationGroupName, new object[] { iaml });
                if (users.Count > 0)
                {
                    foreach (IADUser usr in users)
                    {
                        MandateUser mandateUser = new MandateUser();
                        mandateUser.ADUserName = usr.ADUserName;
                        mandateUser.OrganisationStructureKey = mandateGroup.OrganisationStructure.Key;

                        // if the user and oskey are not in the list then add them
                        if (!mandateUsers.Contains(mandateUser) && usr.GeneralStatusKey.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                        {
                            mandateUsers.Add(mandateUser);
                        }

                        // add user to unique list if doesnt already exist
                        if (!uniqueADUserList.Contains(usr) && usr.GeneralStatusKey.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                        {
                            uniqueADUserList.Add(usr);
                        }
                    }
                }
            }

            return mandateUsers;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Keys"></param>
        /// <returns></returns>
        [Obsolete("SEE GetUsersFromMandate, PerformCreditMandateCheck")]
        private static IReadOnlyEventList<IAllocationMandateSetGroup> GetAllocationMandatesForOrgStructureKeys(List<int> Keys)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Keys.Count; i++)
            {
                sb.AppendFormat(",{0}", Keys[i]);
            }
            sb.Remove(0, 1);
            string HQL = string.Format("from AllocationMandateSetGroup_DAO d where d.OrganisationStructure.Key in ({0})", sb.ToString());
            Castle.ActiveRecord.Queries.SimpleQuery<AllocationMandateSetGroup_DAO> query = new SimpleQuery<AllocationMandateSetGroup_DAO>(HQL);
            AllocationMandateSetGroup_DAO[] arr = query.Execute();

            DAOEventList<AllocationMandateSetGroup_DAO, IAllocationMandateSetGroup, AllocationMandateSetGroup> result = new DAOEventList<AllocationMandateSetGroup_DAO, IAllocationMandateSetGroup, AllocationMandateSetGroup>(arr);
            return new ReadOnlyEventList<IAllocationMandateSetGroup>(result);
        }

        #endregion Mandates

        public void ReassignToPreviousValuationsUserIfExistsElseRoundRobin(string DynamicRole, int OrgStructKey, int ApplicationKey, string Map, Int64 InstanceID, string State, int RoundRobinPointerKey)
        {
            string ADUserName = string.Empty;

            offerRole.WorkflowAssignment ds = workflowSecurityRepository.GetOfferRoleOrganisationStructure();
            int OfferRoleTypeKey = workflowSecurityRepository.GetOfferRoleRow(DynamicRole).OfferRoleTypeKey;
            offerRole.WorkflowAssignment.WFAssignmentDataTable dt = workflowSecurityRepository.GetLastSecurityRecordForApplicationKeyOrgStuctAndOfferRoleType(ApplicationKey, OrgStructKey, OfferRoleTypeKey, Map);

            int ORTKey = -1;
            foreach (offerRole.WorkflowAssignment.WFAssignmentRow row in dt)
            {
                int OSKey = ds.OfferRoleTypeOrganisationStructureMapping.FindByOfferRoleTypeOrganisationStructureMappingKey(row.BlaKey).OrganisationStructureKey;
                ORTKey = ds.OfferRoleTypeOrganisationStructureMapping.FindByOfferRoleTypeOrganisationStructureMappingKey(row.BlaKey).OfferRoleTypeKey;

                // will have been assigned to many before check we actually using ones we care about
                if (OSKey == OrgStructKey && ORTKey == OfferRoleTypeKey)
                {
                    if (IsUserActive(row.ADUserKey))
                    {
                        //string DynamicRole = ds.OfferRoleType.FindByOfferRoleTypeKey(ORTKey).Description;
                        if (IsUserStillInSameOrgStructureForCaseReassign(row.ADUserKey, DynamicRole, InstanceID))
                        {
                            ADUserName = row.ADUserName;
                            break;
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(ADUserName))
            {
                ReactiveUserOrRoundRobin(DynamicRole, ApplicationKey, OrgStructKey, InstanceID, State, Process.Origination, RoundRobinPointerKey);
            }
            else
            {
                ReassignCaseToUser(InstanceID, ApplicationKey, ADUserName, OrgStructKey, ORTKey, State, Process.Origination);
            }
        }

        /// <summary>
        /// Check if the User is still in the Workflow Role
        /// </summary>
        /// <param name="aDUserName"></param>
        /// <param name="workflowRoleTypeKey"></param>
        /// <returns></returns>
        public bool CheckUserInWorkflowRole(string aDUserName, int workflowRoleTypeKey)
        {
            string sql = string.Format(@"
                select
	                count(*)
                from
	                [2am].[dbo].[WorkflowRoleTypeOrganisationStructureMapping] wrtm (nolock)
                join
	                [2am].[dbo].[UserOrganisationStructure] uos (nolock) on uos.OrganisationStructureKey = wrtm.OrganisationStructureKey
                join
	                [2am].[dbo].[ADUser] au (nolock) on au.ADUserKey = uos.ADUserKey
                where
	                wrtm.WorkflowRoleTypeKey = {0}
                and
	                au.ADUserName = '{1}'", workflowRoleTypeKey, aDUserName);

            object o = castleTransactionService.ExecuteScalarOnCastleTran(sql, Databases.TwoAM, null);
            if (o != null && Convert.ToInt32(o) > 0)
                return true;

            return false;
        }

        /// <summary>
        /// Deactivate the Workflow Role Assignments for the Instance
        /// </summary>
        /// <param name="instanceID"></param>
        public void DeactivateAllWorkflowRoleAssigmentsForInstance(long instanceID)
        {
            string sql = string.Format("update x2.WorkflowRoleAssignment with (rowlock) set GeneralStatusKey=2 where InstanceID={0}", instanceID);
            castleTransactionService.ExecuteNonQueryOnCastleTran(sql, Databases.X2, null);
        }

        public bool IsPolicyOverride(long InstanceID, long SourceInstanceID, int GenericKey)
        {
            bool b = false;

            // Given the sourceid we need to check all the child instances that are in credit to see if any of them have policyoverride set.
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select cr.policyoverride from x2.instance i ");
            sb.AppendFormat("join x2.workflow w on i.WorkflowID=w.id ");
            sb.AppendFormat("join x2.x2data.credit cr on cr.instanceid=i.id ");
            sb.AppendFormat("where w.name='Credit' and i.sourceinstanceid={0} and cr.policyoverride = 1", SourceInstanceID);

            object o = castleTransactionService.ExecuteScalarOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection());

            if (null != o)
                b = true;

            return b;
        }

        public string PolicyOverrideReassignToFirstUserOrRoundRobin(long InstanceID, long SourceInstanceID, int GenericKey, string State, SAHL.Common.Globals.Process pName)
        {
            string AssignedUser = string.Empty;

            // We're only in here because the PolicyOverride flag is set in the X2DATA.Credit table
            // We need to assign the case to the first person who worked on the case in Credit
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select top 1 wfa.* from x2.instance i (nolock) ");
            sb.AppendFormat("join x2.workflow w (nolock) on i.WorkflowID=w.id ");
            sb.AppendFormat("join vw_WFAssignment wfa (nolock) on i.id=wfa.IID ");
            sb.AppendFormat("where w.name='Credit' and i.sourceinstanceid={0} and wfa.OfferRoletypeKey != 805 order by wfa.id asc", SourceInstanceID);

            offerRole.WorkflowAssignment.WFAssignmentDataTable dt = new offerRole.WorkflowAssignment.WFAssignmentDataTable();
            castleTransactionService.FillDataTableFromQueryOnCastleTran(dt, sb.ToString(), Databases.X2, null);

            // We're only interested in one user here.
            int PrevADUserKey = -1;
            int PrevOfferRoleTypeKey = -1;
            int PrevBlaKey = -1;
            string PrevDynamicRole = string.Empty;
            int PrevOSKey = -1;
            int RoundRobinPointerKey = 0;
            if (dt.Rows.Count > 0)
            {
                offerRole.WorkflowAssignment dsOS = workflowSecurityRepository.GetOfferRoleOrganisationStructure();
                foreach (offerRole.WorkflowAssignment.WFAssignmentRow row in dt.Rows)
                {
                    PrevADUserKey = row.ADUserKey;
                    PrevBlaKey = row.BlaKey;
                    PrevOfferRoleTypeKey = row.OfferRoleTypeKey;
                    PrevDynamicRole = row.ORT;
                    PrevOSKey = row.OrganisationStructureKey;
                    int ORTKey = dsOS.OfferRoleTypeOrganisationStructureMapping.FindByOfferRoleTypeOrganisationStructureMappingKey(PrevBlaKey).OfferRoleTypeKey;
                    string DynamicRole = dsOS.OfferRoleType.FindByOfferRoleTypeKey(ORTKey).Description;

                    //Set the Round Robin Pointer Key for the particular Bla Key (OfferRoleTypeOrganisationStructureMappingKey)
                    //I was not happy when I did this, hardcoding values is not fun. Because of the design I was told to do this

                    #region Hardcoding

                    switch (row.BlaKey)
                    {
                        //Credit Underwriter
                        case 80:
                            RoundRobinPointerKey = (int)RoundRobinPointers.CreditUnderwritter;
                            break;

                        //Credit Exceptions
                        case 130:
                            RoundRobinPointerKey = (int)RoundRobinPointers.CreditExceptions;
                            break;

                        //Credit Manager
                        case 131:
                            RoundRobinPointerKey = (int)RoundRobinPointers.CreditManager;
                            break;

                        case 132:
                            RoundRobinPointerKey = (int)RoundRobinPointers.CreditSupervisor;
                            break;
                    }

                    #endregion Hardcoding

                    if (IsUserActive(PrevADUserKey))
                    {
                        if (IsUserStillInSameOrgStructureForCaseReassign(PrevADUserKey, DynamicRole, InstanceID))
                        {
                            workflowSecurityRepository.AssignWorkflowRole(InstanceID, PrevADUserKey, PrevBlaKey, State);
                            if (SAHL.Common.Globals.Process.Origination == pName)
                            {
                                int LEKey = dsOS.ADUser.FindByADUserKey(PrevADUserKey).LegalEntityKey;
                                workflowSecurityRepository.Assign2AMOfferRole(GenericKey, ORTKey, LEKey);
                                AssignedUser = row.ADUserName;
                                break;
                            }
                        }
                        else
                        {
                            // RR
                            //AssignedUser = X2RoundRobinForGivenOSKeys(Tran, PrevDynamicRole, GenericKey, PrevOSKey, InstanceID, State, pName);
                            AssignedUser = X2RoundRobinForPointerDescription(InstanceID, RoundRobinPointerKey, GenericKey, DynamicRole, State, pName);
                        }
                    }
                    else
                    {
                        // RR
                        //AssignedUser = X2RoundRobinForGivenOSKeys(Tran, PrevDynamicRole, GenericKey, PrevOSKey, InstanceID, State, pName);
                        AssignedUser = X2RoundRobinForPointerDescription(InstanceID, RoundRobinPointerKey, GenericKey, DynamicRole, State, pName);
                    }
                }
            }
            return AssignedUser;
        }

        public string AssignCaseThatWasPreviouslyInDisputeIndicated(int offerKey, long instanceID)
        {
            string userAssignedTo = String.Empty;

            /*Given the Instance,
             * Check to see if the case was in the Archive Dispute state before it came into Credit Again
             * If it was in that state, get the Workflow Assignment Row so that we may re assign the case to the user who performed the Dispute Indicated Action
             */

            //Get the Parent Instance for the Case
            long sourceInstanceID = workflowSecurityRepository.GetSourceInstanceFromInstanceID(instanceID);
            int RoundRobinPointerKey = 0;

            //Using the Parent Instance, find all the children in order
            //Where the instance is not this instance
            //So that the top one would be the previous case
            DataTable instanceInPreviousStateTable = workflowSecurityRepository.GetWorkflowInstanceInPreviousState(sourceInstanceID, instanceID, "Archive Disputes");
            if (instanceInPreviousStateTable.Rows.Count == 0)
            {
                return userAssignedTo;
            }

            //Get the Instance ID to use
            int instanceIDToQuery = -1;
            if (instanceInPreviousStateTable != null &&
                instanceInPreviousStateTable.Rows.Count > 0 &&
                instanceInPreviousStateTable.Columns.Contains("ID"))
            {
                instanceIDToQuery = int.Parse(instanceInPreviousStateTable.Rows[0]["ID"].ToString());
            }

            //Using this ID, make get the user that performed the 'Dispute Indicated' action
            DataTable instanceForDisputedActionTable = workflowSecurityRepository.GetWorkflowHistoryForActivityByInstance(instanceIDToQuery, "Dispute Indicated");
            string adUserNameToAssignCaseTo = String.Empty;
            if (instanceForDisputedActionTable != null &&
                instanceForDisputedActionTable.Rows.Count > 0 &&
                instanceForDisputedActionTable.Columns.Contains("ADUserName"))
            {
                adUserNameToAssignCaseTo = instanceForDisputedActionTable.Rows[0]["ADUserName"].ToString();
            }

            //Get the ADUser Key to assign the case to from the AD Username
            int adUserKeyToAssignCaseTo = workflowSecurityRepository.GetADUserRowByName(adUserNameToAssignCaseTo).ADUserKey;

            // Get the AD Users LegalEntity Key to assign the case to
            int legalEntityKeyToAssignCaseTo = workflowSecurityRepository.GetADUserRowByName(adUserNameToAssignCaseTo).LegalEntityKey;

            //Get the Latest Workflow Assignment for the user that performed the Dispute Indicated action
            //We are going to use this detail to assign the user to the case
            DataTable workflowAssignmentToAssignToTable = workflowSecurityRepository.GetLatestWorkflowAssignmentForADUserKeyAndInstance(instanceIDToQuery, adUserKeyToAssignCaseTo);

            int blaKey = -1;
            if (workflowAssignmentToAssignToTable != null &&
                workflowAssignmentToAssignToTable.Rows.Count > 0 &&
                workflowAssignmentToAssignToTable.Columns.Contains("OfferRoleTypeOrganisationStructureMappingKey"))
            {
                blaKey = int.Parse(workflowAssignmentToAssignToTable.Rows[0]["OfferRoleTypeOrganisationStructureMappingKey"].ToString());
            }

            //Check if the user is active and still in the organization structure mapping
            offerRole.WorkflowAssignment dsOS = workflowSecurityRepository.GetOfferRoleOrganisationStructure();
            offerRole.WorkflowAssignment.OfferRoleTypeOrganisationStructureMappingRow blaRow = dsOS.OfferRoleTypeOrganisationStructureMapping.FindByOfferRoleTypeOrganisationStructureMappingKey(blaKey);
            offerRole.WorkflowAssignment.OfferRoleTypeRow offerRoleTypeRow = null;
            if (blaRow != null)
            {
                offerRoleTypeRow = blaRow.OfferRoleTypeRow;
            }

            //This is to prevent that the user that was assigned the case is not the user who performed the action,
            //which means that he will not a workflow assignment record in the WorkflowAssignment table
            if (offerRoleTypeRow == null)
            {
                return string.Empty;
            }

            //Ensure that the user who we want to assign to is active and the user is still in the same organization structure
            bool userIsActive = IsUserActive(adUserKeyToAssignCaseTo);

            if (userIsActive && IsUserStillInSameOrgStructureForCaseReassign(adUserKeyToAssignCaseTo, offerRoleTypeRow.Description, instanceIDToQuery))
            {
                //Only if the person has been assigned the case do we set the user assigned to
                userAssignedTo = adUserNameToAssignCaseTo;

                //Perform the assignment
                workflowSecurityRepository.AssignWorkflowRole(instanceID, adUserKeyToAssignCaseTo, blaKey, "Dispute Indicated Assign");

                // insert or reactivate the 2am offerrole
                if (legalEntityKeyToAssignCaseTo > 0)
                {
                    workflowSecurityRepository.Assign2AMOfferRole(offerKey, offerRoleTypeRow.OfferRoleTypeKey, legalEntityKeyToAssignCaseTo);
                }

                return userAssignedTo;
            }

            //Only if the user is not active, do the round robin to a credit underwriter
            else
            {
                //I was not happy when I did this, hardcoding values is not fun. Because of the design I was told to do this
                switch (blaKey)
                {
                    //Credit Underwriter
                    case 80:
                        RoundRobinPointerKey = (int)RoundRobinPointers.CreditUnderwritter;
                        break;

                    //Credit Exceptions
                    case 130:
                        RoundRobinPointerKey = (int)RoundRobinPointers.CreditExceptions;
                        break;

                    //Credit Manager
                    case 131:
                        RoundRobinPointerKey = (int)RoundRobinPointers.CreditManager;
                        break;

                    //Credit Manager
                    case 132:
                        RoundRobinPointerKey = (int)RoundRobinPointers.CreditSupervisor;
                        break;
                }

                //Credit Underwriter (808)
                //Organisation Structure Key (1007) //Credit Underwriter
                //userAssignedTo = X2RoundRobinForGivenOSKeys(tran, offerRoleTypeRow.Description, offerKey, 1007, instanceID, "Dispute Indicated Assign", OriginationClient.ProcessName.Origination);
                userAssignedTo = X2RoundRobinForPointerDescription(instanceID, RoundRobinPointerKey, offerKey, offerRoleTypeRow.Description, "Dispute Indicated Assign", SAHL.Common.Globals.Process.Origination);

                return userAssignedTo;
            }
        }

        public void GetFirstAssignedCreditUser(long SourceInstanceID, out string adUserName, out int offerRoleTypeKey, out int orgStructureKey)
        {
            adUserName = string.Empty;
            offerRoleTypeKey = -1;
            orgStructureKey = -1;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select top 1 wfa.* from x2.instance i (nolock) ");
            sb.AppendFormat("join x2.workflow w (nolock) on i.WorkflowID=w.id ");
            sb.AppendFormat("join vw_WFAssignment wfa (nolock) on i.id=wfa.IID ");
            sb.AppendFormat("where w.name='Credit' and i.SourceInstanceID={0} order by wfa.id asc", SourceInstanceID);

            offerRole.WorkflowAssignment.WFAssignmentDataTable dt = new offerRole.WorkflowAssignment.WFAssignmentDataTable();
            castleTransactionService.FillDataTableFromQueryOnCastleTran(dt, sb.ToString(), Databases.X2, null);

            if (dt.Rows.Count > 0)
            {
                offerRole.WorkflowAssignment.WFAssignmentRow row = dt.Rows[0] as offerRole.WorkflowAssignment.WFAssignmentRow;
                if (!row.IsADUserNameNull())
                    adUserName = row.ADUserName;
                offerRoleTypeKey = row.OfferRoleTypeKey;
                orgStructureKey = row.OrganisationStructureKey;
            }
        }

        public string ReActivateBranchUsersForOrigination(long AppManIID, long AppCapIID, int ApplicationKey, string State, Process pName)
        {
            string assignedUsers = "";

            #region Branch Consultant D

            string DynamicRole = "Branch Consultant D";

            // look to see if we have a security record if so reassign to them
            offerRole.WorkflowAssignment ds = workflowSecurityRepository.GetWFAssignment(ApplicationKey, DynamicRole, AppManIID);
            if (ds.WFAssignment.Rows.Count > 0)
            {
                int OSKey = ((offerRole.WorkflowAssignment.WFAssignmentRow)ds.WFAssignment.Rows[0]).OrganisationStructureKey;

                // we have a role alread so reactivate it
                //ReactiveUserOrRoundRobin(Tran, DynamicRole, ApplicationKey, OSKey, AppManIID, State, pName);
                assignedUsers += " " + ReactivateLastUserToWorkOnCaseIfValid(DynamicRole, ApplicationKey, OSKey, AppManIID, State);

                //assignedUsers = ret.Data.ToString();
            }
            else
            {
                // we havnt assigned the case in this map before so go look for one in app capture
                ds = workflowSecurityRepository.GetWFAssignment(ApplicationKey, DynamicRole, AppCapIID);
                if (ds.WFAssignment.Rows.Count > 0)
                {
                    // we have a role in the leads (App Cap) alread so use it to generate one in this map
                    offerRole.WorkflowAssignment.WFAssignmentRow row = (offerRole.WorkflowAssignment.WFAssignmentRow)ds.WFAssignment.Rows[0];
                    int OSKey = row.OrganisationStructureKey;
                    string ADUser = ((offerRole.WorkflowAssignment.WFAssignmentRow)ds.WFAssignment.Rows[0]).ADUserName;
                    if ((IsUserStillInSameOrgStructureForCaseReassign(row.ADUserKey, DynamicRole, AppCapIID))
                        && IsUserActive(row.ADUserKey))// note I pass in appcapiid for the check!
                    {
                        ReassignCaseToUser(AppManIID, ApplicationKey, ADUser, OSKey, 101, State, pName);
                        assignedUsers = ADUser;
                    }
                    else
                    {
                        //ReactiveUserOrRoundRobin(Tran, DynamicRole, ApplicationKey, OSKey, AppManIID, State, pName);
                        int ManagerOSKey = GetBranchManagerOrgStructureKey(AppManIID);
                        string assignedManager = AssignBranchManagerForOrgStrucKey(AppManIID, "Branch Manager D", ManagerOSKey, ApplicationKey, "ReActivateBranchUsersForOrigination", pName);
                        assignedUsers = assignedManager;
                    }
                }

                //else
                //{
                //    // We should never get here if we have a valid case.
                //    LogPlugin.Logger.LogErrorForSource(string.Format("No security found for case in AppMan {0} or AppCap {1} cant assign branch users", AppManIID, AppCapIID), "SEC");
                //}
            }

            #endregion Branch Consultant D

            #region Branch Admin D

            DynamicRole = "Branch Admin D";

            // look to see if we have a security record if so reassign to them
            ds = workflowSecurityRepository.GetWFAssignment(ApplicationKey, DynamicRole, AppManIID);
            if (ds.WFAssignment.Rows.Count > 0)
            {
                int OSKey = ((offerRole.WorkflowAssignment.WFAssignmentRow)ds.WFAssignment.Rows[0]).OrganisationStructureKey;

                // we have a role alread so reactivate it
                //ReactiveUserOrRoundRobin(Tran, DynamicRole, ApplicationKey, OSKey, AppManIID, State, pName);
                assignedUsers += " " + ReactivateLastUserToWorkOnCaseIfValid(DynamicRole, ApplicationKey, OSKey, AppManIID, State);
            }
            else
            {
                // we havnt assigned the case in this map before so go look for one in app capture
                ds = workflowSecurityRepository.GetWFAssignment(ApplicationKey, DynamicRole, AppCapIID);
                if (ds.WFAssignment.Rows.Count > 0)
                {
                    // we have a role in the leads (App Cap) alread so use it to generate one in this map
                    offerRole.WorkflowAssignment.WFAssignmentRow row = (offerRole.WorkflowAssignment.WFAssignmentRow)ds.WFAssignment.Rows[0];
                    int OSKey = row.OrganisationStructureKey;
                    string ADUser = ((offerRole.WorkflowAssignment.WFAssignmentRow)ds.WFAssignment.Rows[0]).ADUserName;
                    if ((IsUserStillInSameOrgStructureForCaseReassign(row.ADUserKey, DynamicRole, AppCapIID))
                        && IsUserActive(row.ADUserKey))// note I pass in appcapiid for the check!
                    {
                        ReassignCaseToUser(AppManIID, ApplicationKey, ADUser, OSKey, 102, State, pName);
                        assignedUsers += " " + ADUser;
                    }
                    else
                    {
                        //ReactiveUserOrRoundRobin(Tran, DynamicRole, ApplicationKey, OSKey, AppManIID, State, pName);
                        int ManagerOSKey = GetBranchManagerOrgStructureKey(AppManIID);
                        string assignedManager = AssignBranchManagerForOrgStrucKey(AppManIID, "Branch Manager D", ManagerOSKey, ApplicationKey, "ReActivateBranchUsersForOrigination", pName);
                        assignedUsers += " " + assignedManager;
                    }
                }
                else
                {
                    //// we can get here, just means that we never had an admin on this case
                    //String BP = "Here";
                }
            }

            #endregion Branch Admin D

            return assignedUsers;
        }

        public string ReactivateLastUserToWorkOnCaseIfValid(string DynamicRole, int GenericKey, int OSKey, long InstanceID, string State)
        {
            string AssignedUser = string.Empty;

            // Get the existing record for this case.
            offerRole.WorkflowAssignment ds = workflowSecurityRepository.GetWFAssignment(GenericKey, DynamicRole, InstanceID);
            if (ds.WFAssignment.Rows.Count > 0)
            {
                offerRole.WorkflowAssignment.WFAssignmentRow row = (offerRole.WorkflowAssignment.WFAssignmentRow)ds.WFAssignment.Rows[0];
                if (IsUserStillInSameOrgStructureForCaseReassign(row.ADUserKey, DynamicRole, InstanceID))
                {
                    if (IsUserActive(row.ADUserKey))
                    {
                        AssignedUser = row.ADUserName;

                        // reactivate also make sure the offerrole record is reactivated
                        workflowSecurityRepository.AssignWorkflowRole(InstanceID, row.ADUserKey, row.BlaKey, State);

                        //  int LEKey = workflowSecurityRepository.LoadOrgStructureInfo().ADUser.FindByADUserKey(row.ADUserKey).LegalEntityKey;

                        int LEKey = workflowSecurityRepository.GetOfferRoleOrganisationStructure().ADUser.FindByADUserKey(row.ADUserKey).LegalEntityKey;

                        int ORTKey = workflowSecurityRepository.GetOfferRoleRow(DynamicRole).OfferRoleTypeKey;

                        //for Offerrole we need LEKey
                        workflowSecurityRepository.Assign2AMOfferRole(GenericKey, ORTKey, LEKey);
                    }
                    else
                    {
                        //#if DEBUG
                        //LogPlugin.Logger.LogInfo("Cant reactivate:{0} for {1} as user not active", InstanceID, DynamicRole);
                        //#endif
                    }
                }
                else
                {
                    //#if DEBUG
                    //LogPlugin.Logger.LogInfo("Cant reactivate:{0} for {1} as user not in same OrgStuct", InstanceID, DynamicRole);
                    //#endif
                }
            }
            return AssignedUser;
        }

        public string GetUserWhoWorkedOnThisLegalEntitysOtherCasesForDynamicRole(int OfferRoleTypeKey, int ApplicationKey, long InstanceID)
        {
            string ADUserName = string.Empty;
            offerRole.WorkflowAssignment ds = workflowSecurityRepository.GetOfferRoleOrganisationStructure();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select ad.AdUserName,ad.ADUserKey, orr.OfferRoleTypeKey, ad.generalStatusKey ");
            sb.AppendLine("from [2am]..OfferRole orr (nolock) ");
            sb.AppendLine("join  ");
            sb.AppendLine("( ");
            sb.AppendLine("select distinct(orr1.offerkey) ");
            sb.AppendLine("from [2am]..offerrole orr (nolock) ");
            sb.AppendLine("join [2am]..offerrole orr1 (nolock) on orr.legalentitykey=orr1.legalentitykey  ");
            sb.AppendLine("and orr1.offerroletypekey in (8,10,11,12) ");

            //Changed to filter on applications that have not been disbursed
            //----------------------------------------------------------------
            sb.AppendLine("join [2am]..offer ofr1 (nolock) on ofr1.offerkey=orr1.offerkey and ofr1.offerstatuskey in (1,4,5)");
            sb.AppendLine("left outer join [2am]..stagetransitioncomposite stc1 (nolock) on stc1.generickey=ofr1.offerkey and stc1.StageDefinitionStageDefinitionGroupKey in (110,111) ");

            //---------------------------------------------------------------
            sb.AppendFormat("where orr.offerkey={0} ", ApplicationKey);

            //Ignore the offer than we are dealing with
            //-----------------------------------------
            sb.AppendFormat(" and orr1.offerkey<>{0} ", ApplicationKey);

            //Changed to filter on applications that have not been disbursed
            //------------------------------
            sb.AppendLine(" and stc1.stagetransitioncompositekey is null ");

            //------------------------------
            sb.AppendLine(") OL--OfferList ");
            sb.AppendLine("on OL.OfferKey=orr.OfferKey ");
            sb.AppendLine("join [2am]..LegalEntity le (nolock) on orr.legalentitykey=le.legalentitykey ");
            sb.AppendLine("join [2am]..AdUser ad (nolock) on le.legalentitykey=ad.legalentitykey and ad.GeneralStatusKey=1 ");
            sb.AppendFormat("Where orr.OfferRoleTYpeKey={0} ", OfferRoleTypeKey);
            sb.AppendLine("order by orr.StatusChangeDate desc");

            DataTable dt = new DataTable();

            DataSet dataset = new DataSet();
            castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.TwoAM, null, new StringCollection { "DATA" }, dataset);
            dt = dataset.Tables["DATA"];

            //WorkerHelper.FillFromQuery(dt, sb.ToString(), Tran.Context, new ParameterCollection());

            if (dt.Rows.Count > 0)
            {
                if (IsUserActive(Convert.ToInt32(dt.Rows[0]["ADUserKey"])))
                {
                    string DynamicRole = ds.OfferRoleType.FindByOfferRoleTypeKey(OfferRoleTypeKey).Description;
                    if (IsUserStillInSameOrgStructureForCaseReassign(Convert.ToInt32(dt.Rows[0]["ADUserKey"]), DynamicRole, InstanceID))
                    {
                        // ADUserName = row.ADUserName;
                        ADUserName = dt.Rows[0]["ADUserName"].ToString();
                    }
                }
            }
            return ADUserName;
        }

        public string HandleApplicationManagamentRolesOnReturnFromNTUtoPreviousState(long InstanceID, int ApplicationKey, string PreNTUState, bool IsFL, long AppCapIID, Process pName)
        {
            string AssignedUser = string.Empty;
            string State = "NTU";
            List<string> DynamicRoles = new List<string>();
            DynamicRoles.Add("Branch Consultant D");
            DynamicRoles.Add("Branch Admin D");
            DynamicRoles.Add("QA Administrator D");
            DynamicRoles.Add("New Business Processor D");
            DynamicRoles.Add("New Business Supervisor D");
            DynamicRoles.Add("FL Processor D");
            DynamicRoles.Add("FL Supervisor D");
            DynamicRoles.Add("FL Manager D");

            // deactive the users at NTU
            this.DeActiveUsersForInstance(InstanceID, ApplicationKey, DynamicRoles, Process.Origination);

            // Based on where it was go and reinstate the security
            // States that you can NTU from and whose WL they will be on
            // StateName            ISFL                    !ISFL
            //------------------------------------------------------------
            //*Manage Application    FL A D                  NBP D
            //*LOA                   N/A                     BC&BA D
            //QA                    FL A D                  QA A D
            //*Request at QA         FL A D                  BC & BA D
            //*Issue AIP             N/A                     BC & BA D
            //*Application Query     FL A D                  BC & BA D
            //Awaiting Application  Static Role Ignore      N/A
            //*Further Info Request  FL A D                  NBP D
            //*Disputes              FL A D                  BC&BA D
            //Application Hold      Static Ignore           N/A
            //Signed LOA Review     Static Ignore           Static Ignore
            //Application Check     Static Ignore           Static Ignore

            // we can arrive at NTU very easily the trick is getting back the security from where we were
            // so we need to look at the previous state and based on that assign the correct security roles
            switch (PreNTUState.ToUpper())
            {
                case "MANAGE APPLICATION":
                case "FURTHER INFO REQUEST":
                    {
                        if (IsFL)
                        {
                            AssignedUser = ReactiveUserOrRoundRobin("FL Processor D", ApplicationKey, 157, InstanceID, State, Process.Origination, (int)RoundRobinPointers.FLProcessor);
                        }
                        else
                        {
                            // NBP
                            AssignedUser = ReactiveUserOrRoundRobin("New Business Processor D", ApplicationKey, 106, InstanceID, State, Process.Origination, (int)RoundRobinPointers.NewBusinessProcessor);
                        }
                        break;
                    }
                case "LOA":
                case "ISSUE AIP":
                    {
                        if (!IsFL)
                        {
                            // Assign the Branch folks back in
                            //int ManagerOSKey = GetBranchManagerOrgStructureKey(Tran, InstanceID);
                            //AssignedUser = AssignBranchManagerForOrgStrucKey(Tran, InstanceID, "Branch Manager D", ManagerOSKey, ApplicationKey, "Manager Review", pName);

                            AssignedUser = ReActivateBranchUsersForOrigination(InstanceID, AppCapIID, ApplicationKey, "Return from NTU", pName);

                            //int BranchOSKey = GetLastBranchOSKeyBasedOnLastBranchConsultantRole(Tran, InstanceID, AppCapIID);
                            //AssignedUser = ReactivateLastUserToWorkOnCaseIfValid(Tran, "Branch Manager D", ApplicationKey, BranchOSKey, InstanceID, State).Data.ToString();
                            //AssignedUser = ReactiveUserOrRoundRobin(Tran, "Branch Consultant D", ApplicationKey, BranchOSKey, InstanceID, State, ProcessName.Origination).Data.ToString();
                        }
                        break;
                    }
                case "DISPUTES":
                case "APPLICATION QUERY":
                case "REQUEST AT QA":
                    {
                        if (IsFL)
                        {
                            AssignedUser = ReactiveUserOrRoundRobin("FL Processor D", ApplicationKey, 157, InstanceID, State, Process.Origination, (int)RoundRobinPointers.FLProcessor);
                        }
                        else
                        {
                            // Assign the Branch folks back in
                            //int ManagerOSKey = GetBranchManagerOrgStructureKey(Tran, InstanceID);
                            //AssignedUser = AssignBranchManagerForOrgStrucKey(Tran, InstanceID, "Branch Manager D", ManagerOSKey, ApplicationKey, "Manager Review", pName);

                            AssignedUser = ReActivateBranchUsersForOrigination(InstanceID, AppCapIID, ApplicationKey, "Return from NTU", pName);

                            //int BranchOSKey = GetLastBranchOSKeyBasedOnLastBranchConsultantRole(Tran, InstanceID, AppCapIID);
                            //AssignedUser = ReactivateLastUserToWorkOnCaseIfValid(Tran, "Branch Manager D", ApplicationKey, BranchOSKey, InstanceID, State).Data.ToString();
                            //AssignedUser = ReactiveUserOrRoundRobin(Tran, "Branch Consultant D", ApplicationKey, BranchOSKey, InstanceID, State, ProcessName.Origination).Data.ToString();
                        }
                        break;
                    }
                case "QA":
                    {
                        if (IsFL)
                        {
                            // FL
                            AssignedUser = ReactiveUserOrRoundRobin("FL Processor D", ApplicationKey, 157, InstanceID, State, Process.Origination, (int)RoundRobinPointers.FLProcessor);
                        }
                        else
                        {
                            // QA A D
                            List<int> OSKeys = new List<int>();
                            OSKeys.Add(1007);
                            OSKeys.Add(1008);
                            AssignedUser = ReactiveUserOrRoundRobin("QA Administrator D", ApplicationKey, OSKeys, InstanceID, State, Process.Origination, (int)RoundRobinPointers.QAAdministrator);
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return AssignedUser;
        }

        public bool CheckUserInMandate(int applicationKey, string aduserName, string orgStructureName)
        {
            bool userInMandate = false;

            // get the application
            IApplication app = applicationRepo.GetApplicationByKey(applicationKey);
            IApplicationMortgageLoan iaml = app as IApplicationMortgageLoan;
            if (null == iaml)
            {
                Type T = app.GetType();
                throw new Exception(string.Format("Application:{0} is not a Mortgageloan Application, is : {1}", applicationKey, T.FullName));
            }

            // get the list of users in the mandate
            List<IADUser> uniqueADUserList = new List<IADUser>();
            GetUsersFromMandate(iaml, orgStructureName, out uniqueADUserList);

            // check if our user is in the mandated userlist
            var user = (from u in uniqueADUserList
                        where u.ADUserName.ToLower() == aduserName.ToLower()
                        select u).FirstOrDefault();

            if (user != null)
                userInMandate = true;

            return userInMandate;
        }

        public string ReassignToMostSenPersonWhoWorkedOnThisCaseInCredit(long InstanceID, long SourceInstanceID, int OfferKey, string State)
        {
            string AssignedUser = string.Empty;

            // Use this InstanceID to get the SourceInstanceID
            // Then get the other child cases in credit
            // Look at who they have been worked on by
            // Get most sen person (assuming he still is valid)
            // Assign case to that person
            // excluse 805 which is exceptions manager
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select wfa.* from x2.instance i ");
            sb.AppendFormat("join x2.workflow w on i.WorkflowID=w.id ");
            sb.AppendFormat("join vw_WFAssignment wfa on i.id=wfa.IID ");
            sb.AppendFormat("where w.name='Credit' and i.sourceinstanceid={0} and wfa.OfferRoletypeKey != 805 order by wfa.OfferRoleTypeKey asc, wfa.id desc", SourceInstanceID);

            // Get list of cases
            offerRole.WorkflowAssignment.WFAssignmentDataTable dt = new offerRole.WorkflowAssignment.WFAssignmentDataTable();
            castleTransactionService.FillDataTableFromQueryOnCastleTran(dt, sb.ToString(), Databases.X2, null);

            // loop over the last instance (ie the prev visit to credit) and get a list of the sen ppl
            if (dt.Rows.Count > 0)
            {
                Int64 tIID = ((offerRole.WorkflowAssignment.WFAssignmentRow)dt.Rows[0]).IID;

                offerRole.WorkflowAssignment ds = workflowSecurityRepository.GetOfferRoleOrganisationStructure();
                foreach (offerRole.WorkflowAssignment.WFAssignmentRow row in dt.Rows)
                {
                    // check if user still active
                    if (IsUserActive(row.ADUserKey))
                    {
                        int ORTKey = ds.OfferRoleTypeOrganisationStructureMapping.FindByOfferRoleTypeOrganisationStructureMappingKey(row.BlaKey).OfferRoleTypeKey;
                        string DynamicRole = ds.OfferRoleType.FindByOfferRoleTypeKey(ORTKey).Description;

                        // check if user still in same org structure
                        if (IsUserStillInSameOrgStructureForCaseReassign(row.ADUserKey, DynamicRole, InstanceID))
                        {
                            // check if user is a credit manager or still exists in mandate
                            if (ORTKey == (int)SAHL.Common.Globals.OfferRoleTypes.CreditManagerD || CheckUserInMandate(OfferKey, row.ADUserName, "Credit"))
                            {
                                workflowSecurityRepository.AssignWorkflowRole(InstanceID, row.ADUserKey, row.BlaKey, State);
                                int LEKey = ds.ADUser.FindByADUserKey(row.ADUserKey).LegalEntityKey;
                                workflowSecurityRepository.Assign2AMOfferRole(OfferKey, ORTKey, LEKey);
                                AssignedUser = row.ADUserName;
                                break;
                            }
                        }
                    }
                }
            }

            return AssignedUser;
        }

        public string GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole(int OfferRoleTypeKey, int ApplicationKey, long InstanceID)
        {
            string ADUserName = string.Empty;
            offerRole.WorkflowAssignment ds = workflowSecurityRepository.GetOfferRoleOrganisationStructure();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select ad.AdUserName,ad.ADUserKey, orr.OfferRoleTypeKey, ad.generalStatusKey ");
            sb.AppendLine("from [2am]..OfferRole orr (nolock) ");
            sb.AppendLine("join  ");
            sb.AppendLine("( ");
            sb.AppendLine("select distinct(orr1.offerkey) ");
            sb.AppendLine("from [2am]..offerrole orr (nolock) ");
            sb.AppendLine("join [2am]..offerrole orr1 (nolock) on orr.legalentitykey=orr1.legalentitykey  ");
            sb.AppendLine("and orr1.offerroletypekey in (8,10,11,12) ");

            //Changed to filter on applications that have not been disbursed
            //----------------------------------------------------------------
            sb.AppendLine("join [2am]..offer ofr1 (nolock) on ofr1.offerkey=orr1.offerkey and ofr1.offerstatuskey in (1,4,5)");
            sb.AppendLine("left outer join [2am]..stagetransitioncomposite stc1 (nolock) on stc1.generickey=ofr1.offerkey and stc1.StageDefinitionStageDefinitionGroupKey in (110,111) ");

            //---------------------------------------------------------------
            sb.AppendFormat("where orr.offerkey={0} ", ApplicationKey);

            //Ignore the offer than we are dealing with
            //-----------------------------------------
            sb.AppendFormat(" and orr1.offerkey<>{0} ", ApplicationKey);

            //Changed to filter on applications that have not been disbursed
            //------------------------------
            sb.AppendLine(" and stc1.stagetransitioncompositekey is null ");

            //------------------------------
            sb.AppendLine(") OL--OfferList ");
            sb.AppendLine("on OL.OfferKey=orr.OfferKey ");
            sb.AppendLine("join [2am]..LegalEntity le (nolock) on orr.legalentitykey=le.legalentitykey ");
            sb.AppendLine("join [2am]..AdUser ad (nolock) on le.legalentitykey=ad.legalentitykey and ad.GeneralStatusKey=1 ");
            sb.AppendFormat("Where orr.OfferRoleTYpeKey={0} ", OfferRoleTypeKey);
            sb.AppendLine("order by orr.StatusChangeDate desc");

            DataSet dataset = new DataSet();
            castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.TwoAM, null, new StringCollection { "DATA" }, dataset);
            DataTable dt = dataset.Tables["DATA"];

            if (dt.Rows.Count > 0)
            {
                if (IsUserActive(Convert.ToInt32(dt.Rows[0]["ADUserKey"])))
                {
                    string DynamicRole = ds.OfferRoleType.FindByOfferRoleTypeKey(OfferRoleTypeKey).Description;
                    if (IsUserStillInSameOrgStructureForCaseReassign(Convert.ToInt32(dt.Rows[0]["ADUserKey"]), DynamicRole, InstanceID))
                    {
                        // ADUserName = row.ADUserName;
                        ADUserName = dt.Rows[0]["ADUserName"].ToString();
                    }
                }
            }
            return ADUserName;
        }

        public bool IsUserInOrganisationStructureRole(string adUserName, IList<OfferRoleTypes> offerRoleTypes)
        {
            bool userInRole = false;

            // build a list of the offerroletype keys
            string keyList = "";
            for (int i = 0; i < offerRoleTypes.Count; i++)
            {
                int key = (int)offerRoleTypes[i];
                if (i == 0)
                    keyList = key.ToString();
                else
                    keyList += "," + key;
            }

            StringBuilder sql = new StringBuilder();
            sql.Append("select top 1 * from [2am].dbo.OfferRoleTypeOrganisationStructureMapping ortosm (nolock) ");
            sql.Append("join [2am].dbo.UserOrganisationStructure uos (nolock) on uos.OrganisationStructureKey = ortosm.OrganisationStructureKey ");
            sql.Append("join [2am].dbo.ADUser au (nolock) on au.ADUserKey = uos.ADUserKey ");
            sql.AppendFormat("where ortosm.OfferRoleTypeKey in {0} and au.ADUserName = '{1}'", "(" + keyList + ")", adUserName);

            DataTable dt = new DataTable();

            DataSet dataset = new DataSet();
            castleTransactionService.ExecuteQueryOnCastleTran(sql.ToString(), Databases.TwoAM, null, new StringCollection { "DATA" }, dataset);
            dt = dataset.Tables["DATA"];

            //WorkerHelper.FillFromQuery(dt, sql.ToString(), Tran.Context, new ParameterCollection());

            if (dt.Rows.Count > 0)
                userInRole = true;

            return userInRole;
        }

        public string ReturnPolicyOverrideUser(long InstanceID)
        {
            string User = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select top 1 wf.ADUserName from x2.X2.Instance i ");
            sb.AppendLine("join x2.dbo.vw_WFAssignment wf on wf.iid = i.id ");
            sb.AppendFormat("where i.id = {0} order by wf.id desc", InstanceID);

            object o = castleTransactionService.ExecuteScalarOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection());

            if (o != null)
                User = o.ToString();

            return User;
        }

        public string ReturnFeedbackOnverrideUser(long InstanceID)
        {
            string User = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select top 1 wf.adusername from x2.dbo.vw_WFAssignment wf ");
            sb.AppendLine("where wf.id in ( select top 2 wf1.id from x2.X2.Instance i ");
            sb.AppendLine("join x2.dbo.vw_WFAssignment wf1 on wf1.iid = i.id ");
            sb.AppendFormat("where i.id = {0} order by wf1.id desc ) order by wf.id asc ", InstanceID);
            object o = castleTransactionService.ExecuteScalarOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection());

            if (o != null)
                User = o.ToString();

            return User;
        }

        public string ResolveWorkflowRoleAssignment(long InstanceID, WorkflowRoleTypes workflowRoleType, WorkflowRoleTypeGroups workflowRoleTypeGroup)
        {
            object adUserNameObject = null;
            string query = string.Format(@"SELECT	TOP 1 a.ADUserName
                        from [x2].[x2].WorkflowRoleAssignment wfra (nolock)
                        join [2am]..ADUser a (nolock)
	                        on wfra.ADUserKey=a.ADUserKey
                        join [2am]..WorkflowRoleTypeOrganisationStructureMapping wrtosm (nolock)
	                        on wfra.WorkflowRoleTypeOrganisationStructureMappingKey = wrtosm.WorkflowRoleTypeOrganisationStructureMappingKey
                        join [2am]..WorkflowRoleType wrt (nolock)
	                        on wrtosm.WorkflowRoleTypeKey = wrt.WorkflowRoleTypeKey
                        where
	                        wfra.InstanceID = {0}
		                        and
	                        wrt.WorkflowRoleTypeKey = {1}
		                        and
	                        wrt.WorkflowRoleTypeGroupKey = {2}
		                        and
	                        wfra.GeneralStatusKey = 1", InstanceID, (int)workflowRoleType, (int)workflowRoleTypeGroup);
            adUserNameObject = castleTransactionService.ExecuteScalarOnCastleTran(query, Databases.TwoAM, null);
            return adUserNameObject != null ? adUserNameObject.ToString() : String.Empty;
        }

        public string RoundRobinAndAssignOtherFLCases(int applicationKey, string dynamicRole, int orgStructureKey, long instanceID, string state, int roundRobinPointerKey)
        {
            string assignedUser = string.Empty;

            // use this OfferKey to link to account, get other open offers of FL types
            // Get the instances that were created for these other offers
            // RoundRobin the first case
            // Get the ADUser the first case was assigned to
            // Assign the other 2 cases to that person.
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select o.offertypekey, Data.*  ");
            sb.AppendFormat("from [2am]..offer o (nolock) ");
            sb.AppendFormat("join x2data.application_management data (nolock) on o.offerkey=data.applicationkey ");
            sb.AppendFormat("where ReservedAccountKey=(select ReservedAccountKey from [2am]..offer o (nolock) where o.offerkey={0}) ", applicationKey);
            sb.AppendFormat("and o.offertypekey in (2,3,4) ");
            sb.AppendFormat("and offerstatuskey={0} ", (int)OfferStatuses.Open);
            sb.AppendFormat("order by o.offertypekey desc ");

            // The order by will put them in order rapid, further advance, FL
            DataSet ds = new DataSet();
            ds = castleTransactionService.ExecuteQueryOnCastleTran(sb.ToString(), Databases.X2, new ParameterCollection());

            DataTable dt = ds.Tables[0];
            DataRow dr = dt.Rows[0];
            Int64 IID = Convert.ToInt64(dr["InstanceID"]);

            assignedUser = ReactiveUserOrRoundRobin("FL Processor D", applicationKey, (int)SAHL.Common.Globals.OrganisationStructure.ApplicationProcessor, IID, state, SAHL.Common.Globals.Process.Origination, roundRobinPointerKey);

            if (dt.Rows.Count > 1 && (!string.IsNullOrEmpty(assignedUser)))
            {
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    IID = Convert.ToInt64(dr["InstanceID"]);
                    int GK = Convert.ToInt32(dr["ApplicationKey"]);
                    ReassignCaseToUser(IID, GK, assignedUser, (int)SAHL.Common.Globals.OrganisationStructure.ApplicationProcessor, (int)OfferRoleTypes.FLProcessorD, state, SAHL.Common.Globals.Process.Origination);
                }
            }

            return assignedUser;
        }

        public IEventList<IADUser> GetAdUsersByWorkflowRoleTypeKey(int workFlowRoleTypeKey)
        {
            string sql = SAHL.Common.DataAccess.UIStatementRepository.GetStatement("Repositories.WorkflowRoleAssignmentRepository", "GetAdUsersByWorkflowRoleTypeKey");

            SimpleQuery<ADUser_DAO> query = new SimpleQuery<ADUser_DAO>(QueryLanguage.Sql, string.Format(sql, workFlowRoleTypeKey));
            query.AddSqlReturnDefinition(typeof(ADUser_DAO), "ad");

            ADUser_DAO[] users = query.Execute();

            return new DAOEventList<ADUser_DAO, IADUser, ADUser>(users);
        }

        public IRoundRobinPointer DetermineRoundRobinPointerByOfferRoleTypeAndOrgStructure(OfferRoleTypes offerRoleType, int organisationStructureKey)
        {
            string sql = string.Format(@"   select rrp.RoundRobinPointerKey,
	                                               rrp.RoundRobinPointerIndexID,
	                                               rrp.Description,
	                                               rrp.GeneralStatusKey
                                            from [2AM].[dbo].[OfferRoleTypeOrganisationStructureMapping] (nolock) ortosm 
                                            join [2AM].[dbo].[RoundRobinPointerDefinition] (nolock) rrpd on rrpd.GenericKey = ortosm.OfferRoleTypeOrganisationStructureMappingKey
															                                            and rrpd.GenericKeyTypeKey = 25
															                                            and rrpd.GeneralStatusKey = 1
                                            join [2AM].[dbo].[RoundRobinPointer] rrp (nolock) on rrpd.RoundRobinPointerKey = rrp.RoundRobinPointerKey
															                                            and rrp.GeneralStatusKey = 1
                                            where ortosm.OfferRoleTypeKey = {0} and OrganisationStructureKey = {1}", 
                                       (int)offerRoleType, organisationStructureKey);

            SimpleQuery<RoundRobinPointer_DAO> query = new SimpleQuery<RoundRobinPointer_DAO>(QueryLanguage.Sql, sql);
            query.AddSqlReturnDefinition(typeof(RoundRobinPointer_DAO), "rrp");

            RoundRobinPointer_DAO[] rounderRobinPointers = query.Execute();

            if (rounderRobinPointers.Length == 0)
            {
                return null;
            }
            else
            {
                return new RoundRobinPointer(rounderRobinPointers[0]);
            }
        }
    }

    public class MandateUser
    {
        public string ADUserName;
        public int OrganisationStructureKey;
    }
}