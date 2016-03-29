using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class StateFormMap : ClassMap<StateForm>
    {
        public StateFormMap()
        {
            Table("StateForm");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.State).Column("StateID").Cascade.SaveUpdate();
            References(x => x.Form).Column("FormID").Cascade.SaveUpdate();
            Map(x => x.FormOrder).Column("FormOrder").Not.Nullable();
        }
    }
}