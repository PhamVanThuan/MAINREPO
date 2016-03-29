using System;
using System.Collections.Generic;
using SAHL.X2Designer.Documents;

namespace SAHL.X2Designer.Items
{
    public enum WorkflowItemBaseType
    {
        Activity,
        State,
        Role,
        CustomVariable,
        CustomForm,
        Comment
    }

    public enum GeneralCustomFormType
    {
        ExternalActivity,
        CustomVariable,
        CustomForm,
        Role,
        GenericKeyType
    }

    public enum PropertyType
    {
        Role,
        CustomVariable,
        AutoForward,
        IsDynamic,
        Name,
        CustomForm,
        Access,
        WorkList,
        TrackList,
        BusinessStageTransition
    }

    public enum WorkflowItemType
    {
        None,
        ArchiveState,
        UserActivity,
        ConditionalActivity,
        ExternalActivity,
        TimedActivity,
        CallWorkFlowActivity,
        UserState,
        SystemState,
        SystemDecisionState,
        CommonState,
        Comment,
        RoleStatic,
        RoleDynamic,
        CustomVariable,
        CustomForm,
        ReturnWorkFlowActivity,
        HoldState
    }

    public enum ObjectChangeType
    {
        Inserted,
        Deleted,
        Renamed,
        WorkFlowInserted,
        WorkFlowDeleted,
        StageTransitionMessage
    }

    public enum RoleType
    {
        Global,
        WorkFlow
    }

    public enum CutCopyOperationType
    {
        Cut,
        Copy
    }

    public delegate void OnWorkFlowItemNameChangedHandler(BaseItem ChangedItem);

    public interface IWorkflowItem : IComparable
    {
        string Name { get; set; }

        WorkflowItemBaseType WorkflowItemBaseType { get; }

        WorkflowItemType WorkflowItemType { get; }

        List<string> ValidationErrors { get; }

        void Validate();

        // properties attributes
        object Properties { get; }

        // code section attributes
        string[] AvailableCodeSections { get; }

        string GetCodeSectionData(string CodeSectionName);

        void SetCodeSectionData(string CodeSectionName, string SectionData);

        void UpdateCodeSectionData(string OldValue, string NewValue);

        string RefreshCodeSectionData(string CodeSectionName);

        void Copy(BaseItem newItem);
    }

    public interface ISecurity_HasAccessList
    {
        RoleInstanceCollection Access { get; }
    }

    public interface IBusinessStageTransitions
    {
        List<BusinessStageItem> BusinessStageTransitions { get; set; }
    }

    public interface ISecurity_HasWorkTrackLists
    {
        RoleInstanceCollection WorkList { get; }

        RoleInstanceCollection TrackList { get; }
    }

    public interface ISecurity_HasLimitAccessTo
    {
        RolesCollectionItem LimitAccessTo { get; }
    }
}