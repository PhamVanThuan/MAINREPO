using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class Workflow : AbstractNamedPositionableElement
    {
        private List<AbstractCustomVariable> customVariables;
        private List<ExternalActivityDefinition> externalActivityDefinitions;
        private List<CustomForm> customForms;
        private List<AbstractActivity> activities;
        private List<AbstractNamedState> states;
        private List<Comment> comments;
        private List<WorkflowRole> roles;

        public Workflow(string name, Single locationX, Single locationY, int genericKeyType)
            : base(name, locationX, locationY)
        {
            this.GenericKeyType = genericKeyType;
            this.customVariables = new List<AbstractCustomVariable>();
            this.externalActivityDefinitions = new List<ExternalActivityDefinition>();
            this.customForms = new List<CustomForm>();
            this.activities = new List<AbstractActivity>();
            this.states = new List<AbstractNamedState>();
            this.comments = new List<Comment>();
            this.roles = new List<WorkflowRole>();
        }

        public int GenericKeyType { get; protected set; }

        public void UpdateGenericKey(int genericKeyType)
        {
            this.GenericKeyType = genericKeyType;
        }

        public ClapperBoard ClapperBoard
        {
            get;
            set;
        }

        public ReadOnlyCollection<AbstractCustomVariable> CustomVariables
        {
            get
            {
                return new ReadOnlyCollection<AbstractCustomVariable>(this.customVariables);
            }
        }

        public void AddCustomVariable(AbstractCustomVariable customVariable)
        {
            this.customVariables.Add(customVariable);
        }

        public void RemoveWorkflow(AbstractCustomVariable customVariable)
        {
            this.customVariables.Remove(customVariable);
        }

        public ReadOnlyCollection<ExternalActivityDefinition> ExternalActivityDefinitions
        {
            get
            {
                return new ReadOnlyCollection<ExternalActivityDefinition>(this.externalActivityDefinitions);
            }
        }

        public void AddExternalActivityDefinition(ExternalActivityDefinition externalActivityDefinition)
        {
            this.externalActivityDefinitions.Add(externalActivityDefinition);
        }

        public void RemoveExternalActivityDefinition(ExternalActivityDefinition externalActivityDefinition)
        {
            this.externalActivityDefinitions.Remove(externalActivityDefinition);
        }

        public ReadOnlyCollection<CustomForm> CustomForms
        {
            get
            {
                return new ReadOnlyCollection<CustomForm>(this.customForms);
            }
        }

        public void AddCustomForm(CustomForm customForm)
        {
            this.customForms.Add(customForm);
        }

        public void RemoveCustomForm(CustomForm customForm)
        {
            this.customForms.Remove(customForm);
        }

        public ReadOnlyCollection<AbstractActivity> Activities
        {
            get
            {
                return new ReadOnlyCollection<AbstractActivity>(this.activities);
            }
        }

        public void AddActivity(AbstractActivity activity)
        {
            this.activities.Add(activity);
        }

        public void RemoveActivity(AbstractActivity activity)
        {
            this.activities.Remove(activity);
        }

        public ReadOnlyCollection<AbstractNamedState> States
        {
            get
            {
                return new ReadOnlyCollection<AbstractNamedState>(this.states);
            }
        }

        public ReadOnlyCollection<TimedActivity> TimedActivities
        {
            get
            {
                return new ReadOnlyCollection<TimedActivity>(this.Activities.OfType<TimedActivity>().ToList());
            }
        }

        public ReadOnlyCollection<UserActivity> UserActivities
        {
            get
            {
                List<UserActivity> userActivities = new List<UserActivity>();
                foreach (AbstractActivity activity in this.Activities)
                {
                    if (activity is UserActivity)
                        userActivities.Add(activity as UserActivity);
                }

                return new ReadOnlyCollection<UserActivity>(userActivities);
            }
        }

        public ReadOnlyCollection<AbstractActivity> ActivitiesWithStageTransitions
        {
            get
            {
                List<AbstractActivity> stageTransitionActivities = new List<AbstractActivity>();
                foreach (AbstractActivity activity in this.Activities)
                {
                    if (activity.BusinessStageTransitions.Count > 0)
                        stageTransitionActivities.Add(activity);
                }

                return new ReadOnlyCollection<AbstractActivity>(stageTransitionActivities);
            }
        }

        public ReadOnlyCollection<ArchiveState> ArchiveStates
        {
            get
            {
                List<ArchiveState> archiveStates = new List<ArchiveState>();
                foreach (AbstractState state in this.States)
                {
                    if (state is ArchiveState)
                        archiveStates.Add(state as ArchiveState);
                }

                return new ReadOnlyCollection<ArchiveState>(archiveStates);
            }
        }

        public ReadOnlyCollection<SystemState> AutoForwardStates
        {
            get
            {
                List<SystemState> autoForwardStates = new List<SystemState>();
                foreach (AbstractState state in this.States)
                {
                    SystemState sysState = state as SystemState;
                    if (sysState != null && sysState.UseAutoForward == true)
                        autoForwardStates.Add(sysState);
                }

                return new ReadOnlyCollection<SystemState>(autoForwardStates);
            }
        }

        public void AddState(AbstractNamedState state)
        {
            this.states.Add(state);
        }

        public void RemoveState(AbstractNamedState state)
        {
            this.states.Remove(state);
        }

        public ReadOnlyCollection<Comment> Comments
        {
            get
            {
                return new ReadOnlyCollection<Comment>(this.comments);
            }
        }

        public void AddComment(Comment comment)
        {
            this.comments.Add(comment);
        }

        public void RemoveComment(Comment comment)
        {
            this.comments.Remove(comment);
        }

        public ReadOnlyCollection<WorkflowRole> Roles
        {
            get
            {
                return new ReadOnlyCollection<WorkflowRole>(this.roles);
            }
        }

        public ReadOnlyCollection<WorkflowRole> DynamicRoles
        {
            get
            {
                List<WorkflowRole> dynamicRoles = new List<WorkflowRole>();
                foreach (WorkflowRole role in this.Roles)
                {
                    if (role.IsDynamic)
                        dynamicRoles.Add(role);
                }

                return new ReadOnlyCollection<WorkflowRole>(dynamicRoles);
            }
        }

        public void AddRole(WorkflowRole role)
        {
            this.roles.Add(role);
        }

        public void RemoveRole(WorkflowRole role)
        {
            this.roles.Remove(role);
        }
    }
}