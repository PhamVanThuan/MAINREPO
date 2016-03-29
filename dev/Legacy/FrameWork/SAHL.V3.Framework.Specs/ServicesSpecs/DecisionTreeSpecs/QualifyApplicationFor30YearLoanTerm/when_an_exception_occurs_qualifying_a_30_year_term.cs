using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.CacheData;
using SAHL.Common.Security;
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
    public class when_an_exception_occurs_qualifying_a_30_year_term : WithFakes
    {
        private static IDecisionTreeServiceClient decisionTreeServiceClient;
        private static IDecisionTreeService decisionTreeService;
        private static QualifyApplicationFor30YearLoanTermQuery queryModel;

        private static Exception exception;

        Establish context = () =>
        {
            queryModel = Factory.GetDefaultQualifyApplicationFor30YearLoanTermQueryModel();

            decisionTreeServiceClient = An<IDecisionTreeServiceClient>();

            decisionTreeServiceClient.WhenToldTo(x => x.PerformQuery(Param.IsAny<ThirtyYearMortgageLoanEligibility_Query>())).Throw(new NullReferenceException());

            decisionTreeService = new DecisionTreeService(decisionTreeServiceClient);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() =>
            {
                decisionTreeService.QualifyApplicationFor30YearLoanTerm(queryModel);
            });
        };

        It should_catch_the_exception_and_continue_evaluating_the_response = () =>
        {
            exception.ShouldNotBeNull();
        };

        It should_have_the_exception_messages_in_the_principal_cache = () =>
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Count().ShouldEqual(2);
        };
    }
}
