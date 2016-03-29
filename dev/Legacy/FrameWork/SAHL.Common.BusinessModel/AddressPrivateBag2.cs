using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    public partial class AddressPrivateBag : Address, IAddressPrivateBag
    {
        public override string GetFormattedDescription(SAHL.Common.Globals.AddressDelimiters delimiter)
        {
            List<String> addressLines = new List<string>();

            addressLines.Add("Private Bag " + this.PrivateBagNumber);

            addressLines.Add(this.PostOffice.Description);
            // Only add City is different Post Office Description
            if (RRR_CityDescription != PostOffice.Description)
                addressLines.Add(RRR_CityDescription);
            addressLines.Add(RRR_ProvinceDescription);
            addressLines.Add(RRR_PostalCode);
            addressLines.Add(RRR_CountryDescription);

            return base.GetFormattedDescription(addressLines, delimiter);
        }
    }
}