using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IMemoReadOnlyRepository
    {
        IMemo GetMemoByKey(int Key);
    }
}