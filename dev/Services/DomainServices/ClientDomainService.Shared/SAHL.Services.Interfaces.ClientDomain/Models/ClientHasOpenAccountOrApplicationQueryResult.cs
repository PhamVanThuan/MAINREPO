using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class ClientHasOpenAccountOrApplicationQueryResult
    {
        public bool ClientIsAlreadyAnSAHLCustomer { get; set; }
    }
}
