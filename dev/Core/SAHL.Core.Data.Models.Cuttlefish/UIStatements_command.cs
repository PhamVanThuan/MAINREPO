using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string commanddatamodel_selectwhere = "SELECT CommandKey, Data, CommandInsertDate, MachineName, ServiceName, HasCompleted, HasFailed, ContextValues, NotAuthenticated, NotAuthorized FROM [Cuttlefish].[command].[Command] WHERE";
        public const string commanddatamodel_selectbykey = "SELECT CommandKey, Data, CommandInsertDate, MachineName, ServiceName, HasCompleted, HasFailed, ContextValues, NotAuthenticated, NotAuthorized FROM [Cuttlefish].[command].[Command] WHERE CommandKey = @PrimaryKey";
        public const string commanddatamodel_delete = "DELETE FROM [Cuttlefish].[command].[Command] WHERE CommandKey = @PrimaryKey";
        public const string commanddatamodel_deletewhere = "DELETE FROM [Cuttlefish].[command].[Command] WHERE";
        public const string commanddatamodel_insert = "INSERT INTO [Cuttlefish].[command].[Command] (Data, CommandInsertDate, MachineName, ServiceName, HasCompleted, HasFailed, ContextValues, NotAuthenticated, NotAuthorized) VALUES(@Data, @CommandInsertDate, @MachineName, @ServiceName, @HasCompleted, @HasFailed, @ContextValues, @NotAuthenticated, @NotAuthorized); select cast(scope_identity() as int)";
        public const string commanddatamodel_update = "UPDATE [Cuttlefish].[command].[Command] SET Data = @Data, CommandInsertDate = @CommandInsertDate, MachineName = @MachineName, ServiceName = @ServiceName, HasCompleted = @HasCompleted, HasFailed = @HasFailed, ContextValues = @ContextValues, NotAuthenticated = @NotAuthenticated, NotAuthorized = @NotAuthorized WHERE CommandKey = @CommandKey";



    }
}