using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Controls;
using System;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDocumentChecklist : IViewBase
    {
        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        #endregion


        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentList"></param>
        void BindDocumentList(IList<IApplicationDocument> documentList);

        /// <summary>
        /// 
        /// </summary>
        Dictionary<int, bool> GetCheckedItems { get;}

        /// <summary>
        /// 
        /// </summary>
        bool SetViewOnly { set;}

        /// <summary>
        /// 
        /// </summary>
        bool HideControls { set;}

        #endregion

    }
}
