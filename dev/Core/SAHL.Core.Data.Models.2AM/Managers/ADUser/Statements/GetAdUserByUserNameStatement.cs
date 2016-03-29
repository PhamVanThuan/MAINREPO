
namespace SAHL.Core.Data.Models._2AM.Managers.ADUser.Statements
{
    public class GetAdUserByUserNameStatement : ISqlStatement<ADUserDataModel>
    {
        public string UserName { get; protected set; }

        public GetAdUserByUserNameStatement(string userName)
        {
            this.UserName = userName;
        }

        public string GetStatement()
        {
            var query = @"SELECT 
                            * 
                        FROM 
                            [2AM].[dbo].[ADUser] 
                        WHERE 
                            ADUserName = @UserName";
            return query;
        }
    }
}
