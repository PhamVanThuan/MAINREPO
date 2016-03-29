using System.Collections.Generic;

namespace Automation.DataModels
{
    public class X2State : IDataModel
    {
        public X2State()
        {
        }

        public X2State(X2State state, X2Workflow workflow)
        {
            var instance = this;
            Helpers.SetProperties<X2State, X2State>(ref instance, state);
            this.Workflow = workflow;
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public bool ForwardState { get; set; }

        public int Sequence { get; set; }

        public int ReturnWorkflowID { get; set; }

        public int ReturnActivityID { get; set; }

        public int WorkFlowID { get; set; }

        public IEnumerable<X2Activity> Activities { get; set; }

        public X2Workflow Workflow { get; set; }
    }
}