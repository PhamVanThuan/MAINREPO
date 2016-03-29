using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain.Events;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>, IDomainProcessEvent<BankAccountLinkedToClientEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(BankAccountLinkedToClientEvent bankAccountLinkedToClientEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            var stateMachine = this.ProcessState as IApplicationStateMachine;
            stateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.BankAccountCaptureConfirmed, bankAccountLinkedToClientEvent.Id);

            SetAddDebitOrderFlag(bankAccountLinkedToClientEvent, serviceRequestMetadata);

            if (applicationStateMachine.ContainsStateInBreadCrumb(ApplicationState.AllBankAccountsCaptured))
            {
                AddApplicationDebitOrder();
            }
        }

        public void SetAddDebitOrderFlag(BankAccountLinkedToClientEvent bankAccountLinkedToClientEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            foreach (var applicant in DataModel.Applicants)
            {
                if (applicationStateMachine.ClientCollection.Keys.Any(x => x == applicant.IDNumber)
                    && applicationStateMachine.ClientCollection[applicant.IDNumber] == bankAccountLinkedToClientEvent.ClientKey)
                {
                    var bankAccount = applicant.BankAccounts.FirstOrDefault(
                        x => x.AccountName == bankAccountLinkedToClientEvent.AccountName
                        && x.AccountNumber == bankAccountLinkedToClientEvent.AccountNumber
                        && x.IsDebitOrderBankAccount);

                    if (bankAccount != null && DataModel.ApplicationDebitOrder != null)
                    {
                        PopulateDictionary(applicationStateMachine.ClientDebitOrderBankAccountCollection, applicant.IDNumber, bankAccountLinkedToClientEvent.ClientBankAccountKey);
                    }
                }
            }
        }

        public void AddApplicationDebitOrder()
        {
            int clientBankAccountKey = 0;
            Guid correlationId;
            foreach (var applicant in DataModel.Applicants)
            {
                correlationId = combGuidGenerator.Generate();
                try
                {
                    if (applicationStateMachine.ClientDebitOrderBankAccountCollection != null && applicationStateMachine.ClientDebitOrderBankAccountCollection.Keys.Any(x => x == applicant.IDNumber))
                    {
                        clientBankAccountKey = applicationStateMachine.ClientDebitOrderBankAccountCollection[applicant.IDNumber];

                        var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, correlationId);

                        var debitOrderModel = DataModel.ApplicationDebitOrder;
                        var applicationDebitOrderModel = new Services.Interfaces.ApplicationDomain.Models.ApplicationDebitOrderModel(applicationStateMachine.ApplicationNumber, 
                            debitOrderModel.DebitOrderDay, clientBankAccountKey);

                        var command = new AddApplicationDebitOrderCommand(applicationDebitOrderModel);
                        var serviceMessages = this.applicationDomainService.PerformCommand(command, serviceRequestMetadata);
                        CheckForNonCriticalErrors(applicationStateMachine, serviceRequestMetadata.CommandCorrelationId, serviceMessages, ApplicationState.BankAccountCaptured);
                    }
                }
                catch (Exception runtimeException)
                {
                    HandleNonCriticalException(runtimeException, "Application Debit Order could not be saved.", correlationId, applicationStateMachine);
                }
            }
        }
    }
}