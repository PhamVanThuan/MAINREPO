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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.UI;
using SAHL.Common.Globals;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// Legal Attorney - Select Registration Attorney
    /// </summary>
    public class LegalAttorneyRegistrationUser : SAHLCommonBasePresenter<ILegalAttorney>
    {
        IOrganisationStructureRepository _orgRepo;

        int _applicationKey;
        IADUser _regAdminADUser;

        private CBOMenuNode _node;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalAttorneyRegistrationUser(ILegalAttorney view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }
        /// <summary>
        /// OnView Initialised event - Show relevant Panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _applicationKey = Convert.ToInt32(_node.GenericKey);

            _orgRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

            _view.HideAttorneyPanel();

            IApplicationRoleType art = _orgRepo.GetApplicationRoleTypeByKey((int)OfferRoleTypes.RegistrationsAdministratorD);

            IEventList<IADUser> adUserLst = _orgRepo.GetUsersForDynamicRole(art,false);
            _view.BindRegistrationUsers(adUserLst);
            _view.OnUpdateButtonClicked += new EventHandler(_view_OnUpdateButtonClicked);

            _regAdminADUser = GetApplicationRegistrationAdminADUser();
            if (_regAdminADUser != null)
            {
                _view.SetADUserSelected(_regAdminADUser.Key);
            }
        }

        /// <summary>
        /// Nazir J => 2008-07-16 => Start
        /// Set Currently Assigned User On Drop Down List => RegistrationsAdministratorD
        /// </summary>
        /// <returns></returns>
        private IADUser GetApplicationRegistrationAdminADUser()
        {
            //IApplication app = _appRepo.GetApplicationByKey(_applicationKey);
            //IADUser aduser = null;
            //foreach (IApplicationRole appRole in app.ApplicationRoles)
            //{
            //    if (appRole.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active && appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.RegistrationsAdministratorD)
            //    {
            //        aduser = _orgRepo.FindByLegalEntityKey(appRole.LegalEntity.Key);
            //        return aduser;
            //    }
            //}

            IApplicationRole appRole = _orgRepo.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(_applicationKey, (int)OfferRoleTypes.RegistrationsAdministratorD , (int)GeneralStatuses.Active);
            IADUser aduser = _orgRepo.GetAdUserByLegalEntityKey(appRole.LegalEntity.Key);
            return aduser;
        }

        /// <summary>
        /// On Update Button Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnUpdateButtonClicked(object sender, EventArgs e)
        {
            int adUserKey = _view.GetADUserSelected;
            _regAdminADUser = GetApplicationRegistrationAdminADUser();
            if (_regAdminADUser == null || adUserKey != _regAdminADUser.Key)
            {
                TransactionScope txn = new TransactionScope();
                IX2Service svc = ServiceFactory.GetService<IX2Service>();

                try
                {
                    //ILegalEntity legalEntity = _orgRepo.GetADUserByKey(adUserKey).LegalEntity;
                    //// ILegalEntityRepository leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                    //IApplicationRole appRole = _orgRepo.CreateNewApplicationRole();
                    //appRole.Application = _appRepo.GetApplicationByKey(_applicationKey);
                    //appRole.ApplicationRoleType = _appRepo.GetApplicationRoleTypeByKey(OfferRoleTypes.RegistrationsAdministratorD);
                    //appRole.LegalEntity = legalEntity;
                    //appRole.GeneralStatus = _lookups.GeneralStatuses[GeneralStatuses.Active];
                    //appRole.StatusChangeDate = DateTime.Now;

                    //// only fire minimum required field validation
                    //this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);

                    //// Get Current Reg User, if one exists - and make role inactive
                    //IApplicationRole appRoleCurrent = _orgRepo.FindApplicationRoleByApplicationRoleTypeKeyAndApplicationKey(_applicationKey, (int)OfferRoleTypes.RegistrationsAdministratorD);
                    //if (appRoleCurrent != null)
                    //{
                    //    appRoleCurrent.GeneralStatus = _lookups.GeneralStatuses[GeneralStatuses.Inactive];
                    //    _orgRepo.SaveApplicationRole(appRoleCurrent);
                    //}

                    //// save the new application role
                    //_orgRepo.SaveApplicationRole(appRole);

                    IApplicationRole appRoleCurrent = _orgRepo.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(_applicationKey, (int)OfferRoleTypes.RegistrationsAdministratorD, (int)GeneralStatuses.Active);
                    ILegalEntity legalEntity = _orgRepo.GetADUserByKey(adUserKey).LegalEntity;
                    if (appRoleCurrent != null)
                    {
                        //_orgRepo.DeactivateApplicationRole(appRoleCurrent.Key);
                    }
                    //_orgRepo.DeactivateExistingApplicationRoles(_applicationKey, (int)OfferRoleTypes.RegistrationsAdministratorD);
                    _orgRepo.GenerateApplicationRole((int)OfferRoleTypes.RegistrationsAdministratorD, _applicationKey, legalEntity.Key, true);

                    svc.CompleteActivity(_view.CurrentPrincipal, null, false);
                    svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                }
                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (_view.IsValid)
                    {
                        svc.CancelActivity(_view.CurrentPrincipal);
                        throw;
                    }
                }

                finally
                {
                    txn.Dispose();
                }
            }
        }
    }
}
