using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class PremiumHistoryAdmin : PremiumHistoryBase
    {
        private IAccountRepository _accountRepository;
        private SAHL.Common.BusinessModel.Interfaces.IAccount _account;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PremiumHistoryAdmin(IPremiumHistory view, SAHL.Common.Web.UI.SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // get the life account object
            _accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            _account = _accountRepository.GetAccountByKey(base.GenericKey);

            base.AccountKey = _account.Key;
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.ShowLifeWorkFlowHeader = false;
            _view.ShowCancelButton = false;
        }
    }
}
