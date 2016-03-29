using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class CheckCreateActivityHasOnlyStaticRoleCommand : RuleCommand
    {
        public Activity Activity { get; protected set; }

        public string UserName { get; protected set; }

        public CheckCreateActivityHasOnlyStaticRoleCommand(Activity activity, string userName)
        {
            this.Activity = activity;
            this.UserName = userName;
        }
    }
}