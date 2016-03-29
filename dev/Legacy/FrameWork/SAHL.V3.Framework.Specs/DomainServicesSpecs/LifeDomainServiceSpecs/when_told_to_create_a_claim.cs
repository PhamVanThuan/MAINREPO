using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.Services.Interfaces.MortgageLoanDomain.Model;
using SAHL.V3.Framework.DomainServices;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Specs.DomainServicesSpecs.LifeDomainServiceSpecs
{
    public class when_told_to_create_a_claim : WithFakes
    {
        private static AutoMocker<LifeDomainService> autoMocker = new NSubstituteAutoMocker<LifeDomainService>();
        private static int accountLifePolicyKey;
        private static int legalEntityKey;
        private static long instanceId = 0;
        private static bool actualResult;
        private static bool expectedResult;


        Establish context = () =>
            {
                expectedResult = false;
                autoMocker.Get<ILifeDomainServiceClient>().WhenToldTo(x => x.PerformCommand(Param.IsAny<LodgeDisabilityClaimCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(new SystemMessageCollection());

                var serviceQueryResult = new ServiceQueryResult<DisabilityClaimModel>(new DisabilityClaimModel[]
                {
                    new DisabilityClaimModel(1,1,1,DateTime.Now,DateTime.Now,DateTime.Now,"test",1,"test",DateTime.Now,1,DateTime.Now,1,DateTime.Now)
                });

                autoMocker.Get<ILifeDomainServiceClient>().WhenToldTo(x => x.PerformQuery(Param.IsAny<GetPendingDisabilityClaimByAccountQuery>())).Return(new SystemMessageCollection());

                autoMocker.Get<ILifeDomainServiceClient>().WhenToldTo<ILifeDomainServiceClient>(x => x.PerformQuery(Param.IsAny<GetPendingDisabilityClaimByAccountQuery>())).Callback<GetPendingDisabilityClaimByAccountQuery>(y => { y.Result = serviceQueryResult; });

            };

        Because of = () =>
            {
                actualResult = autoMocker.ClassUnderTest.CreateClaim(accountLifePolicyKey, legalEntityKey, out instanceId);
            };

        It should_return_a_successful_result = () =>
            {
                actualResult.ShouldEqual(expectedResult);
            };


    }
}
