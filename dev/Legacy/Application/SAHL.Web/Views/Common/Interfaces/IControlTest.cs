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
using DevExpress.Web.ASPxTreeList;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IControlTest : IViewBase
    {
        #region View Properties

        DataRow GetFocusedNode { get; }

        #endregion

        #region EventHandlers

        event EventHandler RemoveButtonClicked;

        event TreeListNodeDragEventHandler TreeNodeDragged;

        #endregion

        #region  View Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgStructLst"></param>
        void BindOrganisationStructure(DataSet orgStructLst);



        #endregion
    }
}
