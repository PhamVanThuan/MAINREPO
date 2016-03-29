using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Common.BusinessModel.CorrespondenceGeneration.Interfaces
{
    public interface ICorrespondenceStrategy
    {
        int GetAccountKey(int p_BusinessKey, Int64 p_InstanceID);
    }

}
