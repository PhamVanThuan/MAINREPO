using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using System.Collections.Generic;
using SAHL.Common.Exceptions;
using SAHL.Common.UI;

using SAHL.Common.CacheData;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.Service.Interfaces;


namespace SAHL.Web.Views.Origination.Presenters
{
    public class ResendInstruction : SAHLCommonBasePresenter<IResendInstruction>
    {
        ILookupRepository _lookups;
        IAccountRepository accRepo;
        IRegistrationRepository regRepo;
        int AccountKey;
        int ApplicationKey;
        IRegMail regMail;
        IList<IAttorney> attorneyLst;
        CBOMenuNode _node;
        IList<IRegMail> regMailLst;

        public ResendInstruction(IResendInstruction view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (_node.GenericKeyTypeKey == (int)GenericKeyTypes.Account)
            {
                AccountKey = _node.GenericKey;
                ApplicationKey = -1;
            }
            else if (_node.GenericKeyTypeKey == (int)GenericKeyTypes.Offer)
            {
                accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                ApplicationKey = _node.GenericKey;
                IAccount _account = accRepo.GetAccountByApplicationKey(ApplicationKey);
                AccountKey = _account.Key;
            }
            else
            {
                AccountKey = -1;
            }

            if (AccountKey != -1)
            {
                //Creating a list for binding to Grid
                GetRegMailDetail();

                _lookups = RepositoryFactory.GetRepository<ILookupRepository>();
                _view.SetLookups();

                _view.BindDeedsOffice(_lookups.DeedsOffice);
                GetAttorneysByDeedsOffice(_lookups.Attorneys.ObjectDictionary[regMail.AttorneyNumber.ToString()].DeedsOffice.Key);
                _view.BindRegistrationAttorneys(attorneyLst);
                CheckActivityName();

                if (!_view.IsPostBack)
                    _view.SetUpdateableFields(regMail);
            }

            _view.OnDeedsOfficeSelectedIndexChanged += new KeyChangedEventHandler(_view_OnDeedsOfficeSelectedIndexChanged);
            _view.OnUpdateButtonClicked += new EventHandler(_view_OnUpdateButtonClicked);
            _view.OnSendInstructionClicked += new EventHandler(_view_OnSendInstructionClicked);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            if (AccountKey != -1)
            {
                _view.BindInstructionGrid(regMailLst);
                CheckActivityName();
            }


        }

        void _view_OnDeedsOfficeSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToInt32(e.Key) > 0)
                GetAttorneysByDeedsOffice(Convert.ToInt32(e.Key));
        }

        void GetAttorneysByDeedsOffice(int DeedsOfficeKey)
        {
            /*regRepo = RepositoryFactory.GetRepository<IRegistrationRepository>();
            IList<IAttorney> unfilteredAttorneyLst = regRepo.GetAttorneysByDeedsOfficeKey(DeedsOfficeKey);
           
            attorneyLst = new List<IAttorney>();

            for (int i = 0; i < unfilteredAttorneyLst.Count; i++)
            {
                for (int y = 0 ; y < unfilteredAttorneyLst[i].OriginationSources.Count ; y ++)
                {
                    if (unfilteredAttorneyLst[i].OriginationSources[y].Key == (int)OriginationSources.SAHomeLoans)
                    {
                        attorneyLst.Add(unfilteredAttorneyLst[i]);
                        break;
                    }
                }
            }*/
            IRegistrationRepository regRepo = RepositoryFactory.GetRepository<IRegistrationRepository>();
            attorneyLst = regRepo.GetAttorneysByDeedsOfficeKeyAndOSKey(DeedsOfficeKey, (int)OriginationSources.SAHomeLoans);
            _view.BindRegistrationAttorneys(attorneyLst);
        }

        void GetRegMailDetail()
        {
            regRepo = RepositoryFactory.GetRepository<IRegistrationRepository>();
            regMail = regRepo.GetRegmailByAccountKey(AccountKey);
            regMailLst = new List<IRegMail>();
            regMailLst.Add(regMail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnUpdateButtonClicked(object sender, EventArgs e)
        {
            IOrganisationStructureRepository orgRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IApplicationRole currentRole = null;

            TransactionScope txn = new TransactionScope();
            try
            {
                // Add exclusion set
                this.ExclusionSets.Add(RuleExclusionSets.LegalEntityExcludeAll);

                // Get Application Role
                IAttorney attorney = regRepo.GetAttorneyByKey(_view.GetAttorneySelected);

                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                IApplication application = null;

                if (ApplicationKey != -1)
                    application = appRepo.GetApplicationByKey(ApplicationKey);
                else
                {
                    IEventList<IApplication> apps = appRepo.GetApplicationByAccountKey(AccountKey);
                    foreach (IApplication _application in apps)
                    {
                        if (_application.ApplicationStatus.Key == (int)OfferStatuses.Open)
                        {
                            application = _application;
                            break;
                        }
                    }
                }
                
                // Check for duplicate instruction before re-instructing
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                svc.ExecuteRule(spc.DomainMessages, "ApplicatonDuplicateInstructionCheck", application);

                if (!_view.IsValid)
                    throw new DomainValidationException();
                
                // Get Latest Application 
                /*
                if (apps.Count > 0)
                {
                    int latestapp = apps[0].Key;

                    for (int i = 0; i < apps.Count; i++)
                    {
                        if (latestapp < apps[i].Key)
                        {
                            latestapp = apps[i].Key;
                            application = apps[i];
                        }
                    }
                }
                */

                // Check if the Attorney selected, already plays an Active Role in the Application
                foreach (IApplicationRole _applicationRole in application.ApplicationRoles)
                {
                    if (_applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                        _applicationRole.ApplicationRoleType.Key == (int)OfferRoleTypes.ConveyanceAttorney &&
                        _applicationRole.LegalEntity.Key == attorney.LegalEntity.Key)
                    {
                        return;
                    }
                }

                //IApplicationRole appRole = orgRepo.CreateNewApplicationRole();
                //IApplicationRoleType appRoleType = orgRepo.GetApplicationRoleTypeByKey((int)OfferRoleTypes.ConveyanceAttorney);
                //appRole.Application = application;
                //appRole.ApplicationRoleType = appRoleType;
                //appRole.LegalEntity = attorney.LegalEntity;
                //appRole.GeneralStatus = _lookups.GeneralStatuses[GeneralStatuses.Active];
                //appRole.StatusChangeDate = DateTime.Now;

                //currentRole = orgRepo.FindApplicationRoleByApplicationRoleTypeKeyAndApplicationKey(application.Key, (int)OfferRoleTypes.ConveyanceAttorney);
                currentRole = orgRepo.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(application.Key, (int)OfferRoleTypes.ConveyanceAttorney, (int)GeneralStatuses.Active);
                if (currentRole != null)
                {
                    //orgRepo.DeactivateApplicationRole(currentRole.Key);
                }
                // Updated RegmMail
                regMail.AttorneyNumber = attorney.Key;
                regMail.RegMailDateTime = DateTime.Now;
                regMail.DetailTypeNumber = (int)DetailTypes.InstructionSent;

                //if (currentRole != null)
                //{
                //    currentRole.GeneralStatus = _lookups.GeneralStatuses[GeneralStatuses.Inactive];
                //    orgRepo.SaveApplicationRole(currentRole);
                //}
                //orgRepo.SaveApplicationRole(appRole);

                orgRepo.GenerateApplicationRole((int)OfferRoleTypes.ConveyanceAttorney, application.Key, attorney.LegalEntity.Key, true);
                regRepo.SaveRegmail(regMail);
                txn.VoteCommit();
            }

            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }

            finally
            {
                txn.Dispose();
                GetRegMailDetail();
            }

            if (_view.IsValid && currentRole != null)
            {
                base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false, true);
            }
            CheckActivityName();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnSendInstructionClicked(object sender, EventArgs e)
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplication application = null;

            if (ApplicationKey != -1)
                application = appRepo.GetApplicationByKey(ApplicationKey);
            else
            {
                IEventList<IApplication> apps = appRepo.GetApplicationByAccountKey(AccountKey);
                foreach (IApplication _application in apps)
                {
                    if (_application.ApplicationStatus.Key == (int)OfferStatuses.Open)
                    {
                        application = _application;
                        break;
                    }
                }
            }
            // Check for duplicate instruction before re-instructing
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            svc.ExecuteRule(spc.DomainMessages, "ApplicatonDuplicateInstructionCheck", application);
            if (_view.IsValid)
            {
            _view.Navigator.Navigate("Orig_Correspondence_ResendAttorney_Redirect");
        }
        }

        void CheckActivityName()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IX2Info x2Info = spc.X2Info as IX2Info;

            if (x2Info != null && !string.IsNullOrEmpty(x2Info.ActivityName))
                _view.SetUpdateButtonEnabled = true;
            else
                _view.SetUpdateButtonEnabled = false;

        }
    }
}
