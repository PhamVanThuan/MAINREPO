using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class CheckCreationActivityAccessCommand : RuleCommand
    {
        public Activity Activity { get; protected set; }

        public CheckCreationActivityAccessCommand(Activity activity)
        {
            this.Activity = activity;
        }
    }
}