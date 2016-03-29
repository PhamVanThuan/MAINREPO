using System.Collections.Generic;
using System;

namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class Activity
    {
        public Activity()
        {
            this.Security = new List<ActivitySecurity>();
            this.StageActivities = new List<StageActivity>();
        }

        public virtual int Id { get; set; }

        public virtual Guid X2ID { get; set; }

        public virtual WorkFlow WorkFlow { get; set; }

        public virtual ActivityType ActivityType { get; set; }

        public virtual State FromState { get; set; }

        public virtual State ToState { get; set; }

        public virtual Form Form { get; set; }

        public virtual ExternalActivity RaiseExternalActivity { get; set; }

        public virtual ExternalActivityTarget ExternalActivityTarget { get; set; }

        public virtual string Name { get; set; }

        public virtual bool SplitWorkFlow { get; set; }

        public virtual int Priority { get; set; }

        public virtual string ActivityMessage { get; set; }

        public virtual ExternalActivity ActivatedByExternalActivity { get; set; }

        public virtual string ChainedActivityName { get; set; }

        public virtual IList<ActivitySecurity> Security { get; protected set; }

        public virtual IList<StageActivity> StageActivities { get; protected set; }
    }
}