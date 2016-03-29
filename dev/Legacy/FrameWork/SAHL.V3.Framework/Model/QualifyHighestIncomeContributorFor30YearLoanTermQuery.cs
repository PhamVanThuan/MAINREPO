using SAHL.Common.Globals;
using SAHL.DecisionTree.Shared.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Model
{
    public class QualifyHighestIncomeContributorFor30YearLoanTermQuery
    {
        public QualifyHighestIncomeContributorFor30YearLoanTermQuery(int age, int creditScore, string idNumber, string fullName, EmploymentTypes salaryType)
        {
            this.Age = age;
            this.CreditScore = creditScore;
            this.FullName = fullName;
            this.IdNumber = idNumber;
            this.SalaryType = GetDecisionTreeEnum(salaryType);
        }

        private string GetDecisionTreeEnum(EmploymentTypes employmentType)
        {
            string result = String.Empty;
            
            switch (employmentType)
            {
                case EmploymentTypes.Salaried:
                    result = new Enumerations.SAHomeLoans.HouseholdIncomeType().Salaried;
                    break;
                case EmploymentTypes.SelfEmployed:
                    result = new Enumerations.SAHomeLoans.HouseholdIncomeType().SelfEmployed;
                    break;
                case EmploymentTypes.SalariedwithDeduction:
                    result = new Enumerations.SAHomeLoans.HouseholdIncomeType().SalariedwithDeduction;
                    break;
                case EmploymentTypes.Unemployed:
                    result = new Enumerations.SAHomeLoans.HouseholdIncomeType().Unemployed;
                    break;
                default:
                    result = String.Empty;
                    break;
            }

            return result;
        }

        public int Age { get; set; }
        public int CreditScore { get; set; }
        public string IdNumber { get; set; }
        public string FullName { get; set; }
        public string SalaryType { get; set; }
    }
}
