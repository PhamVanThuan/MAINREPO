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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    public class LegalEntityDetailsAddRelate : LegalEntityDetailsAddBase
    {
        public LegalEntityDetailsAddRelate(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            _view.CancelButtonVisible = true;
            _view.SubmitButtonText = "Relate";

            _view.AddRoleTypeVisible = false;
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
    }
}
