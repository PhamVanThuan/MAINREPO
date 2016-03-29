using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class ProcessAssembliesByProcessIdSqlStatement : ISqlStatement<ProcessAssemblyDataModel>
    {
        public int ProcessID { get; set; }

        public ProcessAssembliesByProcessIdSqlStatement(int processId)
        {
            this.ProcessID = processId;
        }

        public string GetStatement()
        {
            string sql = @"select ID, ProcessID, ParentID, DLLName, DLLData
                        from x2.x2.processassembly
                        where ProcessID=@ProcessID";
            return sql;
        }
    }
}