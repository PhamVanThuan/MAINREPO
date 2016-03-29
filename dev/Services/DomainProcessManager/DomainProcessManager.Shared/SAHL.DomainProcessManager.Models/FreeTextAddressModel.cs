using System;
using System.Runtime.Serialization;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class FreeTextAddressModel : AddressModel
    {
        public FreeTextAddressModel(AddressType addressType, string freeText1, string freeText2, string freeText3, string freeText4, string freeText5, string country)
            : base(addressType, AddressFormat.FreeText, null, null, null, null, null, null, null, null, freeText1, freeText2, freeText3, freeText4, freeText5,
                   null, null, null, null, country, false)
        {
            this.AddressType = addressType;
            this.FreeText1   = freeText1;
            this.FreeText2   = freeText2;
            this.FreeText3   = freeText3;
            this.FreeText4   = freeText4;
            this.FreeText5   = freeText5;
            this.Country     = country;
        }
    }
}
