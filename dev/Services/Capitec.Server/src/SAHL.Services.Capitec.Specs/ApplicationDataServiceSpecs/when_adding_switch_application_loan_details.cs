using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Application;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicationDataServiceSpecs
{
    public class when_adding_switch_application_loan_details : WithFakes
    {
        static ApplicationDataManager service;
        static Guid applicationLoanDetailId;
        static decimal cashRequired, currentBalance, estMarketValue, interimInterest;
        private static FakeDbFactory dbFactory;

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            service = new ApplicationDataManager(dbFactory);
            cashRequired = 150000M;
            currentBalance = 200000M;
            estMarketValue = 1000000M;
            interimInterest = 12000M;
            applicationLoanDetailId = Guid.NewGuid();
        };

        Because of = () =>
        {
            service.AddSwitchApplicationLoanDetail(applicationLoanDetailId, cashRequired, currentBalance, estMarketValue, interimInterest);
        };

        It should_insert_switch_application_loan_details_with_the_details_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x=>x.Insert(Arg.Is<SwitchApplicationLoanDetailDataModel>(
                y => y.CashRequired == cashRequired
                && y.CurrentBalance == currentBalance
                && y.EstimatedMarketValueOfTheHome == estMarketValue
                && y.InterimInterest == interimInterest
                && y.Id == applicationLoanDetailId
            )));
        };
    }
}
