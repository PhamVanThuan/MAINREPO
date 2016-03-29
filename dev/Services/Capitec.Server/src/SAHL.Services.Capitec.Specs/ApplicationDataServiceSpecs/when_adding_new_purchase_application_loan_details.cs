using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Specs.ApplicationDataServiceSpecs
{
    public class when_adding_new_purchase_application_loan_details : WithFakes
    {
        static ApplicationDataManager service;
        static Guid applicationLoanDetailId;
        static decimal purchasePrice, deposit;
        private static FakeDbFactory dbFactory;
       
        Establish context = () =>
            {
                dbFactory = new FakeDbFactory();

                service = new ApplicationDataManager(dbFactory);
                applicationLoanDetailId = Guid.NewGuid();
                purchasePrice = 500000M;
                deposit = 100000M;
            };

        Because of = () =>
            {
                service.AddNewPurchaseApplicationLoanDetail(applicationLoanDetailId, purchasePrice, deposit);
            };

        It should_insert_new_purchase_loan_details_using_the_details_provided = () =>
            {
                dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<NewPurchaseApplicationLoanDetailDataModel>(
                        y=>y.Deposit == deposit
                        && y.PurchasePrice == purchasePrice
                        && y.Id == applicationLoanDetailId
                )));
            };
    }
}
