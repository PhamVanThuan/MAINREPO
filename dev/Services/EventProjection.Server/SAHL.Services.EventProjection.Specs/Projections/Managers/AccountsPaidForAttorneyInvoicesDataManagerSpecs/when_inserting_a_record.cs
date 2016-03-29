﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly;
using SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly.Statements;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.Managers.AccountsPaidForAttorneyInvoicesDataManagerSpecs
{
    public class when_inserting_a_record : WithFakes
    {
        private static AccountsPaidForAttorneyInvoicesDataManager dataManager;
        private static FakeDbFactory fakeDbFactory;
        private static Guid attorneyId;
        private static int thirdPartyInvoiceKey;
        private static int accountKey;

        private Establish context = () =>
        {
            attorneyId = Guid.NewGuid();
            thirdPartyInvoiceKey = 123;
            accountKey = 1408282;
            fakeDbFactory = new FakeDbFactory();
            dataManager = new AccountsPaidForAttorneyInvoicesDataManager(fakeDbFactory);
        };

        private Because of = () =>
        {
            dataManager.InsertRecord(attorneyId, thirdPartyInvoiceKey, accountKey);
        };

        private It should_use_the_statement_to_adjust_to_the_value_provided = () =>
        {
            fakeDbFactory.FakedDb.InAppContext().WasToldTo(x => x.ExecuteNonQuery(Param<InsertAccountsPaidForAttorneyInvoicesMonthlyStatement>
                .Matches(y => y.ThirdPartyId == attorneyId && y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey && y.AccountKey == accountKey)));
        };

        private It should_have_performed_a_complete_on_the_context = () =>
        {
            fakeDbFactory.NewDb().InAppContext().WasToldTo(a => a.Complete()).OnlyOnce();
        };
    }
}