using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Testing.Common.Models
{

    public class ThirdPartyInvoicesInstanceDataModel : SAHL.Core.Data.IDataModel
    {
        public int InstanceId { get; set; }
        public int GenericKey { get; set; }
    }
}
