using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.FinancialDomain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.AddressEventHandler
{
    public class when_linking_free_text_residential_throws_an_exception : WithNewPurchaseDomainProcess
    {
        private static int clientKey;
        private static Dictionary<string, int> clientCollection;
        private static Exception thrownException, runtimeException;
        private static NewBusinessApplicationFundedEvent newBusinessApplicationFundedEvent;
        private static int applicationNumber;
        private static string friendlyErrorMessage;

        private Establish context = () =>
        {
            applicationNumber = 12;
            clientKey = 100;

            newBusinessApplicationFundedEvent = new NewBusinessApplicationFundedEvent(new DateTime(2014, 01, 01).Date, applicationNumber);
            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            var applicant = applicationCreationModel.Applicants.First();
            friendlyErrorMessage = string.Format("Address could not be saved for applicant with ID Number: {0}.", applicant.IDNumber).ToString();
            var addresses = applicant.Addresses.ToList();
            addresses.Add(ApplicationCreationTestHelper.PopulatePropertyAddressAsResidential());
            domainProcess.DataModel = applicationCreationModel;

            bankAccountDomainService.WhenToldTo(x => x.PerformCommand(Param.IsAny<LinkBankAccountToClientCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                            .Return(SystemMessageCollection.Empty());
            combGuidGenerator.WhenToldTo(x => x.Generate()).Return(new Guid());

            clientCollection = new Dictionary<string, int>();
            clientCollection.Add(applicant.IDNumber, clientKey);
            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
            runtimeException = new Exception("an error occured.");
            addressDomainService.WhenToldTo(x => x.PerformCommand(Param.IsAny<LinkFreeTextAddressAsResidentialAddressToClientCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                                                    .Throw(runtimeException);
            var freeTextqueryResult = new ServiceQueryResult<GetClientFreeTextAddressQueryResult>( new GetClientFreeTextAddressQueryResult[] { });
            addressDomainService
                .WhenToldTo(ads => ads.PerformQuery(Param.IsAny<GetClientFreeTextAddressQuery>()))
                .Return<GetClientFreeTextAddressQuery>(rqst => { rqst.Result = freeTextqueryResult; return new SystemMessageCollection(); });

            var streetqueryResult = new ServiceQueryResult<GetClientStreetAddressQueryResult>( new GetClientStreetAddressQueryResult[] { });
            addressDomainService
                .WhenToldTo(ads => ads.PerformQuery(Param.IsAny<GetClientStreetAddressQuery>()))
                .Return<GetClientStreetAddressQuery>(rqst => { rqst.Result = streetqueryResult; return new SystemMessageCollection(); });
        };

        private Because of = () =>
        {
            thrownException = Catch.Exception(() => domainProcess.HandleEvent(newBusinessApplicationFundedEvent, serviceRequestMetadata));
        };

        private It should_fire_non_critical_error_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(Arg.Is<ApplicationStateTransitionTrigger>(y => y == ApplicationStateTransitionTrigger.NonCriticalErrorReported), Param.IsAny<Guid>()));
        };

        private It should_contain_an_error_message = () =>
        {
            applicationStateMachine.WasToldTo(x => x.AggregateMessages(Param<ISystemMessageCollection>.Matches(m =>
                m.ErrorMessages().Any(y => y.Message.Contains("Address could not be saved for applicant with ID Number:")) &&
                m.ExceptionMessages().Any(y => y.Message.Contains(runtimeException.ToString())))));
        };

        private It should_log_the_error_message = () =>
        {
            rawLogger.WasToldTo(x => x.LogError(Param.IsAny<LogLevel>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()
             , Param<string>.Matches(m => m.Contains(friendlyErrorMessage)), null));
        };

        private It should_not_throw_an_exception = () =>
        {
            thrownException.ShouldBeNull();
        };
    }
}