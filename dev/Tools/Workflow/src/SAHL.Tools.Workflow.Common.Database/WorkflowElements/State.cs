using System.Collections.Generic;
using System;

namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class State
    {
        public State()
        {
            this.WorkList = new List<StateWorkList>();
            this.Forms = new List<StateForm>();
        }

        public virtual Guid X2ID { get; set; }

        public virtual int Id { get; set; }

        public virtual WorkFlow WorkFlow { get; set; }

        public virtual StateType StateType { get; set; }

        public virtual string Name { get; set; }

        public virtual bool ForwardState { get; set; }

        public virtual WorkFlow ReturnWorkflow { get; set; }

        public virtual Activity ReturnActivity { get; set; }

        public virtual IList<StateWorkList> WorkList { get; protected set; }

        public virtual IList<StateForm> Forms { get; protected set; }
    }
}