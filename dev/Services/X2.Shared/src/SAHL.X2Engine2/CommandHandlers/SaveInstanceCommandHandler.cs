using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class SaveInstanceCommandHandler : IServiceCommandHandler<SaveInstanceCommand>
    {
        public ISystemMessageCollection HandleCommand(SaveInstanceCommand command, IServiceRequestMetadata metadata)
        {
            using (var db = new Db().InWorkflowContext())
            {
                db.Update<InstanceDataModel>(command.Instance);
                db.Complete();
            }
            return new SystemMessageCollection();
        }
    }
}