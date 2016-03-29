using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Identity;
using SAHL.Core.Rules;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess;
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
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.FinancialDomain;
using SAHL.Services.Interfaces.PropertyDomain;
using SAHL.Services.Interfaces.X2;
using System; 
using SAHL.Core.Logging;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks
{
    public class WithNewPurchaseDomainProcess : WithFakes
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
        protected static NewPurchaseApplicationDomainProcess domainProcess;
        protected static DomainProcessServiceRequestMetadata serviceRequestMetadata;
        protected static IDomainRuleManager<ApplicationCreationModel> domainRuleManager;
        protected static IRawLogger rawLogger;
        protected static ILoggerSource loggerSource;
        protected static ILoggerAppSource loggerAppSource;

        private Establish context = () =>
        {
            applicationStateMachine = An<IApplicationStateMachine>();
            applicationDomainService = An<IApplicationDomainServiceClient>();
            clientDomainService = An<IClientDomainServiceClient>();
            addressDomainService = An<IAddressDomainServiceClient>();
            bankAccountDomainService = An<IBankAccountDomainServiceClient>();
            combGuidGenerator = An<ICombGuid>();
            clientDataManager = An<IClientDataManager>();
            x2WorkflowManager = An<IX2WorkflowManager>();
            linkedKeyManager = An<ILinkedKeyManager>();
            rawLogger = An<IRawLogger>();
            loggerSource = An<ILoggerSource>();
            loggerAppSource = An<ILoggerAppSource>();
            propertyDomainService = An<IPropertyDomainServiceClient>();
            communicationManager = An<ICommunicationManager>();
            financialDomainService = An<IFinancialDomainServiceClient>();
            applicationDataManager = An<IApplicationDataManager>();
            domainRuleManager = An<IDomainRuleManager<ApplicationCreationModel>>();
            domainProcess = new NewPurchaseApplicationDomainProcess(
                                      applicationStateMachine
                                    , applicationDomainService
                                    , clientDomainService
                                    , addressDomainService
                                    , financialDomainService
                                    , bankAccountDomainService
                                    , combGuidGenerator
                                    , clientDataManager
                                    , x2WorkflowManager
                                    , linkedKeyManager
                                    , propertyDomainService
                                    , communicationManager
                                    , applicationDataManager
                                    , domainRuleManager
                                    , rawLogger
                                    , loggerSource
                                    , loggerAppSource
                               );
            serviceRequestMetadata = new DomainProcessServiceRequestMetadata(domainProcess.DomainProcessId, Guid.NewGuid());

            IDataModel newPurchaseDataModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            domainProcess.ProcessState = applicationStateMachine;
            domainProcess.Start(newPurchaseDataModel, "FakeEvent");
        };
    }
}