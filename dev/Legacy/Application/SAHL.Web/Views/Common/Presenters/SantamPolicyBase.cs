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
    public class SantamPolicyBase : SAHLCommonBasePresenter<ISantamPolicy>
    {

        private int _genericKey;
        private ISantamPolicyDetailRepository _SantamRepo;
        private ISANTAMPolicyTracking _intSantamPolicy;

        public int GenericKey
        {
            get { return _genericKey; }
            set { _genericKey = value; }
        }

        public ISantamPolicyDetailRepository SantamRepo
        {
            get { return _SantamRepo; }
            set { _SantamRepo = value; }
        }

        public ISANTAMPolicyTracking PolicyDetails
        {
            get { return _intSantamPolicy; }
            set { _intSantamPolicy = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public SantamPolicyBase(ISantamPolicy view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode != null)
                _genericKey = cboNode == null ? -1 : cboNode.GenericKey;

            _SantamRepo = RepositoryFactory.GetRepository<ISantamPolicyDetailRepository>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }
    }
}
