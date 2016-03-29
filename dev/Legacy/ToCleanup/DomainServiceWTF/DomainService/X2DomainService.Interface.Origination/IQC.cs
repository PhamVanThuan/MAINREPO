using System;
using System.Collections.Generic;
using System.Text;
using SAHL.X2.Common;

namespace X2DomainService.Interface.Origination
{
    public interface IQC
    {
        IX2ReturnData QCApproved(int ApplicationKey, Int64 InstanceID);
    }
}
