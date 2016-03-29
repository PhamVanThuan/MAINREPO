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
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.Services.Capitec.Specs.DecisionTreeResultDataServiceSpecs
{
    public class when_saving_credit_assessment_tree_result : WithFakes
    {
        static FakeDbFactory dbFactory;
        private static DecisionTreeResultDataManager service;
        private static Guid decisionTreeResultID, applicantID;
        private static string decisionTreeQuery;
        private static string decisionTreeMessages;
        private static DateTime queryDate;

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            service = new DecisionTreeResultDataManager(dbFactory);

            decisionTreeResultID = Guid.NewGuid();
            applicantID = Guid.NewGuid();
            queryDate = DateTime.Now;
        };
        Because of = () =>
        {
            service.SaveCreditAssessmentTreeResult(decisionTreeResultID, decisionTreeQuery, decisionTreeMessages, applicantID, queryDate);
        };
        It should_insert_the_credit_assessment_tree_result = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<CreditAssessmentTreeResultDataModel>(
                y => y.Id == decisionTreeResultID
                && y.Messages == decisionTreeMessages
                && y.QueryDate == queryDate
                && y.ApplicantID == applicantID
                && y.TreeQuery == decisionTreeQuery)));
        };
    }
}
