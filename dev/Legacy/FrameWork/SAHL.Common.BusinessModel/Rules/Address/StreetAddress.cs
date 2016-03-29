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

    /// <summary>
    /// Params:
    /// 0: IAddressStreet
    /// </summary>
    [RuleInfo]
    [RuleDBTag("AddressStreetConditionalUnitBuildingNoOrNameRequiresStreet",
        "A street number is required when no unit number, building number or building name is supplied",
        "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Address.AddressStreetConditionalUnitBuildingNoOrNameRequiresStreet")]
    public class AddressStreetConditionalUnitBuildingNoOrNameRequiresStreet : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IAddressStreet street = (IAddressStreet)Parameters[0];

            // if there is no (unitno, buildingno, buildingname) then street num is required
            if (String.IsNullOrEmpty(street.UnitNumber) && String.IsNullOrEmpty(street.BuildingNumber) && String.IsNullOrEmpty(street.BuildingName))
            {
                // streetnumber is mandatory
                if (String.IsNullOrEmpty(street.StreetNumber))
                {
                    AddMessage("When no unit number, building number or building name is supplied, Street Number is required.", "", Messages);
                }
            }
            return 1;
        }
    }

    /// <summary>
    /// Params:
    /// 0: IAddressStreet
    /// </summary>
    [RuleInfo]
    [RuleDBTag("AddressStreetMandatoryStreetOrBuildingOrUnit",
        "A street number, building number or unit number is required",
        "SAHL.Rules.DLL",
 "SAHL.Common.BusinessModel.Rules.Address.AddressStreetMandatoryStreetOrBuildingOrUnit")]
    public class AddressStreetMandatoryStreetOrBuildingOrUnit : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IAddressStreet street = (IAddressStreet)Parameters[0];

            // There must always be a number (unit, building or street)
            if (String.IsNullOrEmpty(street.UnitNumber) && String.IsNullOrEmpty(street.BuildingNumber) && String.IsNullOrEmpty(street.StreetNumber))
            {
                AddMessage("A street number, building number or unit number is required.", "", Messages);
            }

            return 1;
        }
    }

    /// <summary>
    /// Params:
    /// 0: IAddressStreet
    /// </summary>
    [RuleInfo]
    [RuleDBTag("AddressStreetMandatoryStreetName",
        "A street name must contain alpha/numeric characters after any leading or trailing white space has been removed.",
        "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.Address.AddressStreetMandatoryStreetName")]
    public class AddressStreetMandatoryStreetName : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IAddressStreet street = (IAddressStreet)Parameters[0];

            // There must always be a street name - and we need to trim all leading and trailing spaces.
            if (String.IsNullOrEmpty(street.StreetName) || string.IsNullOrEmpty(street.StreetName.Trim()))
            {
                string errorMessage = "Please enter a valid street name.";
                AddMessage(errorMessage, errorMessage, Messages);
                return 1;
            }
            return 0;
        }
    }

}
