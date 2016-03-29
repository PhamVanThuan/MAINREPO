using System;
using System.Collections.Generic;
using System.Text;
using SAHL.X2.Common;
using X2DomainService.Interface.Origination;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.X2.Framework.Common;
using SAHL.Common.Security;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using System.Threading;
using SAHL.Common.BusinessModel.DAO;
using DomainService.Repository;
using SAHL.X2.Common.DataAccess;
using Castle.ActiveRecord;
using SAHL.Common.X2.BusinessModel.DAO;
using System.Security.Permissions;
using SAHL.Common.Service.Interfaces;
using System.Data;
using SAHL.Common.DomainMessages;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Globals;

namespace DomainService.Workflow
{


    [Serializable]
    [ServiceAttribute("Common", WorkflowPorts.WorkflowPort, typeof(ICommon), typeof(Common))]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class Common : WorkflowBase, ICommon
    {
        public Common() : base() { }

        public IX2ReturnData GetnWorkingDaysFromToday(int nDays)
        {
            DateTime dt = DateTime.Now.AddDays(nDays);
            using (new SessionScope())
            {
                try
                {
                    string sql = string.Format("select top {0} * from [2am]..calendar where ISSaturday<>1 and ISSunday <> 1 and ISHOliday <> 1 and CalendarDate > getdate()", nDays);
                    DataSet ds = Helpers.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO));
                    dt = Convert.ToDateTime(ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1]["CalendarDate"]);
                }
                catch (Exception ex)
                {
                    LogPlugin.LogError("Unable to GetnWorkingDaysFromToday() {0}", ex.ToString());
                }
            }
            return new X2ReturnData(HandleX2Messages(), dt);
        }
        public IX2ReturnData GetCaseName(out string CaseName, int ApplicationKey)
        {

            CaseName = string.Empty;
            using (TransactionScope ts = new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Commit))
            {
                try
                {
                    if (0 != ApplicationKey)
                    {
                        IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                        IApplication app = appRepo.GetApplicationByKey(ApplicationKey);
                        if (null != app)
                            CaseName = app.GetLegalName(LegalNameFormat.Full);
                    }
                }
                catch (Exception ex)
                {
                    CaseName = "Error Getting CaseName";
                    LogPlugin.LogError("Unable to GetCaseName() {0} AID:{1}", ex.ToString(), ApplicationKey);
                }
            }
            IX2ReturnData ret = new X2ReturnData(HandleX2Messages(), CaseName);
            return ret;
        }
        public IX2ReturnData GetApplicatioIDFromSourceInstanceID(Int64 InstanceID, out int ApplicationID)
        {
            ApplicationID = -1;
            using (new SessionScope())
            {
                try
                {
                    Instance_DAO iid = Instance_DAO.Find(InstanceID);
                    if (null != iid.SourceInstanceID)
                    {
                        Instance_DAO sourceinstnce = Instance_DAO.Find((Int64)iid.SourceInstanceID);
                        IDictionary<string, object> data = Helpers.GetX2DataRow(sourceinstnce.ID);
                        ApplicationID = Convert.ToInt32(data["ApplicationKey"]);
                    }
                    else
                    {
                        LogPlugin.LogError("Source instanceid null for Instance {0}", InstanceID);
                    }
                }
                catch (Exception ex)
                {
                    LogPlugin.LogError("Unable to GetApplicatioIDFromSourceInstanceID() {0} IID:{1}", ex.ToString(), InstanceID);
                }
            }
            return new X2ReturnData(HandleX2Messages(), ApplicationID);
        }
        public IX2ReturnData GetPreviousOfferRoleKey(int ApplicationKey, string DynamicRole, out int Key)
        {
            Key = 0;
            using (new SessionScope())
            {
                try
                {
                    Key = base.GetPreviousOfferRoleKey(ApplicationKey, DynamicRole);
                }
                catch (Exception ex)
                {
                    LogPlugin.LogError("Unable to GetPreviousOfferRoleKey(): {0}, AID:{1} Role:{2}", ex.ToString(), ApplicationKey, DynamicRole);
                }
            }
            return new X2ReturnData(HandleX2Messages(), Key);
        }

        protected string ResolveDynamicRoleToUserNameDirty(string DynamicRole, int ApplicationKey)
        {
            try
            {
                string SQL = string.Format("exec x2.HLP.pr_OriginationResolveDynamicRole '{0}', {1}", DynamicRole, ApplicationKey);
                DataSet ds = Helpers.ExecuteQueryOnCastleTran(SQL, typeof(Instance_DAO));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                LogPlugin.LogError("Unable to resolve dynamic role:{0} GenKey:{1}{2}{3}", DynamicRole, ApplicationKey, Environment.NewLine, ex.ToString());
            }
            return "";
        }

        public IX2ReturnData ResolveDynamicRoleToUserName(out string AssignedUser, int ApplicationKey, string DynamicRole, long InstanceID)
        {
            AssignedUser = string.Empty;
            using (new SessionScope())
            {
                try
                {
                    AssignedUser = ResolveDynamicRoleToUserNameDirty(DynamicRole, ApplicationKey);
                }
                catch (Exception ex)
                {
                    LogPlugin.LogWarning("Unable to resolve dynamic role for : {0} {1}", DynamicRole, ex.ToString());
                }
                return new X2ReturnData(HandleX2Messages(), null);
            }
        }
        public IX2ReturnData GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole(out string ADUserName, int OfferRoleTypeKey, int ApplicationKey)
        {
            ADUserName = string.Empty;
            StringBuilder sb = new StringBuilder();
            using (new SessionScope())
            {
                try
                {
                    sb.AppendLine("select ad.AdUserName,orr.OfferRoleTypeKey, ad.generalStatusKey ");
                    sb.AppendLine("from [2am]..OfferRole orr (nolock) ");
                    sb.AppendLine("join  ");
                    sb.AppendLine("( ");
                    sb.AppendLine("select distinct(orr1.offerkey) ");
                    sb.AppendLine("from [2am]..offerrole orr (nolock) ");
                    sb.AppendLine("join [2am]..offerrole orr1 (nolock) on orr.legalentitykey=orr1.legalentitykey  ");
                    sb.AppendLine("and orr1.offerroletypekey in (8,10,11,12) ");
                    sb.AppendFormat("where orr.offerkey={0} ", ApplicationKey);
                    sb.AppendLine(") OL--OfferList ");
                    sb.AppendLine("on OL.OfferKey=orr.OfferKey ");
                    sb.AppendLine("join [2am]..LegalEntity le (nolock) on orr.legalentitykey=le.legalentitykey ");
                    sb.AppendLine("join [2am]..AdUser ad (nolock) on le.legalentitykey=ad.legalentitykey and ad.GeneralStatusKey=1 ");
                    sb.AppendFormat("Where orr.OfferRoleTYpeKey={0} ", OfferRoleTypeKey);
                    sb.AppendLine("order by orr.OfferRoleKey ");
                    DataSet ds = Helpers.ExecuteQueryOnCastleTran(sb.ToString(), typeof(GeneralStatus_DAO));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ADUserName = ds.Tables[0].Rows[0]["AdUserName"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    LogPlugin.LogError("Unable to GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole() {0}{1}{2}",
                        sb.ToString(), Environment.NewLine, ex.ToString());
                }
            }
            return new X2ReturnData(HandleX2Messages(), ADUserName);
        }
        public IX2ReturnData RoundRobinAssignForGivenOrgStructure(IActiveDataTransaction Tran, out string AssignedUser, string DynamicRole, int GenericKey, List<int> OrgStructureKeys)
        {
            AssignedUser = string.Empty;
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
                {
                    try
                    {
                        AssignedUser = base.RoundRobinAssignForGivenOrgStructure(DynamicRole, GenericKey, OrgStructureKeys);
                        ts.VoteCommit();
                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to RoundRobinAssignForGivenOrgStructure() {0} {1}", DynamicRole, ex.ToString());
                    }
                }
                scope.Complete();
            }
            return new X2ReturnData(HandleX2Messages(), AssignedUser);
        }
        public IX2ReturnData RoundRobinAssignForGivenOrgStructure(IActiveDataTransaction Tran, out string AssignedUser, string DynamicRole, int GenericKey, int OrgStructureKey)
        {
            List<int> OSKeys = new List<int>();
            OSKeys.Add(OrgStructureKey);
            return RoundRobinAssignForGivenOrgStructure(Tran, out AssignedUser, DynamicRole, GenericKey, OSKeys);
        }
        public IX2ReturnData AssignToUserInSameBranch(IActiveDataTransaction Tran, string DynamicRole, int ApplicationKey, long InstanceID, bool UseCaseCreator, string ADUserName, string DynamicRolePrefix)
        {
            string assignedTo = string.Empty;
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
                {
                    try
                    {
                        DomainService.Repository.OrganisationStructureGroup osDynamicRole = DomainService.Repository.OrganisationStructureGroup.Admin;
                        if (DynamicRolePrefix.ToUpper() == "CONSULTANT")
                            osDynamicRole = DomainService.Repository.OrganisationStructureGroup.Consultant;
                        else if (DynamicRolePrefix.ToUpper() == "MANAGER")
                            osDynamicRole = DomainService.Repository.OrganisationStructureGroup.Manager;

                        ApplicationRoleType_DAO art = WorkflowRepository.GetOfferRoleTypeByName(string.Format("{0} {1} D", DynamicRolePrefix, DynamicRole));

                        ADUser_DAO aduser = null;
                        if (UseCaseCreator)
                        {
                            Instance_DAO IID = Instance_DAO.Find(InstanceID);
                            aduser = WorkflowRepository.GetADUSerByName(IID.CreatorADUserName);
                        }
                        else
                        {
                            aduser = WorkflowRepository.GetADUSerByName(ADUserName);
                        }
                        ADUser_DAO[] _users = WorkflowRepository.GetBranchUsersForUserInThisBranch(aduser, osDynamicRole, DynamicRolePrefix);
                        List<ADUser_DAO> users = new List<ADUser_DAO>();
                        foreach (ADUser_DAO tusr in _users)
                        {
                            if (tusr.GeneralStatusKey.Key == 1)
                                users.Add(tusr);
                        }
                        if (null != users && users.Count > 0)
                        {
                            WorkflowRepository.Instance().CreateAndSaveApplicationRoleDAO(ApplicationKey, art.Description, users[0].LegalEntity.Key);
                            assignedTo = users[0].ADUserName;
                        }

                        ts.VoteCommit();
                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to AssignToUserInSameBranch() {0}", ex.ToString());
                    }
                }
                scope.Complete();
            }
            return new X2ReturnData(HandleX2Messages(), assignedTo);
        }
        public IX2ReturnData ReassignOrEscalateCaseToUser(IActiveDataTransaction Tran, int GenericKey, string DynamicRole, string ADUser, bool MarkPreviousRoleAsInactive)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
                {
                    try
                    {
                        Application_DAO app = Application_DAO.Find(GenericKey);
                        ADUser_DAO user = WorkflowRepository.GetADUSerByName(ADUser);

                        WorkflowRepository.Instance().CreateAndSaveApplicationRoleDAO(GenericKey, DynamicRole, user.LegalEntity.Key);
                        if (MarkPreviousRoleAsInactive)
                        {
                            int PrevORRKey = GetPreviousOfferRoleKey(GenericKey, DynamicRole);
                            WorkflowRepository.Instance().MarkRoleAsInactive(PrevORRKey);
                        }
                        ts.VoteCommit();
                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to ReassignOrEscalateCaseToUser() {0}", ex.ToString());
                    }
                }
                scope.Complete();
            }
            return new X2ReturnData(HandleX2Messages(), null);
        }
        public IX2ReturnData UpdateAssignedUserInIDM(IActiveDataTransaction Tran, int ApplicationKey, bool IsFL)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
                {
                    try
                    {
                        int AccountKey, OfferStatusKey;
                        string AssignedUser;
                        Application_DAO app = Application_DAO.Find(ApplicationKey);
                        AccountKey = app.ReservedAccount.Key;
                        OfferStatusKey = app.ApplicationStatus.Key;
                        ApplicationRole_DAO role = null;
                        string HQL = string.Empty;
                        if (IsFL)
                        {
                            HQL = string.Format("from ApplicationRole_DAO o where o.ApplicationKey=? and o.ApplicationRoleType.Key=857");
                        }
                        else
                        {
                            HQL = string.Format("from ApplicationRole_DAO o where o.ApplicationKey=? and o.ApplicationRoleType.Key=694");
                        }
                        //LogPlugin.LogInfo("IDM User Assigned Case Update, HQL{0}{1}", Environment.NewLine, HQL);
                        SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationKey);
                        ApplicationRole_DAO[] arr = q.Execute();
                        if (arr.Length > 0)
                        {
                            role = arr[arr.Length - 1];
                            ADUser_DAO[] ad = ADUser_DAO.FindAllByProperty("LegalEntity.Key", role.LegalEntityKey);
                            if (ad.Length != 0)
                            {
                                AssignedUser = ad[0].ADUserName.Replace(@"SAHL\", "");
                                LogPlugin.LogInfo("Assigning in IDM: {0}, {1}, {2}", ApplicationKey, OfferStatusKey, AssignedUser);
                                string CMD = string.Format("exec [ImageIndex]..pr_UpdateAssignedUserAndStateFromX2 @Accountkey={0}, @OfferStatusKey={1}, @LoginName='{2}'",
                                    ApplicationKey, OfferStatusKey, AssignedUser);
                                Helpers.ExecuteNonQueryOnCastleTran(CMD, typeof(Instance_DAO));
                            }
                            else
                            {
                                LogPlugin.LogError("No users for for LEKey:{0}", role.LegalEntityKey);
                            }
                        }
                        else
                        {
                            LogPlugin.LogError("Unable to get ApplicationRole[] for ApplicationRoleType in (857[Flappproc], 694[NBPUsers]) for ApplicationKey:{0}", ApplicationKey);
                        }
                        ts.VoteCommit();
                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to UpdateAssignedUserInIDM() {0}", ex.ToString());
                    }
                }
                scope.Complete();
            }
            return new X2ReturnData(HandleX2Messages(), null);
        }
        public IX2ReturnData HasCaseBeenAssignedToThisDynamicRoleBefore(string DynamicRole, int ApplicationKey, out string userName)
        {
            userName = string.Empty;
            using (new SessionScope())
            {
                try
                {
                    ApplicationRole_DAO role = WorkflowRepository.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(ApplicationKey, DynamicRole);
                    if (null != role)
                    {
                        ADUser_DAO user = WorkflowRepository.GetADUserForLegalEntityKey(role.LegalEntityKey);
                        userName = user.ADUserName;
                    }
                }
                catch (Exception ex)
                {
                    LogPlugin.LogError("Unable to HasCaseBeenAssignedToThisDynamicRoleBefore:{0} ApplicationKey:{1}", ex.ToString(), ApplicationKey);
                }
            }
            return new X2ReturnData(HandleX2Messages(), userName);
        }
        public IX2ReturnData GetApplicationRoleForCase(int GenericKey, string DynamicRole, out int ApplicationRoleKey)
        {
            ApplicationRoleKey = 0;
            using (new SessionScope())
            {
                try
                {
                    ApplicationRoleType_DAO ort = WorkflowRepository.GetOfferRoleTypeByName(DynamicRole);
                    ApplicationRole_DAO role = WorkflowRepository.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(GenericKey, ort.Key);
                    if (null == role)
                    {
                        SPC.DomainMessages.Add(new Error("No user selected. Please select a user", ""));
                    }
                    else
                    {
                        ApplicationRoleKey = role.Key;
                    }
                }
                catch (Exception ex)
                {
                    LogPlugin.LogError("Unable to GetApplicationRoleForCase() :{0} Role:{1} AppKey:{2}", ex.ToString(), DynamicRole, GenericKey);
                }
            }
            return new X2ReturnData(HandleX2Messages(), ApplicationRoleKey);
        }


        public IX2ReturnData UpdateParentVars(IActiveDataTransaction Tran, Int64 ChildInstanceID, Dictionary<string, object> dict)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
                {
                    try
                    {
                        Instance_DAO iChild = Instance_DAO.Find(ChildInstanceID);
                        Helpers.SetX2DataRow(iChild.ParentInstance.ID, dict, Tran);
                        ts.VoteCommit();
                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to UpdateParentVars() {0} ", ex.ToString());
                    }
                }
                scope.Complete();
            }
            return new X2ReturnData(HandleX2Messages(), null);
        }

        protected override ApplicationRoleType_DAO GetApplicationRoleType(ADUser_DAO user)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        protected ApplicationRole_DAO ResolveApplicationRole(string DynamicRole, int GenericKey, Int64 InstanceID)
        {
            return WorkflowRepository.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(GenericKey, DynamicRole);
        }

        protected class UserOrgStuctObj
        {
            internal int LEKey = -1;
            internal OrganisationStructure_DAO dao = null;
            internal UserOrgStuctObj(int LEKey, OrganisationStructure_DAO dao)
            {
                this.LEKey = LEKey;
                this.dao = dao;
            }
        }

        //protected string RoundRobinAssignForGivenOrgStructure(IActiveDataTransaction Tran, string DynamicRole, int GenericKey, List<int> OrgStructureKeys)
        //{
        //    using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
        //    {
        //        using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits)) {
        //        try
        //        {
        //            string AssignedUser = string.Empty;
        //            if (IsThereExistingActiveRoleForThisRoleType(DynamicRole, GenericKey, out AssignedUser))
        //            {
        //                ts.VoteCommit();
        //                return AssignedUser;
        //            }
        //            List<int> LEKeys = new List<int>();
        //            ApplicationRoleType_DAO rt = WorkflowRepository.GetOfferRoleTypeByName(DynamicRole);
        //            OrganisationStructure_DAO os = null;
        //            int LastLEKeyAssigned = -1;
        //            for (int i = 0; i < rt.OfferRoleTypeOrganisationStructures.Count; i++)
        //            {
        //                os = rt.OfferRoleTypeOrganisationStructures[i];
        //                for (int j = 0; j < OrgStructureKeys.Count; j++)
        //                {
        //                    if (os.Key == OrgStructureKeys[j])
        //                    {
        //                        foreach (ADUser_DAO ad in os.ADUsers)
        //                        {
        //                            if (ad.GeneralStatusKey.Key == 1)
        //                            {
        //                                if (!LEKeys.Contains(ad.LegalEntity.Key))
        //                                    LEKeys.Add(ad.LegalEntity.Key);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            if (LEKeys.Count == 0)
        //                throw new Exception(string.Format("No users found for dynamic role:{0}", DynamicRole));
        //            ApplicationRole_DAO Role = WorkflowRepository.FindLastApplicationRoleByApplicationRoleTypeKeyAndLEKeys(rt.Key, LEKeys);
        //            if (null != Role)
        //            {
        //                LastLEKeyAssigned = Role.LegalEntityKey;
        //                if (LastLEKeyAssigned == null)
        //                    LogPlugin.LogError("Unable to find ADUser for LEKey:{0}", Role.LegalEntityKey);
        //                // we dont have to check for null here although it shouldnt be null
        //            }
        //            else
        //            {
        //                // this case has never beeen assigned before
        //                LastLEKeyAssigned = -1;
        //            }

        //            // find the user to assign to
        //            LEKeys.Sort();
        //            int LETMP = -1;
        //            if (-1 == LastLEKeyAssigned)
        //            {
        //                LETMP = LEKeys[0];
        //            }
        //            else
        //            {
        //                int idx = -1;
        //                for (int i = 0; i < LEKeys.Count; i++)
        //                {
        //                    if (LEKeys[i] == LastLEKeyAssigned)
        //                    {
        //                        idx = i;
        //                        break;
        //                    }
        //                }
        //                if ((idx == -1) || (idx == (LEKeys.Count - 1)))
        //                {
        //                    LETMP = LEKeys[0];
        //                }
        //                else
        //                {
        //                    LETMP = LEKeys[(idx + 1)];
        //                }
        //            }

        //            // create an offerrole 
        //            WorkflowRepository.Instance().CreateAndSaveApplicationRoleDAO(GenericKey, rt.Description, LETMP);
        //            string s = WorkflowRepository.GetADUserForLegalEntityKey(LETMP).ADUserName;
        //            ts.VoteCommit();
        //            return s;
        //        }
        //        catch (Exception ex)
        //        {
        //            ts.VoteRollBack();
        //            LogPlugin.LogError("Error in RoundRobinAssignForGivenOrgStructure() {0}", ex.ToString());
        //            return null;
        //        }
        //    }
        //}
        public IX2ReturnData RecalculateHouseHoldIncomeAndSave(IActiveDataTransaction Tran, int ApplicationKey)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
                {
                    try
                    {
                        IApplication app = appRepo.GetApplicationByKey(ApplicationKey);
                        app.CalculateHouseHoldIncome();
                        appRepo.SaveApplication(app);
                        ts.VoteCommit();
                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to RecalculateHouseHoldIncomeAndSave() {0} AID:{1}", ex.ToString(), ApplicationKey);
                    }
                }
                scope.Complete();
            }
            return new X2ReturnData(HandleX2Messages(), null);
        }
        public IX2ReturnData CreateNewRevision(IActiveDataTransaction Tran, int ApplicationKey)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
                {
                    try
                    {
                        IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                        IApplication app = appRepo.GetApplicationByKey(ApplicationKey);
                        app.CreateRevision();
                        appRepo.SaveApplication(app);
                        ts.VoteCommit();
                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to CreateNewRevision() {0}", ex.ToString());
                    }
                }
                scope.Complete();
            }
            return new X2ReturnData(HandleX2Messages(), null);
        }
        public IX2ReturnData UpdateAccountStatus(IActiveDataTransaction Tran, int ApplicationKey, int AccountStatusKey)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
                {
                    try
                    {
                        Application_DAO app = Application_DAO.Find(ApplicationKey);
                        if (null != app.Account)
                        {
                            if (app.Account.AccountStatus.Key == 3)
                            {
                                // not using try find cause we looking for a lookup value
                                app.Account.AccountStatus = AccountStatus_DAO.Find(AccountStatusKey);
                            }
                        }
                        WorkflowRepository.Instance().SaveApplication(app);
                        ts.VoteCommit();
                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to UpdateAccountStatus() {0}", ex.ToString());
                    }
                }
                scope.Complete();
            }
            return new X2ReturnData(HandleX2Messages(), null);
        }
        public IX2ReturnData CreateAccountForApplication(IActiveDataTransaction Tran, int ApplicationKey)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
                {
                    try
                    {
                        if (appRepo.GetApplicationByKey(ApplicationKey).ApplicationType.Key == 2 || appRepo.GetApplicationByKey(ApplicationKey).ApplicationType.Key == 3)
                        {
                            // WTF
                        }
                        else
                        {
                            appRepo.CreateAccountFromApplication(ApplicationKey);
                        }

                        ts.VoteCommit();
                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to CreateAccountForApplication() {0}", ex.ToString());
                    }
                }
                scope.Complete();
            }
            return new X2ReturnData(HandleX2Messages(), null);
        }

        public IX2ReturnData SendSMSToMainApplicants(string Message, int ApplicationKey)
        {
            try
            {
                using (new SessionScope())
                {
                    IMessageService msg = ServiceFactory.GetService<IMessageService>();
                    IApplication app = appRepo.GetApplicationByKey(ApplicationKey);
                    IReadOnlyEventList<ILegalEntity> les = app.GetLegalEntitiesByRoleType(new OfferRoleTypes[] { OfferRoleTypes.MainApplicant });
                    foreach (ILegalEntity le in les)
                    {
                        if (!string.IsNullOrEmpty(le.CellPhoneNumber))
                        {
                            try
                            {
                                msg.SendSMS(ApplicationKey, Message, le.CellPhoneNumber);
                            }
                            catch (Exception e)
                            {
                                LogPlugin.LogError("Unable to send sms for LE:{0} offerkey:{1} {2}{3}", le.Key, ApplicationKey, Environment.NewLine, e.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogPlugin.LogError("Exception in FL.SendSMS() case:{0}{1}{2}", ApplicationKey, Environment.NewLine, ex.ToString());
            }
            return new X2ReturnData(HandleX2Messages(), null);
        }
        public IX2ReturnData ValuationInProgress(IActiveDataTransaction Tran, Int64 InstanceID, int GenericKey, out bool b)
        {
            b = false;
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
                {
                    try
                    {
                        string SQL = string.Format("select data.applicationkey, s.name, o.reservedaccountkey from x2data.Valuations Data (nolock) join x2.Instance i (nolock) on data.instanceid=i.id join x2.state s (nolock) on i.stateid=s.id join [2am]..offer o (nolock) on data.applicationkey=o.offerkey where data.Applicationkey={0} and s.type not in (5, 6)", GenericKey);
                        DataSet ds = Helpers.ExecuteQueryOnCastleTran(SQL, typeof(Instance_DAO));
                        if (ds.Tables["DATA"].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                SPC.DomainMessages.Add(new Error(string.Format("Application:{0} ReservedAccountKey:{2} in Valuations is not complete. Is at State:{1}", dr[0], dr[1], dr[2]), ""));
                                b = true;
                            }
                        }
                        ts.VoteCommit();
                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to ValuationInProgress() {0}", ex.ToString());
                    }
                }
                scope.Complete();
            }
            return new X2ReturnData(HandleX2Messages(), b);
        }

        public IX2ReturnData HasInstancePerformedActivityBefore(Int64 InstanceID, string Activity)
        {
            bool b = false;
            using (new SessionScope())
            {
                b = base.HasInstancePerformedActivity(InstanceID, Activity);
            }
            return new X2ReturnData(HandleX2Messages(), b);
        }

        public IX2ReturnData HasSourceInstancePerformedActivityBefore(Int64 SourceInstanceID, string Activity)
        {
            bool b = false;
            using (new SessionScope())
            {
                b = base.HasSourceInstancePerformedActivity(SourceInstanceID, Activity);
            }
            return new X2ReturnData(HandleX2Messages(), b);
        }
    }
}
