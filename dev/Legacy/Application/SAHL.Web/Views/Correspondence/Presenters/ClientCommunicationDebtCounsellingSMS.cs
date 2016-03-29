using System;
using SAHL.Web.Views.Correspondence.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Correspondence.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientCommunicationDebtCounsellingSMS : ClientCommunicationDebtCounselling
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ClientCommunicationDebtCounsellingSMS(IClientCommunication view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            // setup screen mode
            _view.CorrespondenceMedium = SAHL.Common.Globals.CorrespondenceMediums.SMS; 
            
            // setup sms types
            base.SMSTypes.Add(SAHL.Common.Globals.SMSTypes.Free_Format);
            base.SMSTypes.Add(SAHL.Common.Globals.SMSTypes.Banking_Details);

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            //Set controls from cached object
            if (this.GlobalCacheData.ContainsKey("ClientCommunication"))
            {
                SelectedClientCommuncation selectedClientCommunication = this.GlobalCacheData["ClientCommunication"] as SelectedClientCommuncation;
                if (selectedClientCommunication.CorrespondenceMedium == CorrespondenceMediums.SMS)
                {
                    _view.SMSText = selectedClientCommunication.Body;
                    _view.SMSType = selectedClientCommunication.SMSType;
                }
            }
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }
    }
}
