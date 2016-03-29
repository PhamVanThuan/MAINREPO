using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class ActivitySecurityMap : ClassMap<ActivitySecurity>
    {
        public ActivitySecurityMap()
        {
            Table("ActivitySecurity");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.Activity).Column("ActivityID");
            References(x => x.SecurityGroup).Column("SecurityGroupID").Cascade.SaveUpdate();
        }
    }
}