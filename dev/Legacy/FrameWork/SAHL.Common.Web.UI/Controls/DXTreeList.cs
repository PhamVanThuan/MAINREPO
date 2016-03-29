using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Web.ASPxTreeList;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;

namespace SAHL.Common.Web.UI.Controls
{
    public class DXTreeList : ASPxTreeList
    {
        #region Overridden Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        public override void DataBind()
        {
            base.DataBind();
        }

        #endregion


        #region Private Methods
        #endregion

        /// <summary>
        /// this method will walk up the treelist expandign all parents from the selected node
        /// </summary>
        /// <param name="tn"></param>
        public void ExpandToSelected(TreeListNode tn)
        {
            tn.Expanded = true;
            if (tn.ParentNode == null)
                return;
            ExpandToSelected(tn.ParentNode);
        }
    }
}
