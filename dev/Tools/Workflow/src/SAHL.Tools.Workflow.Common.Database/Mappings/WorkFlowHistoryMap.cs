using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class WorkFlowHistoryMap : ClassMap<WorkFlowHistory>
    {
        public WorkFlowHistoryMap()
        {
            Table("WorkFlowHistory");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.State).Column("StateID");
            References(x => x.Activity).Column("ActivityID");
            Map(x => x.InstanceID).Column("InstanceID").Not.Nullable();
            Map(x => x.CreatorADUserName).Column("CreatorADUserName").Not.Nullable().Length(128);
            Map(x => x.CreationDate).Column("CreationDate").Not.Nullable();
            Map(x => x.StateChangeDate).Column("StateChangeDate").Not.Nullable();
            Map(x => x.DeadlineDate).Column("DeadlineDate");
            Map(x => x.ActivityDate).Column("ActivityDate").Not.Nullable();
            Map(x => x.Adusername).Column("ADUserName").Not.Nullable().Length(255);
            Map(x => x.Priority).Column("Priority");
        }
    }
}