using System;
using System.Runtime.Serialization;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class PostalAddressBoxModel : PostalAddressModel
    {
        public PostalAddressBoxModel(string boxNumber, string postOffice, string suburb, string city, string postalCode, string province, string country)
            : base(AddressFormat.Box, boxNumber, null, null, null, null, null, null, postOffice, suburb, city, postalCode, province, country)
        {
        }
    }
}
