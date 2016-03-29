using System;
using System.Collections.Generic;
using System.Text;
using SAHL.X2.Common;
//using X2DomainService.Interface.Origination;
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
using SAHL.Common.Service;
using Castle.ActiveRecord.Queries;
using System.Data;
using System.Net.Mail;
using SAHL.Common.DomainMessages;
using SAHL.Common.CacheData;
using System.Runtime.Remoting.Lifetime;
using System.Data.SqlClient;
namespace DomainService.Workflow
{
    [Serializable]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class Base : MarshalByRefObject, ILease
    {
        public Base()
        {
            SAHLPrincipal p = new SAHLPrincipal(new GenericIdentity("X2"));

            Thread.CurrentPrincipal = p;
            string key = p.Identity.Name.ToLower();

            CacheManager principalStore = CacheFactory.GetCacheManager("SAHLPrincipalStore");
            SAHLPrincipalCache principalCache = SAHLPrincipalCache.GetPrincipalCache(p);
            principalStore.Add(key.ToLower(), principalCache);
            principalCache.DomainMessages.Clear();
            //Console.WriteLine("~.ctor() for {0}", this.GetType());
        }
        protected IX2MessageCollection HandleX2Messages()
        {
            IX2MessageCollection mc = new X2MessageCollection();
            SAHLPrincipal p = SAHLPrincipal.GetCurrent();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(p);
            lock (spc.DomainMessages)
            {
                lock (mc)
                {
                    foreach (IDomainMessage msg in spc.DomainMessages)
                    {
                        if (msg.MessageType == DomainMessageType.Error)
                        {
                            mc.Add(new X2Message(msg.Message, X2MessageType.Error));
                        }
                        else if (msg.MessageType == DomainMessageType.Warning)
                        {
                            mc.Add(new X2Message(msg.Message, X2MessageType.Warning));
                        }
                        else
                        {
                            LogPlugin.LogWarning("X2 Doesnt Handle InfoMessages from the Domain {0}", msg.Message);
                        }
                    }
                }
            }
            CleanDomainMessages();
            return mc;
        }
        private void SpitMessages()
        {
            return;
            StringBuilder sb = new StringBuilder();
            foreach (IDomainMessage msg in SPC.DomainMessages)
            {
                sb.AppendFormat("{0} - {1}", msg.MessageType, msg.Message);
                sb.AppendLine();
            }
            if (sb.Length > 0)
                LogPlugin.LogInfo(sb.ToString());
        }
        protected void CleanDomainMessages()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();
        }
        protected void AddErrorMessage(string Message, string Details)
        {
            SPC.DomainMessages.Add(new Error(Message, Details));
        }
        protected bool HasMessages(bool IgnoreWarnings)
        {
            if (SPC.DomainMessages.HasErrorMessages)
            {
                SpitMessages();
                return true;
            }
            if (SPC.DomainMessages.HasWarningMessages && !IgnoreWarnings)
            {
                SpitMessages();
                return true;
            }
            return false;
        }
        internal SAHLPrincipalCache SPC
        {
            get
            {
                return SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            }
        }

        protected bool HasSourceInstancePerformedActivity(Int64 InstanceID, string ActivityName)
        {
            string SQL = string.Format("Select SourceInstanceID from x2.Instance (nolock) where ID={0}", InstanceID);
            DataSet ds = Helpers.ExecuteQueryOnCastleTran(SQL, typeof(Instance_DAO));
            if (ds.Tables[0].Rows.Count > 0)
            {
                Int64 SIID = Convert.ToInt64(ds.Tables[0].Rows[0][0]);
                return HasInstancePerformedActivity(SIID, ActivityName);
            }
            return false;
        }
        protected bool HasInstancePerformedActivity(Int64 InstanceID, string ActivityName)
        {
            Instance_DAO iid = Instance_DAO.Find(InstanceID);
            Activity_DAO Activity = WorkflowRepository.GetActivityByName(ActivityName);
            WorkFlowHistory_DAO[] hist = WorkflowRepository.GetHistoryForInstanceAndActivity(iid, Activity);
            if (null == hist || hist.Length == 0)
                return false;
            return true;
        }

        #region ILease Members

        public TimeSpan CurrentLeaseTime
        {
            get { return new TimeSpan(0, 5, 0); }
        }

        public LeaseState CurrentState
        {
            get { return LeaseState.Active; }
        }

        public TimeSpan InitialLeaseTime
        {
            get
            {
                return new TimeSpan(0, 5, 0);
            }
            set
            {

            }
        }

        public void Register(ISponsor obj)
        {

        }

        public void Register(ISponsor obj, TimeSpan renewalTime)
        {

        }

        public TimeSpan Renew(TimeSpan renewalTime)
        {
            return new TimeSpan(0, 5, 0);
        }

        public TimeSpan RenewOnCallTime
        {
            get
            {
                return new TimeSpan(0, 5, 0);
            }
            set
            {

            }
        }

        public TimeSpan SponsorshipTimeout
        {
            get
            {
                return new TimeSpan(0, 5, 0);
            }
            set
            {
            }
        }

        public void Unregister(ISponsor obj)
        {
        }

        #endregion
    }

    [Serializable]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public abstract class WorkflowBase : Base
    {
        internal const string Origination = "Origination";
        internal const string AppMan = "Application Management";
        internal const string Val = "Valuations";
        internal const string ReadvancePaymenst = "Readvance Payments";
        internal const string AppCap = "Application Capture";
        internal const string Credit = "Credit";
        protected IApplicationRepository appRepo = null;
        protected IOrganisationStructureRepository osRepo = null;
        protected IX2Repository x2Repo = null;
        protected ILookupRepository lRepo = null;
        protected ICommonRepository cRepo = null;
        protected IReasonRepository rRepo = null;
        protected IStageDefinitionRepository sDRepo = null;
        protected ILifeRepository lifeRepo = null;
        protected IAccountRepository accRepo = null;
        protected ICorrespondenceRepository correspRepo = null;
        protected IReportRepository reportRepo = null;

        public WorkflowBase():base()
        {
            try
            {
                appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
                lRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            }
            catch (Exception ex)
            {
                LogPlugin.LogInfo("Unable to create Common and set Thread Principal {0}", ex.ToString());
                throw;
            }
        }

        

        protected void AssignToNextUserForDynamicRole(IActiveDataTransaction Tran, out string AssigndTo, string DynamicRole, string CurrentUser, int GenericKey)//, IActiveDataTransaction Tran, string ADUser, object Data)
        {
            AssigndTo = string.Empty;
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
                {
                    try
                    {
                        // Get all the OrgStructures this guy belongs to
                        // Get all the OrgStructures that the Dynamic role belongs to
                        // Find the intersection point thats the orgstrucure you want. Get the other folks in that group
                        // round robin assign the next dude.
                        int LEKeyToAssignCaseTo = -1;
                        ADUser_DAO aduser = WorkflowRepository.GetADUSerByName(CurrentUser);
                        ApplicationRoleType_DAO art = WorkflowRepository.GetOfferRoleTypeByName(DynamicRole);
                        OrganisationStructure_DAO OSUserBelongsTo = null;
                        IList<UserOrganisationStructure_DAO> uos = aduser.UserOrganisationStructure;
                        if (null == uos)
                        {
                            // check how life does it
                            // maybe look for Branch COnsult under sahl direct .. we do this for RCS?
                            throw new Exception(string.Format("aduser.UserOrganisationStructure is null for user:{0} dynamicrole:{1} GenKey:{2}", CurrentUser, DynamicRole, GenericKey));
                        }
                        else
                        {
                            foreach (UserOrganisationStructure_DAO tuos in uos)
                            {
                                // Find our ADUser
                                if (tuos.ADUser.Key == aduser.Key)
                                {
                                    // go look to see if this OS is one thats mapped to the OfferROleType(DynamicRole)
                                    foreach (OrganisationStructure_DAO os in art.OfferRoleTypeOrganisationStructures)
                                    {
                                        // if it is then this should be the os that we are looking for
                                        if (tuos.OrganisationStructure.Key == os.Key)
                                        {
                                            OSUserBelongsTo = tuos.OrganisationStructure;
                                            break;
                                        }
                                    } if (null != OSUserBelongsTo) break;
                                }
                            }
                        }

                        // Now get the users that are also in this group.
                        LogPlugin.LogWarning("User:{0} OS:{1}", aduser.ADUserName, OSUserBelongsTo.Description);
                        IList<ADUser_DAO> _UsersInGroup = OSUserBelongsTo.ADUsers;
                        List<ADUser_DAO> UsersInGroup = new List<ADUser_DAO>();
                        foreach (ADUser_DAO tusr in _UsersInGroup)
                        {
                            if (tusr.GeneralStatusKey.Key == 1)
                                UsersInGroup.Add(tusr);
                        }
                        List<int> LegalEntityKeys = new List<int>();
                        foreach (ADUser_DAO ad in UsersInGroup)
                        {
                            if (ad.LegalEntity != null)
                            {
                                LegalEntityKeys.Add(ad.LegalEntity.Key);
                            }
                        }

                        // go do a select top X from offerrole where lekey in legalentitykeys order by date desc
                        ApplicationRole_DAO[] offerrole = WorkflowRepository.GetTopXApplicationRolesForLEKeys(LegalEntityKeys, 30);
                        // build up a list of ppl that have had cases assigned to them before in chronological order
                        List<int> LEKeys = new List<int>();
                        foreach (ApplicationRole_DAO ar in offerrole)
                        {
                            if (ar.LegalEntityKey > 0)
                            {
                                if (!LEKeys.Contains(ar.LegalEntityKey))
                                {
                                    LEKeys.Add(ar.LegalEntityKey);
                                }
                            }
                        }
                        // Check to see if anyone has NOT been given a case before. If so assign it to them. If everyone
                        // has had a case at some point assign it to the person who has been the longest since having one
                        int[] lst = new int[LegalEntityKeys.Count];
                        LegalEntityKeys.CopyTo(lst);
                        List<int> tmp = new List<int>(lst);
                        for (int i = 0; i < LEKeys.Count; i++)
                        {
                            if (tmp.Contains(LEKeys[i]))
                                tmp.Remove(LEKeys[i]);
                        }
                        // is anything left in tmp? If so pick the first one and assign the case to that guy
                        if (tmp.Count > 0)
                        {
                            LEKeyToAssignCaseTo = tmp[0];
                        }
                        else
                        {
                            // assign to guy at the back of the LEKeys
                            LEKeyToAssignCaseTo = LEKeys[LEKeys.Count - 1];
                        }
                        WorkflowRepository.Instance().CreateAndSaveApplicationRoleDAO(GenericKey, DynamicRole, LEKeyToAssignCaseTo);
                        AssigndTo = WorkflowRepository.GetADUserForLegalEntityKey(LEKeyToAssignCaseTo).ADUserName;
                        ts.VoteCommit();

                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to AssignToNextUserForDynamicRole DynamicRole:{0} GenKey:{1} CurrnentUser:{2} \r\n{3}", DynamicRole, GenericKey, CurrentUser, ex.ToString());
                    }
                }
                scope.Complete();
            }
        }
        #region Mandates
        /// <summary>
        /// Gets a list of users who fulfil a mandate set. Basically Looks at the dept, gets mandates linked to that dept then 
        /// runs the IApplicationMortgageLoan against the mandates and returns a list of users who pass the mandate checks
        /// *** NB you must wrap this method within an external transaction as it doesnt do it itself.
        /// </summary>
        /// <param name="iaml"></param>
        /// <param name="p_AllUsers"></param>
        /// <param name="Dept"></param>
        /// <returns></returns>
        protected int PopulateUserList(IApplicationMortgageLoan iaml, out List<string> p_AllUsers, string Dept)
        {
            p_AllUsers = new List<string>();
            IMandateService ms = new MandateService();
            OrganisationStructure_DAO osCredit = WorkflowRepository.GetOrgStructureByName(Dept);
            List<int> osKeys = new List<int>();
            foreach (OrganisationStructure_DAO os in osCredit.ChildOrganisationStructures)
            {
                osKeys.Add(os.Key);
            }
            AllocationMandateSetGroup_DAO[] mandates = WorkflowRepository.GetAllocationMandatesForOrgStructureKeys(osKeys);
            string[] MandateList = new string[mandates.Length];
            for (int i = 0; i < MandateList.Length; i++)
            {
                MandateList[i] = mandates[i].AllocationGroupName;
            }
            List<IADUser> AllUsers = new List<IADUser>();
            Dictionary<int, int> LEs = new Dictionary<int, int>();
            foreach (string MandateGroup in MandateList)
            {
                IList<IADUser> users = ms.ExecuteMandateSet(MandateGroup, new object[] { iaml });
                if (users.Count > 0)
                {
                    foreach (IADUser usr in users)
                    {
                        if (!AllUsers.Contains(usr)&&usr.GeneralStatusKey.Key == 1)
                        {
                            AllUsers.Add(usr);
                            p_AllUsers.Add(usr.ADUserName);
                            LEs.Add(usr.LegalEntity.Key, 0);
                        }
                    }
                }
            }
            if (AllUsers.Count == 0) throw new Exception(string.Format("Unable to get users for mandates, AppKey:{0}", iaml.Key));
            // now do the whole round robin thing.
            FindApplicationRoleForLEs(ref LEs);
            int LEKey = -1;
            int ORR = int.MaxValue;
            int[] Keys = new int[LEs.Keys.Count];
            LEs.Keys.CopyTo(Keys, 0);
            for (int i = 0; i < Keys.Length; i++)
            {
                if (LEs[Keys[i]] < ORR)
                {
                    ORR = LEs[Keys[i]];
                    LEKey = Keys[i];
                }
            }
            return LEKey;
        }
        internal void FindApplicationRoleForLEs(ref Dictionary<int, int> LEs)
        {
            int[] Keys = new int[LEs.Keys.Count];
            LEs.Keys.CopyTo(Keys, 0);
            for (int i = 0; i < Keys.Length; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Select top 1 OfferRoleKey from [2am]..OfferRole (nolock) where LegalEntityKey={0} order by offerrolekey desc", Keys[i]);
                DataSet o = Helpers.ExecuteQueryOnCastleTran(sb.ToString(), typeof(Application_DAO));
                if (null == o || o.Tables[0] == null || o.Tables[0].Rows.Count == 0)
                {
                    LEs[Keys[i]] = 0;
                }
                else
                {
                    object tmp = o.Tables[0].Rows[0][0];
                    LEs[Keys[i]] = Convert.ToInt32(tmp);
                }
            }
        }
        #endregion

        public IX2ReturnData AssignCreateorAsDynamicRole(IActiveDataTransaction Tran, out string AssignedTo, int ApplicationKey, long InstanceID, string DynamicRole)
        {
            AssignedTo = string.Empty;
            try
            {
                if (Tran == null || Tran.CurrentTransaction == null)
                {
                    string wtf = "";
                }
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
                {

                    using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
                    {
                        try
                        {

                            // use offerroletype to get a list of mappings to the OfferRoleTypeOrganisationStructureMapping
                            // use the adusername to get the org structure the creator blongs to
                            Instance_DAO instance = Instance_DAO.Find(InstanceID);

                            // Get the ADUser 
                            ADUser_DAO aduser = WorkflowRepository.GetADUSerByName(instance.CreatorADUserName);
                            ApplicationRoleType_DAO art = WorkflowRepository.GetOfferRoleTypeByName(DynamicRole);

                            // check that the creator user IS in fact part of the 
                            bool UserIsInFuncGroup = false;
                            foreach (OrganisationStructure_DAO os in art.OfferRoleTypeOrganisationStructures)
                            {
                                foreach (ADUser_DAO user in os.ADUsers)
                                {
                                    if (user.Key == aduser.Key)
                                    {
                                        UserIsInFuncGroup = true;
                                        break;
                                    }
                                }
                            }
                            if (UserIsInFuncGroup)
                            {
                                WorkflowRepository.Instance().CreateAndSaveApplicationRoleDAO(ApplicationKey, art.Description, aduser.LegalEntity.Key);
                                AssignedTo = aduser.ADUserName;
                                // If the Branch Consultant Created the case assign the 100 role which is the Commission Earner Role
                                // This can be reasigned later using the app. Only do this if the consultant created the case
                                // In the case that the admin creates the case the select consultant screen will write the 100 role.
                                if (art.Key == 101)
                                {
                                    WorkflowRepository.Instance().CreateAndSaveApplicationRoleDAO(ApplicationKey, "Commissionable Consultant", aduser.LegalEntity.Key);
                                }
                            }
                            else
                            {
                                LogPlugin.LogWarning("Case cant be assigned to {0} as they are not a member of {1}", aduser.ADUserName, DynamicRole);
                                AssignedTo = "";
                            }
                            ts.VoteCommit();
                        }
                        catch (Exception ex)
                        {
                            ts.VoteRollBack();
                            LogPlugin.LogError("Unable to AssignCreatorAsDynamicRole() {0}", ex.ToString());
                        }
                    }
                    scope.Complete();
                }
                return new X2ReturnData(HandleX2Messages(), AssignedTo);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        protected abstract ApplicationRoleType_DAO GetApplicationRoleType(ADUser_DAO user);
        protected int GetPreviousOfferRoleKey(int ApplicationKey, string DynamicRole)
        {
            ApplicationRole_DAO dao = WorkflowRepository.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(ApplicationKey, DynamicRole);
            if (null != dao)
                return dao.Key;
            return -1;
        }
        public IX2ReturnData MakeApplicationRoleInactive(IActiveDataTransaction Tran, int ApplicationKey, List<int> ApplicationRoleTypeKeys)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
                {
                    try
                    {
                        //StringBuilder sb = new StringBuilder();
                        //sb.Append("update [2am]..offerrole set generalstatuskey=2 where offerkey in (");
                        //for (int i = 0; i < ApplicationRoleTypeKeys.Count; i++)
                        //{
                        //    sb.AppendFormat("{0},", ApplicationRoleTypeKeys[i]);
                        //}
                        //sb.Remove(sb.Length - 1, 1);
                        //sb.Append(")");
                        //Helpers.ExecuteNonQueryOnCastleTran(sb.ToString(), typeof(GeneralStatus_DAO));
                        ApplicationRole_DAO[] arr = WorkflowRepository.GetApplicationRolesForKeyAndApplicationRoleTypeKey(ApplicationKey, ApplicationRoleTypeKeys);
                        foreach (ApplicationRole_DAO role in arr)
                        {
                            role.GeneralStatus = WorkflowRepository.GetGeneralStatusDAOByName("InActive");
                            WorkflowRepository.Instance().SaveApplicationRole(role);
                        }
                        ts.VoteCommit();
                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to MakeApplicationRoleInactive() {0} AID:{1}", ex.ToString(), ApplicationKey);
                    }
                }
                scope.Complete();
            }
            return new X2ReturnData(HandleX2Messages(), null);
        }
        public IX2ReturnData MarkUsersAsInactive(IActiveDataTransaction Tran, int ApplicationKey, List<int> ApplicationRoleTypeKeys)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
                {
                    try
                    {
                        //string Keys = string.Empty;
                        //for (int i = 0; i < OfferRoleTypeKeys.Count; i++)
                        //{
                        //    Keys = string.Format("{0},{1}", Keys, OfferRoleTypeKeys[i]);
                        //}
                        //Keys = Keys.Remove(0,1);
                        //StringBuilder sb = new StringBuilder();
                        //sb.AppendLine("update [2am]..offerrole set GeneralStatusKey=2 ");
                        //sb.AppendFormat("where offerkey={0} and offerroletypekey in ({1})", ApplicationKey, Keys);
                        //Helpers.ExecuteNonQueryOnCastleTran(sb.ToString(), typeof(GeneralStatus_DAO));
                        ApplicationRole_DAO[] arr = WorkflowRepository.GetApplicationRolesForKeyAndApplicationRoleTypeKey(ApplicationKey, ApplicationRoleTypeKeys);
                        foreach (ApplicationRole_DAO role in arr)
                        {
                            role.GeneralStatus = WorkflowRepository.GetGeneralStatusDAOByName("InActive");
                            WorkflowRepository.Instance().SaveApplicationRole(role);
                        }

                        ts.VoteCommit();
                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to MarkUsersAsInactive() {0} AID:{1}", ex.ToString(), ApplicationKey);
                    }
                }
                scope.Complete();
            }
            return new X2ReturnData(HandleX2Messages(), null);
        }
        protected string RoundRobinAssignForGivenOrgStructure(string DynamicRole, int GenericKey, List<int> OrgStructureKeys)
        {
            CleanDomainMessages();
            List<int> LEKeys = new List<int>();
            ApplicationRoleType_DAO rt = WorkflowRepository.GetOfferRoleTypeByName(DynamicRole);
            OrganisationStructure_DAO os = null;
            bool Found = false;
            for (int i = 0; i < rt.OfferRoleTypeOrganisationStructures.Count; i++)
            {
                os = rt.OfferRoleTypeOrganisationStructures[i];
                for (int j = 0; j < OrgStructureKeys.Count; j++)
                {
                    if (os.Key == OrgStructureKeys[j])
                    {
                        foreach (ADUser_DAO ad in os.ADUsers)
                        {
                            if (!LEKeys.Contains(ad.LegalEntity.Key))
                                LEKeys.Add(ad.LegalEntity.Key);
                        }
                    }
                }
            }
            if (LEKeys.Count == 0)
                throw new Exception(string.Format("No users found for dynamic role:{0}", DynamicRole));
            ApplicationRole_DAO Role = WorkflowRepository.FindLastApplicationRoleByApplicationRoleTypeKeyAndLEKeys(rt.Key, LEKeys);
            ADUser_DAO LastUserAssigned = null;
            if (null != Role)
            {
                LastUserAssigned = WorkflowRepository.GetADUserForLegalEntityKey(Role.LegalEntityKey);
                if (LastUserAssigned == null)
                    LogPlugin.LogWarning("Unable to find ADUser for LEKey:{0}", Role.LegalEntityKey);
                // we dont have to check for null here although it shouldnt be null
            }
            else
            {
                // this case has never beeen assigned before
                LastUserAssigned = null;
            }

            // find the user to assign to
            List<ADUser_DAO> _Users = new List<ADUser_DAO>(os.ADUsers);
            List<ADUser_DAO> Users = new List<ADUser_DAO>();
            foreach (ADUser_DAO usr in _Users)
            {
                if (usr.GeneralStatusKey.Key == 1)
                    Users.Add(usr);
            }
            if (Users.Count == 0)
                throw new Exception(string.Format("No users to assign case:{0}, DynamicRole:{1}{2} are you sure there are active users?",
                    GenericKey, DynamicRole, Environment.NewLine));
            Users.Sort(new ADUserSort());
            LegalEntity_DAO LE = null;
            if (null == LastUserAssigned)
            {
                LE = Users[0].LegalEntity;
            }
            else
            {
                int idx = -1;
                for (int i = 0; i < Users.Count; i++)
                {
                    if (Users[i].Key == LastUserAssigned.Key)
                    {
                        idx = i;
                        break;
                    }
                }
                if ((idx == -1) || (idx == (Users.Count - 1)))
                {
                    LE = Users[0].LegalEntity;
                }
                else
                {
                    LE = Users[(idx + 1)].LegalEntity;
                }
            }

            // create an offerrole 
            WorkflowRepository.Instance().CreateAndSaveApplicationRoleDAO(GenericKey, rt.Description, LE.Key);
            string s = WorkflowRepository.GetADUserForLegalEntityKey(LE.Key).ADUserName;
            return s;
        }
        protected DataTable GetCurrentConsultantAndAdmin(int ApplicationKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select le.EMailAddress, orr.offerroletypekey ");
            sb.AppendLine("from [2am]..offerrole orr (nolock) ");
            sb.AppendLine("join [2am]..legalentity le (nolock) on orr.legalentitykey=le.legalentitykey ");
            sb.AppendLine("join [2am]..aduser a (nolock) on a.legalentitykey=le.legalentitykey ");
            sb.AppendFormat("where orr.offerroletypekey=101 and orr.offerkey={0} ", ApplicationKey);
            sb.AppendLine("union ");
            sb.AppendLine("select le.EMailAddress, orr.offerroletypekey ");
            sb.AppendLine("from [2am]..offerrole orr (nolock) ");
            sb.AppendLine("join [2am]..legalentity le (nolock) on orr.legalentitykey=le.legalentitykey ");
            sb.AppendLine("join [2am]..aduser a (nolock) on a.legalentitykey=le.legalentitykey ");
            sb.AppendFormat("where orr.offerroletypekey=102 and orr.offerkey={0} ", ApplicationKey);
            sb.AppendLine("union ");
            sb.AppendLine("select le.EMailAddress, orr.offerroletypekey ");
            sb.AppendLine("from [2am]..offerrole orr (nolock) ");
            sb.AppendLine("join [2am]..legalentity le (nolock) on orr.legalentitykey=le.legalentitykey ");
            sb.AppendLine("join [2am]..aduser a (nolock) on a.legalentitykey=le.legalentitykey ");
            sb.AppendFormat("where orr.offerroletypekey=857 and orr.offerkey={0} ", ApplicationKey);
            DataSet ds = Helpers.ExecuteQueryOnCastleTran(sb.ToString(), typeof(GeneralStatus_DAO));
            
            
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception(string.Format("No Branch Consultant D or Admin D or FL Processor D Roles found for case:{0}", ApplicationKey));
            }
            else
            {
                DataTable dt = ds.Tables[0];
                return dt;
            }
        }
        protected void ReassignOrEscalateCaseToUser(int GenericKey, string DynamicRole, string ADUser, bool MarkPreviousRoleAsInactive, ApplicationRole_DAO PreviousRole)
        {
            Application_DAO app = Application_DAO.Find(GenericKey);
            ADUser_DAO user = WorkflowRepository.GetADUSerByName(ADUser);

            WorkflowRepository.Instance().CreateAndSaveApplicationRoleDAO(GenericKey, DynamicRole, user.LegalEntity.Key);
            if (MarkPreviousRoleAsInactive)
            {
                if (null == PreviousRole)
                {
                    return;
                }
                WorkflowRepository.Instance().MarkRoleAsInactive(PreviousRole);
            }
        }


        internal bool IsThereExistingActiveRoleForThisRoleType(string DynamicRole, int GenericKey, out string AssignedUser)
        {
            AssignedUser = string.Empty;
            try
            {
                // Check to see if we have an existing offerrole for this applicaiton/dynamic role
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select LegalEntityKey ");
                sb.AppendLine("from [2am]..offerrole orr (nolock) ");
                sb.AppendLine("join [2am]..offerroletype ort (nolock) on orr.offerroletypekey=ort.offerroletypekey ");
                sb.AppendFormat("where ort.description='{0}' and offerkey={1} and GeneralStatusKey=1 ",
                    DynamicRole, GenericKey);
                DataSet ds = Helpers.ExecuteQueryOnCastleTran(sb.ToString(), typeof(GeneralStatus_DAO));
                int LegalEntityKey = -1;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LegalEntityKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    // Check this LE is still part of the correct org structure.
                    sb = new StringBuilder();
                    sb.AppendLine("select a.adusername, os.Description ");
                    sb.AppendLine("from [2am]..aduser a (nolock) ");
                    sb.AppendLine("join [2am]..UserOrganisationStructure uos (nolock) on a.AdUserKey=uos.AdUserKey ");
                    sb.AppendLine("join [2am]..organisationstructure os (nolock) on uos.OrganisationStructureKey = os.OrganisationStructureKey ");
                    sb.AppendLine("join [2am]..OfferRoleTypeOrganisationStructureMapping bla (nolock) on bla.OrganisationStructureKey=os.OrganisationStructureKey ");
                    sb.AppendLine("join [2am]..offerroletype ort (nolock) on bla.offerroletypekey=ort.offerroletypekey ");
                    sb.AppendFormat("where a.legalentitykey={0} and ort.description='{1}' ",
                        LegalEntityKey, DynamicRole);
                    ds = Helpers.ExecuteQueryOnCastleTran(sb.ToString(), typeof(GeneralStatus_DAO));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        AssignedUser = ds.Tables[0].Rows[0][0].ToString();
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogPlugin.LogWarning("Unable to check for existing offerrole:{0} {1}{2}{3}",
                    GenericKey, DynamicRole, Environment.NewLine, ex.ToString());
            }
            return false;
        }
        /// <summary>
        /// Round robin assigns cases to uses in the given org structure. Allows a specific org structure to be targeted. In the case of branches, 
        /// "Branch Consultant D" is mapped to every branch. A simple RoundRobin would not know which branch to perform the assign in.
        /// For Direct / TeleCentre we need to be able to specify
        /// </summary>
        /// <param name="DynamicRole"></param>
        /// <param name="GenericKey"></param>
        /// <param name="OrgStructureKey">Dynamic roles map to n OrgStructures. This allows us to pinpoint the exact one.</param>
        /// <returns></returns>
        protected string RoundRobinAssignForGivenOrgStructure(string DynamicRole, int GenericKey, int OrgStructureKey)
        {
            List<int> OSKeys = new List<int>();
            OSKeys.Add(OrgStructureKey);
            return RoundRobinAssignForGivenOrgStructure(DynamicRole, GenericKey, OSKeys);
        }
        

        public IX2ReturnData GetFollowupTime(int MemoKey)
        {
            DateTime FollowupTime = DateTime.Now;
            using (new SessionScope())
            {
                try
                {
                    string sql = string.Format("select m.ReminderDate from Memo m (nolock) where m.MemoKey={0}", MemoKey);
                    DataSet ds = Helpers.ExecuteQueryOnCastleTran(sql, typeof(Memo_DAO));
                    //Memo_DAO memo = Memo_DAO.Find(MemoKey);
                    //FollowupTime = (DateTime)memo.ReminderDate;
                    if (null != ds || ds.Tables[0].Rows.Count > 0)
                    {
                        FollowupTime = Convert.ToDateTime(ds.Tables[0].Rows[0][0]);
                    }
                }
                catch (Exception ex)
                {
                    LogPlugin.LogError("Unable to GetFollowupTime() {0}", ex.ToString());
                }
            }
            return new X2ReturnData(HandleX2Messages(), FollowupTime);
        }

        public IX2ReturnData UpdateOfferStatus(IActiveDataTransaction Tran, int ApplicationKey, int OfferStatus, int OfferInformationType)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
                {
                    try
                    {
                        Application_DAO app = null;
                        // Using DAO here so rules dont run.
                        if (OfferInformationType != -1)
                        {
                            app = Application_DAO.Find(ApplicationKey);
                            app.ApplicationStatus = ApplicationStatus_DAO.Find(OfferStatus);
                            ApplicationInformation_DAO info = WorkflowRepository.GetLatestApplicationInformationForAppKey(ApplicationKey);
                            info.ApplicationInformationType = ApplicationInformationType_DAO.Find(OfferInformationType);
                            WorkflowRepository.Instance().SaveApplicationInformation(info);
                            WorkflowRepository.Instance().SaveApplication(app);
                        }
                        else
                        {
                            app = Application_DAO.Find(ApplicationKey);
                            app.ApplicationStatus = ApplicationStatus_DAO.Find(OfferStatus);
                            WorkflowRepository.Instance().SaveApplication(app);
                            //string SQL = string.Format("Update [2am]..offer set OfferStatusKey={0} where OfferKey={1}",OfferStatus, ApplicationKey);
                            //Helpers.ExecuteNonQueryOnCastleTran(SQL, typeof(GeneralStatus_DAO));
                        }

                        ts.VoteCommit();
                        // this cases EX in this app if not commented. If commented then the caller cant cmmit
                        //scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to UpdateOfferStatus({1}) {0}", ex.ToString(), ApplicationKey);
                    }
                }
                // this cases EX in this app if not commented. If commented then the caller cant cmmit
                scope.Complete();
            }
            return new X2ReturnData(HandleX2Messages(), null);
        }
        public IX2ReturnData SetOfferEndDate(IActiveDataTransaction Tran, int ApplicationKey)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope(TransactionMode.Inherits))
                {
                    try
                    {
                        Application_DAO app = Application_DAO.Find(ApplicationKey);
                        app.ApplicationEndDate = DateTime.Now;
                        app.SaveAndFlush();
                        ts.VoteCommit();
                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to SetOfferEndDate() {0} AID:{1}", ex.ToString(), ApplicationKey);
                    }
                }
                scope.Complete();
            }
            return new X2ReturnData(HandleX2Messages(), null);
        }

        protected bool SendInternalMail(string ToAddress, string CC, string BCC, string Subject, string Body)
        {
            try
            {
                IMessageService messageService = ServiceFactory.GetService<IMessageService>();
                messageService.SendEmailInternal("HALO@SAHomeloans.com", ToAddress, CC, BCC, Subject, Body);
            }
            catch (SmtpException sme)
            {
                string s = sme.ToString();
                LogPlugin.LogError("Unable to send mail{0}{1}", Environment.NewLine, sme.ToString());
            }
            return false;
        }

        protected void ArchiveValuationsFromSourceInstanceID(Int64 InstanceID, string ADUser, int ApplicationKey)
        {
            // Use the InstanceID to locate cases whose Source InstanceID is InstanceID.ID
            // Check the case is in the Valuations workflow (It should always be just just in case)
            // Look for Kids of the case you find. (Only one possible in Valuations)
            // DONT ARCHIVE THE CHILD at this stage (phase 1 august spet 2008) we are going to let the
            // Valuation complete from adcheck
            // Archive the parent.
            // Dont archive the Valuations Hold case this will be done externally
            Instance_DAO[] iids = Instance_DAO.FindAllByProperty("SourceInstanceID", InstanceID);
            foreach (Instance_DAO iid in iids)
            {
                if (iid.WorkFlow.Name.ToUpper() == "VALUATIONS")
                {
                    Instance_DAO[] Brats = Instance_DAO.FindAllByProperty("ParentInstance.ID", iid.ID);
                    WorkFlow_DAO w = WorkflowRepository.GetWorkFlowByName(Val, Origination);
                    foreach (Instance_DAO Screaming in Brats)
                    {
                        //LogPlugin.LogInfo("Archiving Valuations Child Case:{0} Parent:{1}", Screaming.ID, iid.ID);
                        //WorkflowRepository.Instance().CreateAndSaveActiveExternalActivity("EXTCleanupArchive", Screaming.ID, w.ID, null);
                    }
                    // Archive the valuations case
                    LogPlugin.LogInfo("Archiving Valuations Case:{0}", iid.ID);
                    WorkflowRepository.Instance().CreateAndSaveActiveExternalActivity("EXTCleanupArchive", iid.ID, w.ID, null);
                }
                else
                {
                    LogPlugin.LogError("Valuation Hold Instance:{0} is somehow the parent case for IID:{1} in WF:{2}"
                    , InstanceID, iid.ID, iid.WorkFlow.Name);
                }
            }
            // Dont archive the Valuations Hold case this will be done externally
        }
        protected void ArchiveQuickCashFromSourceInstanceID(Int64 InstanceID, string ADUser, int ApplicationKey)
        {
            // Look at this later when we get to QC
            Instance_DAO[] iids = Instance_DAO.FindAllByProperty("SourceInstanceID", InstanceID);
            foreach (Instance_DAO iid in iids)
            {
                if (iid.WorkFlow.Name.ToUpper() == "QUICK CASH")
                {
                    // archive them look at brats etc
                    LogPlugin.LogInfo("QC Case:{0} CLone:{1}", InstanceID, iid.ID);
                }
                else
                {
                    LogPlugin.LogError("Quick Cash Hold Instance:{0} is somehow the parent case for IID:{1} in WF:{2}"
                    , InstanceID, iid.ID, iid.WorkFlow.Name);
                }
            }
        }

        protected void UpdateChildVars(Int64 ParentInstanceID, Dictionary<string, object> dict, IActiveDataTransaction Tran)
        {
            Helpers.SetX2DataRow(ParentInstanceID, dict, Tran);
            // get the screaming children
            Instance_DAO[] kids = Instance_DAO.FindAllByProperty("ParentInstance.ID", ParentInstanceID);
            // update their adcheckvaluationidstatus's to match the parent.
            foreach (Instance_DAO kid in kids)
            {
                Helpers.SetX2DataRow(kid.ID, dict, Tran);
            }
        }
        
    }
}
