using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// LegalAttorney - Select Registration Attorney
    /// </summary>
    public class LegalAttorneyRegistrationAttorney : SAHLCommonBasePresenter<ILegalAttorney>
    {
        IOrganisationStructureRepository orgRepo;
        ILookupRepository lookups;
        int _applicationKey;
        private CBOMenuNode _node;
        IList<IAttorney> attorneyLst;
        IApplicationRepository appRepo;
        IApplication App;
        IApplicationRole appRoleAttorney;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalAttorneyRegistrationAttorney(ILegalAttorney view, SAHLCommonBaseController controller)
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

            appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            orgRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _applicationKey = Convert.ToInt32(_node.GenericKey);

            _view.HideRegistrationUserPanel();

            lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            _view.BindDeedsOffice(lookups.DeedsOffice);

            App = appRepo.GetApplicationByKey(_applicationKey);

            //appRoleAttorney = FindCurrentAttorney(appRoleAttorney);
            //appRoleAttorney = orgRepo.FindApplicationRoleByApplicationRoleTypeKeyAndApplicationKey(_applicationKey, (int)OfferRoleTypes.ConveyanceAttorney);
            appRoleAttorney = orgRepo.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(_applicationKey, (int)OfferRoleTypes.ConveyanceAttorney, (int)GeneralStatuses.Active);

            if (!_view.IsPostBack && appRoleAttorney != null)
                SelectCurrentAttorney(appRoleAttorney);

            if (appRoleAttorney == null)
                GetAttorneysByDeedsOffice(lookups.DeedsOffice[0].Key);

            _view.OnUpdateButtonClicked += new EventHandler(_view_OnUpdateButtonClicked);
            _view.OnDeedsOfficeSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(OnDeedsOfficeSelectedIndexChanged);
        }

        /*private IApplicationRole FindCurrentAttorney(IApplicationRole appRoleAttorney)
        {
            for (int i = 0; i < App.ApplicationRoles.Count; i++)
            {
                if (App.ApplicationRoles[i].GeneralStatus.Key == (int)GeneralStatuses.Active && App.ApplicationRoles[i].ApplicationRoleType.Key == (int)OfferRoleTypes.ConveyanceAttorney)
                {
                    appRoleAttorney = App.ApplicationRoles[i];
                    break;
                }
            }
            return appRoleAttorney;
        }*/

        private void SelectCurrentAttorney(IApplicationRole appRoleAttorney)
        {
            IRegistrationRepository regRepo = RepositoryFactory.GetRepository<IRegistrationRepository>();
            IAttorney attorney = regRepo.GetAttorneyByLegalEntityKey(appRoleAttorney.LegalEntity.Key);
            _view.SetAttorneyDeedsOffice(attorney.DeedsOffice.Key);
            GetAttorneysByDeedsOffice(attorney.DeedsOffice.Key);
            _view.GetSetAttorneySelected = attorney.Key;
        }

        void OnDeedsOfficeSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToInt32(e.Key) > 0)
                GetAttorneysByDeedsOffice(Convert.ToInt32(e.Key));
        }

        void GetAttorneysByDeedsOffice(int DeedsOfficeKey)
        {
            /*IRegistrationRepository regRepo = RepositoryFactory.GetRepository<IRegistrationRepository>();

            IList<IAttorney> unfilteredAttorneyLst = regRepo.GetAttorneysByDeedsOfficeKey(DeedsOfficeKey);

            attorneyLst = new List<IAttorney>();

            for (int i = 0; i < unfilteredAttorneyLst.Count; i++)
            {
                for (int y = 0; y < unfilteredAttorneyLst[i].OriginationSources.Count; y++)
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

        /// <summary>
        /// Update Button Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		void _view_OnUpdateButtonClicked(object sender, EventArgs e)
		{
			int selectedAttorney = _view.GetSetAttorneySelected;

			if (selectedAttorney > 0)
			{
				// only fire minimum required field validation
				this.ExclusionSets.Add(RuleExclusionSets.LegalEntityExcludeAll);
				TransactionScope txn = new TransactionScope();
				IX2Service svc = ServiceFactory.GetService<IX2Service>();
				try
				{

					IRegistrationRepository regRepo = RepositoryFactory.GetRepository<IRegistrationRepository>();
					IAttorney attorney = regRepo.GetAttorneyByKey(selectedAttorney);

					// Check if the Attorney selected, already plays an Active Role in the Application
					foreach (IApplicationRole _applicationRole in App.ApplicationRoles)
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
					//appRole.Application = App;
					//appRole.ApplicationRoleType = appRoleType;
					//appRole.LegalEntity = attorney.LegalEntity;
					//appRole.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Active];
					//appRole.StatusChangeDate = DateTime.Now;

					//IApplicationRole currentRole = orgRepo.FindApplicationRoleByApplicationRoleTypeKeyAndApplicationKey(App.Key, (int)OfferRoleTypes.ConveyanceAttorney);

					//if (currentRole != null)
					//{
					//    currentRole.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Inactive];
					//    orgRepo.SaveApplicationRole(currentRole);
					//}

					//orgRepo.SaveApplicationRole(appRole);
					//if (appRoleAttorney != null)
					//{
					//    appRoleAttorney.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Inactive];
					//    orgRepo.SaveApplicationRole(appRoleAttorney);
					//}

					if (appRoleAttorney != null)
					{
						//orgRepo.DeactivateApplicationRole(appRoleAttorney.Key);
					}
					//orgRepo.DeactivateExistingApplicationRoles(App.Key, (int)OfferRoleTypes.ConveyanceAttorney);
					orgRepo.GenerateApplicationRole((int)OfferRoleTypes.ConveyanceAttorney, App.Key, attorney.LegalEntity.Key, true);

					if (_view.IsValid)
					{
						txn.VoteCommit();
						svc.CompleteActivity(_view.CurrentPrincipal, null, false);
						svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
					}
					else
						txn.VoteRollBack();
				}
				catch (Exception)
				{
					txn.VoteRollBack();
					svc.CancelActivity(_view.CurrentPrincipal);

					if (_view.IsValid)
						throw;
				}
				finally
				{
					txn.Dispose();
				}
			}
		}
    }
}
