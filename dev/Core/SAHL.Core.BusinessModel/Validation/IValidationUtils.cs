using SAHL.Core.BusinessModel.Enums;
using System;
using System.Linq;

namespace SAHL.Core.BusinessModel.Validation
{
    public interface IValidationUtils
    {
        bool ValidateIDNumber(string idNumber);

        int GetAgeFromDateOfBirth(DateTime dateOfBirth);

        bool ValidatePassportNumber(string passportNumber);

        T ParseEnum<T>(string value);

        string MapComcorpToSAHLProvince(string comcorpProvince);

        bool CheckIfAffordabilityRequiresDescription(AffordabilityType affordabilityType);
    }
}