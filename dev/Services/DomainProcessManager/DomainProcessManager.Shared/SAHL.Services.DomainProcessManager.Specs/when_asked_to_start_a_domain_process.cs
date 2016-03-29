using System;
using System.Threading.Tasks;

using Machine.Fakes;
using Machine.Specifications;

using NSubstitute;

using SAHL.Core.Data;
using SAHL.Services.Interfaces.DomainProcessManager;
using SAHL.Config.Services.DomainProcessManager.Client;

namespace SAHL.Services.DomainProcessManager.Specs
{
    public class when_asked_to_start_a_domain_process : WithFakes
    {
        private static IStartDomainProcessResponse startResponse;
        private static IDomainProcessManagerClient domainProcessManagerService;
        private static ReturnData returnData;
        private static IDomainProcessManagerClientApiFactory factory;

        private Establish context = () =>
        {
            returnData = new ReturnData(Guid.NewGuid());

            domainProcessManagerService = An<IDomainProcessManagerClient>();
            domainProcessManagerService.WhenToldTo(x => x.StartDomainProcess(Arg.Any<StartDomainProcessCommand>())).Return(() =>
                                                                {
                                                                    var taskCompletionSource = new TaskCompletionSource<IStartDomainProcessResponse>();
                                                                    var response = new StartDomainProcessResponse(true, returnData);
                                                                    taskCompletionSource.SetResult(response);
                                                                    return taskCompletionSource.Task;
                                                                });

            factory = new DomainProcessManagerClientApiFactory(domainProcessManagerService);
        };

        private Because of = () =>
        {
            var domainModel = new FakeDomainModel();
            startResponse = factory.Create()
                                        .DataModel(domainModel)
                                        .EventToWaitFor(typeof(FakeEvent).Name)
                                        .StartProcess().Result;
        };

        private It should_return_a_success_result = () =>
        {
            startResponse.Result.ShouldBeTrue();
        };

        private It should_return_a_reference_key = () =>
        {
            startResponse.Data.ShouldNotBeNull();
            startResponse.Data.ShouldBeTheSameAs(returnData);
        };

        private class ReturnData : IDataModel
        {
            public ReturnData(Guid referenceKey)
            {
                this.ReferenceKey = referenceKey;
            }

            public Guid ReferenceKey { get; private set; }
        }
    }
}
