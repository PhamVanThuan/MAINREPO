using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.SharedServices.Common
{
    public class RemoveApplicationAttributeCommand : StandardDomainServiceCommand
    {
        public RemoveApplicationAttributeCommand(int applicationKey, int applicationAttributeTypeKey)
        {
            this.ApplicationKey = applicationKey;
            this.ApplicationAttributeTypeKey = applicationAttributeTypeKey;
        }

        public int ApplicationKey { get; set; }

        public int ApplicationAttributeTypeKey { get; set; }
    }
}
