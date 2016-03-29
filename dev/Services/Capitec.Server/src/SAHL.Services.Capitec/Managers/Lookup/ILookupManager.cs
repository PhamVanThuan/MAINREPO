using System;

namespace SAHL.Services.Capitec.Managers.Lookup
{
    public interface ILookupManager
    {
        Guid GenerateCombGuid();

        int GetSahlKeyByCapitecGuid(Guid capitecGuid);

        string GetDecisionTreeHouseholdIncomeType(Guid householdIncomeTypeId);

        string GetDecisionTreeOccupancyType(Guid occupancyTypeId);

        bool GetDeclarationResultByCapitecGuid(Guid capitecGuid);
    }
}