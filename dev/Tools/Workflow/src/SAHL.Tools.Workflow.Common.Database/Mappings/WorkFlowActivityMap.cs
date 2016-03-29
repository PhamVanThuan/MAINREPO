using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class WorkFlowActivityMap : ClassMap<WorkFlowActivity>
    {
        public WorkFlowActivityMap()
        {
            Table("WorkFlowActivity");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.WorkFlow).Column("WorkFlowID").Cascade.SaveUpdate();
            References(x => x.NextWorkFlow).Column("NextWorkFlowID").Cascade.SaveUpdate();
            References(x => x.NextActivity).Column("NextActivityID").Cascade.SaveUpdate();
            Map(x => x.Name).Column("Name").Not.Nullable().Length(30);
            References(x => x.State).Column("StateID").Cascade.SaveUpdate();
            References(x => x.ReturnActivity).Column("ReturnActivityID").Cascade.SaveUpdate();
        }
    }
}