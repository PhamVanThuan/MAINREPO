using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel
{
    public partial class Vendor : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Vendor_DAO>, IVendor
    {
    }
}
