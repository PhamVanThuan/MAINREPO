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
    public class LegalEntityDetailsUpdateContact : LegalEntityDetailsUpdateBase
    {
        /// <summary>
        /// Presenter constructor
        /// </summary>
        /// <param name="View"></param>
        /// <param name="Controller"></param>
        public LegalEntityDetailsUpdateContact(ILegalEntityDetails View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            // Bind additional life-specific stuff
            _view.BindInsurableInterestType(LookupRepository.LifeInsurableInterestTypes.BindableDictionary, "");

            LoadLegalEntityFromGlobalCache();

            BindLegalEntity();

        }

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

                if (!_view.IsValid)
                    throw new Exception();

                ts.VoteCommit();
            }
            catch (Exception)
            {
                ts.VoteRollBack();

                if (!_view.Messages.HasErrorMessages && !_view.Messages.HasWarningMessages)
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

            base.OnSubmitButtonClicked(sender, e);
        }
    }
}