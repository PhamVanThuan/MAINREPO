using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.Common.Presenters.AffordabilityDetailsPresenters
{
    /// <summary>
    ///
    /// </summary>
    public class AffordabilityDetailsUpdate : AffordabilityDetailsBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public AffordabilityDetailsUpdate(IAffordabilityDetails view, SAHLCommonBaseController controller)
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

            if (!_view.ShouldRunPage) return;

            _view.OnSubmitButtonClicked += _view_OnSubmitButtonClicked;
            _view.application = Application;

            UpdateLegalEntityAffordability();

            _view.LegalEntity = LegalEntity;

            InitialiseAffordabilityModel();

            _view.BindControls();
        }

        private void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();
            try
            {
                foreach (Models.Affordability.AffordabilityModel affordabilityModel in _view.Affordability)
                {
                    LegalEntity.LegalEntityAffordabilities.Update(x => x.Amount = ((x.AffordabilityType.Key == affordabilityModel.Key) && (x.Application == _view.application)) ? affordabilityModel.Amount : x.Amount);
                    LegalEntity.LegalEntityAffordabilities.Update(
                        x => x.Description = (
                            (x.AffordabilityType.Key == affordabilityModel.Key)
                            && (x.Application == _view.application)
                            && (affordabilityModel.DescriptionRequired == true)) ? affordabilityModel.Description : x.Description);

                    if (affordabilityModel.Amount == 0)
                    {
                        var legalEntityAffordabilityToRemove = LegalEntity.LegalEntityAffordabilities.Where(x => (x.AffordabilityType.Key == affordabilityModel.Key) && (x.Application == _view.application)).FirstOrDefault();
                        LegalEntity.LegalEntityAffordabilities.Remove(_view.Messages, legalEntityAffordabilityToRemove);
                    }
                }

                IApplicationMortgageLoan ml = Application as IApplicationMortgageLoan;
                if (ml != null)
                {
                    if (_view.SelectedNumDependantsInHousehold.Length > 0)
                    {
                        ml.DependentsPerHousehold = int.Parse(_view.SelectedNumDependantsInHousehold);
                    }

                    if (_view.SelectedNumContributingDependants.Length > 0)
                    {
                        ml.ContributingDependents = int.Parse(_view.SelectedNumContributingDependants);
                    }
                }

                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                svc.ExecuteRule(spc.DomainMessages, "AffordabilityAtLeastOneIncome", LegalEntity.LegalEntityAffordabilities, Application);

                LegalEntityRepository.SaveLegalEntity(LegalEntity, true);
                ApplicationRepository.SaveApplication(Application);
                ts.VoteCommit();
                _view.Navigator.Navigate("AffordabilityDetails");
            }
            catch (Exception)
            {
                ts.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                ts.Dispose();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.SubmitButtonText = "Update";
            _view.ShowButtons = true;
        }

        private void UpdateLegalEntityAffordability()
        {
            var affordabilityTypes = LegalEntityRepository.GetAffordabilityTypes();

            //find affordabilityTypes that don't exist on the list
            var affordabilityTypesToAdd = affordabilityTypes.Where(x => !LegalEntity.LegalEntityAffordabilities.Any(y => y.AffordabilityType.Key == x.Key
                && y.Application.Key == Application.Key));

            //add new affordabilityTypes
            foreach (var affordabilityType in affordabilityTypesToAdd)
            {
                ILegalEntityAffordability legalEntityAffordability = LegalEntityRepository.GetEmptyLegalEntityAffordability();
                legalEntityAffordability.AffordabilityType = affordabilityType;
                legalEntityAffordability.Application = Application;
                legalEntityAffordability.Amount = 0D;
                legalEntityAffordability.LegalEntity = LegalEntity;
                LegalEntity.LegalEntityAffordabilities.Add(_view.Messages, legalEntityAffordability);
            }
        }
    }
}