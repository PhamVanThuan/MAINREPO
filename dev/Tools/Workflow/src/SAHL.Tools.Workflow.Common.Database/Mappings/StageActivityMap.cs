using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class StageActivityMap : ClassMap<StageActivity>
    {
        public StageActivityMap()
        {
            Table("StageActivity");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.Activity).Column("ActivityID").Cascade.SaveUpdate();
            Map(x => x.StageDefinitionKey).Column("StageDefinitionKey");
            Map(x => x.StageDefinitionStageDefinitionGroupKey).Column("StageDefinitionStageDefinitionGroupKey");
        }
    }
}