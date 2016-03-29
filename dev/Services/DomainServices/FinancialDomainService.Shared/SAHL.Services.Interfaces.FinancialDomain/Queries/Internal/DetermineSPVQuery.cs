using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.FinancialDomain.Commands.Internal
{
    public class DetermineSPVQuery : ServiceQuery<int>, IFinancialDomainQuery
    {
        public DetermineSPVQuery(int applicationNumber, EmploymentType employmentType, decimal householdIncome, bool isStaffLoan, decimal ltv, bool isGEPF)
        {
            this.ApplicationNumber = applicationNumber;
            this.EmploymentType = employmentType;
            this.HouseholdIncome = householdIncome;
            this.IsStaffLoan = isStaffLoan;
            this.LTV = ltv;
            this.IsGEPF = isGEPF;
        }

        public int ApplicationNumber { get; protected set; }

        public SAHL.Core.BusinessModel.Enums.EmploymentType EmploymentType { get; protected set; }

        public decimal HouseholdIncome { get; protected set; }

        public bool IsStaffLoan { get; protected set; }

        public bool IsGEPF { get; protected set; }

        public decimal LTV { get; protected set; }
    }
}