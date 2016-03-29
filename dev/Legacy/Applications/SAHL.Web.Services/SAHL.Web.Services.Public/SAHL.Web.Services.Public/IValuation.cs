using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SAHL.Web.Services.Public
{
    [ServiceContract]
    public interface IValuation
    {
        [OperationContract]
        DataSet SubmitCompletedValuationLightstone(int UniqueID, string XMLData);

        [OperationContract]
        DataSet SubmitAmendedValuationLightstone(int UniqueID, string XMLData);

        [OperationContract]
        DataSet SubmitRejectedValuationLightstone(int UniqueID, string XMLData);
    }
}
