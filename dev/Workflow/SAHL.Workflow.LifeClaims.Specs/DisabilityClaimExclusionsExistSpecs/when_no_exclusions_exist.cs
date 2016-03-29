using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.Workflow.LifeClaims.Specs.Factories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Workflow.LifeClaims.Specs.DisabilityClaimExclusionsExistSpecs
{
    public class when_no_exclusions_exist : WithFakes
    {
        private static ICreateDisabilityClaimDomainProcess domainProcess;
        private static ILifeDomainServiceClient lifeDomainServiceClient;
        private static ISystemMessageCollection messages;
        private static bool exclusionsExist;


        Establish context = () =>
            {
                messages = new SystemMessageCollection();
                lifeDomainServiceClient = An<ILifeDomainServiceClient>();
                domainProcess = new CreateDisabilityClaimDomainProcess(lifeDomainServiceClient);

                IEnumerable<DisabilityClaimFurtherLendingExclusionModel> disabilityExclusions = new List<DisabilityClaimFurtherLendingExclusionModel>();
                var exclusionsServiceQueryResult = new ServiceQueryResult<DisabilityClaimFurtherLendingExclusionModel>(disabilityExclusions);
                lifeDomainServiceClient.WhenToldTo<ILifeDomainServiceClient>(x => x.PerformQuery(Param.IsAny<GetFurtherLendingExclusionsByDisabilityClaimKeyQuery>())).Callback<GetFurtherLendingExclusionsByDisabilityClaimKeyQuery>(y => { y.Result = exclusionsServiceQueryResult; });

            };

        Because of = () =>
            {
                exclusionsExist = domainProcess.CheckIfDisabilityClaimExclusionsExist(new SystemMessageCollection(), 1);
            };

        It should_assert_ = () =>
            {
                exclusionsExist.ShouldBeFalse();
            };
    }
}
