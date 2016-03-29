using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class WorkFlowSecurityMap : ClassMap<WorkFlowSecurity>
    {
        public WorkFlowSecurityMap()
        {
            Table("WorkFlowSecurity");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.WorkFlow).Column("WorkFlowID").Cascade.SaveUpdate();
            References(x => x.SecurityGroup).Column("SecurityGroupID").Cascade.SaveUpdate();
        }
    }
}