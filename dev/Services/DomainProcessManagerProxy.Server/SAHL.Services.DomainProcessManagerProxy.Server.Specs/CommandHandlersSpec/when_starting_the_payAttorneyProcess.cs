using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DomainProcessManagerProxy.CommandHandlers;
using SAHL.Services.DomainProcessManagerProxy.DpmServiceReference;
using SAHL.Services.Interfaces.DomainProcessManagerProxy.Commands;
using SAHL.Services.Interfaces.DomainProcessManagerProxy.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAHL.Services.DomainProcessManagerProxy.Server.Specs.CommandHandlersSpec
{
    public class when_starting_the_payAttorneyProcess : WithFakes
    {
        static IDomainProcessManagerService domainProcessManagerServiceClient;
        static StartPayAttorneyProcessCommand command;
        static StartPayAttorneyProcessCommandHandler handler;
        static IEnumerable<ThirdPartyPaymentModel> thirdPartyPayments;
        static ISystemMessageCollection messages;

        static IServiceRequestMetadata serviceRequestMetadata;

        Establish context = () =>
        {
            serviceRequestMetadata = An<IServiceRequestMetadata>();
            serviceRequestMetadata.WhenToldTo(x => x.UserName).Return(@"SAHL\ClintonS");
            domainProcessManagerServiceClient = An<IDomainProcessManagerService>();
         
            handler = new StartPayAttorneyProcessCommandHandler(domainProcessManagerServiceClient);
            thirdPartyPayments = new List<ThirdPartyPaymentModel> {
                new ThirdPartyPaymentModel(124, 108009, 15340,"sahl-reference")
            };
            command = new StartPayAttorneyProcessCommand(thirdPartyPayments);

            var domainProcessResponse = new Task<StartDomainProcessResponse>(() =>
            {
                return new StartDomainProcessResponse();
            });

            domainProcessManagerServiceClient.WhenToldTo(x => x.StartDomainProcessAsync(
                Param<StartDomainProcessCommand>.Matches(p =>
                    (p.DataModel as PayThirdPartyInvoiceProcessModel).InvoiceCollection.First().AccountNumber == thirdPartyPayments.First().AccountNumber
                    && (p.DataModel as PayThirdPartyInvoiceProcessModel).InvoiceCollection.First().ThirdPartyInvoiceKey == thirdPartyPayments.First().ThirdPartyInvoiceKey
                    && (p.DataModel as PayThirdPartyInvoiceProcessModel).InvoiceCollection.First().InstanceId == thirdPartyPayments.First().InstanceId
                    && (p.DataModel as PayThirdPartyInvoiceProcessModel).InvoiceCollection.First().SAHLReference == thirdPartyPayments.First().SAHLReference)
              )).Return(
                domainProcessResponse
              );
        };

        Because of = () =>
        {

            messages = handler.HandleCommand(command, serviceRequestMetadata);

        };

        It should_start_the_payment_process = () =>
        {
            domainProcessManagerServiceClient.WasToldTo(x => x.StartDomainProcess(
                Param<StartDomainProcessCommand>.Matches(p =>
                    p.StartEventToWaitFor == "ThirdPartyInvoiceAddedToBatchEvent" &&
                    (p.DataModel as PayThirdPartyInvoiceProcessModel).InvoiceCollection.First().AccountNumber == thirdPartyPayments.First().AccountNumber
                    && (p.DataModel as PayThirdPartyInvoiceProcessModel).InvoiceCollection.First().ThirdPartyInvoiceKey == thirdPartyPayments.First().ThirdPartyInvoiceKey
                    && (p.DataModel as PayThirdPartyInvoiceProcessModel).InvoiceCollection.First().InstanceId == thirdPartyPayments.First().InstanceId
                    && (p.DataModel as PayThirdPartyInvoiceProcessModel).InvoiceCollection.First().SAHLReference == thirdPartyPayments.First().SAHLReference)
               ));
        };

        It should_not_return_any_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
