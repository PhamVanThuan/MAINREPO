using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class StateWorkListForStateSqlStatement : ISqlStatement<StateWorkListDataModel>
    {
        public int StateId { get; protected set; }

        public StateWorkListForStateSqlStatement(int stateId)
        {
            this.StateId = stateId;
        }

        public string GetStatement()
        {
            string sql = @"select ID, StateID, SecurityGroupID
                    from x2.x2.stateworklist
                    where StateID=@StateID";
            return sql;
        }
    }
}