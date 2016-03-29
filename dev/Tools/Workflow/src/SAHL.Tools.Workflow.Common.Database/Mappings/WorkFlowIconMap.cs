using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class WorkFlowIconMap : ClassMap<WorkFlowIcon>
    {
        public WorkFlowIconMap()
        {
            Table("WorkFlowIcon");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            Map(x => x.Name).Column("Name").Not.Nullable().Length(128);
            Map(x => x.Icon).Column("Icon").Length(2147483647);
        }
    }
}