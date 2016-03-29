using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;

namespace SAHL.Web.Views.Administration
{
    public partial class CBOSummary : SAHLCommonBaseView, IViewCBOSummary
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!ShouldRunPage) return;
        //}

        #region IViewCBOSummary Members

        public void BindCBO(SAHL.Common.BusinessModel.Interfaces.ICBOMenu CBO)
        {
            if (null != CBO.ParentMenu)
                txtCBOParent.Text = CBO.ParentMenu.Description;
            else
                txtCBOParent.Text = "NULL";
            txtDesc.Text = CBO.Description;
            txtExpandLevel.Text = CBO.ExpandLevel.ToString();
            if (null != CBO.Feature)
                txtFeature.Text = CBO.Feature.ShortName;
            txtGenericKeyType.Text = CBO.GenericKeyType.Key.ToString();
            txtHasOrogSource.Text = CBO.HasOriginationSource.ToString();
            txtIncludeParent.Text = CBO.IncludeParentHeaderIcons.ToString();
            txtIsremovable.Text = CBO.IsRemovable.ToString();
            txtMenuIcon.Text = CBO.MenuIcon;
            txtNodeType.Text = CBO.MenuIcon;
            txtSequence.Text = CBO.Sequence.ToString();
            txtUIStatement.Text = CBO.StatementNameKey;
            txtURL.Text = CBO.URL;
        }

        public event EventHandler OnFinishClick;

        #endregion

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (null != OnFinishClick)
            {
                OnFinishClick(null, null);
            }
        }
    }
}
