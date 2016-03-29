namespace SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore.ver0_3
{
    public class ver0_3_XmlNodeNameManager : IVersionedNodeNameManager
    {
        // Common
        public virtual string NameAttribute { get { return "Name"; } }

        public virtual string LocationXAttribute { get { return "LocationX"; } }

        public virtual string LocationYAttribute { get { return "LocationY"; } }

        public virtual string DescriptionAttribute { get { return "Description"; } }

        // End Common

        // Process
        public virtual string ProcessElementName { get { return "ProcessName"; } }

        public virtual string ProductVersionAttributeName { get { return "ProductVersion"; } }

        public virtual string MapVersionAttributeName { get { return "MapVersion"; } }

        public virtual string RetrievedAttributeName { get { return "Retrieved"; } }

        public virtual string WorkflowsElementName { get { return "WorkFlows"; } }

        public virtual string TypeAttributeName { get { return "Type"; } }

        // End Process

        // Workflow

        public virtual string WorkflowElementName { get { return "WorkFlow"; } }

        public virtual string GenericKeyTypeKey { get { return "GenericKeyTypeKey"; } }

        public virtual string WorkflowNameAttribute { get { return "WorkFlowName"; } }

        // End Workflow

        // Custom Variable

        public virtual string CustomVariablesElementName { get { return "CustomVariables"; } }

        public virtual string CustomVariableElementName { get { return "CustomVariable"; } }

        public virtual string LengthAttributeName { get { return "Length"; } }

        public virtual string Bool { get { return "bool"; } }

        public virtual string BigInteger { get { return "biginteger"; } }

        public virtual string DateTime { get { return "date"; } }

        public virtual string Decimal { get { return "decimal"; } }

        public virtual string Double { get { return "double"; } }

        public virtual string Integer { get { return "integer"; } }

        public virtual string Single { get { return "single"; } }

        public virtual string String { get { return "string"; } }

        public virtual string Text { get { return "text"; } }

        // End Custom Variable

        // External Activity

        public virtual string ExternalActivitiesElementName { get { return "ExternalActivities"; } }

        public virtual string ExternalActivityElementName { get { return "ExternalActivity"; } }

        // End External Activity

        // Custom Form

        public virtual string CustomFormsElementName { get { return "CustomForms"; } }

        public virtual string CustomFormElementName { get { return "CustomForm"; } }

        public virtual string CustomFormCollectionElementName { get { return "CustomFormCollection"; } }

        public virtual string FormNameAttribute { get { return "FormName"; } }

        // End Custom Form

        // ClapperBoard

        public virtual string ClapperBoardElementName { get { return "Clapperboard"; } }

        public virtual string SubjectAttribute { get { return "Subject"; } }

        public virtual string LimitAccessToAttribute { get { return "LimitAccessTo"; } }

        public virtual string KeyVariableAttribute { get { return "KeyVariable"; } }

        // End ClapperBoard

        // States

        public virtual string StateNameAttribute { get { return "StateName"; } }

        public virtual string StatesElementName { get { return "States"; } }

        public virtual string StateElementName { get { return "State"; } }

        public virtual string UserStateType { get { return "SAHL.X2Designer.Items.UserState"; } }

        public virtual string SystemStateType { get { return "SAHL.X2Designer.Items.SystemState"; } }

        public virtual string CommonStateType { get { return "SAHL.X2Designer.Items.CommonState"; } }

        public virtual string AppliesToElementName { get { return "AppliesTo"; } }

        public virtual string AppliesToAttributeName { get { return "AppliesToCollectionItem"; } }

        public virtual string HoldStateType { get { return "SAHL.X2Designer.Items.HoldState"; } }

        public virtual string ArchiveStateType { get { return "SAHL.X2Designer.Items.ArchiveState"; } }

        public virtual string UseAutoForwardAttribute { get { return "UseAutoForward"; } }

        public virtual string ReturnActivityAttribute { get { return "ReturnActivity"; } }

        public virtual string ReturnWorkflowAttribute { get { return "ReturnWorkflow"; } }

        public virtual string WorkListElement { get { return "WorkList"; } }

        public virtual string WorklistItemAttribute { get { return "WorkListCollectionItem"; } }

        public virtual string X2ID { get { return "X2ID"; } }

        // End States

        // CodeSection

        public virtual string CodeSectionsElementName { get { return "CodeSections"; } }

        public virtual string CodeSectionElementName { get { return "CodeSection"; } }

        public virtual string CodeAttribute { get { return "Code"; } }

        public virtual string OnEnterCodeSectionName { get { return "OnEnter"; } }

        public virtual string OnExitCodeSectionName { get { return "OnExit"; } }

        public virtual string OnStartCodeSectionName { get { return "OnStart"; } }

        public virtual string OnCompleteCodeSectionName { get { return "OnComplete"; } }

        public virtual string OnReturnCodeSectionName { get { return "OnReturn"; } }

        public virtual string OnStageActivityCodeSectionName { get { return "OnStageActivity"; } }

        public virtual string GetActivityMessageCodeSection { get { return "OnActivityMessage"; } }

        public virtual string AutoForwardToCodeSection { get { return "AutoForwardTo"; } }

        public virtual string OnTimedActivityCodeSection { get { return "OnTimedActivity"; } }

        public virtual string OnGetRoleCodeSectionName { get { return "OnGetRole"; } }

        // End CodeSection

        // Activity

        public virtual string ActivitiesElementName { get { return "Activities"; } }

        public virtual string ActivityElementName { get { return "Activity"; } }

        public virtual string UserActivityType { get { return "SAHL.X2Designer.Items.UserActivity"; } }

        public virtual string TimedActivityType { get { return "SAHL.X2Designer.Items.TimedActivity"; } }

        public virtual string ExternalActivityType { get { return "SAHL.X2Designer.Items.ExternalActivity"; } }

        public virtual string ConditionalActivityType { get { return "SAHL.X2Designer.Items.ConditionalActivity"; } }

        public virtual string CallWorkflowActivityType { get { return "SAHL.X2Designer.Items.CallWorkFlowActivity"; } }

        public virtual string ReturnWorkflowActivityType { get { return "SAHL.X2Designer.Items.ReturnWorkflowActivity"; } }

        public virtual string FromNodeAttribute { get { return "FromNode"; } }

        public virtual string ToNodeAttribute { get { return "ToNode"; } }

        public virtual string IdAttribute { get { return "Id"; } }

        public virtual string MessageAttribute { get { return "Message"; } }

        public virtual string StageTransitionMessageAttribute { get { return "StageTransitionMessage"; } }

        public virtual string PriorityAttribute { get { return "Priority"; } }

        public virtual string RaiseExternalActivityAttribute { get { return "RaiseExternalActivity"; } }

        public virtual string SplitWorkFlowAttribute { get { return "SplitWorkFlow"; } }

        public virtual string CustomFormAttribute { get { return "CustomForm"; } }

        public virtual string LinkedActivityAttribute { get { return "LinkedActivity"; } }

        public virtual string UseLinkedAttribute { get { return "UseLinkedActivity"; } }

        public virtual string ActivityAccessElement { get { return "Access"; } }

        public virtual string ActivityAccessAttribute { get { return "AccessCollectionItem"; } }

        public virtual string InvokedByAttribute { get { return "InvokedBy"; } }

        public virtual string InvokeTargetAttribute { get { return "ExternalActivityRaiseFolder"; } }

        // End Activity

        // BusinessStageTransitions

        public virtual string StageDefinitionKeyAttribute { get { return "StageDefinitionKey"; } }

        public virtual string DefinitionGroupDescriptionAttribute { get { return "DefinitionGroupDescription"; } }

        public virtual string DefinitionDescriptionAttribute { get { return "DefinitionDescription"; } }

        public virtual string BusinessStageTransitionsElement { get { return "BusinessStageTransitions"; } }

        public virtual string BusinessStageTransitionElement { get { return "BusinessStageTransition"; } }

        // End

        // Role

        public virtual string RolesElement { get { return "Roles"; } }

        public virtual string RoleElement { get { return "Role"; } }

        public virtual string RoleAttribute { get { return "Role"; } }

        public virtual string IsDynamicAttribute { get { return "IsDynamic"; } }

        public virtual string RoleTypeAttribute { get { return "RoleType"; } }

        public virtual string WorkFlowAttribute { get { return "WorkFlow"; } }

        public virtual string GlobalRoleType { get { return "Global"; } }

        public virtual string WorkflowRoleType { get { return "WorkFlow"; } }

        // End Role

        // Using Statements

        public virtual string UsingStatementsElement { get { return "UsingStatements"; } }

        public virtual string UsingStatementElement { get { return "UsingStatement"; } }

        public virtual string StatementAttribute { get { return "Statement"; } }

        // End Using Statements

        // Assembly Reference

        public virtual string AssemblyReferencesElement
        {
            get { return "References"; }
        }

        public virtual string AssemblyReferenceElement
        {
            get { return "Reference"; }
        }

        public virtual string AssemblyChildReferencesElement
        {
            get { return "ChildReferences"; }
        }

        public virtual string AssemblyChildReferenceElement
        {
            get { return "ChildReference"; }
        }

        public virtual string FullNameAttribute
        {
            get { return "FullName"; }
        }

        public virtual string PathAttribute
        {
            get { return "Path"; }
        }

        public virtual string IsAddedExplicitlyAttribute
        {
            get { return "IsAddedExplicitly"; }
        }

        public virtual string VersionAttribute
        {
            get { return "Version"; }
        }

        public virtual string BinaryTypeAttribute
        {
            get { return "BinaryType"; }
        }

        public virtual string BinaryTypeDomainClient { get { return "DomainClient"; } }

        public virtual string BinaryTypeExternal { get { return "External"; } }

        public virtual string BinaryTypeInternal { get { return "Internal"; } }

        public virtual string SourceLocationAttribute
        {
            get { return "SourceLocation"; }
        }

        public virtual string IsGlobalAttribute
        {
            get { return "IsGlobal"; }
        }

        // End Assembly Reference

        public virtual string WorkFlowToCallAttribute { get { return "WorkFlowToCall"; } }

        public virtual string ActivityToCallAttribute { get { return "ActivityToCall"; } }

        public virtual string Legacy { get { return "Legacy"; } }

        public virtual string ViewableOnUserInterfaceVersion { get { return "ViewableOnUserInterfaceVersion"; } }
    }
}