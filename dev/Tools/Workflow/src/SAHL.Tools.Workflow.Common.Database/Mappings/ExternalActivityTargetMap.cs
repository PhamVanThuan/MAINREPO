//using FluentNHibernate.Mapping;
//using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

//namespace SAHL.Tools.Workflow.Common.Database.Mappings
//{
//    public partial class ExternalActivityTargetMap : ClassMap<ExternalActivityTarget>
//    {
//        public ExternalActivityTargetMap()
//        {
//            Table("ExternalActivityTarget");
//            Schema("X2");
//            LazyLoad();
//            Id(x => x.Value).GeneratedBy.Identity().Column("ID");
//            Map(x => x.DisplayName).Column("Name").Not.Nullable().Length(128);
//        }
//    }
//}