using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.V3.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Specs.ModelSpecs.QualifyHighestIncomeContributorQuerySpecs
{
    public class when_creating_a_highest_income_contributor : WithFakes
    {
        private static QualifyHighestIncomeContributorFor30YearLoanTermQuery highestIncomeContributorModel;
        private static QualifyHighestIncomeContributorFor30YearLoanTermQuery defaultModel;
        Establish context = () =>
            {
                defaultModel = Factory.GetDefaultQualifyHighestIncomeContributorFor30YearLoanTermQueryModel();
            };
        Because of = () =>
            {
                highestIncomeContributorModel = new QualifyHighestIncomeContributorFor30YearLoanTermQuery(defaultModel.Age, defaultModel.CreditScore, defaultModel.IdNumber, defaultModel.FullName, Factory.GetDefaultEmploymentType());
            };
        It should_set_the_properties = () =>
        {
            highestIncomeContributorModel.Age.ShouldEqual(defaultModel.Age);
            highestIncomeContributorModel.CreditScore.ShouldEqual(defaultModel.CreditScore);
            highestIncomeContributorModel.FullName.ShouldEqual(defaultModel.FullName);
            highestIncomeContributorModel.IdNumber.ShouldEqual(defaultModel.IdNumber);
            highestIncomeContributorModel.SalaryType.ShouldEqual(defaultModel.SalaryType);
        };
    }
}
