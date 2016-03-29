using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string timeunitreferencedatamodel_selectwhere = "SELECT Id, TimeUnit, ThroughputMetricMessage_id FROM [Cuttlefish].[dbo].[TimeUnitReference] WHERE";
        public const string timeunitreferencedatamodel_selectbykey = "SELECT Id, TimeUnit, ThroughputMetricMessage_id FROM [Cuttlefish].[dbo].[TimeUnitReference] WHERE Id = @PrimaryKey";
        public const string timeunitreferencedatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[TimeUnitReference] WHERE Id = @PrimaryKey";
        public const string timeunitreferencedatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[TimeUnitReference] WHERE";
        public const string timeunitreferencedatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[TimeUnitReference] (TimeUnit, ThroughputMetricMessage_id) VALUES(@TimeUnit, @ThroughputMetricMessage_id); select cast(scope_identity() as int)";
        public const string timeunitreferencedatamodel_update = "UPDATE [Cuttlefish].[dbo].[TimeUnitReference] SET TimeUnit = @TimeUnit, ThroughputMetricMessage_id = @ThroughputMetricMessage_id WHERE Id = @Id";



        public const string logmessagedatamodel_selectwhere = "SELECT Id, MessageDate, LogMessageType, MethodName, Message, Source, UserName, MachineName, Application FROM [Cuttlefish].[dbo].[LogMessage] WHERE";
        public const string logmessagedatamodel_selectbykey = "SELECT Id, MessageDate, LogMessageType, MethodName, Message, Source, UserName, MachineName, Application FROM [Cuttlefish].[dbo].[LogMessage] WHERE Id = @PrimaryKey";
        public const string logmessagedatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[LogMessage] WHERE Id = @PrimaryKey";
        public const string logmessagedatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[LogMessage] WHERE";
        public const string logmessagedatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[LogMessage] (MessageDate, LogMessageType, MethodName, Message, Source, UserName, MachineName, Application) VALUES(@MessageDate, @LogMessageType, @MethodName, @Message, @Source, @UserName, @MachineName, @Application); select cast(scope_identity() as int)";
        public const string logmessagedatamodel_update = "UPDATE [Cuttlefish].[dbo].[LogMessage] SET MessageDate = @MessageDate, LogMessageType = @LogMessageType, MethodName = @MethodName, Message = @Message, Source = @Source, UserName = @UserName, MachineName = @MachineName, Application = @Application WHERE Id = @Id";



        public const string messageparametersdatamodel_selectwhere = "SELECT LogMessage_id, ParameterKey, ParameterValue FROM [Cuttlefish].[dbo].[MessageParameters] WHERE";
        public const string messageparametersdatamodel_selectbykey = "SELECT LogMessage_id, ParameterKey, ParameterValue FROM [Cuttlefish].[dbo].[MessageParameters] WHERE  = @PrimaryKey";
        public const string messageparametersdatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[MessageParameters] WHERE  = @PrimaryKey";
        public const string messageparametersdatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[MessageParameters] WHERE";
        public const string messageparametersdatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[MessageParameters] (LogMessage_id, ParameterKey, ParameterValue) VALUES(@LogMessage_id, @ParameterKey, @ParameterValue); ";
        public const string messageparametersdatamodel_update = "UPDATE [Cuttlefish].[dbo].[MessageParameters] SET LogMessage_id = @LogMessage_id, ParameterKey = @ParameterKey, ParameterValue = @ParameterValue WHERE  = @";



        public const string instantaneousvaluemetricmessagedatamodel_selectwhere = "SELECT Id, InstantaneousValue, Source, UserName, MessageDate, MachineName, Application FROM [Cuttlefish].[dbo].[InstantaneousValueMetricMessage] WHERE";
        public const string instantaneousvaluemetricmessagedatamodel_selectbykey = "SELECT Id, InstantaneousValue, Source, UserName, MessageDate, MachineName, Application FROM [Cuttlefish].[dbo].[InstantaneousValueMetricMessage] WHERE Id = @PrimaryKey";
        public const string instantaneousvaluemetricmessagedatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[InstantaneousValueMetricMessage] WHERE Id = @PrimaryKey";
        public const string instantaneousvaluemetricmessagedatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[InstantaneousValueMetricMessage] WHERE";
        public const string instantaneousvaluemetricmessagedatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[InstantaneousValueMetricMessage] (InstantaneousValue, Source, UserName, MessageDate, MachineName, Application) VALUES(@InstantaneousValue, @Source, @UserName, @MessageDate, @MachineName, @Application); select cast(scope_identity() as int)";
        public const string instantaneousvaluemetricmessagedatamodel_update = "UPDATE [Cuttlefish].[dbo].[InstantaneousValueMetricMessage] SET InstantaneousValue = @InstantaneousValue, Source = @Source, UserName = @UserName, MessageDate = @MessageDate, MachineName = @MachineName, Application = @Application WHERE Id = @Id";



        public const string latencymetricmessagedatamodel_selectwhere = "SELECT Id, StartTime, Duration, Source, UserName, MessageDate, MachineName, Application, Metric FROM [Cuttlefish].[dbo].[LatencyMetricMessage] WHERE";
        public const string latencymetricmessagedatamodel_selectbykey = "SELECT Id, StartTime, Duration, Source, UserName, MessageDate, MachineName, Application, Metric FROM [Cuttlefish].[dbo].[LatencyMetricMessage] WHERE Id = @PrimaryKey";
        public const string latencymetricmessagedatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[LatencyMetricMessage] WHERE Id = @PrimaryKey";
        public const string latencymetricmessagedatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[LatencyMetricMessage] WHERE";
        public const string latencymetricmessagedatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[LatencyMetricMessage] (StartTime, Duration, Source, UserName, MessageDate, MachineName, Application, Metric) VALUES(@StartTime, @Duration, @Source, @UserName, @MessageDate, @MachineName, @Application, @Metric); select cast(scope_identity() as int)";
        public const string latencymetricmessagedatamodel_update = "UPDATE [Cuttlefish].[dbo].[LatencyMetricMessage] SET StartTime = @StartTime, Duration = @Duration, Source = @Source, UserName = @UserName, MessageDate = @MessageDate, MachineName = @MachineName, Application = @Application, Metric = @Metric WHERE Id = @Id";



        public const string throughputmetricmessagedatamodel_selectwhere = "SELECT Id, Source, UserName, MessageDate, MachineName, Application, Metric FROM [Cuttlefish].[dbo].[ThroughputMetricMessage] WHERE";
        public const string throughputmetricmessagedatamodel_selectbykey = "SELECT Id, Source, UserName, MessageDate, MachineName, Application, Metric FROM [Cuttlefish].[dbo].[ThroughputMetricMessage] WHERE Id = @PrimaryKey";
        public const string throughputmetricmessagedatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[ThroughputMetricMessage] WHERE Id = @PrimaryKey";
        public const string throughputmetricmessagedatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[ThroughputMetricMessage] WHERE";
        public const string throughputmetricmessagedatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[ThroughputMetricMessage] (Source, UserName, MessageDate, MachineName, Application, Metric) VALUES(@Source, @UserName, @MessageDate, @MachineName, @Application, @Metric); select cast(scope_identity() as int)";
        public const string throughputmetricmessagedatamodel_update = "UPDATE [Cuttlefish].[dbo].[ThroughputMetricMessage] SET Source = @Source, UserName = @UserName, MessageDate = @MessageDate, MachineName = @MachineName, Application = @Application, Metric = @Metric WHERE Id = @Id";



        public const string applicationconfigdatamodel_selectwhere = "SELECT Id, Name, Description, SystemConfig_id FROM [Cuttlefish].[dbo].[ApplicationConfig] WHERE";
        public const string applicationconfigdatamodel_selectbykey = "SELECT Id, Name, Description, SystemConfig_id FROM [Cuttlefish].[dbo].[ApplicationConfig] WHERE Id = @PrimaryKey";
        public const string applicationconfigdatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[ApplicationConfig] WHERE Id = @PrimaryKey";
        public const string applicationconfigdatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[ApplicationConfig] WHERE";
        public const string applicationconfigdatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[ApplicationConfig] (Name, Description, SystemConfig_id) VALUES(@Name, @Description, @SystemConfig_id); select cast(scope_identity() as int)";
        public const string applicationconfigdatamodel_update = "UPDATE [Cuttlefish].[dbo].[ApplicationConfig] SET Name = @Name, Description = @Description, SystemConfig_id = @SystemConfig_id WHERE Id = @Id";



        public const string environmentconfigdatamodel_selectwhere = "SELECT Id, Name, QueryAnalyserServerName, SystemConfig_id FROM [Cuttlefish].[dbo].[EnvironmentConfig] WHERE";
        public const string environmentconfigdatamodel_selectbykey = "SELECT Id, Name, QueryAnalyserServerName, SystemConfig_id FROM [Cuttlefish].[dbo].[EnvironmentConfig] WHERE Id = @PrimaryKey";
        public const string environmentconfigdatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[EnvironmentConfig] WHERE Id = @PrimaryKey";
        public const string environmentconfigdatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[EnvironmentConfig] WHERE";
        public const string environmentconfigdatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[EnvironmentConfig] (Name, QueryAnalyserServerName, SystemConfig_id) VALUES(@Name, @QueryAnalyserServerName, @SystemConfig_id); select cast(scope_identity() as int)";
        public const string environmentconfigdatamodel_update = "UPDATE [Cuttlefish].[dbo].[EnvironmentConfig] SET Name = @Name, QueryAnalyserServerName = @QueryAnalyserServerName, SystemConfig_id = @SystemConfig_id WHERE Id = @Id";



        public const string machineconfigdatamodel_selectwhere = "SELECT Id, Name, EnvironmentConfig_id FROM [Cuttlefish].[dbo].[MachineConfig] WHERE";
        public const string machineconfigdatamodel_selectbykey = "SELECT Id, Name, EnvironmentConfig_id FROM [Cuttlefish].[dbo].[MachineConfig] WHERE Id = @PrimaryKey";
        public const string machineconfigdatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[MachineConfig] WHERE Id = @PrimaryKey";
        public const string machineconfigdatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[MachineConfig] WHERE";
        public const string machineconfigdatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[MachineConfig] (Name, EnvironmentConfig_id) VALUES(@Name, @EnvironmentConfig_id); select cast(scope_identity() as int)";
        public const string machineconfigdatamodel_update = "UPDATE [Cuttlefish].[dbo].[MachineConfig] SET Name = @Name, EnvironmentConfig_id = @EnvironmentConfig_id WHERE Id = @Id";



        public const string systemconfigdatamodel_selectwhere = "SELECT Id, CuttleFishServerName FROM [Cuttlefish].[dbo].[SystemConfig] WHERE";
        public const string systemconfigdatamodel_selectbykey = "SELECT Id, CuttleFishServerName FROM [Cuttlefish].[dbo].[SystemConfig] WHERE Id = @PrimaryKey";
        public const string systemconfigdatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[SystemConfig] WHERE Id = @PrimaryKey";
        public const string systemconfigdatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[SystemConfig] WHERE";
        public const string systemconfigdatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[SystemConfig] (CuttleFishServerName) VALUES(@CuttleFishServerName); select cast(scope_identity() as int)";
        public const string systemconfigdatamodel_update = "UPDATE [Cuttlefish].[dbo].[SystemConfig] SET CuttleFishServerName = @CuttleFishServerName WHERE Id = @Id";



        public const string applicationmoduledefinitiondatamodel_selectwhere = "SELECT Id, Name FROM [Cuttlefish].[dbo].[ApplicationModuleDefinition] WHERE";
        public const string applicationmoduledefinitiondatamodel_selectbykey = "SELECT Id, Name FROM [Cuttlefish].[dbo].[ApplicationModuleDefinition] WHERE Id = @PrimaryKey";
        public const string applicationmoduledefinitiondatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[ApplicationModuleDefinition] WHERE Id = @PrimaryKey";
        public const string applicationmoduledefinitiondatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[ApplicationModuleDefinition] WHERE";
        public const string applicationmoduledefinitiondatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[ApplicationModuleDefinition] (Name) VALUES(@Name); select cast(scope_identity() as int)";
        public const string applicationmoduledefinitiondatamodel_update = "UPDATE [Cuttlefish].[dbo].[ApplicationModuleDefinition] SET Name = @Name WHERE Id = @Id";



        public const string uniquepropertydefinitiondatamodel_selectwhere = "SELECT Id, DisplayName, PropertyName, IsParameterProperty, ApplicationModuleDefinition_id FROM [Cuttlefish].[dbo].[UniquePropertyDefinition] WHERE";
        public const string uniquepropertydefinitiondatamodel_selectbykey = "SELECT Id, DisplayName, PropertyName, IsParameterProperty, ApplicationModuleDefinition_id FROM [Cuttlefish].[dbo].[UniquePropertyDefinition] WHERE Id = @PrimaryKey";
        public const string uniquepropertydefinitiondatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[UniquePropertyDefinition] WHERE Id = @PrimaryKey";
        public const string uniquepropertydefinitiondatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[UniquePropertyDefinition] WHERE";
        public const string uniquepropertydefinitiondatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[UniquePropertyDefinition] (DisplayName, PropertyName, IsParameterProperty, ApplicationModuleDefinition_id) VALUES(@DisplayName, @PropertyName, @IsParameterProperty, @ApplicationModuleDefinition_id); select cast(scope_identity() as int)";
        public const string uniquepropertydefinitiondatamodel_update = "UPDATE [Cuttlefish].[dbo].[UniquePropertyDefinition] SET DisplayName = @DisplayName, PropertyName = @PropertyName, IsParameterProperty = @IsParameterProperty, ApplicationModuleDefinition_id = @ApplicationModuleDefinition_id WHERE Id = @Id";



        public const string uniquepropertyvaluedatamodel_selectwhere = "SELECT Id, PropertyValue, UniquePropertyDefinition_id FROM [Cuttlefish].[dbo].[UniquePropertyValue] WHERE";
        public const string uniquepropertyvaluedatamodel_selectbykey = "SELECT Id, PropertyValue, UniquePropertyDefinition_id FROM [Cuttlefish].[dbo].[UniquePropertyValue] WHERE Id = @PrimaryKey";
        public const string uniquepropertyvaluedatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[UniquePropertyValue] WHERE Id = @PrimaryKey";
        public const string uniquepropertyvaluedatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[UniquePropertyValue] WHERE";
        public const string uniquepropertyvaluedatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[UniquePropertyValue] (PropertyValue, UniquePropertyDefinition_id) VALUES(@PropertyValue, @UniquePropertyDefinition_id); select cast(scope_identity() as int)";
        public const string uniquepropertyvaluedatamodel_update = "UPDATE [Cuttlefish].[dbo].[UniquePropertyValue] SET PropertyValue = @PropertyValue, UniquePropertyDefinition_id = @UniquePropertyDefinition_id WHERE Id = @Id";



        public const string latencymetricparametersdatamodel_selectwhere = "SELECT LatencyMetricMessage_id, ParameterKey, ParameterValue FROM [Cuttlefish].[dbo].[LatencyMetricParameters] WHERE";
        public const string latencymetricparametersdatamodel_selectbykey = "SELECT LatencyMetricMessage_id, ParameterKey, ParameterValue FROM [Cuttlefish].[dbo].[LatencyMetricParameters] WHERE  = @PrimaryKey";
        public const string latencymetricparametersdatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[LatencyMetricParameters] WHERE  = @PrimaryKey";
        public const string latencymetricparametersdatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[LatencyMetricParameters] WHERE";
        public const string latencymetricparametersdatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[LatencyMetricParameters] (LatencyMetricMessage_id, ParameterKey, ParameterValue) VALUES(@LatencyMetricMessage_id, @ParameterKey, @ParameterValue); ";
        public const string latencymetricparametersdatamodel_update = "UPDATE [Cuttlefish].[dbo].[LatencyMetricParameters] SET LatencyMetricMessage_id = @LatencyMetricMessage_id, ParameterKey = @ParameterKey, ParameterValue = @ParameterValue WHERE  = @";



        public const string throughputmetricparametersdatamodel_selectwhere = "SELECT ThroughputMetricMessage_id, ParameterKey, ParameterValue FROM [Cuttlefish].[dbo].[ThroughputMetricParameters] WHERE";
        public const string throughputmetricparametersdatamodel_selectbykey = "SELECT ThroughputMetricMessage_id, ParameterKey, ParameterValue FROM [Cuttlefish].[dbo].[ThroughputMetricParameters] WHERE  = @PrimaryKey";
        public const string throughputmetricparametersdatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[ThroughputMetricParameters] WHERE  = @PrimaryKey";
        public const string throughputmetricparametersdatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[ThroughputMetricParameters] WHERE";
        public const string throughputmetricparametersdatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[ThroughputMetricParameters] (ThroughputMetricMessage_id, ParameterKey, ParameterValue) VALUES(@ThroughputMetricMessage_id, @ParameterKey, @ParameterValue); ";
        public const string throughputmetricparametersdatamodel_update = "UPDATE [Cuttlefish].[dbo].[ThroughputMetricParameters] SET ThroughputMetricMessage_id = @ThroughputMetricMessage_id, ParameterKey = @ParameterKey, ParameterValue = @ParameterValue WHERE  = @";



        public const string genericstatusdatamodel_selectwhere = "SELECT ID, Description FROM [Cuttlefish].[dbo].[GenericStatus] WHERE";
        public const string genericstatusdatamodel_selectbykey = "SELECT ID, Description FROM [Cuttlefish].[dbo].[GenericStatus] WHERE ID = @PrimaryKey";
        public const string genericstatusdatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[GenericStatus] WHERE ID = @PrimaryKey";
        public const string genericstatusdatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[GenericStatus] WHERE";
        public const string genericstatusdatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[GenericStatus] (ID, Description) VALUES(@ID, @Description); ";
        public const string genericstatusdatamodel_update = "UPDATE [Cuttlefish].[dbo].[GenericStatus] SET ID = @ID, Description = @Description WHERE ID = @ID";



        public const string genericmessagedatamodel_selectwhere = "SELECT ID, MessageDate, MessageContent, MessageType, GenericStatusID, BatchID, FailureCount FROM [Cuttlefish].[dbo].[GenericMessage] WHERE";
        public const string genericmessagedatamodel_selectbykey = "SELECT ID, MessageDate, MessageContent, MessageType, GenericStatusID, BatchID, FailureCount FROM [Cuttlefish].[dbo].[GenericMessage] WHERE ID = @PrimaryKey";
        public const string genericmessagedatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[GenericMessage] WHERE ID = @PrimaryKey";
        public const string genericmessagedatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[GenericMessage] WHERE";
        public const string genericmessagedatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[GenericMessage] (MessageDate, MessageContent, MessageType, GenericStatusID, BatchID, FailureCount) VALUES(@MessageDate, @MessageContent, @MessageType, @GenericStatusID, @BatchID, @FailureCount); select cast(scope_identity() as int)";
        public const string genericmessagedatamodel_update = "UPDATE [Cuttlefish].[dbo].[GenericMessage] SET MessageDate = @MessageDate, MessageContent = @MessageContent, MessageType = @MessageType, GenericStatusID = @GenericStatusID, BatchID = @BatchID, FailureCount = @FailureCount WHERE ID = @ID";



        public const string personalloanleadmessagedatamodel_selectwhere = "SELECT Id, IdNumber, BatchID, FailureCount, Source, UserName, MessageDate, MachineName, Application FROM [Cuttlefish].[dbo].[PersonalLoanLeadMessage] WHERE";
        public const string personalloanleadmessagedatamodel_selectbykey = "SELECT Id, IdNumber, BatchID, FailureCount, Source, UserName, MessageDate, MachineName, Application FROM [Cuttlefish].[dbo].[PersonalLoanLeadMessage] WHERE Id = @PrimaryKey";
        public const string personalloanleadmessagedatamodel_delete = "DELETE FROM [Cuttlefish].[dbo].[PersonalLoanLeadMessage] WHERE Id = @PrimaryKey";
        public const string personalloanleadmessagedatamodel_deletewhere = "DELETE FROM [Cuttlefish].[dbo].[PersonalLoanLeadMessage] WHERE";
        public const string personalloanleadmessagedatamodel_insert = "INSERT INTO [Cuttlefish].[dbo].[PersonalLoanLeadMessage] (IdNumber, BatchID, FailureCount, Source, UserName, MessageDate, MachineName, Application) VALUES(@IdNumber, @BatchID, @FailureCount, @Source, @UserName, @MessageDate, @MachineName, @Application); select cast(scope_identity() as int)";
        public const string personalloanleadmessagedatamodel_update = "UPDATE [Cuttlefish].[dbo].[PersonalLoanLeadMessage] SET IdNumber = @IdNumber, BatchID = @BatchID, FailureCount = @FailureCount, Source = @Source, UserName = @UserName, MessageDate = @MessageDate, MachineName = @MachineName, Application = @Application WHERE Id = @Id";



    }
}