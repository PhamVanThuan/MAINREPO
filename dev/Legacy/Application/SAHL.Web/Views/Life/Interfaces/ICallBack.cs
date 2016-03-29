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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Life.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICallBack : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;
        
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        DateTime? CallBackDate { get; set;}

        /// <summary>
        /// 
        /// </summary>
        string CallBackComment { get; set;}

        /// <summary>
        /// 
        /// </summary>
        int ReasonDefinitionKey { get; set;}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstCallbacks"></param>
        void BindCallBackGrid(IEventList<SAHL.Common.BusinessModel.Interfaces.ICallback> lstCallbacks);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reasons"></param>
        /// <param name="enabled"></param>
        void BindCallBackReasons(IDictionary<int, string> reasons, bool enabled);
    }   
}
