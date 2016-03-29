using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.Address
{
    [RuleInfo]
    [RuleDBTag("AddressFreetextConditionalInternationalAddress",
        "A freetext address can only be used for international address (not South Africa)",
        "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Address.AddressFreetextConditionalInternationalAddress")]
    public class AddressFreetextConditionalInternationalAddress : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IAddress addy = (IAddress)Parameters[0];
            IAddressFreeText freetext = addy as IAddressFreeText;
            if (null != freetext)
            {
                // Freetext can only be used for international addresses
                if (freetext.RRR_CountryDescription == "South Africa")
                {
                    AddMessage("A Freetext address box can only be used for an International Address", "", Messages);
                    return -1;
                }
            }
            
            return 1;
        }
    }

    [RuleInfo]
    [RuleDBTag("AddressFreetextOneLineMandatory",
        "A freetext address must have at least one line specified",
        "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Address.AddressFreetextOneLineMandatory")]
    public class AddressFreetextOneLineMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IAddressFreeText address = (IAddressFreeText)Parameters[0];

            if (String.IsNullOrEmpty(address.FreeText1)
                && String.IsNullOrEmpty(address.FreeText2)
                && String.IsNullOrEmpty(address.FreeText3)
                && String.IsNullOrEmpty(address.FreeText4)
                && String.IsNullOrEmpty(address.FreeText5))
            {
                AddMessage("A Freetext address must have at least one line specified", "", Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleInfo]
    [RuleDBTag("AddressFreeTextCountryMandatory",
        "A freetext address must have a country specified",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Address.AddressFreeTextCountryMandatory")]
    public class AddressFreeTextCountryMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IAddressFreeText address = (IAddressFreeText)Parameters[0];

            // the country link is via the post office
            if (address.PostOffice == null)
            {
                AddMessage("A Freetext address must have a country specified", "", Messages);
                return 0;
            }

            return 1;
        }
    }


}
