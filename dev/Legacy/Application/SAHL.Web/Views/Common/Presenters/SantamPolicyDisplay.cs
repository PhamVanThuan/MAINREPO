using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common.Presenters
{
    public class SantamPolicyDisplay : SantamPolicyBase
    {

       /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public SantamPolicyDisplay(ISantamPolicy view, SAHLCommonBaseController controller)
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
            ISantamPolicyDetailRepository santamRepo = RepositoryFactory.GetRepository<ISantamPolicyDetailRepository>();
            base.PolicyDetails = santamRepo.GetSantamPolicyByAccountKey(base.GenericKey);
            //base.IntSantamPolicyTracking = santamRepo.GetSantamPolicyByAccountKey(1509295);

            if (base.PolicyDetails != null)
            {
                _view.BindPolicyDetails(PolicyDetails);
            }
            else
            {
                //There is no policy info, so display custom message
                _view.DisplayNoPolicy();
            }
            // do the bind of the data
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;
               
        }
    }
}
