using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Rules;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.DomainProcessManager.DomainProcesses.Managers.X2Workflow;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.DomainProcessManager.DomainProcesses.Managers.Client;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.BankAccountDomain;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.FinancialDomain;
using SAHL.Services.Interfaces.PropertyDomain;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public class NewPurchaseApplicationDomainProcess : ApplicationCreationDomainProcess<NewPurchaseApplicationCreationModel>
    {
        public NewPurchaseApplicationDomainProcess(IApplicationStateMachine applicationStateMachine,
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
            )
            : base(applicationStateMachine, applicationDomainService, clientDomainService, addressDomainService, financialDomainService, bankAccountDomainService,
                   combGuidGenerator, clientDataManager, x2WorkflowManager, linkedKeyManager, propertyDomainService, communicationManager, applicationDataManager, domainRuleManager
            , rawLogger, loggerSource, loggerAppSource)
        {
        }

        public override void OnInternalStart()
        {
            if (ValidateApplication(applicationStateMachine, domainRuleManager))
            {
                var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());

                var domainModelMapper = new DomainModelMapper();
                domainModelMapper.CreateMap<NewPurchaseApplicationCreationModel, NewPurchaseApplicationModel>();
                var newPurchaseApplicationCreationModel = domainModelMapper.Map(this.DataModel);

                var addNewPurchaseApplicationCommand = new AddNewPurchaseApplicationCommand(newPurchaseApplicationCreationModel, serviceRequestMetadata.CommandCorrelationId);
                var serviceMessages = applicationDomainService.PerformCommand(addNewPurchaseApplicationCommand, serviceRequestMetadata);
                CheckForCriticalErrors(applicationStateMachine, serviceRequestMetadata.CommandCorrelationId, serviceMessages);
            }
            else
            {
                HandleValidateException(applicationStateMachine);
            }
        }
    }
}