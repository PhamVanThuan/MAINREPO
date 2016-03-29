using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Configuration
{
    public class UrlConfiguration
    {
        public UrlConfiguration(string controller, string controllerAction, UrlAction urlAction)
        {
            this.UrlAction = urlAction;
            this.Controller = controller;
            this.ControllerAction = controllerAction;
        }

        public UrlAction UrlAction { get; protected set; }

        public string Controller { get; protected set; }

        public string ControllerAction { get; protected set; }

        public string GenerateActionUrl()
        {
            return string.Format("{0}/{1}", this.Controller.ToLower(), this.ControllerAction.ToLower());
        }

        public string GenerateActionUrl(BusinessKey businessKey)
        {
            return string.Format("{0}?Key={1}&KeyType={1}", this.GenerateActionUrl(), businessKey.Key, businessKey.KeyType);
        }
    }
}