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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using Castle.ActiveRecord;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.UI;

using SAHL.Common;

namespace SAHL.Web.Views.Common.Presenters.ManualValuationMainDwellingDetails
{
    public class MainDwellingDetailDisplayOrig : MainDwellingDetailDisplay
    {
        /// <summary>
        /// ValuationDwellingDetail Display constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public MainDwellingDetailDisplayOrig(IValuationManualPropertyDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            
            if (!View.ShouldRunPage) 
                return;
        }

        protected override void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.ValuationManual);
            GlobalCacheData.Remove(ViewConstants.ValuationPresenter);

            CBOMenuNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (node == null)
                 _view.Navigator.Navigate("Orig_ValuationDetails");
             else
             {
                 base.X2Service.CancelActivity(_view.CurrentPrincipal);
                 base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
             }
        }

        protected override void _view_OnAddButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            IValuationDiscriminatedSAHLManual valSAHLManualExistingRec = _propRepo.GetValuationByKey(_valManual.Key) as IValuationDiscriminatedSAHLManual;
            CBOMenuNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;

            base.CopyCacheValuesToTarget(valSAHLManualExistingRec);

            // set the property value here so its in the same session
            valSAHLManualExistingRec.Property = _property;

            try
            {
                _propRepo.SaveValuation(valSAHLManualExistingRec);

                // delete the ValuationCottage if it has been "removed" on the update screen
                // if there was never a ValuationCottage record, the delete will run anyway but affect 0 records
                if (valSAHLManualExistingRec.ValuationCottage == null)
                    _propRepo.DeleteValuationCottage(valSAHLManualExistingRec.Key);
                
                txn.VoteCommit();

                base.ClearCache();

                GlobalCacheData.Remove(ViewConstants.ValuationPresenter);
                _view.Navigator.Navigate("Orig_ValuationDetails");
            }

            catch (Exception)
            {
                txn.VoteRollBack();
                if (node != null)
                    base.X2Service.CancelActivity(_view.CurrentPrincipal);

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
