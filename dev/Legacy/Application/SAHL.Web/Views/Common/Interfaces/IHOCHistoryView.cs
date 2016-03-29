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
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// Interface for HOC History View
    /// </summary>
    public interface IHOCHistoryView : IViewBase
    {
        #region events
        /// <summary>
        /// HistoryGrid Selected Index Change event
        /// </summary>
        event KeyChangedEventHandler OnHOCHistoryGridsSelectedIndexChanged;

        #endregion

        #region Methods
        /// <summary>
        /// Bind HOC History Grid
        /// </summary>
        /// <param name="hocHistory"></param>
        void BindHOCHistoryGrid(IEventList<IHOCHistory> hocHistory);
        /// <summary>
        /// Bind HOCHistoryDetail Grid
        /// </summary>
        /// <param name="hocHistoryDetail"></param>
        void BindHOCDetailGrid(IEventList<IHOCHistoryDetail> hocHistoryDetail);
        /// <summary>
        /// Set Postback Type on Grid
        /// </summary>
        void SetPostBackType();

        #endregion

    }
}
