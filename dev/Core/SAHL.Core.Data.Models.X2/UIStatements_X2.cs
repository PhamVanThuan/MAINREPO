using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.X2
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string stuckcasesdatamodel_selectwhere = "SELECT id, MapName, StateName, InstanceID, Subject, OfferKey, StateID, CreatorADUserName, OSKey, Description, ParentKey, PDescription FROM [x2].[x2].[StuckCases] WHERE";
        public const string stuckcasesdatamodel_selectbykey = "SELECT id, MapName, StateName, InstanceID, Subject, OfferKey, StateID, CreatorADUserName, OSKey, Description, ParentKey, PDescription FROM [x2].[x2].[StuckCases] WHERE id = @PrimaryKey";
        public const string stuckcasesdatamodel_delete = "DELETE FROM [x2].[x2].[StuckCases] WHERE id = @PrimaryKey";
        public const string stuckcasesdatamodel_deletewhere = "DELETE FROM [x2].[x2].[StuckCases] WHERE";
        public const string stuckcasesdatamodel_insert = "INSERT INTO [x2].[x2].[StuckCases] (MapName, StateName, InstanceID, Subject, OfferKey, StateID, CreatorADUserName, OSKey, Description, ParentKey, PDescription) VALUES(@MapName, @StateName, @InstanceID, @Subject, @OfferKey, @StateID, @CreatorADUserName, @OSKey, @Description, @ParentKey, @PDescription); select cast(scope_identity() as int)";
        public const string stuckcasesdatamodel_update = "UPDATE [x2].[x2].[StuckCases] SET MapName = @MapName, StateName = @StateName, InstanceID = @InstanceID, Subject = @Subject, OfferKey = @OfferKey, StateID = @StateID, CreatorADUserName = @CreatorADUserName, OSKey = @OSKey, Description = @Description, ParentKey = @ParentKey, PDescription = @PDescription WHERE id = @id";



        public const string workflowactivitydatamodel_selectwhere = "SELECT ID, WorkFlowID, Name, NextWorkFlowID, NextActivityID, StateID, ReturnActivityID FROM [x2].[x2].[WorkFlowActivity] WHERE";
        public const string workflowactivitydatamodel_selectbykey = "SELECT ID, WorkFlowID, Name, NextWorkFlowID, NextActivityID, StateID, ReturnActivityID FROM [x2].[x2].[WorkFlowActivity] WHERE ID = @PrimaryKey";
        public const string workflowactivitydatamodel_delete = "DELETE FROM [x2].[x2].[WorkFlowActivity] WHERE ID = @PrimaryKey";
        public const string workflowactivitydatamodel_deletewhere = "DELETE FROM [x2].[x2].[WorkFlowActivity] WHERE";
        public const string workflowactivitydatamodel_insert = "INSERT INTO [x2].[x2].[WorkFlowActivity] (WorkFlowID, Name, NextWorkFlowID, NextActivityID, StateID, ReturnActivityID) VALUES(@WorkFlowID, @Name, @NextWorkFlowID, @NextActivityID, @StateID, @ReturnActivityID); select cast(scope_identity() as int)";
        public const string workflowactivitydatamodel_update = "UPDATE [x2].[x2].[WorkFlowActivity] SET WorkFlowID = @WorkFlowID, Name = @Name, NextWorkFlowID = @NextWorkFlowID, NextActivityID = @NextActivityID, StateID = @StateID, ReturnActivityID = @ReturnActivityID WHERE ID = @ID";



        public const string stageactivitydatamodel_selectwhere = "SELECT ID, ActivityID, StageDefinitionKey, StageDefinitionStageDefinitionGroupKey FROM [x2].[x2].[StageActivity] WHERE";
        public const string stageactivitydatamodel_selectbykey = "SELECT ID, ActivityID, StageDefinitionKey, StageDefinitionStageDefinitionGroupKey FROM [x2].[x2].[StageActivity] WHERE ID = @PrimaryKey";
        public const string stageactivitydatamodel_delete = "DELETE FROM [x2].[x2].[StageActivity] WHERE ID = @PrimaryKey";
        public const string stageactivitydatamodel_deletewhere = "DELETE FROM [x2].[x2].[StageActivity] WHERE";
        public const string stageactivitydatamodel_insert = "INSERT INTO [x2].[x2].[StageActivity] (ActivityID, StageDefinitionKey, StageDefinitionStageDefinitionGroupKey) VALUES(@ActivityID, @StageDefinitionKey, @StageDefinitionStageDefinitionGroupKey); select cast(scope_identity() as int)";
        public const string stageactivitydatamodel_update = "UPDATE [x2].[x2].[StageActivity] SET ActivityID = @ActivityID, StageDefinitionKey = @StageDefinitionKey, StageDefinitionStageDefinitionGroupKey = @StageDefinitionStageDefinitionGroupKey WHERE ID = @ID";



        public const string scheduledactivitybakupdatamodel_selectwhere = "SELECT InstanceID, Time, ActivityID, Priority, WorkflowProviderName FROM [x2].[x2].[ScheduledActivityBakup] WHERE";
        public const string scheduledactivitybakupdatamodel_selectbykey = "SELECT InstanceID, Time, ActivityID, Priority, WorkflowProviderName FROM [x2].[x2].[ScheduledActivityBakup] WHERE  = @PrimaryKey";
        public const string scheduledactivitybakupdatamodel_delete = "DELETE FROM [x2].[x2].[ScheduledActivityBakup] WHERE  = @PrimaryKey";
        public const string scheduledactivitybakupdatamodel_deletewhere = "DELETE FROM [x2].[x2].[ScheduledActivityBakup] WHERE";
        public const string scheduledactivitybakupdatamodel_insert = "INSERT INTO [x2].[x2].[ScheduledActivityBakup] (InstanceID, Time, ActivityID, Priority, WorkflowProviderName) VALUES(@InstanceID, @Time, @ActivityID, @Priority, @WorkflowProviderName); ";
        public const string scheduledactivitybakupdatamodel_update = "UPDATE [x2].[x2].[ScheduledActivityBakup] SET InstanceID = @InstanceID, Time = @Time, ActivityID = @ActivityID, Priority = @Priority, WorkflowProviderName = @WorkflowProviderName WHERE  = @";



        public const string scheduledactivitylifedatamodel_selectwhere = "SELECT InstanceID, Time, ActivityID, Priority, WorkflowProviderName FROM [x2].[x2].[scheduledActivityLife] WHERE";
        public const string scheduledactivitylifedatamodel_selectbykey = "SELECT InstanceID, Time, ActivityID, Priority, WorkflowProviderName FROM [x2].[x2].[scheduledActivityLife] WHERE  = @PrimaryKey";
        public const string scheduledactivitylifedatamodel_delete = "DELETE FROM [x2].[x2].[scheduledActivityLife] WHERE  = @PrimaryKey";
        public const string scheduledactivitylifedatamodel_deletewhere = "DELETE FROM [x2].[x2].[scheduledActivityLife] WHERE";
        public const string scheduledactivitylifedatamodel_insert = "INSERT INTO [x2].[x2].[scheduledActivityLife] (InstanceID, Time, ActivityID, Priority, WorkflowProviderName) VALUES(@InstanceID, @Time, @ActivityID, @Priority, @WorkflowProviderName); ";
        public const string scheduledactivitylifedatamodel_update = "UPDATE [x2].[x2].[scheduledActivityLife] SET InstanceID = @InstanceID, Time = @Time, ActivityID = @ActivityID, Priority = @Priority, WorkflowProviderName = @WorkflowProviderName WHERE  = @";



        public const string instancelogdatamodel_selectwhere = "SELECT ID, RequestID, InstanceID, ParentID, SourceID, ActivityName, Time, Message, ErrorLevel, IsUserActivity, ADUser, WFName FROM [x2].[x2].[InstanceLog] WHERE";
        public const string instancelogdatamodel_selectbykey = "SELECT ID, RequestID, InstanceID, ParentID, SourceID, ActivityName, Time, Message, ErrorLevel, IsUserActivity, ADUser, WFName FROM [x2].[x2].[InstanceLog] WHERE ID = @PrimaryKey";
        public const string instancelogdatamodel_delete = "DELETE FROM [x2].[x2].[InstanceLog] WHERE ID = @PrimaryKey";
        public const string instancelogdatamodel_deletewhere = "DELETE FROM [x2].[x2].[InstanceLog] WHERE";
        public const string instancelogdatamodel_insert = "INSERT INTO [x2].[x2].[InstanceLog] (RequestID, InstanceID, ParentID, SourceID, ActivityName, Time, Message, ErrorLevel, IsUserActivity, ADUser, WFName) VALUES(@RequestID, @InstanceID, @ParentID, @SourceID, @ActivityName, @Time, @Message, @ErrorLevel, @IsUserActivity, @ADUser, @WFName); select cast(scope_identity() as int)";
        public const string instancelogdatamodel_update = "UPDATE [x2].[x2].[InstanceLog] SET RequestID = @RequestID, InstanceID = @InstanceID, ParentID = @ParentID, SourceID = @SourceID, ActivityName = @ActivityName, Time = @Time, Message = @Message, ErrorLevel = @ErrorLevel, IsUserActivity = @IsUserActivity, ADUser = @ADUser, WFName = @WFName WHERE ID = @ID";



        public const string processdatamodel_selectwhere = "SELECT ID, ProcessAncestorID, Name, Version, DesignerData, CreateDate, MapVersion, ConfigFile, ViewableOnUserInterfaceVersion, IsLegacy FROM [x2].[x2].[Process] WHERE";
        public const string processdatamodel_selectbykey = "SELECT ID, ProcessAncestorID, Name, Version, DesignerData, CreateDate, MapVersion, ConfigFile, ViewableOnUserInterfaceVersion, IsLegacy FROM [x2].[x2].[Process] WHERE ID = @PrimaryKey";
        public const string processdatamodel_delete = "DELETE FROM [x2].[x2].[Process] WHERE ID = @PrimaryKey";
        public const string processdatamodel_deletewhere = "DELETE FROM [x2].[x2].[Process] WHERE";
        public const string processdatamodel_insert = "INSERT INTO [x2].[x2].[Process] (ProcessAncestorID, Name, Version, DesignerData, CreateDate, MapVersion, ConfigFile, ViewableOnUserInterfaceVersion, IsLegacy) VALUES(@ProcessAncestorID, @Name, @Version, @DesignerData, @CreateDate, @MapVersion, @ConfigFile, @ViewableOnUserInterfaceVersion, @IsLegacy); select cast(scope_identity() as int)";
        public const string processdatamodel_update = "UPDATE [x2].[x2].[Process] SET ProcessAncestorID = @ProcessAncestorID, Name = @Name, Version = @Version, DesignerData = @DesignerData, CreateDate = @CreateDate, MapVersion = @MapVersion, ConfigFile = @ConfigFile, ViewableOnUserInterfaceVersion = @ViewableOnUserInterfaceVersion, IsLegacy = @IsLegacy WHERE ID = @ID";



        public const string processassemblynugetinfodatamodel_selectwhere = "SELECT ID, ProcessID, PackageName, PackageVersion FROM [x2].[x2].[ProcessAssemblyNugetInfo] WHERE";
        public const string processassemblynugetinfodatamodel_selectbykey = "SELECT ID, ProcessID, PackageName, PackageVersion FROM [x2].[x2].[ProcessAssemblyNugetInfo] WHERE ID = @PrimaryKey";
        public const string processassemblynugetinfodatamodel_delete = "DELETE FROM [x2].[x2].[ProcessAssemblyNugetInfo] WHERE ID = @PrimaryKey";
        public const string processassemblynugetinfodatamodel_deletewhere = "DELETE FROM [x2].[x2].[ProcessAssemblyNugetInfo] WHERE";
        public const string processassemblynugetinfodatamodel_insert = "INSERT INTO [x2].[x2].[ProcessAssemblyNugetInfo] (ProcessID, PackageName, PackageVersion) VALUES(@ProcessID, @PackageName, @PackageVersion); select cast(scope_identity() as int)";
        public const string processassemblynugetinfodatamodel_update = "UPDATE [x2].[x2].[ProcessAssemblyNugetInfo] SET ProcessID = @ProcessID, PackageName = @PackageName, PackageVersion = @PackageVersion WHERE ID = @ID";



        public const string workflowicondatamodel_selectwhere = "SELECT ID, Name, Icon FROM [x2].[x2].[WorkFlowIcon] WHERE";
        public const string workflowicondatamodel_selectbykey = "SELECT ID, Name, Icon FROM [x2].[x2].[WorkFlowIcon] WHERE ID = @PrimaryKey";
        public const string workflowicondatamodel_delete = "DELETE FROM [x2].[x2].[WorkFlowIcon] WHERE ID = @PrimaryKey";
        public const string workflowicondatamodel_deletewhere = "DELETE FROM [x2].[x2].[WorkFlowIcon] WHERE";
        public const string workflowicondatamodel_insert = "INSERT INTO [x2].[x2].[WorkFlowIcon] (Name, Icon) VALUES(@Name, @Icon); select cast(scope_identity() as int)";
        public const string workflowicondatamodel_update = "UPDATE [x2].[x2].[WorkFlowIcon] SET Name = @Name, Icon = @Icon WHERE ID = @ID";



        public const string requestdatamodel_selectwhere = "SELECT RequestID, Contents, RequestStatusID, RequestDate, RequestUpdatedDate, RequestTimeoutRetries FROM [x2].[x2].[Request] WHERE";
        public const string requestdatamodel_selectbykey = "SELECT RequestID, Contents, RequestStatusID, RequestDate, RequestUpdatedDate, RequestTimeoutRetries FROM [x2].[x2].[Request] WHERE RequestID = @PrimaryKey";
        public const string requestdatamodel_delete = "DELETE FROM [x2].[x2].[Request] WHERE RequestID = @PrimaryKey";
        public const string requestdatamodel_deletewhere = "DELETE FROM [x2].[x2].[Request] WHERE";
        public const string requestdatamodel_insert = "INSERT INTO [x2].[x2].[Request] (RequestID, Contents, RequestStatusID, RequestDate, RequestUpdatedDate, RequestTimeoutRetries) VALUES(@RequestID, @Contents, @RequestStatusID, @RequestDate, @RequestUpdatedDate, @RequestTimeoutRetries); ";
        public const string requestdatamodel_update = "UPDATE [x2].[x2].[Request] SET RequestID = @RequestID, Contents = @Contents, RequestStatusID = @RequestStatusID, RequestDate = @RequestDate, RequestUpdatedDate = @RequestUpdatedDate, RequestTimeoutRetries = @RequestTimeoutRetries WHERE RequestID = @RequestID";



        public const string scheduledactivitydatamodel_selectwhere = "SELECT InstanceID, Time, ActivityID, Priority, WorkFlowProviderName, ID FROM [x2].[x2].[ScheduledActivity] WHERE";
        public const string scheduledactivitydatamodel_selectbykey = "SELECT InstanceID, Time, ActivityID, Priority, WorkFlowProviderName, ID FROM [x2].[x2].[ScheduledActivity] WHERE ID = @PrimaryKey";
        public const string scheduledactivitydatamodel_delete = "DELETE FROM [x2].[x2].[ScheduledActivity] WHERE ID = @PrimaryKey";
        public const string scheduledactivitydatamodel_deletewhere = "DELETE FROM [x2].[x2].[ScheduledActivity] WHERE";
        public const string scheduledactivitydatamodel_insert = "INSERT INTO [x2].[x2].[ScheduledActivity] (InstanceID, Time, ActivityID, Priority, WorkFlowProviderName) VALUES(@InstanceID, @Time, @ActivityID, @Priority, @WorkFlowProviderName); select cast(scope_identity() as int)";
        public const string scheduledactivitydatamodel_update = "UPDATE [x2].[x2].[ScheduledActivity] SET InstanceID = @InstanceID, Time = @Time, ActivityID = @ActivityID, Priority = @Priority, WorkFlowProviderName = @WorkFlowProviderName WHERE ID = @ID";



        public const string workflowroleassignmentdatamodel_selectwhere = "SELECT ID, InstanceID, WorkflowRoleTypeOrganisationStructureMappingKey, ADUserKey, GeneralStatusKey, InsertDate, Message FROM [x2].[x2].[WorkflowRoleAssignment] WHERE";
        public const string workflowroleassignmentdatamodel_selectbykey = "SELECT ID, InstanceID, WorkflowRoleTypeOrganisationStructureMappingKey, ADUserKey, GeneralStatusKey, InsertDate, Message FROM [x2].[x2].[WorkflowRoleAssignment] WHERE ID = @PrimaryKey";
        public const string workflowroleassignmentdatamodel_delete = "DELETE FROM [x2].[x2].[WorkflowRoleAssignment] WHERE ID = @PrimaryKey";
        public const string workflowroleassignmentdatamodel_deletewhere = "DELETE FROM [x2].[x2].[WorkflowRoleAssignment] WHERE";
        public const string workflowroleassignmentdatamodel_insert = "INSERT INTO [x2].[x2].[WorkflowRoleAssignment] (InstanceID, WorkflowRoleTypeOrganisationStructureMappingKey, ADUserKey, GeneralStatusKey, InsertDate, Message) VALUES(@InstanceID, @WorkflowRoleTypeOrganisationStructureMappingKey, @ADUserKey, @GeneralStatusKey, @InsertDate, @Message); select cast(scope_identity() as int)";
        public const string workflowroleassignmentdatamodel_update = "UPDATE [x2].[x2].[WorkflowRoleAssignment] SET InstanceID = @InstanceID, WorkflowRoleTypeOrganisationStructureMappingKey = @WorkflowRoleTypeOrganisationStructureMappingKey, ADUserKey = @ADUserKey, GeneralStatusKey = @GeneralStatusKey, InsertDate = @InsertDate, Message = @Message WHERE ID = @ID";



        public const string activitytypedatamodel_selectwhere = "SELECT ID, Name FROM [x2].[x2].[ActivityType] WHERE";
        public const string activitytypedatamodel_selectbykey = "SELECT ID, Name FROM [x2].[x2].[ActivityType] WHERE ID = @PrimaryKey";
        public const string activitytypedatamodel_delete = "DELETE FROM [x2].[x2].[ActivityType] WHERE ID = @PrimaryKey";
        public const string activitytypedatamodel_deletewhere = "DELETE FROM [x2].[x2].[ActivityType] WHERE";
        public const string activitytypedatamodel_insert = "INSERT INTO [x2].[x2].[ActivityType] (Name) VALUES(@Name); select cast(scope_identity() as int)";
        public const string activitytypedatamodel_update = "UPDATE [x2].[x2].[ActivityType] SET Name = @Name WHERE ID = @ID";



        public const string statetypedatamodel_selectwhere = "SELECT ID, Name FROM [x2].[x2].[StateType] WHERE";
        public const string statetypedatamodel_selectbykey = "SELECT ID, Name FROM [x2].[x2].[StateType] WHERE ID = @PrimaryKey";
        public const string statetypedatamodel_delete = "DELETE FROM [x2].[x2].[StateType] WHERE ID = @PrimaryKey";
        public const string statetypedatamodel_deletewhere = "DELETE FROM [x2].[x2].[StateType] WHERE";
        public const string statetypedatamodel_insert = "INSERT INTO [x2].[x2].[StateType] (Name) VALUES(@Name); select cast(scope_identity() as int)";
        public const string statetypedatamodel_update = "UPDATE [x2].[x2].[StateType] SET Name = @Name WHERE ID = @ID";



        public const string externalactivitydatamodel_selectwhere = "SELECT ID, WorkFlowID, Name, Description FROM [x2].[x2].[ExternalActivity] WHERE";
        public const string externalactivitydatamodel_selectbykey = "SELECT ID, WorkFlowID, Name, Description FROM [x2].[x2].[ExternalActivity] WHERE ID = @PrimaryKey";
        public const string externalactivitydatamodel_delete = "DELETE FROM [x2].[x2].[ExternalActivity] WHERE ID = @PrimaryKey";
        public const string externalactivitydatamodel_deletewhere = "DELETE FROM [x2].[x2].[ExternalActivity] WHERE";
        public const string externalactivitydatamodel_insert = "INSERT INTO [x2].[x2].[ExternalActivity] (WorkFlowID, Name, Description) VALUES(@WorkFlowID, @Name, @Description); select cast(scope_identity() as int)";
        public const string externalactivitydatamodel_update = "UPDATE [x2].[x2].[ExternalActivity] SET WorkFlowID = @WorkFlowID, Name = @Name, Description = @Description WHERE ID = @ID";



        public const string sessiondatamodel_selectwhere = "SELECT SessionID, ADUserName, SessionStartTime, LastActivityTime FROM [x2].[x2].[Session] WHERE";
        public const string sessiondatamodel_selectbykey = "SELECT SessionID, ADUserName, SessionStartTime, LastActivityTime FROM [x2].[x2].[Session] WHERE SessionID = @PrimaryKey";
        public const string sessiondatamodel_delete = "DELETE FROM [x2].[x2].[Session] WHERE SessionID = @PrimaryKey";
        public const string sessiondatamodel_deletewhere = "DELETE FROM [x2].[x2].[Session] WHERE";
        public const string sessiondatamodel_insert = "INSERT INTO [x2].[x2].[Session] (SessionID, ADUserName, SessionStartTime, LastActivityTime) VALUES(@SessionID, @ADUserName, @SessionStartTime, @LastActivityTime); ";
        public const string sessiondatamodel_update = "UPDATE [x2].[x2].[Session] SET SessionID = @SessionID, ADUserName = @ADUserName, SessionStartTime = @SessionStartTime, LastActivityTime = @LastActivityTime WHERE SessionID = @SessionID";



        public const string externalactivitytargetdatamodel_selectwhere = "SELECT ID, Name FROM [x2].[x2].[ExternalActivityTarget] WHERE";
        public const string externalactivitytargetdatamodel_selectbykey = "SELECT ID, Name FROM [x2].[x2].[ExternalActivityTarget] WHERE ID = @PrimaryKey";
        public const string externalactivitytargetdatamodel_delete = "DELETE FROM [x2].[x2].[ExternalActivityTarget] WHERE ID = @PrimaryKey";
        public const string externalactivitytargetdatamodel_deletewhere = "DELETE FROM [x2].[x2].[ExternalActivityTarget] WHERE";
        public const string externalactivitytargetdatamodel_insert = "INSERT INTO [x2].[x2].[ExternalActivityTarget] (Name) VALUES(@Name); select cast(scope_identity() as int)";
        public const string externalactivitytargetdatamodel_update = "UPDATE [x2].[x2].[ExternalActivityTarget] SET Name = @Name WHERE ID = @ID";



        public const string workflowproviderinstancesdatamodel_selectwhere = "SELECT ID, WorkFlowProviderName, ActiveDate FROM [x2].[x2].[WorkFlowProviderInstances] WHERE";
        public const string workflowproviderinstancesdatamodel_selectbykey = "SELECT ID, WorkFlowProviderName, ActiveDate FROM [x2].[x2].[WorkFlowProviderInstances] WHERE ID = @PrimaryKey";
        public const string workflowproviderinstancesdatamodel_delete = "DELETE FROM [x2].[x2].[WorkFlowProviderInstances] WHERE ID = @PrimaryKey";
        public const string workflowproviderinstancesdatamodel_deletewhere = "DELETE FROM [x2].[x2].[WorkFlowProviderInstances] WHERE";
        public const string workflowproviderinstancesdatamodel_insert = "INSERT INTO [x2].[x2].[WorkFlowProviderInstances] (WorkFlowProviderName, ActiveDate) VALUES(@WorkFlowProviderName, @ActiveDate); select cast(scope_identity() as int)";
        public const string workflowproviderinstancesdatamodel_update = "UPDATE [x2].[x2].[WorkFlowProviderInstances] SET WorkFlowProviderName = @WorkFlowProviderName, ActiveDate = @ActiveDate WHERE ID = @ID";



        public const string securitygroupdatamodel_selectwhere = "SELECT ID, IsDynamic, Name, Description, ProcessID, WorkFlowID FROM [x2].[x2].[SecurityGroup] WHERE";
        public const string securitygroupdatamodel_selectbykey = "SELECT ID, IsDynamic, Name, Description, ProcessID, WorkFlowID FROM [x2].[x2].[SecurityGroup] WHERE ID = @PrimaryKey";
        public const string securitygroupdatamodel_delete = "DELETE FROM [x2].[x2].[SecurityGroup] WHERE ID = @PrimaryKey";
        public const string securitygroupdatamodel_deletewhere = "DELETE FROM [x2].[x2].[SecurityGroup] WHERE";
        public const string securitygroupdatamodel_insert = "INSERT INTO [x2].[x2].[SecurityGroup] (IsDynamic, Name, Description, ProcessID, WorkFlowID) VALUES(@IsDynamic, @Name, @Description, @ProcessID, @WorkFlowID); select cast(scope_identity() as int)";
        public const string securitygroupdatamodel_update = "UPDATE [x2].[x2].[SecurityGroup] SET IsDynamic = @IsDynamic, Name = @Name, Description = @Description, ProcessID = @ProcessID, WorkFlowID = @WorkFlowID WHERE ID = @ID";



        public const string workflowdatamodel_selectwhere = "SELECT ID, ProcessID, WorkFlowAncestorID, Name, CreateDate, StorageTable, StorageKey, IconID, DefaultSubject, GenericKeyTypeKey FROM [x2].[x2].[WorkFlow] WHERE";
        public const string workflowdatamodel_selectbykey = "SELECT ID, ProcessID, WorkFlowAncestorID, Name, CreateDate, StorageTable, StorageKey, IconID, DefaultSubject, GenericKeyTypeKey FROM [x2].[x2].[WorkFlow] WHERE ID = @PrimaryKey";
        public const string workflowdatamodel_delete = "DELETE FROM [x2].[x2].[WorkFlow] WHERE ID = @PrimaryKey";
        public const string workflowdatamodel_deletewhere = "DELETE FROM [x2].[x2].[WorkFlow] WHERE";
        public const string workflowdatamodel_insert = "INSERT INTO [x2].[x2].[WorkFlow] (ProcessID, WorkFlowAncestorID, Name, CreateDate, StorageTable, StorageKey, IconID, DefaultSubject, GenericKeyTypeKey) VALUES(@ProcessID, @WorkFlowAncestorID, @Name, @CreateDate, @StorageTable, @StorageKey, @IconID, @DefaultSubject, @GenericKeyTypeKey); select cast(scope_identity() as int)";
        public const string workflowdatamodel_update = "UPDATE [x2].[x2].[WorkFlow] SET ProcessID = @ProcessID, WorkFlowAncestorID = @WorkFlowAncestorID, Name = @Name, CreateDate = @CreateDate, StorageTable = @StorageTable, StorageKey = @StorageKey, IconID = @IconID, DefaultSubject = @DefaultSubject, GenericKeyTypeKey = @GenericKeyTypeKey WHERE ID = @ID";



        public const string publishedstatemappingdatamodel_selectwhere = "SELECT OldWorkFlowID, OldStateID, NewWorkFlowID, NewStateID FROM [x2].[x2].[PublishedStateMapping] WHERE";
        public const string publishedstatemappingdatamodel_selectbykey = "SELECT OldWorkFlowID, OldStateID, NewWorkFlowID, NewStateID FROM [x2].[x2].[PublishedStateMapping] WHERE  = @PrimaryKey";
        public const string publishedstatemappingdatamodel_delete = "DELETE FROM [x2].[x2].[PublishedStateMapping] WHERE  = @PrimaryKey";
        public const string publishedstatemappingdatamodel_deletewhere = "DELETE FROM [x2].[x2].[PublishedStateMapping] WHERE";
        public const string publishedstatemappingdatamodel_insert = "INSERT INTO [x2].[x2].[PublishedStateMapping] (OldWorkFlowID, OldStateID, NewWorkFlowID, NewStateID) VALUES(@OldWorkFlowID, @OldStateID, @NewWorkFlowID, @NewStateID); ";
        public const string publishedstatemappingdatamodel_update = "UPDATE [x2].[x2].[PublishedStateMapping] SET OldWorkFlowID = @OldWorkFlowID, OldStateID = @OldStateID, NewWorkFlowID = @NewWorkFlowID, NewStateID = @NewStateID WHERE  = @";



        public const string processassemblydatamodel_selectwhere = "SELECT ID, ProcessID, ParentID, DLLName, DLLData FROM [x2].[x2].[ProcessAssembly] WHERE";
        public const string processassemblydatamodel_selectbykey = "SELECT ID, ProcessID, ParentID, DLLName, DLLData FROM [x2].[x2].[ProcessAssembly] WHERE ID = @PrimaryKey";
        public const string processassemblydatamodel_delete = "DELETE FROM [x2].[x2].[ProcessAssembly] WHERE ID = @PrimaryKey";
        public const string processassemblydatamodel_deletewhere = "DELETE FROM [x2].[x2].[ProcessAssembly] WHERE";
        public const string processassemblydatamodel_insert = "INSERT INTO [x2].[x2].[ProcessAssembly] (ProcessID, ParentID, DLLName, DLLData) VALUES(@ProcessID, @ParentID, @DLLName, @DLLData); select cast(scope_identity() as int)";
        public const string processassemblydatamodel_update = "UPDATE [x2].[x2].[ProcessAssembly] SET ProcessID = @ProcessID, ParentID = @ParentID, DLLName = @DLLName, DLLData = @DLLData WHERE ID = @ID";



        public const string activitydatamodel_selectwhere = "SELECT ID, WorkFlowID, Name, Type, StateID, NextStateID, SplitWorkFlow, Priority, FormID, ActivityMessage, RaiseExternalActivity, ExternalActivityTarget, ActivatedByExternalActivity, ChainedActivityName, Sequence, X2ID FROM [x2].[x2].[Activity] WHERE";
        public const string activitydatamodel_selectbykey = "SELECT ID, WorkFlowID, Name, Type, StateID, NextStateID, SplitWorkFlow, Priority, FormID, ActivityMessage, RaiseExternalActivity, ExternalActivityTarget, ActivatedByExternalActivity, ChainedActivityName, Sequence, X2ID FROM [x2].[x2].[Activity] WHERE ID = @PrimaryKey";
        public const string activitydatamodel_delete = "DELETE FROM [x2].[x2].[Activity] WHERE ID = @PrimaryKey";
        public const string activitydatamodel_deletewhere = "DELETE FROM [x2].[x2].[Activity] WHERE";
        public const string activitydatamodel_insert = "INSERT INTO [x2].[x2].[Activity] (WorkFlowID, Name, Type, StateID, NextStateID, SplitWorkFlow, Priority, FormID, ActivityMessage, RaiseExternalActivity, ExternalActivityTarget, ActivatedByExternalActivity, ChainedActivityName, Sequence, X2ID) VALUES(@WorkFlowID, @Name, @Type, @StateID, @NextStateID, @SplitWorkFlow, @Priority, @FormID, @ActivityMessage, @RaiseExternalActivity, @ExternalActivityTarget, @ActivatedByExternalActivity, @ChainedActivityName, @Sequence, @X2ID); select cast(scope_identity() as int)";
        public const string activitydatamodel_update = "UPDATE [x2].[x2].[Activity] SET WorkFlowID = @WorkFlowID, Name = @Name, Type = @Type, StateID = @StateID, NextStateID = @NextStateID, SplitWorkFlow = @SplitWorkFlow, Priority = @Priority, FormID = @FormID, ActivityMessage = @ActivityMessage, RaiseExternalActivity = @RaiseExternalActivity, ExternalActivityTarget = @ExternalActivityTarget, ActivatedByExternalActivity = @ActivatedByExternalActivity, ChainedActivityName = @ChainedActivityName, Sequence = @Sequence, X2ID = @X2ID WHERE ID = @ID";



        public const string statedatamodel_selectwhere = "SELECT ID, WorkFlowID, Name, Type, ForwardState, Sequence, ReturnWorkflowID, ReturnActivityID, X2ID FROM [x2].[x2].[State] WHERE";
        public const string statedatamodel_selectbykey = "SELECT ID, WorkFlowID, Name, Type, ForwardState, Sequence, ReturnWorkflowID, ReturnActivityID, X2ID FROM [x2].[x2].[State] WHERE ID = @PrimaryKey";
        public const string statedatamodel_delete = "DELETE FROM [x2].[x2].[State] WHERE ID = @PrimaryKey";
        public const string statedatamodel_deletewhere = "DELETE FROM [x2].[x2].[State] WHERE";
        public const string statedatamodel_insert = "INSERT INTO [x2].[x2].[State] (WorkFlowID, Name, Type, ForwardState, Sequence, ReturnWorkflowID, ReturnActivityID, X2ID) VALUES(@WorkFlowID, @Name, @Type, @ForwardState, @Sequence, @ReturnWorkflowID, @ReturnActivityID, @X2ID); select cast(scope_identity() as int)";
        public const string statedatamodel_update = "UPDATE [x2].[x2].[State] SET WorkFlowID = @WorkFlowID, Name = @Name, Type = @Type, ForwardState = @ForwardState, Sequence = @Sequence, ReturnWorkflowID = @ReturnWorkflowID, ReturnActivityID = @ReturnActivityID, X2ID = @X2ID WHERE ID = @ID";



        public const string activeexternalactivitydatamodel_selectwhere = "SELECT ID, ExternalActivityID, WorkFlowID, ActivatingInstanceID, ActivationTime, ActivityXMLData, WorkFlowProviderName FROM [x2].[x2].[ActiveExternalActivity] WHERE";
        public const string activeexternalactivitydatamodel_selectbykey = "SELECT ID, ExternalActivityID, WorkFlowID, ActivatingInstanceID, ActivationTime, ActivityXMLData, WorkFlowProviderName FROM [x2].[x2].[ActiveExternalActivity] WHERE ID = @PrimaryKey";
        public const string activeexternalactivitydatamodel_delete = "DELETE FROM [x2].[x2].[ActiveExternalActivity] WHERE ID = @PrimaryKey";
        public const string activeexternalactivitydatamodel_deletewhere = "DELETE FROM [x2].[x2].[ActiveExternalActivity] WHERE";
        public const string activeexternalactivitydatamodel_insert = "INSERT INTO [x2].[x2].[ActiveExternalActivity] (ExternalActivityID, WorkFlowID, ActivatingInstanceID, ActivationTime, ActivityXMLData, WorkFlowProviderName) VALUES(@ExternalActivityID, @WorkFlowID, @ActivatingInstanceID, @ActivationTime, @ActivityXMLData, @WorkFlowProviderName); select cast(scope_identity() as int)";
        public const string activeexternalactivitydatamodel_update = "UPDATE [x2].[x2].[ActiveExternalActivity] SET ExternalActivityID = @ExternalActivityID, WorkFlowID = @WorkFlowID, ActivatingInstanceID = @ActivatingInstanceID, ActivationTime = @ActivationTime, ActivityXMLData = @ActivityXMLData, WorkFlowProviderName = @WorkFlowProviderName WHERE ID = @ID";



        public const string instanceactivitysecuritydatamodel_selectwhere = "SELECT ID, InstanceID, ActivityID, ADUserName FROM [x2].[x2].[InstanceActivitySecurity] WHERE";
        public const string instanceactivitysecuritydatamodel_selectbykey = "SELECT ID, InstanceID, ActivityID, ADUserName FROM [x2].[x2].[InstanceActivitySecurity] WHERE ID = @PrimaryKey";
        public const string instanceactivitysecuritydatamodel_delete = "DELETE FROM [x2].[x2].[InstanceActivitySecurity] WHERE ID = @PrimaryKey";
        public const string instanceactivitysecuritydatamodel_deletewhere = "DELETE FROM [x2].[x2].[InstanceActivitySecurity] WHERE";
        public const string instanceactivitysecuritydatamodel_insert = "INSERT INTO [x2].[x2].[InstanceActivitySecurity] (InstanceID, ActivityID, ADUserName) VALUES(@InstanceID, @ActivityID, @ADUserName); select cast(scope_identity() as int)";
        public const string instanceactivitysecuritydatamodel_update = "UPDATE [x2].[x2].[InstanceActivitySecurity] SET InstanceID = @InstanceID, ActivityID = @ActivityID, ADUserName = @ADUserName WHERE ID = @ID";



        public const string workflowhistorydatamodel_selectwhere = "SELECT ID, InstanceID, StateID, ActivityID, CreatorADUserName, CreationDate, StateChangeDate, DeadlineDate, ActivityDate, ADUserName, Priority FROM [x2].[x2].[WorkFlowHistory] WHERE";
        public const string workflowhistorydatamodel_selectbykey = "SELECT ID, InstanceID, StateID, ActivityID, CreatorADUserName, CreationDate, StateChangeDate, DeadlineDate, ActivityDate, ADUserName, Priority FROM [x2].[x2].[WorkFlowHistory] WHERE ID = @PrimaryKey";
        public const string workflowhistorydatamodel_delete = "DELETE FROM [x2].[x2].[WorkFlowHistory] WHERE ID = @PrimaryKey";
        public const string workflowhistorydatamodel_deletewhere = "DELETE FROM [x2].[x2].[WorkFlowHistory] WHERE";
        public const string workflowhistorydatamodel_insert = "INSERT INTO [x2].[x2].[WorkFlowHistory] (InstanceID, StateID, ActivityID, CreatorADUserName, CreationDate, StateChangeDate, DeadlineDate, ActivityDate, ADUserName, Priority) VALUES(@InstanceID, @StateID, @ActivityID, @CreatorADUserName, @CreationDate, @StateChangeDate, @DeadlineDate, @ActivityDate, @ADUserName, @Priority); select cast(scope_identity() as int)";
        public const string workflowhistorydatamodel_update = "UPDATE [x2].[x2].[WorkFlowHistory] SET InstanceID = @InstanceID, StateID = @StateID, ActivityID = @ActivityID, CreatorADUserName = @CreatorADUserName, CreationDate = @CreationDate, StateChangeDate = @StateChangeDate, DeadlineDate = @DeadlineDate, ActivityDate = @ActivityDate, ADUserName = @ADUserName, Priority = @Priority WHERE ID = @ID";



        public const string logdatamodel_selectwhere = "SELECT ID, Date, ProcessID, WorkFlowID, InstanceID, StateID, ActivityID, ADUserName, Message, StackTrace FROM [x2].[x2].[Log] WHERE";
        public const string logdatamodel_selectbykey = "SELECT ID, Date, ProcessID, WorkFlowID, InstanceID, StateID, ActivityID, ADUserName, Message, StackTrace FROM [x2].[x2].[Log] WHERE ID = @PrimaryKey";
        public const string logdatamodel_delete = "DELETE FROM [x2].[x2].[Log] WHERE ID = @PrimaryKey";
        public const string logdatamodel_deletewhere = "DELETE FROM [x2].[x2].[Log] WHERE";
        public const string logdatamodel_insert = "INSERT INTO [x2].[x2].[Log] (Date, ProcessID, WorkFlowID, InstanceID, StateID, ActivityID, ADUserName, Message, StackTrace) VALUES(@Date, @ProcessID, @WorkFlowID, @InstanceID, @StateID, @ActivityID, @ADUserName, @Message, @StackTrace); select cast(scope_identity() as int)";
        public const string logdatamodel_update = "UPDATE [x2].[x2].[Log] SET Date = @Date, ProcessID = @ProcessID, WorkFlowID = @WorkFlowID, InstanceID = @InstanceID, StateID = @StateID, ActivityID = @ActivityID, ADUserName = @ADUserName, Message = @Message, StackTrace = @StackTrace WHERE ID = @ID";



        public const string activitysecuritydatamodel_selectwhere = "SELECT ID, ActivityID, SecurityGroupID FROM [x2].[x2].[ActivitySecurity] WHERE";
        public const string activitysecuritydatamodel_selectbykey = "SELECT ID, ActivityID, SecurityGroupID FROM [x2].[x2].[ActivitySecurity] WHERE ID = @PrimaryKey";
        public const string activitysecuritydatamodel_delete = "DELETE FROM [x2].[x2].[ActivitySecurity] WHERE ID = @PrimaryKey";
        public const string activitysecuritydatamodel_deletewhere = "DELETE FROM [x2].[x2].[ActivitySecurity] WHERE";
        public const string activitysecuritydatamodel_insert = "INSERT INTO [x2].[x2].[ActivitySecurity] (ActivityID, SecurityGroupID) VALUES(@ActivityID, @SecurityGroupID); select cast(scope_identity() as int)";
        public const string activitysecuritydatamodel_update = "UPDATE [x2].[x2].[ActivitySecurity] SET ActivityID = @ActivityID, SecurityGroupID = @SecurityGroupID WHERE ID = @ID";



        public const string stateformdatamodel_selectwhere = "SELECT ID, StateID, FormID, FormOrder FROM [x2].[x2].[StateForm] WHERE";
        public const string stateformdatamodel_selectbykey = "SELECT ID, StateID, FormID, FormOrder FROM [x2].[x2].[StateForm] WHERE ID = @PrimaryKey";
        public const string stateformdatamodel_delete = "DELETE FROM [x2].[x2].[StateForm] WHERE ID = @PrimaryKey";
        public const string stateformdatamodel_deletewhere = "DELETE FROM [x2].[x2].[StateForm] WHERE";
        public const string stateformdatamodel_insert = "INSERT INTO [x2].[x2].[StateForm] (StateID, FormID, FormOrder) VALUES(@StateID, @FormID, @FormOrder); select cast(scope_identity() as int)";
        public const string stateformdatamodel_update = "UPDATE [x2].[x2].[StateForm] SET StateID = @StateID, FormID = @FormID, FormOrder = @FormOrder WHERE ID = @ID";



        public const string tracklistdatamodel_selectwhere = "SELECT ID, InstanceID, ADUserName, ListDate FROM [x2].[x2].[TrackList] WHERE";
        public const string tracklistdatamodel_selectbykey = "SELECT ID, InstanceID, ADUserName, ListDate FROM [x2].[x2].[TrackList] WHERE ID = @PrimaryKey";
        public const string tracklistdatamodel_delete = "DELETE FROM [x2].[x2].[TrackList] WHERE ID = @PrimaryKey";
        public const string tracklistdatamodel_deletewhere = "DELETE FROM [x2].[x2].[TrackList] WHERE";
        public const string tracklistdatamodel_insert = "INSERT INTO [x2].[x2].[TrackList] (InstanceID, ADUserName, ListDate) VALUES(@InstanceID, @ADUserName, @ListDate); select cast(scope_identity() as int)";
        public const string tracklistdatamodel_update = "UPDATE [x2].[x2].[TrackList] SET InstanceID = @InstanceID, ADUserName = @ADUserName, ListDate = @ListDate WHERE ID = @ID";



        public const string worklistdatamodel_selectwhere = "SELECT ID, InstanceID, ADUserName, ListDate, Message FROM [x2].[x2].[WorkList] WHERE";
        public const string worklistdatamodel_selectbykey = "SELECT ID, InstanceID, ADUserName, ListDate, Message FROM [x2].[x2].[WorkList] WHERE ID = @PrimaryKey";
        public const string worklistdatamodel_delete = "DELETE FROM [x2].[x2].[WorkList] WHERE ID = @PrimaryKey";
        public const string worklistdatamodel_deletewhere = "DELETE FROM [x2].[x2].[WorkList] WHERE";
        public const string worklistdatamodel_insert = "INSERT INTO [x2].[x2].[WorkList] (InstanceID, ADUserName, ListDate, Message) VALUES(@InstanceID, @ADUserName, @ListDate, @Message); select cast(scope_identity() as int)";
        public const string worklistdatamodel_update = "UPDATE [x2].[x2].[WorkList] SET InstanceID = @InstanceID, ADUserName = @ADUserName, ListDate = @ListDate, Message = @Message WHERE ID = @ID";



        public const string stateworklistdatamodel_selectwhere = "SELECT ID, StateID, SecurityGroupID FROM [x2].[x2].[StateWorkList] WHERE";
        public const string stateworklistdatamodel_selectbykey = "SELECT ID, StateID, SecurityGroupID FROM [x2].[x2].[StateWorkList] WHERE ID = @PrimaryKey";
        public const string stateworklistdatamodel_delete = "DELETE FROM [x2].[x2].[StateWorkList] WHERE ID = @PrimaryKey";
        public const string stateworklistdatamodel_deletewhere = "DELETE FROM [x2].[x2].[StateWorkList] WHERE";
        public const string stateworklistdatamodel_insert = "INSERT INTO [x2].[x2].[StateWorkList] (StateID, SecurityGroupID) VALUES(@StateID, @SecurityGroupID); select cast(scope_identity() as int)";
        public const string stateworklistdatamodel_update = "UPDATE [x2].[x2].[StateWorkList] SET StateID = @StateID, SecurityGroupID = @SecurityGroupID WHERE ID = @ID";



        public const string statetracklistdatamodel_selectwhere = "SELECT ID, StateID, SecurityGroupID FROM [x2].[x2].[StateTrackList] WHERE";
        public const string statetracklistdatamodel_selectbykey = "SELECT ID, StateID, SecurityGroupID FROM [x2].[x2].[StateTrackList] WHERE ID = @PrimaryKey";
        public const string statetracklistdatamodel_delete = "DELETE FROM [x2].[x2].[StateTrackList] WHERE ID = @PrimaryKey";
        public const string statetracklistdatamodel_deletewhere = "DELETE FROM [x2].[x2].[StateTrackList] WHERE";
        public const string statetracklistdatamodel_insert = "INSERT INTO [x2].[x2].[StateTrackList] (StateID, SecurityGroupID) VALUES(@StateID, @SecurityGroupID); select cast(scope_identity() as int)";
        public const string statetracklistdatamodel_update = "UPDATE [x2].[x2].[StateTrackList] SET StateID = @StateID, SecurityGroupID = @SecurityGroupID WHERE ID = @ID";



        public const string instancedatamodel_selectwhere = "SELECT ID, WorkFlowID, ParentInstanceID, Name, Subject, WorkFlowProvider, StateID, CreatorADUserName, CreationDate, StateChangeDate, DeadlineDate, ActivityDate, ActivityADUserName, ActivityID, Priority, SourceInstanceID, ReturnActivityID FROM [x2].[x2].[Instance] WHERE";
        public const string instancedatamodel_selectbykey = "SELECT ID, WorkFlowID, ParentInstanceID, Name, Subject, WorkFlowProvider, StateID, CreatorADUserName, CreationDate, StateChangeDate, DeadlineDate, ActivityDate, ActivityADUserName, ActivityID, Priority, SourceInstanceID, ReturnActivityID FROM [x2].[x2].[Instance] WHERE ID = @PrimaryKey";
        public const string instancedatamodel_delete = "DELETE FROM [x2].[x2].[Instance] WHERE ID = @PrimaryKey";
        public const string instancedatamodel_deletewhere = "DELETE FROM [x2].[x2].[Instance] WHERE";
        public const string instancedatamodel_insert = "INSERT INTO [x2].[x2].[Instance] (WorkFlowID, ParentInstanceID, Name, Subject, WorkFlowProvider, StateID, CreatorADUserName, CreationDate, StateChangeDate, DeadlineDate, ActivityDate, ActivityADUserName, ActivityID, Priority, SourceInstanceID, ReturnActivityID) VALUES(@WorkFlowID, @ParentInstanceID, @Name, @Subject, @WorkFlowProvider, @StateID, @CreatorADUserName, @CreationDate, @StateChangeDate, @DeadlineDate, @ActivityDate, @ActivityADUserName, @ActivityID, @Priority, @SourceInstanceID, @ReturnActivityID); select cast(scope_identity() as int)";
        public const string instancedatamodel_update = "UPDATE [x2].[x2].[Instance] SET WorkFlowID = @WorkFlowID, ParentInstanceID = @ParentInstanceID, Name = @Name, Subject = @Subject, WorkFlowProvider = @WorkFlowProvider, StateID = @StateID, CreatorADUserName = @CreatorADUserName, CreationDate = @CreationDate, StateChangeDate = @StateChangeDate, DeadlineDate = @DeadlineDate, ActivityDate = @ActivityDate, ActivityADUserName = @ActivityADUserName, ActivityID = @ActivityID, Priority = @Priority, SourceInstanceID = @SourceInstanceID, ReturnActivityID = @ReturnActivityID WHERE ID = @ID";



        public const string workflowsecuritydatamodel_selectwhere = "SELECT ID, WorkFlowID, SecurityGroupID FROM [x2].[x2].[WorkFlowSecurity] WHERE";
        public const string workflowsecuritydatamodel_selectbykey = "SELECT ID, WorkFlowID, SecurityGroupID FROM [x2].[x2].[WorkFlowSecurity] WHERE ID = @PrimaryKey";
        public const string workflowsecuritydatamodel_delete = "DELETE FROM [x2].[x2].[WorkFlowSecurity] WHERE ID = @PrimaryKey";
        public const string workflowsecuritydatamodel_deletewhere = "DELETE FROM [x2].[x2].[WorkFlowSecurity] WHERE";
        public const string workflowsecuritydatamodel_insert = "INSERT INTO [x2].[x2].[WorkFlowSecurity] (WorkFlowID, SecurityGroupID) VALUES(@WorkFlowID, @SecurityGroupID); select cast(scope_identity() as int)";
        public const string workflowsecuritydatamodel_update = "UPDATE [x2].[x2].[WorkFlowSecurity] SET WorkFlowID = @WorkFlowID, SecurityGroupID = @SecurityGroupID WHERE ID = @ID";



        public const string formdatamodel_selectwhere = "SELECT ID, Name, Description, WorkFlowID FROM [x2].[x2].[Form] WHERE";
        public const string formdatamodel_selectbykey = "SELECT ID, Name, Description, WorkFlowID FROM [x2].[x2].[Form] WHERE ID = @PrimaryKey";
        public const string formdatamodel_delete = "DELETE FROM [x2].[x2].[Form] WHERE ID = @PrimaryKey";
        public const string formdatamodel_deletewhere = "DELETE FROM [x2].[x2].[Form] WHERE";
        public const string formdatamodel_insert = "INSERT INTO [x2].[x2].[Form] (Name, Description, WorkFlowID) VALUES(@Name, @Description, @WorkFlowID); select cast(scope_identity() as int)";
        public const string formdatamodel_update = "UPDATE [x2].[x2].[Form] SET Name = @Name, Description = @Description, WorkFlowID = @WorkFlowID WHERE ID = @ID";



        public const string unassignedcasesdatamodel_selectwhere = "SELECT InstanceID, StateName, Subject, OfferKey, CreatorADUserName, Migrated, IsFL, OSKey, Description, ParentKey, ParentDescription FROM [x2].[x2].[UnassignedCases] WHERE";
        public const string unassignedcasesdatamodel_selectbykey = "SELECT InstanceID, StateName, Subject, OfferKey, CreatorADUserName, Migrated, IsFL, OSKey, Description, ParentKey, ParentDescription FROM [x2].[x2].[UnassignedCases] WHERE  = @PrimaryKey";
        public const string unassignedcasesdatamodel_delete = "DELETE FROM [x2].[x2].[UnassignedCases] WHERE  = @PrimaryKey";
        public const string unassignedcasesdatamodel_deletewhere = "DELETE FROM [x2].[x2].[UnassignedCases] WHERE";
        public const string unassignedcasesdatamodel_insert = "INSERT INTO [x2].[x2].[UnassignedCases] (InstanceID, StateName, Subject, OfferKey, CreatorADUserName, Migrated, IsFL, OSKey, Description, ParentKey, ParentDescription) VALUES(@InstanceID, @StateName, @Subject, @OfferKey, @CreatorADUserName, @Migrated, @IsFL, @OSKey, @Description, @ParentKey, @ParentDescription); ";
        public const string unassignedcasesdatamodel_update = "UPDATE [x2].[x2].[UnassignedCases] SET InstanceID = @InstanceID, StateName = @StateName, Subject = @Subject, OfferKey = @OfferKey, CreatorADUserName = @CreatorADUserName, Migrated = @Migrated, IsFL = @IsFL, OSKey = @OSKey, Description = @Description, ParentKey = @ParentKey, ParentDescription = @ParentDescription WHERE  = @";



        public const string assignmentdatamodel_selectwhere = "SELECT ID, InstanceId, AssignmentDate, UserOrganisationStructureKey, CapabilityKey FROM [x2].[x2].[Assignment] WHERE";
        public const string assignmentdatamodel_selectbykey = "SELECT ID, InstanceId, AssignmentDate, UserOrganisationStructureKey, CapabilityKey FROM [x2].[x2].[Assignment] WHERE ID = @PrimaryKey";
        public const string assignmentdatamodel_delete = "DELETE FROM [x2].[x2].[Assignment] WHERE ID = @PrimaryKey";
        public const string assignmentdatamodel_deletewhere = "DELETE FROM [x2].[x2].[Assignment] WHERE";
        public const string assignmentdatamodel_insert = "INSERT INTO [x2].[x2].[Assignment] (InstanceId, AssignmentDate, UserOrganisationStructureKey, CapabilityKey) VALUES(@InstanceId, @AssignmentDate, @UserOrganisationStructureKey, @CapabilityKey); select cast(scope_identity() as int)";
        public const string assignmentdatamodel_update = "UPDATE [x2].[x2].[Assignment] SET InstanceId = @InstanceId, AssignmentDate = @AssignmentDate, UserOrganisationStructureKey = @UserOrganisationStructureKey, CapabilityKey = @CapabilityKey WHERE ID = @ID";



        public const string instancearchivedatamodel_selectwhere = "SELECT ID, WorkFlowID, ParentInstanceID, Name, Subject, WorkFlowProvider, StateID, CreatorADUserName, CreationDate, StateChangeDate, DeadlineDate, Priority, SourceInstanceID, ReturnActivityID, ArchiveDate FROM [x2].[x2].[InstanceArchive] WHERE";
        public const string instancearchivedatamodel_selectbykey = "SELECT ID, WorkFlowID, ParentInstanceID, Name, Subject, WorkFlowProvider, StateID, CreatorADUserName, CreationDate, StateChangeDate, DeadlineDate, Priority, SourceInstanceID, ReturnActivityID, ArchiveDate FROM [x2].[x2].[InstanceArchive] WHERE ID = @PrimaryKey";
        public const string instancearchivedatamodel_delete = "DELETE FROM [x2].[x2].[InstanceArchive] WHERE ID = @PrimaryKey";
        public const string instancearchivedatamodel_deletewhere = "DELETE FROM [x2].[x2].[InstanceArchive] WHERE";
        public const string instancearchivedatamodel_insert = "INSERT INTO [x2].[x2].[InstanceArchive] (ID, WorkFlowID, ParentInstanceID, Name, Subject, WorkFlowProvider, StateID, CreatorADUserName, CreationDate, StateChangeDate, DeadlineDate, Priority, SourceInstanceID, ReturnActivityID, ArchiveDate) VALUES(@ID, @WorkFlowID, @ParentInstanceID, @Name, @Subject, @WorkFlowProvider, @StateID, @CreatorADUserName, @CreationDate, @StateChangeDate, @DeadlineDate, @Priority, @SourceInstanceID, @ReturnActivityID, @ArchiveDate); ";
        public const string instancearchivedatamodel_update = "UPDATE [x2].[x2].[InstanceArchive] SET ID = @ID, WorkFlowID = @WorkFlowID, ParentInstanceID = @ParentInstanceID, Name = @Name, Subject = @Subject, WorkFlowProvider = @WorkFlowProvider, StateID = @StateID, CreatorADUserName = @CreatorADUserName, CreationDate = @CreationDate, StateChangeDate = @StateChangeDate, DeadlineDate = @DeadlineDate, Priority = @Priority, SourceInstanceID = @SourceInstanceID, ReturnActivityID = @ReturnActivityID, ArchiveDate = @ArchiveDate WHERE ID = @ID";



        public const string workflowassignmentdatamodel_selectwhere = "SELECT ID, InstanceID, OfferRoleTypeOrganisationStructureMappingKey, ADUserKey, GeneralStatusKey, InsertDate, StateName FROM [x2].[x2].[WorkflowAssignment] WHERE";
        public const string workflowassignmentdatamodel_selectbykey = "SELECT ID, InstanceID, OfferRoleTypeOrganisationStructureMappingKey, ADUserKey, GeneralStatusKey, InsertDate, StateName FROM [x2].[x2].[WorkflowAssignment] WHERE ID = @PrimaryKey";
        public const string workflowassignmentdatamodel_delete = "DELETE FROM [x2].[x2].[WorkflowAssignment] WHERE ID = @PrimaryKey";
        public const string workflowassignmentdatamodel_deletewhere = "DELETE FROM [x2].[x2].[WorkflowAssignment] WHERE";
        public const string workflowassignmentdatamodel_insert = "INSERT INTO [x2].[x2].[WorkflowAssignment] (InstanceID, OfferRoleTypeOrganisationStructureMappingKey, ADUserKey, GeneralStatusKey, InsertDate, StateName) VALUES(@InstanceID, @OfferRoleTypeOrganisationStructureMappingKey, @ADUserKey, @GeneralStatusKey, @InsertDate, @StateName); select cast(scope_identity() as int)";
        public const string workflowassignmentdatamodel_update = "UPDATE [x2].[x2].[WorkflowAssignment] SET InstanceID = @InstanceID, OfferRoleTypeOrganisationStructureMappingKey = @OfferRoleTypeOrganisationStructureMappingKey, ADUserKey = @ADUserKey, GeneralStatusKey = @GeneralStatusKey, InsertDate = @InsertDate, StateName = @StateName WHERE ID = @ID";



    }
}