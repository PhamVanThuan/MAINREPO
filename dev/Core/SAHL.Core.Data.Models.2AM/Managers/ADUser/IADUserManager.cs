using System.Collections.Generic;

namespace SAHL.Core.Data.Models._2AM.Managers.ADUser
{
    public interface IADUserManager
    {
        IEnumerable<ADUserDataModel> GetAdUserByUserName(string userName);

        int? GetAdUserKeyByUserName(string userName);
    }
}
