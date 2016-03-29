using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Workflow.CreateInstanceV3.Specs.CreateCaseSpecs
{
    public class when_told_create_case : WithFakes
    {
        private static ICreateInstanceV3DomainProcess domainProcess;
        private static IApplicationDomainServiceClient appDomainServiceClient;
        private static ISystemMessageCollection messages;
        private static Boolean result;

        Establish context = () =>
        {
            appDomainServiceClient = An<IApplicationDomainServiceClient>();
            appDomainServiceClient.WhenToldTo(x => x.PerformCommand(Param.IsAny<IApplicationDomainCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(new SystemMessageCollection());
            domainProcess = new CreateInstanceV3DomainProcess(appDomainServiceClient);
            messages = new SystemMessageCollection();
        };

        Because of = () =>
        {
            result = domainProcess.CreateCase(messages);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
