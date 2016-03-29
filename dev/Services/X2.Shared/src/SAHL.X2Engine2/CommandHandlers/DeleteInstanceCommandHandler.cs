using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class DeleteInstanceCommandHandler : IServiceCommandHandler<DeleteInstanceCommand>
    {
        public ISystemMessageCollection HandleCommand(DeleteInstanceCommand command, IServiceRequestMetadata metadata)
        {
            using (var db = new Db().InWorkflowContext())
            {
                db.DeleteByKey<InstanceDataModel, long>(command.InstanceID);
                db.Complete();
            }
            return new SystemMessageCollection();
        }
    }
}