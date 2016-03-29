using System;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Administration.Presenters.DebtCounselling
{
    public class PaymentDistributionAgentLegalEntityRemove : PaymentDistributionAgentLegalEntityBase
    {
        /// <summary>
		/// Constructor
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
        public PaymentDistributionAgentLegalEntityRemove(IExternalOrganisationStructureLegalEntity view, SAHLCommonBaseController controller)
			: base(view, controller)
		{

		}

		#region Events

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            if (!_leKeySet || _leKey < 1 || !_osKeySet || _osKey < 1)
            {
                _view.Messages.Add(new Error("No Legal Entity Key set", "No Legal Entity Key set"));
                _view.ShouldRunPage = false;
                return;
            }

            _view.onSubmitButtonClicked += new EventHandler(_view_onRemoveButtonClicked);

            _view.AddressCaptureEnabled = false;
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            SetUpControlsForUpdate();
            _view.SubmitButtonText = "Remove";
            _view.SubmitButtonVisible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            SetUpViewForDisplay();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected void _view_onRemoveButtonClicked(object sender, EventArgs e)
        {
           
            TransactionScope txn = new TransactionScope(TransactionMode.Inherits);
            try
            {
                // the default rule exclusionset (LegalEntityPaymentDistributionAgent) will be applied here - configured in web.config
                
                SelectedOrgNode.RemoveMe(_legalEntity);
                DebtCounsellingRepo.SavePaymentDistributionAgentOrganisationStructure(SelectedOrgNode);

                if (_view.IsValid)
                    txn.VoteCommit();
                else
                    txn.VoteRollBack();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                try
                {
                    txn.Dispose();
                }
                catch (Exception)
                {
                    if (_view.IsValid)
                        throw;
                }
            }

            if (_view.IsValid)
            {
                //set an item in the global cache so that the view can expand to the selected item
                Navigator.Navigate("Submit");
            }
        }

		#endregion
    }
}
