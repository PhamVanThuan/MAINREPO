using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class WorkFlowMap : ClassMap<WorkFlow>
    {
        public WorkFlowMap()
        {
            Table("WorkFlow");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.Process).Column("ProcessID");
            References(x => x.WorkFlowAncestor).Column("WorkFlowAncestorID");
            Map(x => x.WorkFlowIconId).Column("IconID");
            Map(x => x.Name).Column("Name").Not.Nullable().Length(128);
            Map(x => x.CreateDate).Column("CreateDate").Not.Nullable();
            Map(x => x.StorageTable).Column("StorageTable").Not.Nullable().Length(128);
            Map(x => x.StorageKey).Column("StorageKey").Not.Nullable().Length(128);
            Map(x => x.DefaultSubject).Column("DefaultSubject").Length(128);
            Map(x => x.GenericKeyTypeKey).Column("GenericKeyTypeKey");

            HasMany(x => x.SecurityGroups).KeyColumn("WorkFlowID").Cascade.All().OrderBy("Name");
            HasMany(x => x.Forms).KeyColumn("WorkFlowID").Cascade.All().OrderBy("Name");
            HasMany(x => x.ExternalActivities).KeyColumn("WorkFlowID").Cascade.All().OrderBy("Name");
            HasMany(x => x.States).KeyColumn("WorkFlowID").Cascade.All().OrderBy("Name");
            HasMany(x => x.Activities).KeyColumn("WorkFlowID").Cascade.All().OrderBy("Name");
            HasMany(x => x.CallWorkFlowActivities).KeyColumn("WorkFlowID").Cascade.All().OrderBy("Name");
        }
    }
}