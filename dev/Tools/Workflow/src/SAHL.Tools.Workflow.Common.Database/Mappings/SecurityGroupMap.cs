using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class SecurityGroupMap : ClassMap<SecurityGroup>
    {
        public SecurityGroupMap()
        {
            Table("SecurityGroup");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.Process).Column("ProcessID").Cascade.SaveUpdate();
            References(x => x.WorkFlow).Column("WorkFlowID").Cascade.SaveUpdate();
            Map(x => x.IsDynamic).Column("IsDynamic").Not.Nullable();
            Map(x => x.Name).Column("Name").Not.Nullable().Length(128);
            Map(x => x.Description).Column("Description").Length(255);
        }
    }
}