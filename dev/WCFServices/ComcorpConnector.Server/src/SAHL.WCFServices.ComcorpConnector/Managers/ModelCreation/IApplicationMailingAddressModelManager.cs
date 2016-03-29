using SAHL.DomainProcessManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public interface IApplicationMailingAddressModelManager
    {
        ApplicationMailingAddressModel PopulateApplicationMailingAddress(AddressModel address, int? clientKey); 
    }
}
