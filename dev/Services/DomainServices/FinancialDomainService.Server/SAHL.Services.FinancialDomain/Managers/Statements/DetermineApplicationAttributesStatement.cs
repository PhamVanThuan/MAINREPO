using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Services.FinancialDomain.Managers.Models;

namespace SAHL.Services.FinancialDomain.Managers.Statements
{
    public class DetermineApplicationAttributesStatement : ISqlStatement<GetOfferAttributesModel>
    {
        public decimal LTV { get; protected set; }

        public EmploymentType EmploymentType { get; protected set; }

        public decimal HouseHoldIncome { get; protected set; }

        public bool IsStaffLoan { get; protected set; }

        public bool IsGEPF { get; protected set; }

        public int ApplicationNumber { get; protected set; }

        public DetermineApplicationAttributesStatement(int applicationNumber, decimal ltv, EmploymentType employmentType, decimal householdIncome, bool isStaffLoan, bool isGEPF)
        {
            this.ApplicationNumber = applicationNumber;
            this.LTV = ltv;
            this.EmploymentType = employmentType;
            this.HouseHoldIncome = householdIncome;
            this.IsStaffLoan = isStaffLoan;
            this.IsGEPF = isGEPF;
        }

        public string GetStatement()
        {
            return @"select OfferAttributeTypeKey,
                            Remove
                    from [2AM].dbo.[GetOfferAttributes](@LTV, @EmploymentType, @HouseholdIncome, @IsStaffLoan, @isGEPF, @ApplicationNumber)";
        }
    }
}