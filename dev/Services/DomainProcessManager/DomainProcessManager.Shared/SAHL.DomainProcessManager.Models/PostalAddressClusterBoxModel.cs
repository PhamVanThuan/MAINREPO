using System;
using System.Runtime.Serialization;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class PostalAddressClusterBoxModel : PostalAddressModel
    {
        public PostalAddressClusterBoxModel(string clusterBoxNumber, string postOffice, string suburb, string city, string postalCode, string province, string country)
            : base(AddressFormat.ClusterBox, clusterBoxNumber, null, null, null, null, null, null, postOffice, suburb, city, postalCode, province, country)
        {
        }
    }
}
