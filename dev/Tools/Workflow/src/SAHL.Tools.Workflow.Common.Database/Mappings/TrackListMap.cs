using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class TrackListMap : ClassMap<TrackList>
    {
        public TrackListMap()
        {
            Table("TrackList");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.Instance).Column("InstanceID");
            Map(x => x.Adusername).Column("ADUserName").Not.Nullable().Length(255);
            Map(x => x.ListDate).Column("ListDate").Not.Nullable();
        }
    }
}