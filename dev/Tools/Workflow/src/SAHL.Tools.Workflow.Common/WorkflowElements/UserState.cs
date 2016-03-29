using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class UserState : AbstractNamedState
    {
        private List<AbstractRole> workList;
        private List<CustomForm> forms;

        public UserState(string name, Single locationX, Single locationY, CodeSection onEnterState, CodeSection onExitState, Guid X2ID)
            : base(name, locationX, locationY, onEnterState, onExitState, X2ID)
        {
            this.workList = new List<AbstractRole>();
            this.forms = new List<CustomForm>();
        }

        public ReadOnlyCollection<AbstractRole> WorkList
        {
            get
            {
                return new ReadOnlyCollection<AbstractRole>(this.workList);
            }
        }

        public ReadOnlyCollection<CustomForm> Forms
        {
            get
            {
                return new ReadOnlyCollection<CustomForm>(this.forms);
            }
        }

        public void AddRoleToWorkList(AbstractRole roleToAdd)
        {
            if (!this.WorkList.Contains(roleToAdd))
            {
                this.workList.Add(roleToAdd);
            }
        }

        public void AddCustomForm(CustomForm formToAdd)
        {
            if (!this.forms.Contains(formToAdd))
            {
                this.forms.Add(formToAdd);
            }
        }
    }
}