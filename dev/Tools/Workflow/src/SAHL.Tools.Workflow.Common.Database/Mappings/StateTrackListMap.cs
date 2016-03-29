using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class StateTrackListMap : ClassMap<StateTrackList>
    {
        public StateTrackListMap()
        {
            Table("StateTrackList");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.State).Column("StateID");
            References(x => x.SecurityGroup).Column("SecurityGroupID");
        }
    }
}