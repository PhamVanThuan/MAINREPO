using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class ActiveExternalActivityMap : ClassMap<ActiveExternalActivity>
    {
        public ActiveExternalActivityMap()
        {
            Table("ActiveExternalActivity");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.ExternalActivity).Column("ExternalActivityID");
            Map(x => x.WorkFlowID).Column("WorkFlowID").Not.Nullable();
            Map(x => x.ActivatingInstanceID).Column("ActivatingInstanceID");
            Map(x => x.ActivationTime).Column("ActivationTime").Not.Nullable();
            Map(x => x.ActivityXMLData).Column("ActivityXMLData").Length(2147483647);
            Map(x => x.WorkFlowProviderName).Column("WorkFlowProviderName").Length(128);
        }
    }
}