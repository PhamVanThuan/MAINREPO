
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Data.Models._2AM.Managers.ADUser
{
    public class ADUserManager : IADUserManager
    {
        private IADUserDataManager ADUserDataManager;

        public ADUserManager(IADUserDataManager ADUserDataManager)
        {
            this.ADUserDataManager = ADUserDataManager;
        }

        public IEnumerable<ADUserDataModel> GetAdUserByUserName(string userName)
        {
            return ADUserDataManager.GetAdUserByUserName(userName);
        }

        public int? GetAdUserKeyByUserName(string userName)
        {
            int? adUserKey = null;
            var userCollection = ADUserDataManager.GetAdUserByUserName(userName);
            if (userCollection.Any())
            {
                adUserKey = userCollection.First().ADUserKey;
            }
            return adUserKey;
        }
    }
}
