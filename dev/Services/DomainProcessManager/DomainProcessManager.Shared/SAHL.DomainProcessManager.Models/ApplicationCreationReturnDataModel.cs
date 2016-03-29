using SAHL.Core.Data;
using System;
using System.Runtime.Serialization;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicationCreationReturnDataModel : IDataModel
    {
        public ApplicationCreationReturnDataModel(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }

        [DataMember]
        public int ApplicationNumber { get; private set; }
    }
}