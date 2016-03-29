using FluentNHibernate.Mapping;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Mappings
{
    public partial class ProcessAssemblyMap : ClassMap<ProcessAssembly>
    {
        public ProcessAssemblyMap()
        {
            Table("ProcessAssembly");
            Schema("X2");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            References(x => x.Process).Column("ProcessID");
            References(x => x.ParentProcessAssembly).Column("ParentID");
            Map(x => x.Dllname).Column("DLLName").Not.Nullable().Length(255);
            Map(x => x.Dlldata).Column("DLLData").Not.Nullable().Length(2147483647);
            HasMany(x => x.ProcessAssemblies).KeyColumn("ParentID").Cascade.SaveUpdate().OrderBy("DLLName");
        }
    }
}