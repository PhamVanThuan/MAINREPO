using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class ScheduledActivityMap : ClassMap<ScheduledActivity>
    {
        public ScheduledActivityMap()
        {
            Table("ScheduledActivity");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.Instance).Column("InstanceID");
            References(x => x.Activity).Column("ActivityID");
            Map(x => x.Time).Column("Time");
            Map(x => x.Priority).Column("Priority").Not.Nullable();
            Map(x => x.WorkFlowProviderName).Column("WorkFlowProviderName").Length(128);
        }
    }
}