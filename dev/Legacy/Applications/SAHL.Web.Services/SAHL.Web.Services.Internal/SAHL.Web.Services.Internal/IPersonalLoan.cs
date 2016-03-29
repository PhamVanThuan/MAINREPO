using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SAHL.Web.Services.Internal
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPersonalLoan" in both code and config file together.
    [ServiceContract]
    public interface IPersonalLoan
    {
        /// <summary>
        /// Creates a personal loan campaign lead given a legal entity's id number
        /// </summary>
        /// <param name="IDNumber"></param>
        [OperationContract]
        bool CreatePersonalLoanLeadFromIdNumber(string idNumber, int messageId);
    }
}
