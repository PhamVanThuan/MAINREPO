using SAHL.Core.Data.Models._2AM.Managers.ADUser.Statements;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Data.Models._2AM.Managers.ADUser
{
    public class ADUserDataManager : IADUserDataManager
    {
        private IDbFactory dbFactory;

        public ADUserDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public IEnumerable<ADUserDataModel> GetAdUserByUserName(string userName)
        {
            IEnumerable<ADUserDataModel> userCollection;
            var query = new GetAdUserByUserNameStatement(userName);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                userCollection = db.Select<ADUserDataModel>(query);
            }
            return userCollection;
        }
    }
}
