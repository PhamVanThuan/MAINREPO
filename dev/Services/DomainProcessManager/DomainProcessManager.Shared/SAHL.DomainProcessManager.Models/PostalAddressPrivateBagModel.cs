using System;
using System.Runtime.Serialization;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class PostalAddressPrivateBagModel : PostalAddressModel
    {
        public PostalAddressPrivateBagModel(string privateBagNumber, string postOffice, string suburb, string city, string postalCode, string province, string country)
            : base(AddressFormat.PrivateBag, privateBagNumber, null, null, null, null, null, null, postOffice, suburb, city, postalCode, province, country)
        {
        }
    }
}
