using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Correspondence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.Views.Correspondence.Presenters
{
    public class ClientCommunicationPersonalLoanEmail : ClientCommunicationPersonalLoan
    {

        public ClientCommunicationPersonalLoanEmail(IClientCommunication view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            // setup screen mode
            _view.CorrespondenceMedium = CorrespondenceMediums.Email;

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            //Set controls from cached object
            if (this.GlobalCacheData.ContainsKey("ClientCommunication"))
            {
                SelectedClientCommuncation selectedClientCommunication = this.GlobalCacheData["ClientCommunication"] as SelectedClientCommuncation;
                if (selectedClientCommunication.CorrespondenceMedium == CorrespondenceMediums.Email)
                {
                    _view.EmailBody = selectedClientCommunication.Body;
                    _view.EmailSubject = selectedClientCommunication.Subject;
                }
            }
        }
    }
}