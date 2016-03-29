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
using Castle.ActiveRecord;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    public class LegalEntityDetailsUpdate: LegalEntityDetailsUpdateBase
    {
        public LegalEntityDetailsUpdate(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            // disable the ajax functionality so that the users cannot use the idnumber ajax to "pull in" another legal entities information
            //_view.DisableAjaxFunctionality = true;

            LoadLegalEntityFromCBO();
            BindLegalEntity();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected override void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();

            try
            {
				base.ReloadLegalEntityIfTypeChanged();

                // Get the details from the screen
                _view.PopulateLegalEntityDetailsForUpdate(base.LegalEntity);
                // Populate the marketing options ...
                PopulateMarketingOptions();

                // if the locked update controls are disabled then disable relevant rules ie: MartialStatus
                if (_view.LockedUpdateControlsVisible == false)
                    this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLockedUpdate);

                LegalEntityRepository.SaveLegalEntity(base.LegalEntity, true);

                if (!_view.IsValid)
                    throw new Exception();

                ts.VoteCommit();

                // The base will attempt to navigate, so save first
                base.OnSubmitButtonClicked(sender, e);
            }
            catch (Exception)
            {
                ts.VoteRollBack();

                if (_view.IsValid)
                    throw;
            }
            finally
            {
                //db triggers could have rolled the transaction back
                //only throw an error if there are no DMC's
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
    }
}
