using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    public class LegalEntityDetailsAddAffordabilityAssessmentRelate : LegalEntityDetailsAddBase
    {
        public LegalEntityDetailsAddAffordabilityAssessmentRelate(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.CancelButtonVisible = true;
            _view.SubmitButtonText = "Relate";
            _view.AddRoleTypeVisible = false;
            _view.LegalEntityTypeEnabled = false;
        }

        protected override void ReBindLegalEntity(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            base.ReBindLegalEntity(sender, e);
            Navigator.Navigate("Rebind");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected override void SubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();

            try
            {
                // Create a blank LE populate it and save it
                LegalEntity = LegalEntityRepository.GetEmptyLegalEntity((LegalEntityTypes)_view.SelectedLegalEntityType);

                // Get the details from the screen
                _view.PopulateLegalEntityDetailsForAdd(base.LegalEntity);

                base.LegalEntity.IntroductionDate = DateTime.Now;

                // Populate the marketing options ...
                PopulateMarketingOptions();

                this.ExclusionSets.Add(RuleExclusionSets.AffordabilityAssessmentRelatedLegalEntity);

                // Save the legalentity
                base.LegalEntityRepository.SaveLegalEntity(base.LegalEntity, false);

                if (!_view.IsValid)
                    throw new Exception();

                ts.VoteCommit();

                // Add the saved LE to the Global Cache
                base.PersistLegalEntity();

                Navigator.Navigate("Submit");
            }
            catch (Exception)
            {
                ts.VoteRollBack();

                if (_view.IsValid)
                    throw;
            }
            finally
            {
                //db can rollback txn when rules fail, need to not throw ex
                //if view is valid
                try
                {
                    ts.Dispose();
                }
                catch (Exception)
                {
                    if (_view.IsValid)
                        throw;
                }
            }
        }

        protected override void CancelButtonClicked(object sender, EventArgs e)
        {
            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            // int keyType = cboNode.GenericKeyTypeKey;
            // int applicationKey = cboNode.GenericKey;
            if (cboNode.GenericKeyTypeKey == (int) GenericKeyTypes.Offer)
            {
                Navigator.Navigate("Cancel");
            }
            if(cboNode.GenericKeyTypeKey == (int) GenericKeyTypes.AffordabilityAssessment)
            {
                Navigator.Navigate("Link");
            }
        }


    }
}