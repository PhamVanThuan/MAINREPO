using System;
using SAHL.Web.Views.Correspondence.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Correspondence.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientCommunicationDebtCounsellingEmail : ClientCommunicationDebtCounselling
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ClientCommunicationDebtCounsellingEmail(IClientCommunication view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            // setup screen mode
            _view.CorrespondenceMedium = SAHL.Common.Globals.CorrespondenceMediums.Email; 

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
