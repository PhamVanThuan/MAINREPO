using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using SAHL.V3.Framework.Model;
using SAHL.V3.Framework.Services;
using SAHL.V3.Framework.Specs.ModelSpecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Specs.ServicesSpecs.DecisionTreeSpecs
{
    public class when_failing_to_qualify_for_a_30_year_term : WithFakes
    {
        private static IDecisionTreeServiceClient decisionTreeServiceClient;
        private static IDecisionTreeService decisionTreeService;
        private static ISystemMessageCollection messages;
        private static QualifyApplicationFor30YearLoanTermQuery queryModel;

        Establish context = () =>
        {
            decisionTreeServiceClient = An<IDecisionTreeServiceClient>();

            queryModel = Factory.GetDefaultQualifyApplicationFor30YearLoanTermQueryModel();

            var thirtyYearMortgageLoanEligibility_QueryResult = new ThirtyYearMortgageLoanEligibility_QueryResult[] { Factory.GetFailedDecisionTreeThirtyYearMortgageLoanEligibility_QueryResult() };
            IServiceQueryResult<ThirtyYearMortgageLoanEligibility_QueryResult> result = new ServiceQueryResult<ThirtyYearMortgageLoanEligibility_QueryResult>(thirtyYearMortgageLoanEligibility_QueryResult);

            messages = SystemMessageCollection.Empty();
            messages.AddMessage(new SystemMessage("warning", SystemMessageSeverityEnum.Warning));

            decisionTreeServiceClient.WhenToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param.IsAny<ThirtyYearMortgageLoanEligibility_Query>())).Callback<ThirtyYearMortgageLoanEligibility_Query>(
                y =>
                {
                    y.Result = result;
                });
            decisionTreeServiceClient.WhenToldTo(x => x.PerformQuery(Param.IsAny<ThirtyYearMortgageLoanEligibility_Query>())).Return(messages);

            decisionTreeService = new DecisionTreeService(decisionTreeServiceClient);
        };

        Because of = () =>
        {
            decisionTreeService.QualifyApplicationFor30YearLoanTerm(queryModel);
        };

        It should_return_warnings = () =>
        {
            queryModel.Messages.HasWarnings.ShouldBeTrue();
            queryModel.Messages.WarningMessages().Count().ShouldBeGreaterThan(0);
        };

        It should_have_an_unsuccessful_result = () =>
        {
            queryModel.Result.QualifiesForThirtyYearLoanTerm.ShouldBeFalse();

            queryModel.Result.InstalmentThirtyYear.ShouldEqual(-1);
            queryModel.Result.InterestRateThirtyYear.ShouldEqual(-1);
            queryModel.Result.LoantoValueThirtyYear.ShouldEqual(-1);
            queryModel.Result.PaymenttoIncomeThirtyYear.ShouldEqual(-1);
            queryModel.Result.PricingAdjustmentThirtyYear.ShouldEqual(-1);
        };
        It should_have_the_messages_that_were_added = () =>
        {
            queryModel.Messages.AllMessages.Count().ShouldEqual(messages.AllMessages.Count());
        };
    }
}
