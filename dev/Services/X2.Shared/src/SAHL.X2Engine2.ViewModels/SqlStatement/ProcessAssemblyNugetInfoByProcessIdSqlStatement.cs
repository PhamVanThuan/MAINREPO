using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class ProcessAssemblyNugetInfoByProcessIdSqlStatement : ISqlStatement<ProcessAssemblyNugetInfoDataModel>
    {
        public int ProcessId { get; set; }

        public ProcessAssemblyNugetInfoByProcessIdSqlStatement(int processId)
        {
            this.ProcessId = processId;
        }

        public string GetStatement()
        {
            string sql = @"select ID, ProcessID, PackageName, PackageVersion from x2.x2.ProcessAssemblyNugetInfo where ProcessId=@ProcessId";
            return sql;
        }
    }
}