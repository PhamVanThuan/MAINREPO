using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class InstanceActivitySecurityMap : ClassMap<InstanceActivitySecurity>
    {
        public InstanceActivitySecurityMap()
        {
            Table("InstanceActivitySecurity");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.Instance).Column("InstanceID").Cascade.SaveUpdate();
            References(x => x.Activity).Column("ActivityID").Cascade.SaveUpdate();
            Map(x => x.Id).Column("ID").Not.Nullable();
            Map(x => x.Adusername).Column("ADUserName").Not.Nullable().Length(255);
        }
    }
}