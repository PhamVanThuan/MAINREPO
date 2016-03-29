using SAHL.Core.Data.Models._2AM;
using System.Collections.Generic;

namespace SAHL.Core.Data.Models._2AM.Managers.ADUser
{
    public interface IADUserDataManager
    {
        IEnumerable<ADUserDataModel> GetAdUserByUserName(string userName);
    }
}
