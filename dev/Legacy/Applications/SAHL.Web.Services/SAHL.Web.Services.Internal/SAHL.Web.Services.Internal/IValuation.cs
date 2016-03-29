using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;

namespace SAHL.Web.Services.Internal
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IValuation" in both code and config file together.
    [ServiceContract]
    public interface IValuation
    {
        [OperationContract]
        DataSet SubmitCompletedValuationLightstone(int UniqueID, string XMLData);

        [OperationContract]
        DataSet SubmitRejectedValuationLightstone(int UniqueID, string XMLData);

        [OperationContract]
        DataSet SubmitAmendedValuationLightstone(int UniqueID, string XMLData);
    }
}
