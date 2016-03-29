using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class WorkFlowProviderInstanceMap : ClassMap<WorkFlowProviderInstance>
    {
        public WorkFlowProviderInstanceMap()
        {
            Table("WorkFlowProviderInstances");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            Map(x => x.WorkFlowProviderName).Column("WorkFlowProviderName").Not.Nullable().Length(128);
            Map(x => x.ActiveDate).Column("ActiveDate").Not.Nullable();
        }
    }
}