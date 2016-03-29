using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.UI;
using SAHL.Web.Controls;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.Security;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Exceptions;
using SAHL.Common;
using SAHL.Web.Controls.Interfaces;
using System.Collections;


namespace SAHL.Web.Views.Common.Presenters.Employment
{
    /// <summary>
    /// Base class for views displaying legal entity employment information.
    /// </summary>
    public class LegalEntityEmploymentBase<T> : SAHLCommonBasePresenter<T>
    {
        private ILegalEntityRepository _legalEntityRepository;
        private IEmploymentRepository _employmentRepository;
        private ILookupRepository _lookupRepository;
        private ICommonRepository _commonRepository;
        public ICommonRepository _commRepo = RepositoryFactory.GetRepository<ICommonRepository>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="controller"></param>
        public LegalEntityEmploymentBase(T t, SAHLCommonBaseController controller)
            : base(t, controller)
        {
        }

        #region Properties


        /// <summary>
        /// Gets/sets an employer key from the global cache.
        /// </summary>
        protected int? CachedEmploymentConfirmationSourceKey
        {
            get
            {
                if (GlobalCacheData.ContainsKey(ViewConstants.EmploymentConfirmationSourceKey))
                    return Convert.ToInt32(GlobalCacheData[ViewConstants.EmploymentConfirmationSourceKey]);
                else
                    return new int?();
            }
            set
            {
                if (!value.HasValue)
                    GlobalCacheData.Remove(ViewConstants.EmploymentConfirmationSourceKey);
                else if (GlobalCacheData.ContainsKey(ViewConstants.EmploymentConfirmationSourceKey))
                    GlobalCacheData[ViewConstants.EmploymentConfirmationSourceKey] = value;
                else
                    GlobalCacheData.Add(ViewConstants.EmploymentConfirmationSourceKey, value, new List<ICacheObjectLifeTime>());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Presenter caches this collection.")]
        protected IList CachedDeleteEmploymentVerificationProcess
        {
            get 
            {
                if (GlobalCacheData.ContainsKey(ViewConstants.DeleteEmploymentVerificationProcessList))
                    return GlobalCacheData[ViewConstants.DeleteEmploymentVerificationProcessList] as IList;

                return null;
            }
            set 
            {
                if (value == null)
                    GlobalCacheData.Remove(ViewConstants.DeleteEmploymentVerificationProcessList);
                else if (GlobalCacheData.ContainsKey(ViewConstants.DeleteEmploymentVerificationProcessList))
                    GlobalCacheData[ViewConstants.DeleteEmploymentVerificationProcessList] = value;
                else
                    GlobalCacheData.Add(ViewConstants.DeleteEmploymentVerificationProcessList, value, new List<ICacheObjectLifeTime>());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Presenter caches this collection.")]
        protected IList CachedAddEmploymentVerificationProcess
        {
            get
            {
                if (GlobalCacheData.ContainsKey(ViewConstants.AddEmploymentVerificationProcessList))
                    return GlobalCacheData[ViewConstants.AddEmploymentVerificationProcessList] as IList;

                return null;
            }
            set
            {
                if (value == null)
                    GlobalCacheData.Remove(ViewConstants.AddEmploymentVerificationProcessList);
                else if (GlobalCacheData.ContainsKey(ViewConstants.AddEmploymentVerificationProcessList))
                    GlobalCacheData[ViewConstants.AddEmploymentVerificationProcessList] = value;
                else
                    GlobalCacheData.Add(ViewConstants.AddEmploymentVerificationProcessList, value, new List<ICacheObjectLifeTime>());
            }
        }

        /// <summary>
        /// Gets/sets an employment object from the global cache.
        /// </summary>
        protected IEmployment CachedEmployment
        {
            get
            {
                if (GlobalCacheData.ContainsKey(ViewConstants.Employment))
                    return GlobalCacheData[ViewConstants.Employment] as IEmployment;

                return null;
            }
            set
            {
                if (value == null)
                    GlobalCacheData.Remove(ViewConstants.Employment);
                else if (GlobalCacheData.ContainsKey(ViewConstants.Employment))
                    GlobalCacheData[ViewConstants.Employment] = value;
                else
                    GlobalCacheData.Add(ViewConstants.Employment, value, new List<ICacheObjectLifeTime>());

            }
        }

        /// <summary>
        /// Gets/sets an employer key from the global cache.
        /// </summary>
        protected int CachedEmployerKey
        {
            get
            {
                if (GlobalCacheData.ContainsKey(ViewConstants.EmployerKey))
                    return Convert.ToInt32(GlobalCacheData[ViewConstants.EmployerKey]);
                else
                    return -1;
            }
            set
            {
                if (value == -1)
                    GlobalCacheData.Remove(ViewConstants.EmployerKey);
                else if (GlobalCacheData.ContainsKey(ViewConstants.EmployerKey))
                    GlobalCacheData[ViewConstants.EmployerKey] = value;
                else
                    GlobalCacheData.Add(ViewConstants.EmployerKey, value, new List<ICacheObjectLifeTime>());

            }
        }

        /// <summary> 
        /// Gets a <see cref="IEmploymentRepository"/> for use on the view.
        /// </summary>
        protected IEmploymentRepository EmploymentRepository
        {
            get
            {
                if (_employmentRepository == null)
                    _employmentRepository = RepositoryFactory.GetRepository<IEmploymentRepository>();
                return _employmentRepository;
            }
        }

        /// <summary>
        /// Gets a reference to the current legal entity.
        /// </summary>
        protected ILegalEntity GetLegalEntity(SAHLPrincipal principal)
        {
            CBOMenuNode node = CBOManager.GetCurrentCBONode(principal) as CBOMenuNode;
            return LegalEntityRepository.GetLegalEntityByKey(Convert.ToInt32(node.GenericKey));
        }

        #region Unused Code


        /// <summary>
        /// Gets the application key.
        /// </summary>
        //protected IAccount GetApplicationAccount(SAHLPrincipal principal, IApplication app)
        //{
        //    CBOMenuNode node = CBOManager.GetCurrentCBONode(principal) as CBOMenuNode;
        //    IApplicationRepository appRepo;
        //    IAccountRepository accRep = RepositoryFactory.GetRepository<IAccountRepository>();

        //    switch (node.GenericKeyTypeKey)
        //    {
        //        case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
        //            return accRep.GetAccountByKey(node.ParentNode.GenericKey);
                    
        //        case (int)SAHL.Common.Globals.GenericKeyTypes.LegalEntity:
        //            //property = RepositoryFactory.GetRepository<IPropertyRepository>().GetPropertyByKey(_genericKey);
        //            //_properties = new EventList<IProperty>();
        //            //_properties.Add(_view.Messages, property);
        //            if (node.ParentNode != null)
        //            {
        //                if (node.ParentNode.GenericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.Account)
        //                {
        //                    return accRep.GetAccountByKey(node.ParentNode.GenericKey);
        //                }

        //                //    _hocAccount = _hocRepo.RetrieveHOCByAccountKey(_node.ParentNode.GenericKey, ref _hocRetrievedByAccount);
        //                else if (node.ParentNode.GenericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.Offer)
        //                {
        //                    appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
        //                    app = appRepo.GetApplicationByKey(node.ParentNode.GenericKey);

        //                    return app.Account;
        //                }
        //                //    _hocAccount = _hocRepo.RetrieveHOCByOfferKey(_node.ParentNode.GenericKey, ref _hocRetrievedByAccount);
        //                else
        //                {
        //                    appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
        //                    app = appRepo.GetApplicationByKey(node.GenericKey);
        //                    if (app == null)
        //                        return null;

        //                    else
        //                    {
        //                        IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
        //                        IAccount acc = accRepo.GetAccountByKey(node.GenericKey);

        //                        return acc;
        //                    }
        //                    //throw new Exception("GenericKeyTypeKey not supported in HOC Account retrieve");
        //                    //appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
        //                    //app = appRepo.GetApplicationByKey(node.ParentNode.GenericKey);
        //                    //return app.Account;
        //                }
        //            }
        //            else
        //            {
        //                appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
        //                app = appRepo.GetApplicationByKey(node.GenericKey);

        //                if (app == null)
        //                    return null;
        //                else
        //                {
        //                    IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
        //                    IAccount acc = accRepo.GetAccountByKey(node.GenericKey);   
                            
        //                    return acc;
        //                }
                            
        //            }
                    
                       
        //            break;

        //        case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
        //            //IApplicationMortgageLoan app = RepositoryFactory.GetRepository<IApplicationRepository>().GetApplicationByKey(_genericKey) as IApplicationMortgageLoan;

        //            //if (app != null)
        //            //{
        //            //    if (_properties == null)
        //            //        _properties = new EventList<IProperty>();
        //            //    _properties.Add(_view.Messages, app.Property);

        //            //    _hocAccount = _hocRepo.RetrieveHOCByOfferKey(_genericKey, ref _hocRetrievedByAccount);
        //            //}
        //            appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
        //            app = appRepo.GetApplicationByKey(node.ParentNode.GenericKey);
        //            return app.Account;

                   
                    


        //        default:
        //            appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
        //            app = appRepo.GetApplicationByKey(node.ParentNode.GenericKey);
        //            if (app == null)
        //                return null;
        //            else
        //                return app.Account;

        //    }
        //}
        #endregion
        /// <summary>
        /// Gets a <see cref="ICommonRepository"/> for use on the view.
        /// </summary>
        protected ICommonRepository CommonRepository
        {
            get
            {
                if (_commonRepository == null)
                    _commonRepository = RepositoryFactory.GetRepository<ICommonRepository>();
                return _commonRepository;
            }
        }

        /// <summary>
        /// Gets a <see cref="ILegalEntityRepository"/> for use on the view.
        /// </summary>
        protected ILegalEntityRepository LegalEntityRepository
        {
            get
            {
                if (_legalEntityRepository == null)
                    _legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                return _legalEntityRepository;
            }
        }

        /// <summary>
        /// Gets a <see cref="ILookupRepository"/> for use on the view.
        /// </summary>
        protected ILookupRepository LookupRepository
        {
            get
            {
                if (_lookupRepository == null)
                    _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                return _lookupRepository;
            }
        }

        /// <summary>
        /// Used to keep a record of the previous page in the employment process.  This is because the 
        /// extended employment screen can be bypassed altogether, and subsidy details needs to know 
        /// where to navigate to if the user clicks back.
        /// </summary>
        protected string PreviousView
        {
            get
            {
                if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
                    return GlobalCacheData[ViewConstants.NavigateTo].ToString();
                else
                    return null;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    GlobalCacheData.Remove(ViewConstants.NavigateTo);
                    return;
                }

                if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
                    GlobalCacheData.Remove(ViewConstants.NavigateTo);

                GlobalCacheData.Add(ViewConstants.NavigateTo, value, new List<ICacheObjectLifeTime>());

            }
        }

        #endregion

        #region Methods

        private static void DeleteEmploymentVerificationProcess(IEmployment employment, int iDeleteItem, IViewBase view)
        {
            for (int i = 0; i < employment.EmploymentVerificationProcesses.Count; i++)
            {
                if (employment.EmploymentVerificationProcesses[i].EmploymentVerificationProcessType.Key == iDeleteItem)
                {
                    employment.EmploymentVerificationProcesses.RemoveAt(view.Messages, i);
                    return;
                }
            }

        }

        private void AddEmploymentVerificationProcess(IEmployment employment, int iAddItem, IViewBase view)
        {
            for (int i = 0; i < employment.EmploymentVerificationProcesses.Count; i++)
            {
                if (employment.EmploymentVerificationProcesses[i].EmploymentVerificationProcessType.Key == iAddItem)
                    return;
            }

            IEmploymentVerificationProcess employmentVerificationProcess = EmploymentRepository.GetEmptyEmploymentVerificationProcess();
            employmentVerificationProcess.EmploymentVerificationProcessType = LookupRepository.EmploymentVerificationProcessTypes.ObjectDictionary[iAddItem.ToString()];
            employmentVerificationProcess.UserID = view.CurrentPrincipal.Identity.Name;
            employmentVerificationProcess.ChangeDate = DateTime.Now;
            employmentVerificationProcess.Employment = employment;
            employment.EmploymentVerificationProcesses.Add(view.Messages, employmentVerificationProcess);
        }

        protected void ClearCachedData()
        {
            CachedEmployerKey = -1;
            CachedEmployment = null;
            PreviousView = null;
            CachedAddEmploymentVerificationProcess = null;
            CachedDeleteEmploymentVerificationProcess = null;
            CachedEmploymentConfirmationSourceKey = new int?();
        }

        // TODO: use SaveOrUpdate
        // This method doesn't need to exist as we've worked out how to reattach to the session - use SaveOrUpdate 
        // and chat to GaryD about doing this
        protected void CopyCachedValues(SAHLPrincipal principal, IEmployment target)
        {
            IEmployment source = CachedEmployment;

            //if (source.Key != 0)
            //    _commRepo.AttachUnModifiedToCurrentNHibernateSession(source, source.Key.ToString());

            // basic details
            if (source != null)
            {
                target.EmploymentStatus = source.EmploymentStatus;
                target.EmploymentStartDate = source.EmploymentStartDate;
                target.EmploymentEndDate = source.EmploymentEndDate;
                target.BasicIncome = source.BasicIncome;
                target.ConfirmedBasicIncome = source.ConfirmedBasicIncome;
                target.ConfirmedBy = source.ConfirmedBy;
                target.ConfirmedDate = source.ConfirmedDate;
                target.ContactPerson = source.ContactPerson;
                target.ContactPhoneCode = source.ContactPhoneCode;
                target.ContactPhoneNumber = source.ContactPhoneNumber;
                target.Department = source.Department;
                target.RemunerationType = source.RemunerationType;
                target.BasicIncome = source.BasicIncome;
                target.ConfirmedBasicIncome = source.ConfirmedBasicIncome;
                target.ConfirmedEmploymentFlag = source.ConfirmedEmploymentFlag;
                target.ConfirmedIncomeFlag = source.ConfirmedIncomeFlag;
            }

            // legal entity
            target.LegalEntity = GetLegalEntity(principal);

            // employer
            if (CachedEmployerKey > 0)
                target.Employer = EmploymentRepository.GetEmployerByKey(CachedEmployerKey);

            // extended information
            if (source != null && source.RequiresExtended)
            {
                target.ExtendedEmployment.Allowances = source.ExtendedEmployment.Allowances;
                target.ExtendedEmployment.Commission = source.ExtendedEmployment.Commission;
                target.ExtendedEmployment.ConfirmedAllowances = source.ExtendedEmployment.ConfirmedAllowances;
                target.ExtendedEmployment.ConfirmedCommission = source.ExtendedEmployment.ConfirmedCommission;
                target.ExtendedEmployment.ConfirmedMedicalAid = source.ExtendedEmployment.ConfirmedMedicalAid;
                target.ExtendedEmployment.ConfirmedOvertime = source.ExtendedEmployment.ConfirmedOvertime;
                target.ExtendedEmployment.ConfirmedPAYE = source.ExtendedEmployment.ConfirmedPAYE;
                target.ExtendedEmployment.ConfirmedPensionProvident = source.ExtendedEmployment.ConfirmedPensionProvident;
                target.ExtendedEmployment.ConfirmedPerformance = source.ExtendedEmployment.ConfirmedPerformance;
                target.ExtendedEmployment.ConfirmedShift = source.ExtendedEmployment.ConfirmedShift;
                target.ExtendedEmployment.ConfirmedUIF = source.ExtendedEmployment.ConfirmedUIF;
                target.ExtendedEmployment.MedicalAid = source.ExtendedEmployment.MedicalAid;
                target.ExtendedEmployment.Overtime = source.ExtendedEmployment.Overtime;
                target.ExtendedEmployment.PAYE = source.ExtendedEmployment.PAYE;
                target.ExtendedEmployment.PensionProvident = source.ExtendedEmployment.PensionProvident;
                target.ExtendedEmployment.Performance = source.ExtendedEmployment.Performance;
                target.ExtendedEmployment.Shift = source.ExtendedEmployment.Shift;
                target.ExtendedEmployment.UIF = source.ExtendedEmployment.UIF;
            }
        }

        /// <summary>
        /// Helper method to populate the employment details control with the values from an IEmployment entity.
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="view"></param>
        protected void PopulateEmploymentDetails(IEmployment emp, IEmploymentView view)
        {

            IEmploymentDetails ed = view.EmploymentDetails;
            if (emp != null)
            {
                ed.EmploymentType = emp.EmploymentType;
                ed.EmploymentStatus = emp.EmploymentStatus;
                ed.RemunerationType = emp.RemunerationType;
                ed.StartDate = emp.EmploymentStartDate;
                ed.EndDate = emp.EmploymentEndDate;
                ed.ConfirmedBy = emp.ConfirmedBy;
                ed.ConfirmedDate = emp.ConfirmedDate;
                ed.BasicIncome = emp.MonthlyIncome;
                ed.ConfirmedBasicIncome = emp.ConfirmedIncome;
                ed.ConfirmedEmployment = emp.ConfirmedEmploymentFlag;
                ed.ConfirmedIncome = emp.ConfirmedIncomeFlag;
                ed.ConfirmedBy = emp.ConfirmedBy != null ? emp.ConfirmedBy : "-";
                ed.ConfirmedDate = emp.ConfirmedDate.HasValue ? emp.ConfirmedDate.Value : new DateTime?();
            }

        }

        protected void PopulateReadOnlyEmploymentDetails(IEmployment emp, IEmploymentView view)
        {
            IEmploymentDetails ed = view.EmploymentDetails;
            if (ed.EmploymentTypeReadOnly)
                ed.EmploymentType = emp.EmploymentType;
            if (ed.EmploymentStatusReadOnly)
                ed.EmploymentStatus = emp.EmploymentStatus;
            if (ed.RemunerationTypeReadOnly)
                ed.RemunerationType = emp.RemunerationType;
            if (ed.StartDateReadOnly)
                ed.StartDate = emp.EmploymentStartDate;
            if (ed.EndDateReadOnly)
                ed.EndDate = emp.EmploymentEndDate;
            //if (ed.BasicIncomeReadOnly)
            if (emp.MonthlyIncome > 0D)
                ed.BasicIncome = emp.MonthlyIncome;
            //if (ed.ConfirmedBasicIncomeReadOnly)
            if (emp.ConfirmedIncome > 0D)
                ed.ConfirmedBasicIncome = emp.ConfirmedIncome;
            if (ed.ConfirmedEmploymentReadOnly)
                ed.ConfirmedEmployment = emp.ConfirmedEmploymentFlag;
            if (ed.ConfirmedIncomeReadOnly)
                ed.ConfirmedIncome = emp.ConfirmedIncomeFlag;

            ed.ConfirmedBy = emp.ConfirmedBy != null ? emp.ConfirmedBy : "-";
            ed.ConfirmedDate = emp.ConfirmedDate.HasValue ? emp.ConfirmedDate.Value : new DateTime?();
        }

        /// <summary>
        /// Used to save an employment record, and (optionally) navigate to the next screen if the save 
        /// was successful.  This will set the ConfirmedBy and ConfirmedDate values on the employment 
        /// record if the confirmed income has changed.
        /// </summary>
        /// <param name="view">The view on which the save is being done.</param>
        /// <param name="employment">The updated employment entity.</param>
        /// <param name="navigateTarget">Where to navigate to on a successful save.  If this is set to null or empty, navigation will not occur.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected void SaveEmployment(IViewBase view, IEmployment employment, string navigateTarget)
        {
            bool success = false;
            TransactionScope txn = new TransactionScope();
            try
            {
                /* This will be set by another presenter under specific conditions. */
                // set the confirmation details if the confirmed income has changed
                //if (employment.ConfirmedIncomeChanged || employment.ContactPersonChanged || employment.ContactPhoneNumberChanged || employment.DepartmentChanged || employment.ContactPhoneCodeChanged)
                //{
                //    employment.ConfirmedBy = view.CurrentPrincipal.Identity.Name;
                //    employment.ConfirmedDate = DateTime.Now;
                //}

                UpdateEmploymentVerificationProcess(view, employment);

                if (CachedEmploymentConfirmationSourceKey.HasValue)
                    employment.EmploymentConfirmationSource = LookupRepository.EmploymentConfirmationSources.ObjectDictionary[CachedEmploymentConfirmationSourceKey.Value.ToString()];

                EmploymentRepository.SaveEmployment(employment);
                txn.VoteCommit();
                ClearCachedData();
                success = true;

            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (view.IsValid)
                    throw;
            }
            finally
            {
                //Getting dispose errors here
                try
                {
                    txn.Dispose();
                }
                catch (Exception)
                {

                    if (view.IsValid)
                        throw;
                }
                
            }

            if (success && (!String.IsNullOrEmpty(navigateTarget)) && (view.IsValid))
            {
                view.ShouldRunPage = false;
                view.Navigator.Navigate(navigateTarget);
            }

        }

        protected void UpdateEmploymentVerificationProcess(IViewBase view, IEmployment employment)
        {
            if (CachedDeleteEmploymentVerificationProcess != null)
            {
                foreach (int iDeleteItem in CachedDeleteEmploymentVerificationProcess)
                {
                    DeleteEmploymentVerificationProcess(employment, iDeleteItem, view);
                }
            }

            if (CachedAddEmploymentVerificationProcess != null)
            {
                foreach (int iAddItem in CachedAddEmploymentVerificationProcess)
                {
                    AddEmploymentVerificationProcess(employment, iAddItem, view);
                }
            }
        }

        /// <summary>
        /// Validates the employment record as we move between screens.  This will use an 
        /// exclusion set if the employment requires extended info.
        /// </summary>
        /// <param name="employment"></param>
        protected void ValidateEmployment(IEmployment employment)
        {
            // validate here - we want to prevent any data capture before saving or 
            // allowing them to transfer to the next screen - although if extended details 
            // are required then don't validate the basic income now
            if (employment.RequiresExtended)
                ExclusionSets.Add(RuleExclusionSets.EmploymentBasicView);
            employment.ValidateEntity();
            if (employment.RequiresExtended)
                ExclusionSets.Remove(RuleExclusionSets.EmploymentBasicView);

        }

        #endregion

    }
}
