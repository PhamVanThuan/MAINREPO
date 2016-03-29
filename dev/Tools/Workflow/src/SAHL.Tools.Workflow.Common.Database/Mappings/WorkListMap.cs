using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class WorkListMap : ClassMap<WorkList>
    {
        public WorkListMap()
        {
            Table("WorkList");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.Instance).Column("InstanceID");
            Map(x => x.Id).Column("ID").Not.Nullable();
            Map(x => x.Adusername).Column("ADUserName").Not.Nullable().Length(255);
            Map(x => x.ListDate).Column("ListDate").Not.Nullable();
            Map(x => x.Message).Column("Message").Not.Nullable().Length(255);
        }
    }
}