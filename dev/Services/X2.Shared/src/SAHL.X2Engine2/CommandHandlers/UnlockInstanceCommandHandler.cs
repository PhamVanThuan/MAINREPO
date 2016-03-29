using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.ViewModels.SqlStatement;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class UnlockInstanceCommandHandler : IServiceCommandHandler<UnlockInstanceCommand>
    {
        public ISystemMessageCollection HandleCommand(UnlockInstanceCommand command, IServiceRequestMetadata metadata)
        {
            try
            {
                using (var db = new Db().InWorkflowContext())
                {
                    InstanceDataModel instance = db.SelectOne<InstanceDataModel>(UIStatements.instancedatamodel_selectbykey, new { PrimaryKey = command.InstanceID });
                    instance.ActivityADUserName = null;
                    instance.ActivityID = null;
                    instance.ActivityDate = null;

                    db.Update<InstanceDataModel>(new UnlockInstanceSqlStatement(command.InstanceID));
                    db.Complete();
                }
                return new SystemMessageCollection();
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}