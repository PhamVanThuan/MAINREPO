using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SAHL.Common.Web.UI.Controls
{
/// <summary>
/// Summary description for SAHLAddressInput
/// </summary>
//    [ToolboxBitmap(typeof(SAHLAddressInput), "Resources.SAHLAddressInput.bmp")]
    [ToolboxData("<{0}:SAHLAddressInput runat=server></{0}:SAHLAddressInput>")]
    public class SAHLAddressInput : System.Web.UI.WebControls.WebControl, System.Web.UI.ICallbackEventHandler
    {
        public SAHLAddressInput()
        {

        }

        #region ICallbackEventHandler Members

        public string GetCallbackResult()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
