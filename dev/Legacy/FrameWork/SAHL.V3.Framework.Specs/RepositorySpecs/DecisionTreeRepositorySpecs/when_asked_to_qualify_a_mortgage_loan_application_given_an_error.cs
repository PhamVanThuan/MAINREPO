using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.V3.Framework.Model;
using SAHL.V3.Framework.Repositories;
using SAHL.V3.Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Specs.RepositorySpecs.DecisionTreeRepositorySpecs
{

    public class when_asked_to_qualify_a_mortgage_loan_application_given_an_error : BaseSpec
    {
        private static Exception exception;

        Establish context = () =>
        {
            decisionTreeService.WhenToldTo<IDecisionTreeService>(x => x.QualifyApplicationFor30YearLoanTerm(Param.IsAny<QualifyApplicationFor30YearLoanTermQuery>())).Callback<QualifyApplicationFor30YearLoanTermQuery>(y =>
            {
                throw new Exception("general infrastrcture exception");
            });
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => result = decisionTreeRepo.QualifyApplication(application.Key));
        };

        It should_result_in_an_exception = () =>
        {
            exception.ShouldNotBeNull();
        };
    }
}
