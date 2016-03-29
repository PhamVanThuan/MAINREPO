using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.MortgageLoanDomain;
using SAHL.Services.Interfaces.MortgageLoanDomain.Model;
using SAHL.Services.Interfaces.MortgageLoanDomain.Queries;
using SAHL.V3.Framework.DomainServices;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Specs.DomainServicesSpecs.MortgageLoanDomainServiceSpecs
{
    public class when_asking_for_the_debit_order_day_given_a_mortgageloanaccount : WithFakes
    {
        private static AutoMocker<MortgageLoanDomainService> autoMocker = new NSubstituteAutoMocker<MortgageLoanDomainService>();
        private static int accountKey = 1;
        private static int expectedResult = 25;
        private static int actualResult = 0;

        Establish context = () =>
            {
                IMortgageLoanDomainServiceClient client = An<IMortgageLoanDomainServiceClient>();
                autoMocker.Inject(client);
                var serviceQueryResult = new ServiceQueryResult<GetDebitOrderDayByAccountQueryResult>(new GetDebitOrderDayByAccountQueryResult[]
                {
                    new GetDebitOrderDayByAccountQueryResult(expectedResult)
                });

                client.WhenToldTo<IMortgageLoanDomainServiceClient>(x => x.PerformQuery(Param.IsAny<GetDebitOrderDayByAccountQuery>())).Callback<GetDebitOrderDayByAccountQuery>(y => { y.Result = serviceQueryResult; });
            };

        Because of = () =>
            {
                actualResult = autoMocker.ClassUnderTest.GetDebitOrderDayByAccount(accountKey);
            };

        It should_return_the_correct_debit_order_day = () =>
            {
                actualResult.ShouldEqual(expectedResult);
            };
    }
}
