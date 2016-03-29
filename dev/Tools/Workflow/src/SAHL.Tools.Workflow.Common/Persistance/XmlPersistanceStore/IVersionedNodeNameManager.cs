namespace SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore
{
    public interface IVersionedNodeNameManager
    {
        string ActivitiesElementName { get; }

        string ActivityElementName { get; }

        string ArchiveStateType { get; }

        string AutoForwardToCodeSection { get; }

        string BigInteger { get; }

        string Bool { get; }

        string ClapperBoardElementName { get; }

        string CodeAttribute { get; }

        string CodeSectionElementName { get; }

        string CodeSectionsElementName { get; }

        string CommonStateType { get; }

        string AppliesToElementName { get; }

        string AppliesToAttributeName { get; }

        string ConditionalActivityType { get; }

        string CustomFormAttribute { get; }

        string CustomFormElementName { get; }

        string CustomFormsElementName { get; }

        string CustomFormCollectionElementName { get; }

        string FormNameAttribute { get; }

        string CustomVariableElementName { get; }

        string CustomVariablesElementName { get; }

        string DateTime { get; }

        string Decimal { get; }

        string DescriptionAttribute { get; }

        string Double { get; }

        string ExternalActivitiesElementName { get; }

        string ExternalActivityElementName { get; }

        string ExternalActivityType { get; }

        string FromNodeAttribute { get; }

        string GenericKeyTypeKey { get; }

        string GetActivityMessageCodeSection { get; }

        string GlobalRoleType { get; }

        string HoldStateType { get; }

        string IdAttribute { get; }

        string Integer { get; }

        string IsDynamicAttribute { get; }

        string KeyVariableAttribute { get; }

        string LengthAttributeName { get; }

        string LimitAccessToAttribute { get; }

        string LinkedActivityAttribute { get; }

        string LocationXAttribute { get; }

        string LocationYAttribute { get; }

        string MapVersionAttributeName { get; }

        string MessageAttribute { get; }

        string NameAttribute { get; }

        string OnCompleteCodeSectionName { get; }

        string OnEnterCodeSectionName { get; }

        string OnExitCodeSectionName { get; }

        string OnReturnCodeSectionName { get; }

        string OnStageActivityCodeSectionName { get; }

        string OnStartCodeSectionName { get; }

        string OnTimedActivityCodeSection { get; }

        string OnGetRoleCodeSectionName { get; }

        string PriorityAttribute { get; }

        string ProcessElementName { get; }

        string ProductVersionAttributeName { get; }

        string RaiseExternalActivityAttribute { get; }

        string RetrievedAttributeName { get; }

        string ReturnActivityAttribute { get; }

        string ReturnWorkflowAttribute { get; }

        string RoleAttribute { get; }

        string RoleElement { get; }

        string RolesElement { get; }

        string RoleTypeAttribute { get; }

        string Single { get; }

        string SplitWorkFlowAttribute { get; }

        string StageTransitionMessageAttribute { get; }

        string StateElementName { get; }

        string StatementAttribute { get; }

        string StatesElementName { get; }

        string String { get; }

        string SubjectAttribute { get; }

        string SystemStateType { get; }

        string Text { get; }

        string TimedActivityType { get; }

        string CallWorkflowActivityType { get; }

        string ReturnWorkflowActivityType { get; }

        string ToNodeAttribute { get; }

        string TypeAttributeName { get; }

        string UseAutoForwardAttribute { get; }

        string UseLinkedAttribute { get; }

        string UserActivityType { get; }

        string UserStateType { get; }

        string UsingStatementElement { get; }

        string UsingStatementsElement { get; }

        string WorkFlowAttribute { get; }

        string WorkflowElementName { get; }

        string WorkflowRoleType { get; }

        string WorkflowsElementName { get; }

        string AssemblyReferencesElement { get; }

        string AssemblyReferenceElement { get; }

        string FullNameAttribute { get; }

        string PathAttribute { get; }

        string VersionAttribute { get; }

        string WorkflowNameAttribute { get; }

        string StateNameAttribute { get; }

        string StageDefinitionKeyAttribute { get; }

        string DefinitionGroupDescriptionAttribute { get; }

        string DefinitionDescriptionAttribute { get; }

        string BusinessStageTransitionsElement { get; }

        string BusinessStageTransitionElement { get; }

        string WorkListElement { get; }

        string WorklistItemAttribute { get; }

        string X2ID { get; }

        string ActivityAccessElement { get; }

        string ActivityAccessAttribute { get; }

        string WorkFlowToCallAttribute { get; }

        string ActivityToCallAttribute { get; }

        string InvokedByAttribute { get; }

        string InvokeTargetAttribute { get; }

        string Legacy { get; }

        string ViewableOnUserInterfaceVersion { get; }

    }
}