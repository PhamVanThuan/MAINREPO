using SAHL.Core.Data.Models.X2;

namespace SAHL.X2Engine2.Commands
{
    public class CheckActivityIsValidForStateCommand : RuleCommand
    {
        public InstanceDataModel Instance { get; protected set; }

        public int? StateId { get; protected set; }

        public CheckActivityIsValidForStateCommand(InstanceDataModel instance, int? stateId)
        {
            this.Instance = instance;
            this.StateId = stateId;
        }
    }
}