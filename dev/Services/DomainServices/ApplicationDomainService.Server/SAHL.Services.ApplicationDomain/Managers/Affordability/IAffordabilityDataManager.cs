using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Managers.Affordability
{
    public interface IAffordabilityDataManager
    {
        void SaveAffordability(AffordabilityTypeModel affordabilityTypeModel, int clientKey, int applicationNumber);

        bool IsDescriptionRequired(AffordabilityType affordabilityType);

        IEnumerable<LegalEntityAffordabilityDataModel> GetAffordabilityAssessment(int clientKey, int applicationNumber);
    }
}