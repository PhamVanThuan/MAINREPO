using SAHL.Core.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using System;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
        where T : ApplicationCreationModel
    {
        public void AddClientsBankAccountDetails(IEnumerable<ApplicantModel> applicants)
        {
            foreach (var applicant in applicants)
            {
                AddBankAccounts(applicant);
            }
        }

        private void AddBankAccounts(ApplicantModel applicant)
        {
            foreach (var bankAccount in applicant.BankAccounts)
            {
                var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());

                try
                {
                    DomainModelMapper mapper = new DomainModelMapper();
                    mapper.CreateMap<BankAccountModel, Services.Interfaces.BankAccountDomain.Models.BankAccountModel>();
                    var bankAccountModel = mapper.Map(bankAccount);

                    int clientKey = applicationStateMachine.ClientCollection[applicant.IDNumber];
                    var bankAccountGuid = this.combGuidGenerator.Generate();

                    var linkBankAccountToClientCommand = new LinkBankAccountToClientCommand(bankAccountModel, clientKey, bankAccountGuid);

                    var serviceMessages = this.bankAccountDomainService.PerformCommand(linkBankAccountToClientCommand, serviceRequestMetadata);
                    CheckForNonCriticalErrors(applicationStateMachine, serviceRequestMetadata.CommandCorrelationId, serviceMessages, ApplicationState.BankAccountCaptured);
                }
                catch (Exception runTimeException)
                {
                    var userFriendlyErrorMessage = string.Format("Bank account {0} could not be saved for applicant with ID Number: {1}.", bankAccount.AccountNumber, applicant.IDNumber);
                    HandleNonCriticalException(runTimeException, userFriendlyErrorMessage, serviceRequestMetadata.CommandCorrelationId, applicationStateMachine);
                }
            }
        }
    }
}