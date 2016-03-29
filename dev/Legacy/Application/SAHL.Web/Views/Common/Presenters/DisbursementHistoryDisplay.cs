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
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel;
using SAHL.Common.UI;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System.Collections;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class DisbursementHistoryDisplay : DisbursementHistoryBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DisbursementHistoryDisplay(IDisbursementHistory view, SAHLCommonBaseController controller)
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
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            base.Account = accRepo.GetAccountByKey(base.GenericKey);

            // do the bind of the data
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;
               
        }
    }
}
