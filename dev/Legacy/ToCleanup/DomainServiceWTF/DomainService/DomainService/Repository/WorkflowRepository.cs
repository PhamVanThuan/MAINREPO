using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.X2.BusinessModel.DAO;
using System.Data;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.X2.Common.DataAccess;
using SAHL.Common.Security;
using SAHL.Common.Globals;
using SAHL.X2.Framework.DataAccess;
using DomainService.Workflow;
using SAHL.Common.CacheData;

namespace DomainService.Repository
{
    internal class ADUserSort : IComparer<ADUser_DAO>
    {
        #region IComparer<ADUser_DAO> Members

        public int Compare(ADUser_DAO x, ADUser_DAO y)
        {
            return (x.ADUserName.CompareTo(y.ADUserName));
        }

        #endregion
    }

    /// <summary>
    /// Allows the workflow to save objects without being constrained by the rules. This class should be used for updating object status's
    /// Doing offerrole assignment. It should NOT be used to save objects where the business process would have the rules run normally.
    /// Example getting a valuation back from AdCheck.
    /// The RuleService being a singleton
    /// </summary>
    public class WorkflowRepository : IDisposable
    {
        private static WorkflowRepository Ref = null;
        private static object syncObj = new object();
        bool Disposing = false;

        IRuleService svc = null;
        private WorkflowRepository()
        {
            svc = ServiceFactory.GetService<IRuleService>();
            svc.Enabled = true;
        }


        public static WorkflowRepository Instance()
        {
            if (null == Ref)
            {
                lock (syncObj)
                {
                    WorkflowRepository tmp = new WorkflowRepository();
                    if (null != tmp)
                    {
                        Ref = tmp;
                        tmp = null;
                    }
                }
            }
            return Ref;
        }

        public void Dispose()
        {
            Ref = null;
        }

        /// <summary>
        /// Saves and External activity that the engine will pickup and execute (FLAG activities)
        /// </summary>
        /// <param name="activity"></param>
        public void SaveApplicationRole(ApplicationRole_DAO role)
        {
            try
            {
                role.SaveAndFlush();
                //                StringBuilder sb = new StringBuilder();
                //                if (role.Key > 0)
                //                {
                //                    // Update
                //                    sb.AppendFormat("update [2am]..offerrole set LegalEntityKey={0}, ", role.LegalEntityKey);
                //                    sb.AppendFormat("StatusChangeDate=getdate(), ");
                //                    sb.AppendFormat("GeneralStatusKey={0}", role.GeneralStatus.Key);
                //                    sb.AppendFormat("where OfferRoleKey={0}", role.Key);
                //                }
                //                else
                //                {
                //                    // Insert
                //                    sb.AppendFormat("insert into [2am]..offerRole values (");
                //                    sb.AppendFormat("{0}, {1}, {2}, {3}, getdate())", role.LegalEntityKey,
                //                        role.ApplicationKey, role.ApplicationRoleType.Key, role.GeneralStatus.Key);
                //                }

                //                Helpers.ExecuteNonQueryOnCastleTran(sb.ToString(), typeof(GeneralStatus_DAO));

                //                // store Audit
                //                string query = @"INSERT INTO [Warehouse].[dbo].[Audits]
                //           ([AuditDate],[ApplicationName],[HostName],[WorkStationID],[WindowsLogon]
                //           ,[FormName],[TableName],[PrimaryKeyName],[PrimaryKeyValue],[AuditData])
                //     VALUES(getdate(), 'DomainService', 'sahls118', '1', 'sahl\bla', 'form', 'OfferRole', 
                //            'OfferRoleKey', '" + role.Key + "', '<XML>')";
                //                Helpers.ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO));
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }

        public void SaveApplication(Application_DAO app)
        {
            app.SaveAndFlush();
        }

        public void SaveApplicationInformation(ApplicationInformation_DAO info)
        {
            info.SaveAndFlush();
        }

        //public void SaveDetail(Detail_DAO detail)
        //{
        //    IDAOObject dao = detail as IDAOObject;
        //    Detail_DAO det = (Detail_DAO)dao.GetDAOObject();
        //    det.SaveAndFlush();
        //}

        public void SaveAccount(Account_DAO Account)
        {
            Account.SaveAndFlush();
        }
        public void SaveMemo(Memo_DAO m)
        {
            m.SaveAndFlush();
        }
        public void MarkRoleAsInactive(int PrevAppRoleKey)
        {
            ApplicationRole_DAO role = ApplicationRole_DAO.Find(PrevAppRoleKey);
            role.GeneralStatus = GeneralStatus_DAO.Find(2);
            role.SaveAndFlush();
        }
        public void MarkRoleAsInactive(ApplicationRole_DAO role)
        {
            role.GeneralStatus = WorkflowRepository.GetGeneralStatusDAOByName("Inactive");// RepositoryFactory.GetRepository<ILookupRepository>().GeneralStatuses.ObjectDictionary["2"];
            SaveApplicationRole(role);
        }

        //public void SaveValuationWithoutrules(IValuation valuation)
        //{
        //    Valuation_DAO dao = (Valuation_DAO)((IDAOObject)valuation).GetDAOObject();

        //    if (valuation.IsActive)
        //    {
        //        //make sure any other active valuations are deactivated
        //        if (dao.Property.Valuations != null)
        //        {
        //            // add the rule exclusion set
        //            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
        //            spc.ExclusionSets.Add(RuleExclusionSets.ValuationUpdatePreviousToInactive);
        //            for (int i = 0; i < dao.Property.Valuations.Count; i++)
        //            {
        //                Valuation_DAO valDAO = dao.Property.Valuations[i];
        //                // dont update the current valuation record that we are saving - this will be done outside the loop further down
        //                if (valDAO.IsActive && valDAO.Key != valuation.Key)
        //                {
        //                    valDAO.IsActive = false;
        //                    valDAO.SaveAndFlush();
        //                }
        //            }
        //            // remove the rule exclusion set
        //            spc.ExclusionSets.Remove(RuleExclusionSets.ValuationUpdatePreviousToInactive);
        //        }

        //        //have to make sure we update PropertyValuation in OfferInformationVariableLoan if IsActive is set
        //        string HQL = "select ai from ApplicationMortgageLoanDetail_DAO amld join amld.Application.ApplicationInformations ai where amld.Property.Key = ? and amld.Application.ApplicationStatus.Key = 1 order by ai.Key desc";
        //        SimpleQuery<ApplicationInformation_DAO> q = new SimpleQuery<ApplicationInformation_DAO>(HQL, valuation.Property.Key);
        //        ApplicationInformation_DAO[] res = q.Execute();

        //        if (res.Length > 0)
        //        {
        //            foreach (ApplicationInformation_DAO aiDAO in res) //Further Lending could have more than one open application
        //            {
        //                aiDAO.ApplicationInformationVariableLoan.PropertyValuation = valuation.ValuationAmount;

        //                // Change in active valuation must result in recalc of application detail becuase of the change in risk (LTV)
        //                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

        //                IApplication App = BMTM.GetMappedType<IApplication>(aiDAO.Application);

        //                IApplicationMortgageLoan appML = App as IApplicationMortgageLoan;
        //                if (appML != null)
        //                    appML.CalculateApplicationDetail();

        //                aiDAO.ApplicationInformationVariableLoan.SaveAndFlush();
        //            }
        //        }

        //        dao.IsActive = true;
        //    }

        //    dao.SaveAndFlush();
        //}

        /// <summary>
        /// Creates a new offerrole record for an application for a given roletype and aduser
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="ORT"></param>
        /// <param name="ADUserName"></param>
        public void CreateAndSaveApplicationRoleDAO(int ApplicationKey, string ORT, string ADUserName)
        {
            ApplicationRole_DAO role = new ApplicationRole_DAO();
            role.ApplicationKey = ApplicationKey;
            role.ApplicationRoleType = GetOfferRoleTypeByName(ORT);
            role.GeneralStatus = GetGeneralStatusDAOByName("Active");
            role.LegalEntityKey = GetADUSerByName(ADUserName).LegalEntity.Key;
            role.StatusChangeDate = DateTime.Now;
            role.SaveAndFlush();
        }
        /// <summary>
        /// Creates a new offerrole record for an application for a given roletype and LegalEntityKey
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="ORT"></param>
        /// <param name="LEKey"></param>
        public void CreateAndSaveApplicationRoleDAO(int ApplicationKey, string ORT, int LEKey)
        {
            ApplicationRole_DAO role = new ApplicationRole_DAO();
            role.ApplicationKey = ApplicationKey;
            role.ApplicationRoleType = GetOfferRoleTypeByName(ORT);
            role.GeneralStatus = GetGeneralStatusDAOByName("Active");
            role.LegalEntityKey = LEKey;
            role.StatusChangeDate = DateTime.Now;
            //SaveApplicationRole(role);
            role.SaveAndFlush();
        }


        /// <summary>
        /// Inserts a ercord into the X2.ActiveExternalActivity Table. This will be picked up by the engine and executed. Used to 
        /// create cases in remote workflows (Submit applicaiton in App Man an example) or to pickup a case and move it within 
        /// a workflow (App Man. Clone case sits with a 15 day timer. The parent raises an EXT activity to archive the child)
        /// </summary>
        /// <param name="ExtActivityName"></param>
        /// <param name="ActivatingInstanceID"></param>
        /// <param name="WorkflowID"></param>
        /// <param name="XMLFieldInputs"></param>
        public void CreateAndSaveActiveExternalActivity(string ExtActivityName, Int64 ActivatingInstanceID, int WorkflowID, string XMLFieldInputs)
        {
            ActiveExternalActivity_DAO dao = new ActiveExternalActivity_DAO();
            dao.ActivatingInstanceID = ActivatingInstanceID;
            dao.ActivationTime = DateTime.Now;
            dao.ActivityXMLData = XMLFieldInputs;
            dao.ExternalActivity = GetExternalActivityByName(ExtActivityName);
            dao.WorkFlowID = WorkflowID;
            dao.WorkFlowProviderName = "";
            dao.SaveAndFlush();
        }


        #region Get Helpers
        #region Application Role and ADUser
        /// <summary>
        /// User the description field to search the offerroletype table for a given description. We can assume that there will always be one
        /// so return the first one.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        internal static ApplicationRoleType_DAO GetOfferRoleTypeByName(string Name)
        {
            ApplicationRoleType_DAO[] arr = ApplicationRoleType_DAO.FindAllByProperty("Description", Name);
            if (null == arr || arr.Length == 0)
                throw new Exception(string.Format("Unable to load offerroletype:{0}", Name));
            return arr[arr.Length - 1];
        }
        /// <summary>
        /// User the description field to search the GeneralStatus table for a given description. We can assume that there will always be one
        /// so return the first one.
        /// </summary>
        /// <param name="Name">Value in the Description Field. There will only be Active and Inactive</param>
        /// <returns></returns>
        internal static GeneralStatus_DAO GetGeneralStatusDAOByName(string Name)
        {
            GeneralStatus_DAO[] arr = GeneralStatus_DAO.FindAllByProperty("Description", Name);
            if (null == arr || arr.Length == 0)
                throw new Exception(string.Format("Unable to load GeneralStatus:{0}", Name));
            return arr[arr.Length - 1];
        }
        /// <summary>
        /// User the description field to search the GeneralStatus table for a given description. We can assume that there will always be one
        /// so return the first one.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        internal static ADUser_DAO GetADUSerByName(string Name)
        {
            ADUser_DAO[] arr = ADUser_DAO.FindAllByProperty("ADUserName", Name);
            if (null == arr || arr.Length == 0)
                throw new Exception(string.Format("Unable to load ADUser:{0}", Name));
            return arr[arr.Length - 1];
        }
        /// <summary>
        /// Loads an aduser given the legalentitykey. Useful when you have an offerrole record that has an LE key and you 
        /// need to get the corresponding ADUser
        /// </summary>
        /// <param name="LEKey"></param>
        /// <returns></returns>
        internal static ADUser_DAO GetADUserForLegalEntityKey(int LEKey)
        {
            string HQL = "from ADUser_DAO o where o.LegalEntity.Key=?";
            SimpleQuery<ADUser_DAO> query = new SimpleQuery<ADUser_DAO>(HQL, LEKey);
            ADUser_DAO[] arr = query.Execute();
            if (null == arr || arr.Length == 0)
                throw new Exception(string.Format("Unable to load ADUser for LEKey:{0}", LEKey));
            return arr[arr.Length - 1];
        }
        internal static ApplicationRole_DAO GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(int ApplicationKey, string OfferRoleTypeName)
        {
            return GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(ApplicationKey, GetOfferRoleTypeByName(OfferRoleTypeName).Key);
        }
        /// <summary>
        /// Looks at the offerrole records for a given offerkey and offerroletypekey where the records are ACTIVE and returns the latest
        /// entry
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="ApplicationRoleTypeKey"></param>
        /// <returns></returns>
        internal static ApplicationRole_DAO GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(int ApplicationKey, int ApplicationRoleTypeKey)
        {
            string HQL = "from ApplicationRole_DAO o where o.ApplicationKey=? and o.ApplicationRoleType.Key=? and o.GeneralStatus.Key=1 order by o.Key desc";
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationKey, ApplicationRoleTypeKey);
            q.SetQueryRange(5);
            ApplicationRole_DAO[] res = q.Execute();
            if (null == res || res.Length == 0)
            {
                return null;
                //throw new Exception(string.Format("No Application Roles found for AppKey:{0} OfferRoleTypeKey:{1} Check that the group has members", ApplicationKey, ApplicationRoleTypeKey));
            }
            return res[res.GetLowerBound(0)];
        }
        /// <summary>
        /// Looks at the offerrole records for a given offerkey and offerroletypekey where the records are of any status and returns the latest
        /// entry
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="ApplicationRoleTypeKey"></param>
        /// <returns></returns>
        internal static ApplicationRole_DAO GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyForAnyStatus(int ApplicationKey, int ApplicationRoleTypeKey)
        {
            string HQL = "from ApplicationRole_DAO o where o.ApplicationKey = ? and o.ApplicationRoleType.Key=? order by o.Key desc";
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationKey, ApplicationRoleTypeKey);
            q.SetQueryRange(5);
            ApplicationRole_DAO[] res = q.Execute();
            if (null == res || res.Length == 0)
            {
                return null;
                //throw new Exception(string.Format("No Application Roles found for AppKey:{0} OfferRoleTypeKey:{1} Check that the group has members", ApplicationKey, ApplicationRoleTypeKey));
            }
            return res[res.Length - 1];
        }
        /// <summary>
        /// Gets all offerrole records for an application where the offerroletype is in the list specified.
        /// </summary>
        /// <param name="Keys"></param>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        internal static ApplicationRole_DAO[] GetApplicationRoleTypesForKeys(List<int> Keys, int ApplicationKey)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Keys.Count; i++)
            {
                sb.AppendFormat(",{0}", Keys[i]);
            }
            sb.Remove(0, 1);
            // order by the offerroletypekey they are ordered from senior to least senior
            string HQL = string.Format("from ApplicationRole_DAO d where d.ApplicationKey=? and d.ApplicationRoleType.Key in ({0}) order by d.ApplicationRoleType.Key", sb.ToString());
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationKey);
            ApplicationRole_DAO[] arr = q.Execute();
            return arr;
        }
        /// <summary>
        /// Gets the LATEST offerrole records for a specific offerrole type where the legalentity in the offerrole is part of a list provided.
        /// 
        /// </summary>
        /// <param name="OfferRoleTypeKey"></param>
        /// <param name="LEKeys"></param>
        /// <returns></returns>
        internal static ApplicationRole_DAO GetLastApplicationRoleByApplicationRoleTypeKeyAndLEKeys(int OfferRoleTypeKey, List<int> LEKeys)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int i in LEKeys)
            {
                sb.AppendFormat(",{0}", i);
            }
            sb.Remove(0, 1);
            string HQL = string.Format("from ApplicationRole_DAO o where o.ApplicationRoleType.Key=? and o.GeneralStatus.Key=1 and o.LegalEntityKey in ({0}) order by o.Key desc", sb.ToString());
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, OfferRoleTypeKey);
            q.SetQueryRange(5);
            ApplicationRole_DAO[] res = q.Execute();
            if (null == res || res.Length == 0)
            {
                throw new Exception(string.Format("No Application Roles found for OfferRoleTypeKey:{0}, LEKeys:{1}", OfferRoleTypeKey, sb));
            }
            return res[res.Length - 1];
        }
        /// <summary>
        /// Gets the latest x offerrole records for a given list of users. Used when performing round robins
        /// </summary>
        /// <param name="LEKeys"></param>
        /// <returns></returns>
        internal static ApplicationRole_DAO[] GetTopXApplicationRolesForLEKeys(List<int> LEKeys, int count)
        {
            string HQL = string.Format("from ApplicationRole_DAO o where o.LegalEntityKey in (:lekeys) order by o.StatusChangeDate desc");
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL);
            q.SetParameterList("lekeys", LEKeys);
            q.SetQueryRange(count);
            ApplicationRole_DAO[] res = q.Execute();
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OfferRoleTypeKey"></param>
        /// <param name="LEKeys"></param>
        /// <returns></returns>
        internal static ApplicationRole_DAO FindLastApplicationRoleByApplicationRoleTypeKeyAndLEKeys(int OfferRoleTypeKey, List<int> LEKeys)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int i in LEKeys)
            {
                sb.AppendFormat(",{0}", i);
            }
            sb.Remove(0, 1);
            //string HQL = string.Format("from ApplicationRole_DAO o where o.ApplicationRoleType.Key=? and o.GeneralStatus.Key=1 and o.LegalEntityKey in ({0}) order by o.Key desc", sb.ToString());
            string HQL = string.Format("from ApplicationRole_DAO o where o.ApplicationRoleType.Key=? and o.LegalEntityKey in ({0}) order by o.Key desc", sb.ToString());
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, OfferRoleTypeKey);
            q.SetQueryRange(5);
            ApplicationRole_DAO[] res = q.Execute();
            if (res.Length == 0)
            {
                //throw new Exception(string.Format("No Application Roles found for AppKey:{0} OfferRoleTypeKey:{1} Check that the group has members", ApplicationKey, OfferRoleTypeKey));
                return null;
            }
            return res[0];
        }
        #endregion

        #region Application
        /// <summary>
        /// Gets a list of offerinformation records adn returns the latest one
        /// </summary>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        internal static ApplicationInformation_DAO GetLatestApplicationInformationForAppKey(int ApplicationKey)
        {
            ApplicationInformation_DAO[] arr = ApplicationInformation_DAO.FindAllByProperty("Key", "Application.Key", ApplicationKey);
            return arr[arr.Length - 1];
        }
        /// <summary>
        /// Gets the application roles for an application for SAHL roles (IE NOT Main APP, SURETOR, ATTORNEY etc)
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        internal static ApplicationRole_DAO[] GetApplicationRolesForKey(int ApplicationKey)
        {
            string HQL = "from ApplicationRole_DAO d where d.ApplicationKey=? and d.ApplicationRoleType.ApplicationRoleTypeGroup.Key = 1 order by d.Key desc";
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationKey);
            ApplicationRole_DAO[] arr = q.Execute();
            return arr;
        }
        internal static ApplicationRole_DAO[] GetApplicationRolesForKeyAndApplicationRoleTypeKey(int ApplicationKey, List<int> ApplicationRoleTypeKeys)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ApplicationRoleTypeKeys.Count; i++)
            {
                sb.AppendFormat("{0},", ApplicationRoleTypeKeys[i]);
            }
            sb.Remove(sb.Length - 1, 1);
            ApplicationRole_DAO d;

            string HQL = string.Format("from ApplicationRole_DAO d where d.ApplicationKey=? and d.ApplicationRoleType.ApplicationRoleTypeGroup.Key = 1 and d.ApplicationRoleType.Key in ({0}) order by d.Key desc", sb.ToString());
            SimpleQuery<ApplicationRole_DAO> q = new SimpleQuery<ApplicationRole_DAO>(HQL, ApplicationKey);
            ApplicationRole_DAO[] arr = q.Execute();
            return arr;
        }
        #endregion

        #region OrgStructure
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        internal static OrganisationStructure_DAO GetOrgStructureByName(string Name)
        {
            OrganisationStructure_DAO[] daos = OrganisationStructure_DAO.FindAllByProperty("Description", Name);
            return daos[daos.Length - 1];
        }
        /// <summary>
        /// loop through the orgstructures for this app role and find the user we were passed in the org structure.
        /// start by going to the parent and looking in all of the parents children
        /// heres why
        /// you looking for a consultant but you are an admin. There will never be an intersection between Branch Consultant D and Admin
        /// Three will be an intersection of Branch Consultant D and Consultant in OS
        /// SO 
        /// Goto the parent of the Consultant OS and loop through all the parents children. Althoguh it will mena that
        /// we search Admin, Consultant and Manager it does mean we will find the intersection to the branch.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orgStructure"></param>
        /// <param name="Prefix"></param>
        /// <returns></returns>
        internal static ADUser_DAO[] GetBranchUsersForUserInThisBranch(ADUser_DAO user, OrganisationStructureGroup orgStructure, string Prefix)
        {
            ApplicationRoleType_DAO art = null;
            switch (orgStructure)
            {
                case OrganisationStructureGroup.Consultant:
                    {
                        art = GetOfferRoleTypeByName(string.Format("{0} Consultant D", Prefix));
                        break;
                    }
                case OrganisationStructureGroup.Admin:
                    {
                        art = GetOfferRoleTypeByName(string.Format("{0} Admin D", Prefix));
                        break;
                    }
                case OrganisationStructureGroup.Manager:
                    {
                        art = GetOfferRoleTypeByName(string.Format("{0} Manager D", Prefix));
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }

            // loop through the orgstructures for this app role and find the user we were passed in the org structure.
            // start by going to the parent and looking in all of the parents children
            // heres why
            // you looking for a consultant but you are an admin. There will never be an intersection between Branch Consultant D and Admin
            // Three will be an intersection of Branch Consultant D and Consultant in OS
            // SO 
            // Goto the parent of the Consultant OS and loop through all the parents children. Althoguh it will mena that
            // we search Admin, Consultant and Manager it does mean we will find the intersection to the branch.
            foreach (OrganisationStructure_DAO os in art.OfferRoleTypeOrganisationStructures)
            {
                // loop throught the children of this groups parents. This will search the current and all on the same level
                foreach (OrganisationStructure_DAO osTmp in os.Parent.ChildOrganisationStructures)
                {
                    // check if the user we are looking for is in the current group
                    foreach (ADUser_DAO tUsr in osTmp.ADUsers)
                    {
                        if (tUsr.Key == user.Key)
                        {
                            // we have found the aduser. We dont care if he is a Consultant, Admin or Manager all we want
                            // is the list of consultants. Go to the parent and loop through the children looking for "Admin"
                            foreach (OrganisationStructure_DAO osTmp1 in os.Parent.ChildOrganisationStructures)
                            {
                                if (osTmp1.Description == orgStructure.ToString())
                                {
                                    ADUser_DAO[] arr = new ADUser_DAO[osTmp1.ADUsers.Count];
                                    osTmp1.ADUsers.CopyTo(arr, 0);
                                    return arr;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region X2
        /// <summary>
        /// Gets the latest workflow object for a process and workflowname.
        /// </summary>
        /// <param name="WorkFlowName"></param>
        /// <param name="ProcessName"></param>
        /// <returns></returns>
        internal static WorkFlow_DAO GetWorkFlowByName(string WorkFlowName, string ProcessName)
        {
            string query = string.Format("from WorkFlow_DAO w where w.Process.Name = ? and w.Name = ?");
            SimpleQuery<WorkFlow_DAO> SQ = new SimpleQuery<WorkFlow_DAO>(query, ProcessName, WorkFlowName);
            WorkFlow_DAO[] arr = SQ.Execute();
            if (null == arr || arr.Length == 0)
                throw new Exception(string.Format("Unable to get Workflow for WF:{0} Proc:{1}", WorkFlowName, ProcessName));
            return arr[arr.Length - 1];
        }

        /// <summary>
        /// Gets and External Activity for the name field.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        internal static ExternalActivity_DAO GetExternalActivityByName(string Name)
        {
            ExternalActivity_DAO[] arr = ExternalActivity_DAO.FindAllByProperty("Name", Name);
            if (null == arr || arr.Length == 0)
                throw new Exception(string.Format("Unable to get ExternalActivity:{0}", Name));
            return arr[arr.Length - 1];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InstanceID"></param>
        /// <param name="ActivityName"></param>
        /// <returns></returns>
        internal static string GetUserWhoPerformedActivity(Int64 InstanceID, string ActivityName)
        {
            string HQL = "from WorkFlowHistory_DAO wfh where wfh.Activity.Name=? and wfh.InstanceID=? order by wfh.ID desc";
            SimpleQuery<WorkFlowHistory_DAO> query = new SimpleQuery<WorkFlowHistory_DAO>(HQL, ActivityName, InstanceID);
            WorkFlowHistory_DAO[] arr = query.Execute();
            if (null != arr && arr.Length > 0)
                return arr[arr.Length - 1].ADUserName;
            return "";
        }

        /// <summary>
        /// Gets an activity based on the name
        /// </summary>
        /// <param name="InstanceID"></param>
        /// <param name="ActivityName"></param>
        /// <returns></returns>
        internal static Activity_DAO GetActivityByName(string ActivityName)
        {
            string HQL = "from Activity_DAO d where d.Name=? order by d.id desc";
            SimpleQuery<Activity_DAO> q = new SimpleQuery<Activity_DAO>(HQL, ActivityName);
            q.SetQueryRange(1);
            Activity_DAO[] arr = q.Execute();
            if (null == arr || arr.Length == 0)
                throw new Exception(string.Format("Unable to find Activity with name {0}", ActivityName));
            return arr[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        internal static WorkFlowHistory_DAO[] GetHistoryForInstanceAndActivity(Instance_DAO ID, Activity_DAO activity)
        {
            // get activity name ... get all the id's that this activity has had in the past (each time we republish the id's get updated)
            // however the workflowhistory table ISNT updated with new ID's .. this is cause id's can be removed, renamed etc

            string HQL = "from Activity_DAO o where o.Name = ? order by o.id desc";
            SimpleQuery<Activity_DAO> aquery = new SimpleQuery<Activity_DAO>(HQL, activity.Name);
            Activity_DAO[] activities = aquery.Execute();
            // loop throgh the workflowhistory table looking for a match
            for (int i = 0; i < activities.Length; i++)
            {
                Activity_DAO a = activities[i];
                try
                {
                    HQL = "from WorkFlowHistory_DAO o where o.Activity.ID=? and o.InstanceID=? order by o.id desc";
                    SimpleQuery<WorkFlowHistory_DAO> query = new SimpleQuery<WorkFlowHistory_DAO>(HQL, a.ID, ID.ID);
                    query.SetQueryRange(5);
                    WorkFlowHistory_DAO[] arr = query.Execute();
                    if (arr.Length > 0)
                        return arr;
                }
                catch (Exception ex)
                {
                    string s = ex.ToString();
                }
            }
            return null;
        }
        /// <summary>
        /// Gets an instnace record for an offer key.
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="WorkflowName"></param>
        /// <param name="ProcessName"></param>
        /// <returns></returns>
        public static Instance_DAO GetInstanceForGenericKey(int GenericKey, string WorkflowName, string ProcessName)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            WorkFlow_DAO workFlow = GetWorkFlowByName(WorkflowName, ProcessName);

            string query = string.Format("SELECT top 10 InstanceID FROM X2.X2DATA.{0} (nolock) WHERE {1}={2} order by InstanceID", workFlow.StorageTable, workFlow.StorageKey, GenericKey);
            DataSet ds = Helpers.ExecuteQueryOnCastleTran(query, typeof(Instance_DAO));
            Dictionary<string, object> dict = new Dictionary<string, object>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                Int64 IID = Convert.ToInt64(ds.Tables[0].Rows[0][0]);
                Instance_DAO instance = Instance_DAO.Find(IID);
                return instance;
            }
            return null;
        }

        /// <summary>
        /// Returns the history audit trail for an instance.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        internal static WorkFlowHistory_DAO[] GetWorkflowHistoryForInstanceAndActivity(Instance_DAO instance, Activity_DAO activity)
        {
            string HQL = "";
            SimpleQuery<WorkFlowHistory_DAO> aquery = null;
            if (null != activity)
            {
                HQL = "from WorkFlowHistory_DAO o where o.InstanceID = ? and o.activity.ID = ? order by o.id desc";
                aquery = new SimpleQuery<WorkFlowHistory_DAO>(HQL, instance.ID, activity.ID);
            }
            else
            {
                HQL = "from WorkFlowHistory_DAO o where o.InstanceID = ? order by o.id desc";
                aquery = new SimpleQuery<WorkFlowHistory_DAO>(HQL, instance.ID);
            }

            WorkFlowHistory_DAO[] hist = aquery.Execute();
            return hist;
        }
        #endregion

        #region Mandates
        internal static AllocationMandateSetGroup_DAO[] GetAllocationMandatesForOrgStructureKeys(List<int> Keys)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Keys.Count; i++)
            {
                sb.AppendFormat(",{0}", Keys[i]);
            }
            sb.Remove(0, 1);
            string HQL = string.Format("from AllocationMandateSetGroup_DAO d where d.OrganisationStructure.Key in ({0})", sb.ToString());
            SimpleQuery<AllocationMandateSetGroup_DAO> query = new SimpleQuery<AllocationMandateSetGroup_DAO>(HQL);
            AllocationMandateSetGroup_DAO[] arr = query.Execute();
            return arr;
        }
        #endregion

        #endregion
    }

    public enum OrganisationStructureGroup
    {
        Consultant,
        Admin,
        Manager,
        Supervisor
    };
}
