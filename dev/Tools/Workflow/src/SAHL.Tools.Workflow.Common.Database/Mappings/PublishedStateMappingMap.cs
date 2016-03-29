using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class PublishedStateMappingMap : ClassMap<PublishedStateMapping>
    {
        public PublishedStateMappingMap()
        {
            Table("PublishedStateMapping");
            Schema("X2");
            LazyLoad();
            CompositeId()
                .KeyProperty(x => x.OldWorkFlowID, "OldWorkFlowID")
                .KeyProperty(x => x.OldStateID, "OldStateID")
                .KeyProperty(x => x.NewWorkFlowID, "NewWorkFlowID")
                .KeyProperty(x => x.NewStateID, "NewStateID");
            /*
            Map(x => x.OldWorkFlowID).Column("OldWorkFlowID").Not.Nullable();
            Map(x => x.OldStateID).Column("OldStateID").Not.Nullable();
            Map(x => x.NewWorkFlowID).Column("NewWorkFlowID").Not.Nullable();
            Map(x => x.NewStateID).Column("NewStateID").Not.Nullable();
            */
        }
    }
}