using System;

namespace Automation.DataModels
{
    public class X2Activity : IComparable<X2Activity>, IDataModel
    {
        public X2Activity()
        {
        }

        public X2Activity(X2Activity activity, X2Workflow workflow, X2State state)
        {
            var instance = this;
            Helpers.SetProperties<X2Activity, X2Activity>(ref instance, activity);
            this.Workflow = workflow;
            this.State = state;
        }

        public int ID { get; set; }

        public int WorkFlowID { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public string ChainedActivityName { get; set; }

        public string Sequence { get; set; }

        public bool SplitWorkFlow { get; set; }

        public int Priority { get; set; }

        public int FormID { get; set; }

        public string ActivityMessage { get; set; }

        public int StateID { get; set; }

        public int NextStateID { get; set; }

        public int RaiseExternalActivity { get; set; }

        public int ExternalActivityTarget { get; set; }

        public int ActivatedByExternalActivity { get; set; }

        public X2State NextState { get; set; }

        public X2State State { get; set; }

        public X2Workflow Workflow { get; set; }

        public int CompareTo(X2Activity other)
        {
            if (this.ID == other.ID)
                return 1;
            return 0;
        }
    }
}