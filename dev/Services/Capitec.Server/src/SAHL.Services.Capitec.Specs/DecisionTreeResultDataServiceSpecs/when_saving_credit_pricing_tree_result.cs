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
    public class when_saving_credit_pricing_tree_result : WithFakes
    {
        static FakeDbFactory dbFactory;
        private static DecisionTreeResultDataManager service;
        private static Guid decisionTreeResultID, applicationID;
        private static string decisionTreeQuery;
        private static string decisionTreeMessages;
        private static DateTime queryDate;

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            service = new DecisionTreeResultDataManager(dbFactory);

            decisionTreeResultID = Guid.NewGuid();
            applicationID = Guid.NewGuid();
            queryDate = DateTime.Now;
        };
        Because of = () =>
        {
            service.SaveCreditPricingTreeResult(decisionTreeResultID, decisionTreeQuery, decisionTreeMessages, applicationID, queryDate);
        };
        It should_insert_the_credit_pricing_tree_result = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<CreditPricingTreeResultDataModel>(
                y => y.Id == decisionTreeResultID
                && y.Messages == decisionTreeMessages
                && y.QueryDate == queryDate
                && y.ApplicationID == applicationID
                && y.TreeQuery == decisionTreeQuery)));
        };
    }
}
