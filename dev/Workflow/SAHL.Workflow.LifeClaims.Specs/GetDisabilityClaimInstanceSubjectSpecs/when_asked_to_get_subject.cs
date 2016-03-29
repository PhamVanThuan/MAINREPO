using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Workflow.LifeClaims.Specs.GetDisabilityClaimInstanceSubjectSpecs
{
    public class when_asked_to_get_subject : WithFakes
    {
        private static ICreateDisabilityClaimDomainProcess domainProcess;
        private static ILifeDomainServiceClient lifeDomainServiceClient;
        private static ISystemMessageCollection messages;
        private static string result;


        Establish context = () =>
            {
                lifeDomainServiceClient = An<ILifeDomainServiceClient>();

                var serviceQueryResult = new ServiceQueryResult<string>(new string[]
                {
                    "test"
                });

                lifeDomainServiceClient.WhenToldTo<ILifeDomainServiceClient>(x => x.PerformQuery(Param.IsAny<GetDisabilityClaimInstanceSubjectQuery>())).Callback<GetDisabilityClaimInstanceSubjectQuery>(y => { y.Result = serviceQueryResult; });
                domainProcess = new CreateDisabilityClaimDomainProcess(lifeDomainServiceClient);
                messages = new SystemMessageCollection();
            };

        Because of = () =>
            {
                result = domainProcess.GetDisabilityClaimInstanceSubject(new SystemMessageCollection(), 1);
            };

        It should_assert_ = () =>
            {
                result.ShouldNotBeNull();
            };


    }
}
