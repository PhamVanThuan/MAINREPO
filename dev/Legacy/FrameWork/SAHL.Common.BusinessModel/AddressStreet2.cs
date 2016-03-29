using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Utils;

namespace SAHL.Common.BusinessModel
{
    public partial class AddressStreet : Address, IAddressStreet
    {
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("AddressStreetConditionalUnitBuildingNoOrNameRequiresStreet");
            Rules.Add("AddressStreetMandatoryStreetOrBuildingOrUnit");
            Rules.Add("AddressStreetMandatoryStreetName");
        }

        public override string GetFormattedDescription(SAHL.Common.Globals.AddressDelimiters delimiter)
        {
            List<String> addressLines = new List<string>();
            if (UnitNumber != null && UnitNumber.Length > 0) addressLines.Add(UnitNumber);
            addressLines.Add(StringUtils.JoinNullableStrings(BuildingNumber, " ", BuildingName));
            addressLines.Add(StringUtils.JoinNullableStrings(StreetNumber, " ", StreetName));
            if (RRR_SuburbDescription != null && RRR_SuburbDescription.Length > 0) addressLines.Add(RRR_SuburbDescription);
            if (RRR_CityDescription != RRR_SuburbDescription && RRR_CityDescription != null && RRR_CityDescription.Length > 0)
                addressLines.Add(RRR_CityDescription);
            if (RRR_ProvinceDescription != null && RRR_ProvinceDescription.Length > 0) addressLines.Add(RRR_ProvinceDescription);
            if (RRR_PostalCode != null && RRR_PostalCode.Length > 0) addressLines.Add(RRR_PostalCode);
            if (RRR_CountryDescription != null && RRR_CountryDescription.Length > 0) addressLines.Add(RRR_CountryDescription);

            return base.GetFormattedDescription(addressLines, delimiter);
        }
    }
}