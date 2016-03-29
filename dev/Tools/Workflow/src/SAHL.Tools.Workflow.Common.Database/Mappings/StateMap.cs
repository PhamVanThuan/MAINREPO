using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class StateMap : ClassMap<State>
    {
        public StateMap()
        {
            Table("State");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.WorkFlow).Column("WorkFlowID");
            Map(x => x.X2ID).Column("X2ID");
            Map(x => x.StateType).Column("Type");
            Map(x => x.Name).Column("Name").Not.Nullable().Length(30);
            Map(x => x.ForwardState).Column("ForwardState").Not.Nullable();
            References(x => x.ReturnWorkflow).Column("ReturnWorkflowID").Cascade.SaveUpdate();
            References(x => x.ReturnActivity).Column("ReturnActivityID").Cascade.SaveUpdate();
            HasMany(x => x.WorkList).KeyColumn("StateID").Cascade.SaveUpdate().OrderBy("StateID");
            HasMany(x => x.Forms).KeyColumn("StateID").Cascade.SaveUpdate().OrderBy("FormOrder");
        }
    }
}