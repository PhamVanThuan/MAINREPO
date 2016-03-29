using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.DomainQuery.Model
{
    
    public class GetClientAddressQueryResult
    {
        
        public int ClientAddressKey { get; set; }

        public int ClientKey { get; set; }

        public int AddressKey { get; set; }

        public int AddressFormatKey { get; set; }
    }
}