using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class FormMap : ClassMap<Form>
    {
        public FormMap()
        {
            Table("Form");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.WorkFlow).Column("WorkFlowID").Cascade.SaveUpdate();
            Map(x => x.Name).Column("Name").Not.Nullable().Length(128);
            Map(x => x.Description).Column("Description").Length(1024);
        }
    }
}