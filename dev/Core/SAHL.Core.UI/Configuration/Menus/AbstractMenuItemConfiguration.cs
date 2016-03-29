using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Configuration.Menus
{
    public abstract class AbstractMenuItemConfiguration : IRequiredFeatureAccess
    {
        public AbstractMenuItemConfiguration(int sequence, string controller, string controllerAction, UrlAction urlAction, string requiredFeatureAccess)
        {
            this.Sequence = sequence;
            this.RequiredFeatureAccess = requiredFeatureAccess;
            this.Url = new UrlConfiguration(controller, controllerAction, urlAction);
        }

        public AbstractMenuItemConfiguration(int sequence, string controller, string controllerAction, string requiredFeatureAccess)
            : this(sequence, controller, controllerAction, UrlAction.LinkNavigation, requiredFeatureAccess)
        {
        }

        public int Sequence { get; protected set; }

        public UrlConfiguration Url { get; protected set; }

        public string RequiredFeatureAccess { get; protected set; }
    }
}