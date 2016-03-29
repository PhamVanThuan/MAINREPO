using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class StateWorkListMap : ClassMap<StateWorkList>
    {
        public StateWorkListMap()
        {
            Table("StateWorkList");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.State).Column("StateID").Cascade.SaveUpdate();
            References(x => x.SecurityGroup).Column("SecurityGroupID").Cascade.SaveUpdate();
        }
    }
}