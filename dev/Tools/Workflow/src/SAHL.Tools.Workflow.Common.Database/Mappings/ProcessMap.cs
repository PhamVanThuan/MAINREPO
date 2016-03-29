using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class ProcessMap : ClassMap<Process>
    {
        public ProcessMap()
        {
            Table("Process");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.ParentProcess).Column("ProcessAncestorID");
            Map(x => x.Name).Column("Name").Length(255);
            Map(x => x.Version).Column("Version").Length(50);
            Map(x => x.DesignerData).Column("DesignerData").Length(2147483647);
            Map(x => x.CreateDate).Column("CreateDate").Not.Nullable();
            Map(x => x.MapVersion).Column("MapVersion").Length(50);
            Map(x => x.ConfigFile).Column("ConfigFile").Length(8096);
            Map(x => x.IsLegacy).Column("IsLegacy");
            Map(x => x.ViewableOnUserInterfaceVersion).Column("ViewableOnUserInterfaceVersion");
            HasMany(x => x.ProcessAssemblies).KeyColumn("ProcessID").Cascade.All().OrderBy("DLLName");
            HasMany(x => x.ProcessAssemblyNuGetInfos).KeyColumn("ProcessID").Cascade.All().OrderBy("PackageName");
            HasMany(x => x.SecurityGroups).KeyColumn("ProcessID").Cascade.All().OrderBy("Name");
            HasMany(x => x.WorkFlows).KeyColumn("ProcessID").Cascade.All().OrderBy("Name");
        }
    }
}