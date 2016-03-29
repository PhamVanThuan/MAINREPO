using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    public partial class AddressBox : Address, IAddressBox
    {
        public override string GetFormattedDescription(SAHL.Common.Globals.AddressDelimiters delimiter)
        {
            List<String> addressLines = new List<string>();

            if (PostOffice != null)
            {
                addressLines.Add("P O Box " + this.BoxNumber);
                addressLines.Add(PostOffice.Description);
                if (RRR_CityDescription != PostOffice.Description)
                    addressLines.Add(RRR_CityDescription);
                addressLines.Add(RRR_ProvinceDescription);
                addressLines.Add(RRR_PostalCode);
                addressLines.Add(RRR_CountryDescription);
            }
            return base.GetFormattedDescription(addressLines, delimiter);
        }
    }
}