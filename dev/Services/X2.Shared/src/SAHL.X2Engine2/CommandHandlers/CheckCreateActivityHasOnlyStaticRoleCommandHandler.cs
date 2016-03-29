using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class CheckCreateActivityHasOnlyStaticRoleCommandHandler : IServiceCommandHandler<CheckCreateActivityHasOnlyStaticRoleCommand>
    {
        public IWorkflowDataProvider WorkflowDataProvider { get; protected set; }

        public CheckCreateActivityHasOnlyStaticRoleCommandHandler(IWorkflowDataProvider workflowDataProvider)
        {
            this.WorkflowDataProvider = workflowDataProvider;
        }

        public ISystemMessageCollection HandleCommand(CheckCreateActivityHasOnlyStaticRoleCommand command, IServiceRequestMetadata metadata)
        {
            var activitySecurities = WorkflowDataProvider.GetActivitySecurityForActivity(command.Activity.ActivityID);
            foreach (var activitySecurity in activitySecurities)
            {
                var securityGroup = WorkflowDataProvider.GetSecurityGroup(activitySecurity.SecurityGroupID);
                switch (securityGroup.Name)
                {
                    case "Everyone":
                        {
                            command.Result = true;
                            return new SystemMessageCollection();
                        }
                    case "Originator":
                        {
                            // there is no originator yet in a create activity
                            command.Result = false;
                            return new SystemMessageCollection();
                        }
                    case "WorkList":
                        {
                            // there is no worklist yet in a create activity
                            command.Result = false;
                            return new SystemMessageCollection();
                        }
                    // there is no tracklist yet in a create activity
                    case "TrackList":
                        {
                            command.Result = false;
                            return new SystemMessageCollection();
                        }
                    default:
                        break;
                }
            }
            command.Result = true;
            return new SystemMessageCollection();
        }
    }
}