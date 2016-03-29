using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.V3.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Specs.ModelSpecs.QualifyApplicationFor30YearLoanTermQuerySpecs
{
    public class when_creating_a_30_year_term_query : WithFakes
    {
        private static QualifyApplicationFor30YearLoanTermQuery queryModel;
        private static QualifyApplicationFor30YearLoanTermQuery defaultModel;
        Establish context = () =>
            {
                defaultModel = Factory.GetDefaultQualifyApplicationFor30YearLoanTermQueryModel();
            };
        Because of = () =>
            {
                queryModel = new QualifyApplicationFor30YearLoanTermQuery(defaultModel.DisqualifiedByCredit, defaultModel.LTV, defaultModel.PTI, defaultModel.HouseholdIncome, defaultModel.MarketRate, defaultModel.TotalRateAdjustments, defaultModel.LinkRate, defaultModel.LoanAmount, Factory.GetDefaultOfferType(), Factory.GetDefaultProduct(), defaultModel.PropertyValue, defaultModel.IsAlphaHousingApplication, defaultModel.HighestIncomeContributor,defaultModel.IsInterestOnly);
            };
        It should_set_the_properties = () =>
        {
            queryModel.DisqualifiedByCredit.ShouldEqual(defaultModel.DisqualifiedByCredit);
            queryModel.LTV.ShouldEqual(defaultModel.LTV);
            queryModel.PTI.ShouldEqual(defaultModel.PTI);
            queryModel.HouseholdIncome.ShouldEqual(defaultModel.HouseholdIncome);
            queryModel.MarketRate.ShouldEqual(defaultModel.MarketRate);
            queryModel.TotalRateAdjustments.ShouldEqual(defaultModel.TotalRateAdjustments);
            queryModel.LinkRate.ShouldEqual(defaultModel.LinkRate);
            queryModel.LoanAmount.ShouldEqual(defaultModel.LoanAmount);
            queryModel.ApplicationType.ShouldEqual(defaultModel.ApplicationType);
            queryModel.Product.ShouldEqual(defaultModel.Product);
            queryModel.PropertyValue.ShouldEqual(defaultModel.PropertyValue);
            queryModel.IsAlphaHousingApplication.ShouldEqual(defaultModel.IsAlphaHousingApplication);
            queryModel.HighestIncomeContributor.ShouldEqual(defaultModel.HighestIncomeContributor);
            queryModel.EffectiveRate.ShouldEqual(defaultModel.EffectiveRate);
            queryModel.IsInterestOnly.ShouldEqual(defaultModel.IsInterestOnly); 
        };
    }
}
