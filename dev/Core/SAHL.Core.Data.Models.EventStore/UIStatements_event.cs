using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.EventStore
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string domaindatamodel_selectwhere = "SELECT DomainKey, Name FROM [eventstore].[event].[Domain] WHERE";
        public const string domaindatamodel_selectbykey = "SELECT DomainKey, Name FROM [eventstore].[event].[Domain] WHERE DomainKey = @PrimaryKey";
        public const string domaindatamodel_delete = "DELETE FROM [eventstore].[event].[Domain] WHERE DomainKey = @PrimaryKey";
        public const string domaindatamodel_deletewhere = "DELETE FROM [eventstore].[event].[Domain] WHERE";
        public const string domaindatamodel_insert = "INSERT INTO [eventstore].[event].[Domain] (Name) VALUES(@Name); select cast(scope_identity() as int)";
        public const string domaindatamodel_update = "UPDATE [eventstore].[event].[Domain] SET Name = @Name WHERE DomainKey = @DomainKey";



        public const string eventtypedatamodel_selectwhere = "SELECT EventTypeKey, Name, ClassName, DomainKey, Version FROM [eventstore].[event].[EventType] WHERE";
        public const string eventtypedatamodel_selectbykey = "SELECT EventTypeKey, Name, ClassName, DomainKey, Version FROM [eventstore].[event].[EventType] WHERE EventTypeKey = @PrimaryKey";
        public const string eventtypedatamodel_delete = "DELETE FROM [eventstore].[event].[EventType] WHERE EventTypeKey = @PrimaryKey";
        public const string eventtypedatamodel_deletewhere = "DELETE FROM [eventstore].[event].[EventType] WHERE";
        public const string eventtypedatamodel_insert = "INSERT INTO [eventstore].[event].[EventType] (EventTypeKey, Name, ClassName, DomainKey, Version) VALUES(@EventTypeKey, @Name, @ClassName, @DomainKey, @Version); ";
        public const string eventtypedatamodel_update = "UPDATE [eventstore].[event].[EventType] SET EventTypeKey = @EventTypeKey, Name = @Name, ClassName = @ClassName, DomainKey = @DomainKey, Version = @Version WHERE EventTypeKey = @EventTypeKey";



        public const string eventdatamodel_selectwhere = "SELECT EventKey, EventTypeKey, GenericKey, GenericKeyTypeKey, Data, CorrelationID, EventInsertDate, EventEffectiveDate, Processed, Metadata FROM [eventstore].[event].[Event] WHERE";
        public const string eventdatamodel_selectbykey = "SELECT EventKey, EventTypeKey, GenericKey, GenericKeyTypeKey, Data, CorrelationID, EventInsertDate, EventEffectiveDate, Processed, Metadata FROM [eventstore].[event].[Event] WHERE EventKey = @PrimaryKey";
        public const string eventdatamodel_delete = "DELETE FROM [eventstore].[event].[Event] WHERE EventKey = @PrimaryKey";
        public const string eventdatamodel_deletewhere = "DELETE FROM [eventstore].[event].[Event] WHERE";
        public const string eventdatamodel_insert = "INSERT INTO [eventstore].[event].[Event] (EventTypeKey, GenericKey, GenericKeyTypeKey, Data, CorrelationID, EventInsertDate, EventEffectiveDate, Processed, Metadata) VALUES(@EventTypeKey, @GenericKey, @GenericKeyTypeKey, @Data, @CorrelationID, @EventInsertDate, @EventEffectiveDate, @Processed, @Metadata); select cast(scope_identity() as int)";
        public const string eventdatamodel_update = "UPDATE [eventstore].[event].[Event] SET EventTypeKey = @EventTypeKey, GenericKey = @GenericKey, GenericKeyTypeKey = @GenericKeyTypeKey, Data = @Data, CorrelationID = @CorrelationID, EventInsertDate = @EventInsertDate, EventEffectiveDate = @EventEffectiveDate, Processed = @Processed, Metadata = @Metadata WHERE EventKey = @EventKey";



    }
}