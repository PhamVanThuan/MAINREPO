using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ClientDomain.Managers.Client
{
    public class ClientManager : IClientManager
    {
        private IClientDataManager clientDataManager;

        public ClientManager(IClientDataManager clientDataManager)
        {
            this.clientDataManager = clientDataManager;
        }
    }
}
