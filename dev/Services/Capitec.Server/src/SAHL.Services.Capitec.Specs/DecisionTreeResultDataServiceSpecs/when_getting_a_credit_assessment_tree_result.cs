using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Services.Capitec.Managers.DecisionTreeResult;
using SAHL.Services.Capitec.Managers.DecisionTreeResult.Statements;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.Services.Capitec.Specs.DecisionTreeResultDataServiceSpecs
{
    public class when_getting_a_credit_assessment_tree_result : WithFakes
    {
        static FakeDbFactory dbFactory;
        private static DecisionTreeResultDataManager service;
        private static Guid decisionTreeResultID, applicantID;
        private static string decisionTreeQuery;
        private static string decisionTreeMessages;
        private static DateTime queryDate;

        private static CreditAssessmentTreeResultDataModel model;

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            service = new DecisionTreeResultDataManager(dbFactory);

            decisionTreeResultID = Guid.NewGuid();
            decisionTreeQuery = "query";
            decisionTreeMessages = "messages";
            applicantID = Guid.NewGuid();
            queryDate = DateTime.Now;

            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x=>x.SelectOne(Param.IsAny<GetCreditAssessmentForApplicantQuery>())).Return(new CreditAssessmentTreeResultDataModel(decisionTreeResultID, decisionTreeMessages, decisionTreeQuery, applicantID, queryDate));
        };
        Because of = () =>
        {
            model = service.GetCreditBureauAssessmentTreeResultForApplicant(applicantID);
        };
        It should_return_the_credit_assessment_tree_result = () =>
        {
            model.Id.ShouldEqual(decisionTreeResultID);
            model.TreeQuery.ShouldEqual(decisionTreeQuery);
            model.Messages.ShouldEqual(decisionTreeMessages);
        };
    }
}
