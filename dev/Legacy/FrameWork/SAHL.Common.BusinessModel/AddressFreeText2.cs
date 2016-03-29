using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    public partial class AddressFreeText : Address, IAddressFreeText
    {
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("AddressFreetextOneLineMandatory");
            Rules.Add("AddressFreetextConditionalInternationalAddress");
            Rules.Add("AddressFreeTextCountryMandatory");
        }

        public override string GetFormattedDescription(SAHL.Common.Globals.AddressDelimiters delimiter)
        {
            List<String> addressLines = new List<string>();
            if (FreeText1 != null && FreeText1.Length > 0) addressLines.Add(FreeText1);
            if (FreeText2 != null && FreeText2.Length > 0) addressLines.Add(FreeText2);
            if (FreeText3 != null && FreeText3.Length > 0) addressLines.Add(FreeText3);
            if (FreeText4 != null && FreeText4.Length > 0) addressLines.Add(FreeText4);
            if (FreeText5 != null && FreeText5.Length > 0) addressLines.Add(FreeText5);
            if (RRR_CountryDescription != null && RRR_CountryDescription.Length > 0) addressLines.Add(RRR_CountryDescription);

            return base.GetFormattedDescription(addressLines, delimiter);
        }
    }
}