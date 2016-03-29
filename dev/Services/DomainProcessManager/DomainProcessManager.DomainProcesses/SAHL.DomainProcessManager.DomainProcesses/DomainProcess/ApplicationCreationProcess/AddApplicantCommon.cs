using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SAHL.Core.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ClientDomain.Queries;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
        where T : ApplicationCreationModel
    {
        private void AddApplicant(ApplicantModel applicant)
        {
            var findClientByIdNumberQuery = new FindClientByIdNumberQuery(applicant.IDNumber);
            this.clientDomainService.PerformQuery(findClientByIdNumberQuery);
            var existingApplicant = findClientByIdNumberQuery.Result.Results.FirstOrDefault();

            if (existingApplicant != null && existingApplicant.LegalEntityKey > 0)
            {
                int clientKey = existingApplicant.LegalEntityKey;
                if (!applicationStateMachine.ClientCollection.ContainsKey(existingApplicant.IDNumber))
                {
                    applicationStateMachine.ClientCollection.Add(existingApplicant.IDNumber, clientKey);
                }

                var clientHasOpenApplicationOrAccountQuery = new ClientHasOpenAccountOrApplicationQuery(clientKey);
                var queryMessages = this.clientDomainService.PerformQuery(clientHasOpenApplicationOrAccountQuery);
                if (queryMessages.HasErrors)
                {
                    CheckForCriticalErrors(applicationStateMachine, Guid.Empty, queryMessages);
                }
                else
                {
                    bool applicantHasOpenAccountOrApplication =
                        clientHasOpenApplicationOrAccountQuery.Result.Results.Any(rslt => rslt.ClientIsAlreadyAnSAHLCustomer);
                    if (applicantHasOpenAccountOrApplication)
                    {
                        UpdateActiveClient(existingApplicant, applicant);
                    }
                    else
                    {
                        UpdateInactiveClient(existingApplicant, applicant);
                    }

                    var addApplicantToApplicationCommand = new AddLeadApplicantToApplicationCommand(combGuidGenerator.Generate(),
                        existingApplicant.LegalEntityKey,
                        applicationStateMachine.ApplicationNumber,
                        applicant.ApplicantRoleType);
                    var addLeadApplicantMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());
                    var addLeadApplicantMessages = this.applicationDomainService.PerformCommand(addApplicantToApplicationCommand,
                        addLeadApplicantMetadata);
                    CheckForCriticalErrors(applicationStateMachine, addLeadApplicantMetadata.CommandCorrelationId, addLeadApplicantMessages);
                }
            }
            else
            {
                AddNewClient(applicant);
            }
        }

        private void UpdateActiveClient(ClientDetailsQueryResult existingApplicant, ApplicantModel applicant)
        {
            Dictionary<string, string> differencesList = GetDifferencesInExistingAndSuppliedClientDetail(existingApplicant, applicant);
            if (differencesList.Count > 0)
            {
                var legalEntityName = string.Format("{0} {1} {2}", applicant.Salutation.ToString(), applicant.FirstName, applicant.Surname);
                bool dateOfBirthSetToComcorpProvided = !existingApplicant.DateOfBirth.HasValue;

                this.communicationManager.SendClientDetailComparisonFailedEmail(differencesList,
                    legalEntityName,
                    applicant.IDNumber,
                    applicationStateMachine.ApplicationNumber,
                    dateOfBirthSetToComcorpProvided);
            }

            var mapper = new DomainModelMapper();
            mapper.CreateMap<ApplicantModel, ActiveNaturalPersonClientModel>();
            var activeNaturalPersonClient = mapper.Map(applicant);

            var command = new UpdateActiveNaturalPersonClientCommand(existingApplicant.LegalEntityKey, activeNaturalPersonClient);
            var updateActiveClientMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());
            var updateActiveClientMessages = this.clientDomainService.PerformCommand(command, updateActiveClientMetadata);
            CheckForCriticalErrors(applicationStateMachine, updateActiveClientMetadata.CommandCorrelationId, updateActiveClientMessages);
        }

        private void UpdateInactiveClient(ClientDetailsQueryResult existingApplicant, ApplicantModel applicant)
        {
            var mapper = new DomainModelMapper();
            mapper.CreateMap<ApplicantModel, NaturalPersonClientModel>();
            var naturalPersonClient = mapper.Map(applicant);

            var command = new UpdateInactiveNaturalPersonClientCommand(existingApplicant.LegalEntityKey, naturalPersonClient);
            var updateInactiveClientMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());
            var updateInactiveClientMessages = this.clientDomainService.PerformCommand(command, updateInactiveClientMetadata);
            CheckForCriticalErrors(applicationStateMachine, updateInactiveClientMetadata.CommandCorrelationId, updateInactiveClientMessages);
        }

        private void AddNewClient(ApplicantModel applicant)
        {
            var mapper = new DomainModelMapper();
            mapper.CreateMap<ApplicantModel, NaturalPersonClientModel>();
            var naturalPerson = mapper.Map(applicant);

            var command = new AddNaturalPersonClientCommand(naturalPerson);
            var addClientMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());
            addClientMetadata.Add("IdNumberOfAddedClient", applicant.IDNumber);
            var addClientMessages = this.clientDomainService.PerformCommand(command, addClientMetadata);
            CheckForCriticalErrors(applicationStateMachine, addClientMetadata.CommandCorrelationId, addClientMessages);
        }

        private Dictionary<string, string> GetDifferencesInExistingAndSuppliedClientDetail(ClientDetailsQueryResult existingApplicant,
            ApplicantModel applicant)
        {
            ReadOnlyDictionary<string, string> fieldsForComparison = new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>
                {
                    { "FirstName", "FirstNames" },
                    { "Surname", "Surname" },
                    { "Gender", "GenderKey" },
                    { "MaritalStatus", "MaritalStatusKey" },
                    { "PopulationGroup", "PopulationGroupKey" },
                    { "CitizenshipType", "CitizenTypeKey" },
                    { "DateOfBirth", "DateOfBirth" },
                    { "PassportNumber", "PassportNumber" }
                });
            var fieldsWithDifferences = new Dictionary<string, string>();
            foreach (var suppliedFieldName in fieldsForComparison.Keys)
            {
                var existingFieldName = fieldsForComparison[suppliedFieldName];
                var suppliedValue = applicant.GetType().GetProperty(suppliedFieldName).GetValue(applicant);
                var existingValue = existingApplicant.GetType().GetProperty(existingFieldName).GetValue(existingApplicant);

                if (suppliedValue is Enum)
                {
                    if ((int)suppliedValue != (int)existingValue)
                    {
                        fieldsWithDifferences.Add(suppliedFieldName, suppliedValue.ToString());
                    }
                }
                else if (!String.Equals(
                    (suppliedValue != null ? suppliedValue.ToString() : String.Empty),
                    (existingValue != null ? existingValue.ToString() : String.Empty),
                    StringComparison.InvariantCulture)
                    )
                {
                    fieldsWithDifferences.Add(suppliedFieldName, suppliedValue.ToString());
                }
            }
            return fieldsWithDifferences;
        }
    }
}
