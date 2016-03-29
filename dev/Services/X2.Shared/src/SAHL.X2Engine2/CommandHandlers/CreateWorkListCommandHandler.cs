using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class CreateWorkListCommandHandler : IServiceCommandHandler<CreateWorkListCommand>
    {
        public ISystemMessageCollection HandleCommand(CreateWorkListCommand command, IServiceRequestMetadata metadata)
        {
            using (var db = new Db().InWorkflowContext())
            {
                db.Insert<WorkListDataModel>(command.WorkListDataModels);
                db.Complete();
            }
            return new SystemMessageCollection();
        }
    }
}