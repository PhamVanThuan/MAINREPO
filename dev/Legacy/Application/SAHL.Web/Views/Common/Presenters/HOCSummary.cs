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


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// HOCSummary Presenter
    /// </summary>
    public class HOCSummary : SAHLCommonBasePresenter<IHOCSummary>
    {
        /// <summary>
        /// CBO Menu Node
        /// </summary>
        protected CBOMenuNode _node;

        /// <summary>
        /// HOC Summary Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public HOCSummary(IHOCSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
            }
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            IHOC _HOC;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            IAccountRepository AccRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            IAccountHOC hocAcct = AccRepo.GetAccountByKey(int.Parse(_node.GenericKey.ToString())) as IAccountHOC;
           
            // This is so the data can be bound to the grid the pages was written with the intention to have > HOC
            List<IHOC> hocs = new List<IHOC>();
            hocs.Add(hocAcct.HOC);

            _HOC = hocAcct.HOC;
       
            if (_HOC != null)
            {
                _view.BindMasterDataControls(_HOC);
                _view.BindDetailDataGrid(hocs);
            }
        }

    }
}
