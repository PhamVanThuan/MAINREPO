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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Factories;
using SAHL.Web.Views.Common.Presenters.FutureDatedTransactions;
using SAHL.Common.Web.UI.Controls;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters.DebitOrderDetailsFinancialService
{
    /// <summary>
    /// 
    /// </summary>
    public class DebitOrderDetailsFSDelete : DebitOrderDetailsFSBase
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DebitOrderDetailsFSDelete(IDebitOrderDetails view, SAHLCommonBaseController controller)
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

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnDeleteButtonClicked += new KeyChangedEventHandler(_view_OnDeleteButtonClicked);

            // IFinancialServiceRepository FSR = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            //IFinancialService fs = FSR.GetFinancialServiceByKey(int.Parse(base.MenuNode.GenericKey.ToString()));            
            _view.gridPostBackType = GridPostBackType.SingleClick;

            _view.BindGrid(base.FinancialService);

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
            _view.ShowButtons = true;
            _view.ShowControls = false;
            _view.ShowLabels = false;
            _view.ButtonDeleteVisible = true;
        }

        void _view_OnDeleteButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            if (_view.FutureDatedChangeKey == -1)
            {
                _view.Messages.Add(new Error("Cannot delete the active debit order", "Cannot delete the active debit order"));
                return;
            }
            BuildMappingTable();
            TransactionScope ts = new TransactionScope();
            try
            {
                IFutureDatedChangeRepository FDCRepo = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();
                IFutureDatedChange fdChange = FDCRepo.GetFutureDatedChangeByKey(_view.FutureDatedChangeKey.Value);
                
                IFutureDatedChange fdcFixed = null;
                if (FDCMap.ContainsKey(fdChange.Key))
                    fdcFixed = FDCRepo.GetFutureDatedChangeByKey(Convert.ToInt32(FDCMap[fdChange.Key]));

                if (fdChange != null)
                    FDCRepo.DeleteFutureDateChangeByKey(fdChange.Key,true);

                if (fdcFixed != null)
                    FDCRepo.DeleteFutureDateChangeByKey(fdcFixed.Key, true);
               
                ts.VoteCommit();
                _view.Navigator.Navigate("DebitOrderDetails");
            }
            
            catch (Exception)
            {
                ts.VoteRollBack();
                if (_view.IsValid)
                {
                    throw;
                }
            }
            finally
            {
                ts.Dispose();
            }
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("DebitOrderDetails");
        }
    }
}
