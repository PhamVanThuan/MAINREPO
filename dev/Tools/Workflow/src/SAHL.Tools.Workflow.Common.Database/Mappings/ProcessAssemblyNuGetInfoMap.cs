using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class ProcessAssemblyNuGetInfoMap : ClassMap<ProcessAssemblyNuGetInfo>
    {
        public ProcessAssemblyNuGetInfoMap()
        {
            Table("ProcessAssemblyNuGetInfo");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.Process).Column("ProcessID");
            Map(x => x.PackageName).Column("PackageName").Not.Nullable().Length(100);
            Map(x => x.PackageVersion).Column("PackageVersion").Not.Nullable().Length(100);
        }
    }
}