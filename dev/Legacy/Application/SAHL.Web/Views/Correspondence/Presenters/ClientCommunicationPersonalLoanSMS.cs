using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Correspondence.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.Correspondence.Presenters
{
    public class ClientCommunicationPersonalLoanSMS : ClientCommunicationPersonalLoan
    {
        public ClientCommunicationPersonalLoanSMS(IClientCommunication view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            // setup screen mode
            _view.CorrespondenceMedium = CorrespondenceMediums.SMS;

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