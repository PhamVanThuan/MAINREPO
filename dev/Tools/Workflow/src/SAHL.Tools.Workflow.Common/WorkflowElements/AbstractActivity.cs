using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class AbstractActivity : AbstractNamedPositionableElement
    {
        private List<BusinessStageTransition> businessStageTransitions;

        public AbstractActivity(string name, Single locationX, Single locationY, CodeSection onStartActivity, CodeSection onCompleteActivity, CodeSection onGetStageTransition, Guid x2ID)
            : base(name, locationX, locationY)
        {
            this.OnStartActivityCode = onStartActivity;
            this.OnCompleteActivityCode = onCompleteActivity;
            this.GetStageTransitionCode = onGetStageTransition;
            this.X2ID = x2ID;
            base.AddCodeSection(this.OnStartActivityCode);
            base.AddCodeSection(this.OnCompleteActivityCode);
            base.AddCodeSection(this.GetStageTransitionCode);

            this.businessStageTransitions = new List<BusinessStageTransition>();
        }

        public virtual int Id { get; set; }

        public virtual Workflow Workflow { get; set; }

        public virtual string Message { get; set; }

        public virtual string StageTransitionMessage { get; set; }

        public virtual string Description { get; set; }

        public virtual int Priority { get; set; }

        public virtual ExternalActivityDefinition RaiseExternalActivity { get; set; }

        public virtual bool SplitWorkflow { get; set; }

        public virtual Guid X2ID { get; protected set; }

        public AbstractState FromStateNode { get; set; }

        public AbstractNamedState ToStateNode { get; set; }

        public CodeSection OnStartActivityCode { get; protected set; }

        public CodeSection OnCompleteActivityCode { get; protected set; }

        public CodeSection GetStageTransitionCode { get; protected set; }
        
        public ReadOnlyCollection<BusinessStageTransition> BusinessStageTransitions
        {
            get
            {
                return new ReadOnlyCollection<BusinessStageTransition>(this.businessStageTransitions);
            }
        }

        public void AddBusinessStageTransition(BusinessStageTransition transition)
        {
            this.businessStageTransitions.Add(transition);
        }

        public void RemoveBusinessStageTransition(BusinessStageTransition transition)
        {
            this.businessStageTransitions.Remove(transition);
        }
    }
}