using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class ActivityMap : ClassMap<Activity>
    {
        public ActivityMap()
        {
            Table("Activity");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.WorkFlow).Column("WorkFlowID").Cascade.SaveUpdate();
            References(x => x.FromState).Column("StateID").Cascade.SaveUpdate();
            References(x => x.ToState).Column("NextStateID").Cascade.SaveUpdate();
            References(x => x.Form).Column("FormID").Cascade.SaveUpdate();
            References(x => x.RaiseExternalActivity).Column("RaiseExternalActivity").Cascade.SaveUpdate();
            References(x => x.ActivatedByExternalActivity).Column("ActivatedByExternalActivity").Cascade.SaveUpdate();
            Map(x => x.X2ID).Column("X2ID");
            Map(x => x.ExternalActivityTarget).Column("ExternalActivityTarget");
            Map(x => x.ActivityType).Column("Type");
            Map(x => x.Name).Column("Name").Not.Nullable().Length(30);
            Map(x => x.SplitWorkFlow).Column("SplitWorkFlow").Not.Nullable();
            Map(x => x.Priority).Column("Priority").Not.Nullable();
            Map(x => x.ActivityMessage).Column("ActivityMessage").Length(128);
            Map(x => x.ChainedActivityName).Column("ChainedActivityName").Length(30);
            HasMany(x => x.Security).KeyColumn("ActivityID").Cascade.SaveUpdate().OrderBy("SecurityGroupID");
            HasMany(x => x.StageActivities).KeyColumn("ActivityID").Cascade.SaveUpdate().OrderBy("ID");
        }
    }
}