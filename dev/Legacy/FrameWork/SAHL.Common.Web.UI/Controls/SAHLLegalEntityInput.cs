using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace SAHL.Common.Web.UI.Controls
{
    /// <summary>
    /// Summary description for SAHLLegalEntityInput
    /// </summary>
    [ToolboxBitmap(typeof(SAHLAddressInput), "Resources.SAHLAddressInput.bmp")]
    [ToolboxData("<{0}:SAHLLegalEntityInput runat=server></{0}:SAHLLegalEntityInput>")]
    public class SAHLLegalEntityInput : WebControl, ICallbackEventHandler
    {
        public SAHLLegalEntityInput()
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
