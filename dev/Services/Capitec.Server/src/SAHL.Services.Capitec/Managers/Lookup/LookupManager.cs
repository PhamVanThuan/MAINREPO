using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Identity;
using SAHL.DecisionTree.Shared.Globals;
using SAHL.Services.Capitec.Managers.CapitecApplication.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.Capitec.Managers.Lookup
{
    public class LookupManager : ILookupManager
    {
        private readonly Dictionary<Guid, int> capitecToSahlLookupKeyMapping;

        private IDbFactory dbFactory;

        public LookupManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;

            using (IReadOnlyDbContext db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                GetCapitecGuidToSAHL_KeyMappingQuery getCapitecGuidToSAHL_KeyMappingQuery = new GetCapitecGuidToSAHL_KeyMappingQuery();
                capitecToSahlLookupKeyMapping = db.Select(getCapitecGuidToSAHL_KeyMappingQuery).ToDictionary(item => item.CapitecKey, item => item.SAHL_Key);
            }
        }

        public Guid GenerateCombGuid()
        {
            return CombGuid.Instance.Generate();
        }

        public int GetSahlKeyByCapitecGuid(Guid capitecGuid)
        {
            int sahlKey = 0;

            capitecToSahlLookupKeyMapping.TryGetValue(capitecGuid, out sahlKey);

            return sahlKey;
        }

        public bool GetDeclarationResultByCapitecGuid(Guid capitecGuid)
        {
            if (DeclarationTypeEnumDataModel.YES == capitecGuid.ToString())
                return true;
            else
                return false;
        }

        public string GetDecisionTreeHouseholdIncomeType(Guid householdIncomeTypeId)
        {
            var householdIncomeType = new Enumerations.SAHomeLoans.HouseholdIncomeType();
            if (householdIncomeTypeId == Guid.Parse(EmploymentTypeEnumDataModel.SALARIED))
                return householdIncomeType.Salaried;
            if (householdIncomeTypeId == Guid.Parse(EmploymentTypeEnumDataModel.SELF_EMPLOYED))
                return householdIncomeType.SelfEmployed;
            if (householdIncomeTypeId == Guid.Parse(EmploymentTypeEnumDataModel.SALARIED_WITH_HOUSING_ALLOWANCE))
                return householdIncomeType.SalariedwithDeduction;
            if (householdIncomeTypeId == Guid.Parse(EmploymentTypeEnumDataModel.SALARIED_WITH_COMMISSION))
                return householdIncomeType.Salaried;
            if (householdIncomeTypeId == Guid.Parse(EmploymentTypeEnumDataModel.UNEMPLOYED))
                return householdIncomeType.Unemployed;
            else
                throw new NotSupportedException();
        }

        public string GetDecisionTreeOccupancyType(Guid occupancyTypeId)
        {
            var occupancyType = new Enumerations.SAHomeLoans.PropertyOccupancyType();
            if (occupancyTypeId == Guid.Parse(OccupancyTypeEnumDataModel.INVESTMENT_PROPERTY))
                return occupancyType.InvestmentProperty;
            else if (occupancyTypeId == Guid.Parse(OccupancyTypeEnumDataModel.OWNER_OCCUPIED))
                return occupancyType.OwnerOccupied;
            else
                throw new NotSupportedException();
        }
    }
}