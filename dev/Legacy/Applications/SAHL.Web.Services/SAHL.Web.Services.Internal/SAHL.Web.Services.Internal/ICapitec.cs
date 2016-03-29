using SAHL.Services.Capitec.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SAHL.Web.Services.Internal
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICapitec" in both code and config file together.
    [ServiceContract]
    public interface ICapitec
    {
        [OperationContract]
        bool CreateCapitecNewPurchaseApplication(NewPurchaseApplication application, int messageId);

        [OperationContract]
        bool CreateCapitecSwitchLoanApplication(SwitchLoanApplication application, int messageId);
    }
}
