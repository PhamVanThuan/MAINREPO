using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Presenters
{
    public class ApplicantsRemove : ApplicantsOfferBase
    {
        /// <summary>
        /// Consructor. Gets the View and controller pairs.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ApplicantsRemove(IApplicants view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Add any additional event hooks
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
            _view.OnRemoveButtonClicked += new KeyChangedEventHandler(OnRemoveButtonClicked);
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage) return;

            // set the applicationroletypes to display
            base.ApplicationRoleTypes = new Dictionary<string, string>();
            string roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.MainApplicant);
            base.ApplicationRoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.MainApplicant]));
            roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant);
            base.ApplicationRoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.LeadMainApplicant]));
            roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.Suretor);
            base.ApplicationRoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.Suretor]));
            roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.LeadSuretor);
            base.ApplicationRoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.LeadSuretor]));

            _view.GridHeading = "Applicants";

            // call the base initialise to handle the binding etc
            base.OnViewInitialised(sender, e);
        }

        private void OnRemoveButtonClicked(object sender, KeyChangedEventArgs e)
        {
            IRuleService ruleService = ServiceFactory.GetService<IRuleService>();
            object[] parameters = new object[2] { Application.Key, _view.SelectedLegalEntityKey };
            ruleService.ExecuteRule(_view.Messages, "CanRemoveClientFromApplicationLinkedAffordabilityAssessmentCheck", parameters);
            if (_view.IsValid)
            {
                TransactionScope txn = new TransactionScope();

                try
                {
                    // get the selected LegalEntity
                    ILegalEntityRepository legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                    ILegalEntity le = legalEntityRepo.GetLegalEntityByKey(_view.SelectedLegalEntityKey);

                    // if there is a pending legalentitydomicilium then get it here so we can delete after we have removed the role
                    IApplicationRole applicationRole = ApplicationRepository.GetApplicationRoleByKey(_view.SelectedApplicationRoleKey);
                    ILegalEntityDomicilium legalEntityDomicilium = applicationRole.ApplicationRoleDomicilium != null ? applicationRole.ApplicationRoleDomicilium.LegalEntityDomicilium : null;

                    // remove the role from the application
                    base.Application.RemoveRolesForLegalEntity(_view.Messages, le, new int[] { _view.SelectedApplicationRoleTypeKey });

                    // Need To Update The Refresh The Application and Update
                    base.Application.SetApplicantType();

                    // save the application
                    ApplicationRepository.SaveApplication(base.Application);
                    base.Application.CalculateApplicationDetail(false, false);

                    // if the application role we have deleted has an associated Pending LegalEntityDomicilium then lets delete it
                    if (legalEntityDomicilium != null && legalEntityDomicilium.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Pending)
                    {
                        legalEntityRepo.DeleteLegalEntityDomicilium(legalEntityDomicilium);
                    }

                    // we need to update the subject on the instance record.
                    IX2Repository _x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
                    _x2Repo.UpdateInstanceSubject(base.Application.Key, base.Application.GetLegalName(LegalNameFormat.Full));

                    txn.VoteCommit();

                    //remove the node here
                    CBOManager.RefreshInstanceNode(_view.CurrentPrincipal);

                    _view.Navigator.Navigate("Submit");
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
                }
            }
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.ButtonsVisible = true;
        }
    }
}