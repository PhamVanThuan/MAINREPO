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

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    /// <summary>
    /// 
    /// </summary>
    public class LegalEntityDetailsUpdateLifeContact : LegalEntityDetailsUpdateBase
    {
        /// <summary>
        /// Presenter constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityDetailsUpdateLifeContact(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            // disable the ajax functionality so that the users cannot use the idnumber ajax to "pull in" another legal entities information
            //_view.DisableAjaxFunctionality = true;

            // Bind additional life-specific stuff
            _view.BindInsurableInterestType(LookupRepository.LifeInsurableInterestTypes.BindableDictionary, "");

            LoadLegalEntityFromGlobalCache();

            BindLegalEntity();

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.NonContactDetailsDisabled = true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected override void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();

            try
            {
                // Get the details from the screen
                _view.PopulateLegalEntityDetailsForUpdate(base.LegalEntity);
                // Populate the marketing options ...
                PopulateMarketingOptions();

                LegalEntityRepository.SaveLegalEntity(base.LegalEntity, true);

                ts.VoteCommit();

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