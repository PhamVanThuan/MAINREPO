using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Identity;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.DomainProcessManager.DomainProcesses.Managers.X2Workflow;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.DomainProcessManager.DomainProcesses.Managers.Client;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.BankAccountDomain;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.FinancialDomain;
using SAHL.Services.Interfaces.PropertyDomain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks
{
    public class WithDomainServiceMocks : WithFakes
    {
        protected static IApplicationStateMachine applicationStateMachine;
        protected static IApplicationDomainServiceClient applicationDomainService;
        protected static IClientDomainServiceClient clientDomainService;
        protected static IAddressDomainServiceClient addressDomainService;
        protected static IFinancialDomainServiceClient financialDomainService;
        protected static IBankAccountDomainServiceClient bankAccountDomainService;
        protected static ICombGuid combGuidGenerator;
        protected static IClientDataManager clientDataManager;
        protected static IX2WorkflowManager x2WorkflowManager;
        protected static ILinkedKeyManager linkedKeyManager;
        protected static IPropertyDomainServiceClient propertyDomainService;
        protected static ICommunicationManager communicationManager;
        protected static IApplicationDataManager applicationDataManager;

        protected static event ServiceHttpClient.CurrentlyPerformingRequestHandler CurrentlyPerformingRequest;
        protected static Dictionary<Guid,Type> sentCommands = new Dictionary<Guid, Type>();
        protected static IDomainRuleManager<ApplicationCreationModel> domainRuleManager;

        private Establish context = () =>
        {
            applicationDomainService = An<IApplicationDomainServiceClient>();
            clientDomainService = An<IClientDomainServiceClient>();
            addressDomainService = An<IAddressDomainServiceClient>();
            bankAccountDomainService = An<IBankAccountDomainServiceClient>();
            combGuidGenerator = An<ICombGuid>();
            clientDataManager = An<IClientDataManager>();
            x2WorkflowManager = An<IX2WorkflowManager>();
            linkedKeyManager = An<ILinkedKeyManager>();
            propertyDomainService = An<IPropertyDomainServiceClient>();
            communicationManager = An<ICommunicationManager>();
            financialDomainService = An<IFinancialDomainServiceClient>();
            applicationDataManager = An<IApplicationDataManager>();
            domainRuleManager = An<IDomainRuleManager<ApplicationCreationModel>>();

            int currentGuidIndex = 0;
            combGuidGenerator.WhenToldTo(x => x.Generate()).Return(() =>
            {
                var guid = GetNewGuid().ToList()[currentGuidIndex];
                currentGuidIndex++;
                return guid;
            });

            applicationDomainService.WhenToldTo<IApplicationDomainServiceClient>(x =>
                x.PerformCommand(Param.IsAny<IApplicationDomainCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Callback<IApplicationDomainCommand, IServiceRequestMetadata>((command, metadata) => HandlePerformingRequestCallback(command, metadata));

            clientDomainService.WhenToldTo<IClientDomainServiceClient>(x =>
                x.PerformCommand(Param.IsAny<IClientDomainCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Callback<IClientDomainCommand, IServiceRequestMetadata>((command, metadata) => HandlePerformingRequestCallback(command, metadata));

            addressDomainService.WhenToldTo<IAddressDomainServiceClient>(x =>
                x.PerformCommand(Param.IsAny<IAddressDomainCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Callback<IAddressDomainCommand, IServiceRequestMetadata>((command, metadata) => HandlePerformingRequestCallback(command, metadata));

            bankAccountDomainService.WhenToldTo<IBankAccountDomainServiceClient>(x =>
                x.PerformCommand(Param.IsAny<IBankAccountDomainCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Callback<IBankAccountDomainCommand, IServiceRequestMetadata>((command, metadata) => HandlePerformingRequestCallback(command, metadata));

            propertyDomainService.WhenToldTo<IPropertyDomainServiceClient>(x =>
                x.PerformCommand(Param.IsAny<IPropertyDomainCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Callback<IPropertyDomainCommand, IServiceRequestMetadata>((command, metadata) => HandlePerformingRequestCallback(command, metadata));

            financialDomainService.WhenToldTo<IFinancialDomainServiceClient>(x =>
                x.PerformCommand(Param.IsAny<IFinancialDomainCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Callback<IFinancialDomainCommand, IServiceRequestMetadata>((command, metadata) => HandlePerformingRequestCallback(command, metadata));
        };

        protected static Guid GetGuidForCommandType(Type commandType)
        {
            var guid = sentCommands.First(x => x.Value == commandType).Key;
            sentCommands.Remove(guid);
            return guid;
        }

        private static void HandlePerformingRequestCallback(IServiceCommand command, IServiceRequestMetadata metadata)
        {
            sentCommands.Add(Guid.Parse(metadata[DomainProcessManagerGlobals.CommandCorrelationKey]), command.GetType());
            var currentlyPerformingRequest = CurrentlyPerformingRequest;
            if (currentlyPerformingRequest != null)
            {
                currentlyPerformingRequest(null, new CurrentlyPerformingRequestEventArgs(command.GetType(), metadata));
            }
        }

        private static IEnumerable<Guid> GetNewGuid()
        {
            for (int i = 0; i <= 100; i++)
            {
                yield return Guid.NewGuid();
            }
        }
    }
}