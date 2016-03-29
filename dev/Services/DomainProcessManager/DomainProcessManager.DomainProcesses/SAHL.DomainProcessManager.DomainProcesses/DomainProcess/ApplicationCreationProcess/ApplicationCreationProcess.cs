using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.DomainProcess;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Rules;
using SAHL.Core.Web.Services;
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

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
        where T : ApplicationCreationModel
    {
        protected IApplicationStateMachine applicationStateMachine;
        protected readonly IApplicationDomainServiceClient applicationDomainService;
        protected readonly IClientDomainServiceClient clientDomainService;
        protected readonly IAddressDomainServiceClient addressDomainService;
        protected readonly IFinancialDomainServiceClient financialDomainService;
        protected readonly IBankAccountDomainServiceClient bankAccountDomainService;
        protected readonly ICombGuid combGuidGenerator;
        protected readonly IClientDataManager clientDataManager;
        protected readonly IX2WorkflowManager x2WorkflowManager;
        protected readonly ICommunicationManager communicationManager;
        protected readonly IApplicationDataManager applicationDataManager;
        protected readonly IDomainRuleManager<ApplicationCreationModel> domainRuleManager;
        private readonly ILinkedKeyManager LinkedKeyManager;
        private readonly IPropertyDomainServiceClient PropertyDomainService;
        private readonly IRawLogger rawLogger;
        private readonly ILoggerSource loggerSource;
        private readonly ILoggerAppSource loggerAppSource;
        

        public ApplicationCreationDomainProcess(IApplicationStateMachine applicationStateMachine,
                                                IApplicationDomainServiceClient applicationDomainService,
                                                IClientDomainServiceClient clientDomainService,
                                                IAddressDomainServiceClient addressDomainService,
                                                IFinancialDomainServiceClient financialDomainService,
                                                IBankAccountDomainServiceClient bankAccountDomainService,
                                                ICombGuid combGuidGenerator,
                                                IClientDataManager clientDataManager,
                                                IX2WorkflowManager x2WorkflowManager,
                                                ILinkedKeyManager linkedKeyManager,
                                                IPropertyDomainServiceClient propertyDomainService,
                                                ICommunicationManager communicationManager,
                                                IApplicationDataManager applicationDataManager,
                                                IDomainRuleManager<ApplicationCreationModel> domainRuleManager,
                                                IRawLogger rawLogger,
                                                ILoggerSource loggerSource,
                                                ILoggerAppSource loggerAppSource
            ) : base(rawLogger, loggerSource, loggerAppSource)
        {
            if (applicationStateMachine == null) { throw new ArgumentNullException("applicationStateMachine"); }
            if (applicationDomainService == null) { throw new ArgumentNullException("applicationDomainService"); }
            if (clientDomainService == null) { throw new ArgumentNullException("clientDomainService"); }
            if (addressDomainService == null) { throw new ArgumentNullException("addressDomainService"); }
            if (financialDomainService == null) { throw new ArgumentNullException("financialDomainService"); }
            if (bankAccountDomainService == null) { throw new ArgumentNullException("bankAccountDomainService"); }
            if (combGuidGenerator == null) { throw new ArgumentNullException("combGuidGenerator"); }
            if (clientDataManager == null) { throw new ArgumentNullException("clientDataManager"); }
            if (x2WorkflowManager == null) { throw new ArgumentNullException("x2WorkflowManager"); }
            if (linkedKeyManager == null) { throw new ArgumentNullException("linkedKeyManager"); }
            if (propertyDomainService == null) { throw new ArgumentNullException("propertyDomainService"); }
            if (communicationManager == null) { throw new ArgumentNullException("communicationManager"); }
            if (applicationDataManager == null) { throw new ArgumentNullException("applicationDataManager"); }
            if (domainRuleManager == null) { throw new ArgumentNullException("domainRuleManager"); }

            this.applicationStateMachine = applicationStateMachine;
            this.applicationDomainService = applicationDomainService;
            this.clientDomainService = clientDomainService;
            this.addressDomainService = addressDomainService;
            this.financialDomainService = financialDomainService;
            this.bankAccountDomainService = bankAccountDomainService;
            this.combGuidGenerator = combGuidGenerator;
            this.x2WorkflowManager = x2WorkflowManager;
            this.clientDataManager = clientDataManager;
            this.LinkedKeyManager = linkedKeyManager;
            this.PropertyDomainService = propertyDomainService;
            this.communicationManager = communicationManager;
            this.applicationDataManager = applicationDataManager;
            this.domainRuleManager = domainRuleManager;
            this.applicationDomainService.CurrentlyPerformingRequest += new ServiceHttpClient.CurrentlyPerformingRequestHandler(StoreCurrentlyPerformingRequestCounter);
            this.clientDomainService.CurrentlyPerformingRequest += new ServiceHttpClient.CurrentlyPerformingRequestHandler(StoreCurrentlyPerformingRequestCounter);
            this.addressDomainService.CurrentlyPerformingRequest += new ServiceHttpClient.CurrentlyPerformingRequestHandler(StoreCurrentlyPerformingRequestCounter);
            this.financialDomainService.CurrentlyPerformingRequest += new ServiceHttpClient.CurrentlyPerformingRequestHandler(StoreCurrentlyPerformingRequestCounter);
            this.bankAccountDomainService.CurrentlyPerformingRequest += new ServiceHttpClient.CurrentlyPerformingRequestHandler(StoreCurrentlyPerformingRequestCounter);
            this.PropertyDomainService.CurrentlyPerformingRequest += new ServiceHttpClient.CurrentlyPerformingRequestHandler(StoreCurrentlyPerformingRequestCounter);
        }

        public override void Initialise(IDataModel dataModel)
        {
            if (dataModel == null) { throw new ArgumentNullException("dataModel"); }
            if (!(dataModel is ApplicationCreationModel))
            {
                throw new Exception(string.Format("Invalid Data Model. Expected {0} but received {1}", typeof(ApplicationCreationModel).Name, dataModel.GetType().Name));
            }

            applicationStateMachine.CreateStateMachine((ApplicationCreationModel)dataModel, this.DomainProcessId);
            this.ProcessState = applicationStateMachine;
        }

        public override void RestoreState(IDataModel stateMachineMissingTriggers)
        {
            if (stateMachineMissingTriggers == null) { throw new ArgumentNullException("dataModel"); }
            if (!(stateMachineMissingTriggers is ApplicationStateMachine))
            {
                throw new Exception(string.Format("Invalid Data Model. Expected {0} but received {1}", typeof(ApplicationStateMachine).Name, stateMachineMissingTriggers.GetType().Name));
            }
            ApplicationStateMachine stateMachine = stateMachineMissingTriggers as ApplicationStateMachine;
            stateMachine.InitializeMachine(stateMachine.State);
            this.applicationStateMachine = stateMachine;
            this.ProcessState = stateMachine;
        }

        public void StoreCurrentlyPerformingRequestCounter(object sender, CurrentlyPerformingRequestEventArgs e)
        {
            if (e.Metadata != null)
            {
                var commandCorrelationGuid = Guid.Parse(e.Metadata[DomainProcessManagerGlobals.CommandCorrelationKey]);
                applicationStateMachine.RecordCommandSent(e.RequestType, commandCorrelationGuid);
            }
        }
    }
}