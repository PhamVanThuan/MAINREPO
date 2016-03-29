using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class ADUserByAdUserNameSqlStatement : ISqlStatement<ADUserDataModel>
    {
        public string ADUserName { get; protected set; }

        public ADUserByAdUserNameSqlStatement(string adUserName)
        {
            this.ADUserName = adUserName;
        }

        public string GetStatement()
        {
            string sql = @"select top 1 ADUserKey, ADUserName, GeneralStatusKey, Password as 'Password', PasswordQuestion, PasswordAnswer, LegalEntityKey
from [2am].dbo.ADUser
where ADUserName=@ADUserName
order by 1 desc";
            return sql;
        }
    }
}