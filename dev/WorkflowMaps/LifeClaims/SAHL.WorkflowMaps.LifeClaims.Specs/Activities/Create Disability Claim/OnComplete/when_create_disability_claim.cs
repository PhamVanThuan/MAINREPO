using Machine.Specifications;
using Machine.Fakes;
using SAHL.Workflow.LifeClaims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.SystemMessages;

namespace SAHL.WorkflowMaps.LifeClaims.Specs.Activities.Create_Disability_Claim.OnComplete
{
    public class when_create_disability_claim : WorkflowSpecDisabilityClaim
    {
        private static bool result;
        private static ICreateDisabilityClaimDomainProcess createDisabilityClaimDomainProcess;

        Establish context = () =>
        {
            result = false;
            createDisabilityClaimDomainProcess = An<ICreateDisabilityClaimDomainProcess>();
            domainServiceLoader.RegisterMockForType<ICreateDisabilityClaimDomainProcess>(createDisabilityClaimDomainProcess);
            createDisabilityClaimDomainProcess.WhenToldTo(x => x.GetDisabilityClaimInstanceSubject(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<int>())).Return("test");
        };

        Because of = () =>
        {
            try
            {
                string message = string.Empty;
                result = workflow.OnCompleteActivity_Create_Disability_Claim(instanceData, workflowData, paramsData, messages, ref message);

            }
            catch (Exception)
            {
                
                throw;
            }
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
