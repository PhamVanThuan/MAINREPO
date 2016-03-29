using System;
using System.Runtime.Serialization;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class PostalAddressPostnetSuiteModel : PostalAddressModel
    {
        public PostalAddressPostnetSuiteModel(string privateBagNumber, string suiteNumber, string postOffice, string suburb, string city, string postalCode, 
                                              string province, string country)
            : base(AddressFormat.PostNetSuite, privateBagNumber, null, null, null, null, null, null, postOffice, suburb, city, postalCode, province, country)
        {
        }
    }
}
