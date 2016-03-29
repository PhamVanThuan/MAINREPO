using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class InstanceMap : ClassMap<Instance>
    {
        public InstanceMap()
        {
            Table("Instance");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.WorkFlow).Column("WorkFlowID");
            References(x => x.ParentInstance).Column("ParentInstanceID");
            References(x => x.State).Column("StateID");
            Map(x => x.Name).Column("Name").Not.Nullable().Length(128);
            Map(x => x.Subject).Column("Subject").Length(800);
            Map(x => x.WorkFlowProvider).Column("WorkFlowProvider").Not.Nullable().Length(128);
            Map(x => x.CreatorADUserName).Column("CreatorADUserName").Not.Nullable().Length(128);
            Map(x => x.CreationDate).Column("CreationDate").Not.Nullable();
            Map(x => x.StateChangeDate).Column("StateChangeDate");
            Map(x => x.DeadlineDate).Column("DeadlineDate");
            Map(x => x.ActivityDate).Column("ActivityDate");
            Map(x => x.ActivityADUserName).Column("ActivityADUserName").Length(128);
            Map(x => x.ActivityID).Column("ActivityID");
            Map(x => x.Priority).Column("Priority");
            Map(x => x.SourceInstanceID).Column("SourceInstanceID");
            Map(x => x.ReturnActivityID).Column("ReturnActivityID");
        }
    }
}