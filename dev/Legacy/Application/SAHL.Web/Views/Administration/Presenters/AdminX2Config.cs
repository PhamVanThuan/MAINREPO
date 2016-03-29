using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class AdminX2Config : SAHLCommonBasePresenter<IX2Config>
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public AdminX2Config(IX2Config view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);


            using (DBHelper db = new DBHelper(Databases.X2))
            {
                DataTable dt = new DataTable();
                db.Fill(dt, UIStatementRepository.GetStatement("X2", "ProcessLatestGet"));
                _view.BindProcessInfo(dt);
                dt.Dispose();
            }

        }

    }
}
