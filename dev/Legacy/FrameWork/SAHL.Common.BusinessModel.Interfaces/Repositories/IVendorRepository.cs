using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IVendorRepository
    {
        IVendor GetVendorByLegalEntityKey(int legalEntityKey);
    }
}
