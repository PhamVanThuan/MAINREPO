using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class SaveInstanceCommand : ServiceCommand
    {
        public InstanceDataModel Instance { get; protected set; }

        public SaveInstanceCommand(InstanceDataModel instance)
        {
            this.Instance = instance;
        }
    }
}