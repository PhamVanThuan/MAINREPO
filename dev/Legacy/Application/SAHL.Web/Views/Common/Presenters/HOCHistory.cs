using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;



namespace SAHL.Web.Views.Common.Presenters
{
   /// <summary>
   /// HOCHistory Presenter
   /// </summary>
    public class HOCHistory : SAHLCommonBasePresenter<IHOCHistoryView>
    {
        /// <summary>
        /// Selected Index on Grid
        /// </summary>
        private int _gridIndexSelected;
        private IEventList<IHOCHistory> hocHistory;
        private IEventList<IHOCHistoryDetail> hocHistoryDetail;
        private IAccountHOC _hocAccount;
        private CBOMenuNode _node;

         /// <summary>
        /// Constructor for HOCFSSummary Presnter
        /// </summary>
        protected IAccountRepository _accRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public HOCHistory(IHOCHistoryView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
            }
        }
      
        /// <summary>
        /// OnView Initialised Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            IFinancialServiceRepository FSR = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            IFinancialService FS = FSR.GetFinancialServiceByKey(_node.GenericKey);
            _hocAccount = FS.Account as IAccountHOC;
            hocHistory = _hocAccount.HOC.HOCHistories;

            _view.SetPostBackType();
            _view.OnHOCHistoryGridsSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnHistoryGridSelectedIndexChanged);

            _view.BindHOCHistoryGrid(hocHistory);
                           
        }
        /// <summary>
        /// View PreRender Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            IHOCHistory hochistory = _hocAccount.HOC.HOCHistories[_gridIndexSelected];
			hocHistoryDetail = hochistory.HOCHistoryDetails;     

            _view.BindHOCDetailGrid(hocHistoryDetail);
        }

        void _view_OnHistoryGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            _gridIndexSelected = (Convert.ToInt32(e.Key));
                
        }
        /// <summary>
        /// Use by Test
        /// </summary>
        public IAccountHOC HOCAccount
        {
            set { _hocAccount = value; }
        }
    }
}
