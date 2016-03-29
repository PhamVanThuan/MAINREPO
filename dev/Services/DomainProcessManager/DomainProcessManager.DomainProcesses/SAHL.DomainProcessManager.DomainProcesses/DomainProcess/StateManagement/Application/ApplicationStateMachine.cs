using Newtonsoft.Json;
using SAHL.Core.Data;
using SAHL.Core.Identity;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement
{
    public partial class ApplicationStateMachine : IDataModel, IApplicationStateMachine
    {
        [JsonProperty]
        private Guid domainProcessId;

        private ApplicationState defaultInitialState = ApplicationState.Processing;

        [JsonProperty]
        private ApplicationState serializationStateSnapshot;

        [JsonProperty]
        private SerialisationFriendlyStateMachine<ApplicationState,ApplicationStateTransitionTrigger> machine;

        [JsonProperty]
        private Dictionary<Guid, ApplicationState> statesBreadCrumb;

        [JsonProperty]
        private Dictionary<Guid, Tuple<Type, bool>> sentCommands;

        [JsonProperty]
        private Dictionary<ApplicationState, int> fullAddressExpectedStateFrequencyChain;

        [JsonProperty]
        private Dictionary<ApplicationState, int> fullBankAccountExpectedStateFrequencyChain;

        [JsonProperty]
        private Dictionary<ApplicationState, int> fullAssetLiabilityDetailExpectedStateFrequencyChain;

        [JsonProperty]
        private Dictionary<ApplicationState, int> expectedCriticalStateFrequencyChain;

        [JsonProperty]
        private int applicationNumber;

        public IDictionary<Guid, ApplicationState> StatesBreadCrumb { get { return statesBreadCrumb; } }

        public ApplicationState State
        {
            get
            {
                ApplicationState actualState = machine.State;
                if (statesBreadCrumb.Count > 1 && IsInState(ApplicationState.Processing))
                {
                    actualState = serializationStateSnapshot;
                }
                return actualState;
            }
        }

        public int ApplicationNumber { get { return applicationNumber; } protected set { applicationNumber = value; } }

        public IDictionary<string, int> ClientCollection { get; protected set; }

        public int? MailingClientAddressKey { get; set; }

        public IDictionary<string, int> ClientDomicilumAddressCollection { get; protected set; }

        public IDictionary<string, int> ClientDebitOrderBankAccountCollection { get; protected set; }

        public ISystemMessageCollection SystemMessages { get; protected set; }

        public SerialisationFriendlyStateMachine<ApplicationState,ApplicationStateTransitionTrigger> Machine { get { return machine; } }

        public Dictionary<ApplicationState, string> NodeId = new Dictionary<ApplicationState, string>();

        public List<ApplicationState> ErrorStates { get; protected set; }

        public IList<int> EmploymentKeys { get; protected set; }

        public ApplicationStateMachine()
        {
            SystemMessages = SystemMessageCollection.Empty();
            ClientCollection = new Dictionary<string, int>();
            ClientDomicilumAddressCollection = new Dictionary<string, int>();
            ClientDebitOrderBankAccountCollection = new Dictionary<string, int>();
            sentCommands = new Dictionary<Guid, Tuple<Type, bool>>();
            ErrorStates = new List<ApplicationState>();
            EmploymentKeys = new List<int>();

            expectedCriticalStateFrequencyChain = new Dictionary<ApplicationState, int>();
            fullBankAccountExpectedStateFrequencyChain = new Dictionary<ApplicationState, int>();
            fullAddressExpectedStateFrequencyChain = new Dictionary<ApplicationState, int>();
            fullAssetLiabilityDetailExpectedStateFrequencyChain = new Dictionary<ApplicationState, int>();
            statesBreadCrumb = new Dictionary<Guid, ApplicationState>();
        }

        public SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger> InitializeMachine(ApplicationState state)
        {
            machine = new SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger>(state);
            SetupCommandCorrelatedTriggers(machine);

            InitializeCriticalPath(machine);

            InitializeStandaloneNonCriticalPath(machine);

            InitializeAddressPath(machine);

            InitializeBankAccountPath(machine);

            return machine;
        }

        private void InitializeCriticalPath(SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger> machine)
        {
            machine.Configure(ApplicationState.Processing).SubstateOf(ApplicationState.CriticalPath)
                .Permit(ApplicationStateTransitionTrigger.BasicApplicationCreationConfirmed, ApplicationState.BasicApplicationCreated);

            machine.Configure(ApplicationState.BasicApplicationCreated)
                .SubstateOf(ApplicationState.CriticalPath)
                .OnEntryFrom(BasicApplicationCreatedTrigger, (correlationId, applicationNumber) =>
                {
                    RecordStateHistory(correlationId, ApplicationState.BasicApplicationCreated);
                    ApplicationNumber = applicationNumber;
                })
                .Permit(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed, ApplicationState.ApplicantAdded);

            machine.Configure(ApplicationState.ApplicantAdded).SubstateOf(ApplicationState.CriticalPath)
                .PermitReentry(ApplicationStateTransitionTrigger.ApplicantAdditionConfirmed)
                .OnEntryFrom(ApplicantAddedTrigger, (correlationId) =>
                {
                    RecordStateHistory(correlationId, ApplicationState.ApplicantAdded);
                })
                .Permit(ApplicationStateTransitionTrigger.EmploymentAdditionConfirmed, ApplicationState.EmploymentAdded);

            machine.Configure(ApplicationState.EmploymentAdded).SubstateOf(ApplicationState.CriticalPath)
                .OnEntryFrom(EmploymentAddedTrigger, (correlationId, employmentKey) =>
                {
                    if (!EmploymentKeys.Contains(employmentKey))
                    {
                        EmploymentKeys.Add(employmentKey);
                    }

                    RecordStateHistory(correlationId, ApplicationState.EmploymentAdded);
                })
                .PermitIf(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, ApplicationState.ApplicationEmploymentDetermined, ClientEmploymentsFullyCaptured)
                .PermitIf(ApplicationStateTransitionTrigger.ApplicationHouseHoldIncomeDeterminationConfirmed, ApplicationState.ApplicationHouseHoldIncomeDetermined, ClientEmploymentsFullyCaptured)
                .PermitReentry(ApplicationStateTransitionTrigger.EmploymentAdditionConfirmed);

            machine.Configure(ApplicationState.ApplicationEmploymentDetermined).SubstateOf(ApplicationState.CriticalPath)
                .OnEntryFrom(ApplicationEmploymentDeterminedTrigger, correlationId =>
                {
                    RecordStateHistory(correlationId, ApplicationState.ApplicationEmploymentDetermined);
                })
                .Permit(ApplicationStateTransitionTrigger.ApplicationHouseHoldIncomeDeterminationConfirmed, ApplicationState.ApplicationHouseHoldIncomeDetermined)
                .PermitIf(ApplicationStateTransitionTrigger.ApplicationPricingConfirmed, ApplicationState.ApplicationPriced, () => statesBreadCrumb.Values.Contains(ApplicationState.ApplicationHouseHoldIncomeDetermined));

            machine.Configure(ApplicationState.ApplicationHouseHoldIncomeDetermined).SubstateOf(ApplicationState.CriticalPath)
              .OnEntryFrom(ApplicationHouseHoldIncomeDeterminedTrigger, correlationId =>
              {
                  RecordStateHistory(correlationId, ApplicationState.ApplicationHouseHoldIncomeDetermined);
              })
              .PermitIf(ApplicationStateTransitionTrigger.ApplicationPricingConfirmed, ApplicationState.ApplicationPriced, () => statesBreadCrumb.Values.Contains(ApplicationState.ApplicationEmploymentDetermined))
              .Permit(ApplicationStateTransitionTrigger.ApplicationEmploymentDeterminationConfirmed, ApplicationState.ApplicationEmploymentDetermined);

            machine.Configure(ApplicationState.ApplicationPriced)
                .SubstateOf(ApplicationState.CriticalPath)
                .OnEntryFrom(ApplicationPricedTrigger, correlationId =>
                    {
                        RecordStateHistory(correlationId, ApplicationState.ApplicationPriced);
                    })
                .Permit(ApplicationStateTransitionTrigger.ApplicationFundingConfirmed, ApplicationState.ApplicationFunded);

            machine.Configure(ApplicationState.ApplicationFunded)
                .SubstateOf(ApplicationState.CriticalPath)
                 .OnEntryFrom(ApplicationFundedTrigger, correlationId =>
                {
                    RecordStateHistory(correlationId, ApplicationState.ApplicationFunded);
                    if (CriticalPathFullyCaptured())
                    {
                        machine.Fire(ApplicationStateTransitionTrigger.ValidApplicationCreationConfirmed);
                    }
                })
               .Permit(ApplicationStateTransitionTrigger.ValidApplicationCreationConfirmed, ApplicationState.ValidMinimumApplicationCreated);

            machine.Configure(ApplicationState.CriticalErrorOccured)
                .SubstateOf(ApplicationState.CriticalPath)
                .OnEntryFrom(CriticalErrorOccuredTrigger, correlationId => RecordStateHistory(correlationId, ApplicationState.CriticalErrorOccured))
                .PermitReentry(ApplicationStateTransitionTrigger.CriticalErrorReported);

            machine.Configure(ApplicationState.CriticalPath)
                .Permit(ApplicationStateTransitionTrigger.CriticalErrorReported, ApplicationState.CriticalErrorOccured);
        }

        private void InitializeAddressPath(SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger> machine)
        {
            machine.Configure(ApplicationState.AddressStates).SubstateOf(ApplicationState.NonCriticalPath)
               .Permit(ApplicationStateTransitionTrigger.BankAccountCaptureConfirmed, ApplicationState.BankAccountCaptured)
               .Permit(ApplicationStateTransitionTrigger.AllBankAccountsCaptured, ApplicationState.AllBankAccountsCaptured)
               .Permit(ApplicationStateTransitionTrigger.ApplicationDebitOrderCaptureConfirmed, ApplicationState.ApplicationDebitOrderCaptured);

            machine.Configure(ApplicationState.AddressCaptured).SubstateOf(ApplicationState.AddressStates)
              .OnEntryFrom(AddressCapturedTrigger, (correlationId) =>
                {
                    RecordStateHistory(correlationId, ApplicationState.AddressCaptured);
                    if (AllAddressesFullyCaptured())
                    {
                        RecordStateHistory(CombGuid.Instance.Generate(), ApplicationState.AllAddressesCaptured);
                        machine.Fire(ApplicationStateTransitionTrigger.AllAddressesCaptured);
                    }
                })
              .PermitReentry(ApplicationStateTransitionTrigger.AddressCaptureConfirmed)
              .Permit(ApplicationStateTransitionTrigger.AllAddressesCaptured, ApplicationState.AllAddressesCaptured);

            machine.Configure(ApplicationState.AllAddressesCaptured).SubstateOf(ApplicationState.AddressStates)
               .Permit(ApplicationStateTransitionTrigger.ApplicationMailingAddressCaptureConfirmed, ApplicationState.ApplicationMailingAddressCaptured)
               .Permit(ApplicationStateTransitionTrigger.ClientPendingDomiciliumCaptureConfirmed, ApplicationState.ClientPendingDomiciliumCaptured)
               .Permit(ApplicationStateTransitionTrigger.DomiciliumAddressCaptureConfirmed, ApplicationState.DomiciliumAddressCaptured)
               .Permit(ApplicationStateTransitionTrigger.AssetLiabilityDetailCapturedConfirmed, ApplicationState.AssetLiabilityDetailCaptured);

            machine.Configure(ApplicationState.ApplicationMailingAddressCaptured).SubstateOf(ApplicationState.AddressStates)
                .OnEntryFrom(ApplicationMailingAddressCapturedTrigger, correlationId =>
                {
                    RecordStateHistory(correlationId, ApplicationState.ApplicationMailingAddressCaptured);
                })
               .Permit(ApplicationStateTransitionTrigger.ClientPendingDomiciliumCaptureConfirmed, ApplicationState.ClientPendingDomiciliumCaptured)
               .Permit(ApplicationStateTransitionTrigger.DomiciliumAddressCaptureConfirmed, ApplicationState.DomiciliumAddressCaptured)
               .Permit(ApplicationStateTransitionTrigger.AssetLiabilityDetailCapturedConfirmed, ApplicationState.AssetLiabilityDetailCaptured);

            machine.Configure(ApplicationState.ClientPendingDomiciliumCaptured).SubstateOf(ApplicationState.AddressStates)
                .PermitReentry(ApplicationStateTransitionTrigger.ClientPendingDomiciliumCaptureConfirmed)
                .OnEntryFrom(ClientPendingDomiciliumCaptureTrigger, correlationId =>
                {
                    RecordStateHistory(correlationId, ApplicationState.ClientPendingDomiciliumCaptured);
                })
               .Permit(ApplicationStateTransitionTrigger.ApplicationMailingAddressCaptureConfirmed, ApplicationState.ApplicationMailingAddressCaptured)
               .Permit(ApplicationStateTransitionTrigger.DomiciliumAddressCaptureConfirmed, ApplicationState.DomiciliumAddressCaptured)
               .Permit(ApplicationStateTransitionTrigger.AssetLiabilityDetailCapturedConfirmed, ApplicationState.AssetLiabilityDetailCaptured);

            machine.Configure(ApplicationState.DomiciliumAddressCaptured).SubstateOf(ApplicationState.AddressStates)
                .PermitReentry(ApplicationStateTransitionTrigger.DomiciliumAddressCaptureConfirmed)
                .OnEntryFrom(DomiciliumAddressCapturedTrigger, correlationId =>
                {
                    RecordStateHistory(correlationId, ApplicationState.DomiciliumAddressCaptured);
                })
               .Permit(ApplicationStateTransitionTrigger.ApplicationMailingAddressCaptureConfirmed, ApplicationState.ApplicationMailingAddressCaptured)
               .Permit(ApplicationStateTransitionTrigger.ClientPendingDomiciliumCaptureConfirmed, ApplicationState.ClientPendingDomiciliumCaptured)
               .Permit(ApplicationStateTransitionTrigger.AssetLiabilityDetailCapturedConfirmed, ApplicationState.AssetLiabilityDetailCaptured);

            machine.Configure(ApplicationState.AssetLiabilityDetailCaptured).SubstateOf(ApplicationState.AddressStates)
                .PermitReentry(ApplicationStateTransitionTrigger.AssetLiabilityDetailCapturedConfirmed)
                 .OnEntryFrom(AssetLiabilityCapturedTrigger, correlationId =>
                 {
                     RecordStateHistory(correlationId, ApplicationState.AssetLiabilityDetailCaptured);
                 })
               .Permit(ApplicationStateTransitionTrigger.ApplicationMailingAddressCaptureConfirmed, ApplicationState.ApplicationMailingAddressCaptured)
               .Permit(ApplicationStateTransitionTrigger.DomiciliumAddressCaptureConfirmed, ApplicationState.DomiciliumAddressCaptured)
               .Permit(ApplicationStateTransitionTrigger.ClientPendingDomiciliumCaptureConfirmed, ApplicationState.ClientPendingDomiciliumCaptured);
        }

        private void InitializeBankAccountPath(SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger> machine)
        {
            machine.Configure(ApplicationState.BankAccountStates)
                .SubstateOf(ApplicationState.NonCriticalPath)
                .Permit(ApplicationStateTransitionTrigger.AllAddressesCaptured, ApplicationState.AllAddressesCaptured)
                .Permit(ApplicationStateTransitionTrigger.AddressCaptureConfirmed, ApplicationState.AddressCaptured)
                .Permit(ApplicationStateTransitionTrigger.ApplicationMailingAddressCaptureConfirmed, ApplicationState.ApplicationMailingAddressCaptured)
                .Permit(ApplicationStateTransitionTrigger.ClientPendingDomiciliumCaptureConfirmed, ApplicationState.ClientPendingDomiciliumCaptured)
                .Permit(ApplicationStateTransitionTrigger.DomiciliumAddressCaptureConfirmed, ApplicationState.DomiciliumAddressCaptured)
                .Permit(ApplicationStateTransitionTrigger.AssetLiabilityDetailCapturedConfirmed, ApplicationState.AssetLiabilityDetailCaptured);

            machine.Configure(ApplicationState.BankAccountCaptured)
                .SubstateOf(ApplicationState.BankAccountStates)
                .OnEntryFrom(BankAccountCapturedTrigger, correlationId =>
                {
                    RecordStateHistory(correlationId, ApplicationState.BankAccountCaptured);
                    if (AllBankAccountsFullyCaptured())
                    {
                        RecordStateHistory(CombGuid.Instance.Generate(), ApplicationState.AllBankAccountsCaptured);
                        machine.Fire(ApplicationStateTransitionTrigger.AllBankAccountsCaptured);
                    }
                })
                .PermitReentry(ApplicationStateTransitionTrigger.BankAccountCaptureConfirmed)
                .Permit(ApplicationStateTransitionTrigger.AllBankAccountsCaptured, ApplicationState.AllBankAccountsCaptured);

            machine.Configure(ApplicationState.AllBankAccountsCaptured)
                .SubstateOf(ApplicationState.BankAccountStates)
                .Permit(ApplicationStateTransitionTrigger.ApplicationDebitOrderCaptureConfirmed, ApplicationState.ApplicationDebitOrderCaptured);

            machine.Configure(ApplicationState.ApplicationDebitOrderCaptured)
                .SubstateOf(ApplicationState.BankAccountStates)
               .OnEntryFrom(ApplicationDebitOrderCapturedTrigger, correlationId =>
                {
                    RecordStateHistory(correlationId, ApplicationState.ApplicationDebitOrderCaptured);
                });
        }

        private void InitializeStandaloneNonCriticalPath(SerialisationFriendlyStateMachine<ApplicationState, ApplicationStateTransitionTrigger> machine)
        {
            machine.Configure(ApplicationState.NonCriticalPath)
               .Permit(ApplicationStateTransitionTrigger.X2CaseCreationConfirmed, ApplicationState.X2CaseCreated)
               .Permit(ApplicationStateTransitionTrigger.ApplicationLinkingToExternalVendorConfirmed, ApplicationState.ApplicationLinkedToExternalVendor)
               .Permit(ApplicationStateTransitionTrigger.ComcorpPropertyCaptureConfirmed, ApplicationState.NonTrackedState)
               .Permit(ApplicationStateTransitionTrigger.AffordabilityDetailCaptureConfirmed, ApplicationState.AffordabilityDetailCaptured)
               .Permit(ApplicationStateTransitionTrigger.DeclarationsCaptureConfirmed, ApplicationState.DeclarationsCaptured)
               .Permit(ApplicationStateTransitionTrigger.NonCriticalErrorReported, ApplicationState.NonCriticalErrorOccured)
               .PermitIf(ApplicationStateTransitionTrigger.CompletionConfirmed, ApplicationState.Completed, HasProcessCompletedWithCriticalPathFullyCaptured);

            machine.Configure(ApplicationState.StandaloneNonCriticalStates)
                .SubstateOf(ApplicationState.NonCriticalPath)
                .Permit(ApplicationStateTransitionTrigger.AllAddressesCaptured, ApplicationState.AllAddressesCaptured)
                .Permit(ApplicationStateTransitionTrigger.AddressCaptureConfirmed, ApplicationState.AddressCaptured)
                .Permit(ApplicationStateTransitionTrigger.ApplicationMailingAddressCaptureConfirmed, ApplicationState.ApplicationMailingAddressCaptured)
                .Permit(ApplicationStateTransitionTrigger.ClientPendingDomiciliumCaptureConfirmed, ApplicationState.ClientPendingDomiciliumCaptured)
                .Permit(ApplicationStateTransitionTrigger.DomiciliumAddressCaptureConfirmed, ApplicationState.DomiciliumAddressCaptured)
                .Permit(ApplicationStateTransitionTrigger.BankAccountCaptureConfirmed, ApplicationState.BankAccountCaptured)
                .Permit(ApplicationStateTransitionTrigger.AllBankAccountsCaptured, ApplicationState.AllBankAccountsCaptured)
                .Permit(ApplicationStateTransitionTrigger.ApplicationDebitOrderCaptureConfirmed, ApplicationState.ApplicationDebitOrderCaptured)
                .Permit(ApplicationStateTransitionTrigger.AssetLiabilityDetailCapturedConfirmed, ApplicationState.AssetLiabilityDetailCaptured);

            machine.Configure(ApplicationState.ValidMinimumApplicationCreated)
                .SubstateOf(ApplicationState.NonCriticalPath)
                .Permit(ApplicationStateTransitionTrigger.BankAccountCaptureConfirmed, ApplicationState.BankAccountCaptured)
                .Permit(ApplicationStateTransitionTrigger.AddressCaptureConfirmed, ApplicationState.AddressCaptured);

            machine.Configure(ApplicationState.ApplicationLinkedToExternalVendor)
                .SubstateOf(ApplicationState.StandaloneNonCriticalStates);

            machine.Configure(ApplicationState.AffordabilityDetailCaptured)
                .SubstateOf(ApplicationState.StandaloneNonCriticalStates)
                .PermitReentry(ApplicationStateTransitionTrigger.AffordabilityDetailCaptureConfirmed)
                .OnEntryFrom(AffordabilityDetailsCapturedTrigger, correlationId =>
                {
                    RecordStateHistory(correlationId, ApplicationState.AffordabilityDetailCaptured);
                });

            machine.Configure(ApplicationState.DeclarationsCaptured)
                .SubstateOf(ApplicationState.StandaloneNonCriticalStates)
                .PermitReentry(ApplicationStateTransitionTrigger.DeclarationsCaptureConfirmed)
                .OnEntryFrom(DeclarationsCapturedTrigger, correlationId =>
                {
                    RecordStateHistory(correlationId, ApplicationState.DeclarationsCaptured);
                });

            machine.Configure(ApplicationState.NonCriticalErrorOccured)
               .SubstateOf(ApplicationState.StandaloneNonCriticalStates)
               .OnEntryFrom(NonCriticalErrorOccuredTrigger, correlationId =>
                {
                    RecordStateHistory(correlationId, ApplicationState.NonCriticalErrorOccured);
                })
                .PermitReentry(ApplicationStateTransitionTrigger.NonCriticalErrorReported)
                .PermitIf(ApplicationStateTransitionTrigger.X2CaseCreationConfirmed, ApplicationState.X2CaseCreated, CriticalPathFullyCaptured)
                .Permit(ApplicationStateTransitionTrigger.AddressCaptureConfirmed, ApplicationState.AddressCaptured)
                .Permit(ApplicationStateTransitionTrigger.BankAccountCaptureConfirmed, ApplicationState.BankAccountCaptured)
                .Permit(ApplicationStateTransitionTrigger.ApplicationDebitOrderCaptureConfirmed, ApplicationState.ApplicationDebitOrderCaptured)
                .Permit(ApplicationStateTransitionTrigger.ApplicationMailingAddressCaptureConfirmed, ApplicationState.ApplicationMailingAddressCaptured)
                .Permit(ApplicationStateTransitionTrigger.AssetLiabilityDetailCapturedConfirmed, ApplicationState.AssetLiabilityDetailCaptured)
                .Permit(ApplicationStateTransitionTrigger.AffordabilityDetailCaptureConfirmed, ApplicationState.AffordabilityDetailCaptured)
                .Permit(ApplicationStateTransitionTrigger.DeclarationsCaptureConfirmed, ApplicationState.DeclarationsCaptured)
                .Permit(ApplicationStateTransitionTrigger.DomiciliumAddressCaptureConfirmed, ApplicationState.DomiciliumAddressCaptured)
                .Permit(ApplicationStateTransitionTrigger.ClientPendingDomiciliumCaptureConfirmed, ApplicationState.ClientPendingDomiciliumCaptured)
                .Permit(ApplicationStateTransitionTrigger.AllBankAccountsCaptured, ApplicationState.AllBankAccountsCaptured)
                .Permit(ApplicationStateTransitionTrigger.AllAddressesCaptured, ApplicationState.AllAddressesCaptured);

            machine.Configure(ApplicationState.X2CaseCreated).SubstateOf(ApplicationState.StandaloneNonCriticalStates)
                .OnEntryFrom(X2CaseCreationTrigger, correlationId =>
                {
                    RecordStateHistory(correlationId, ApplicationState.X2CaseCreated);
                });

            machine.OnTransitioned((transition) =>
            {
                var message = string.Format("State Transitioned from {0} to {1}", transition.Source, transition.Destination);
#if DEBUG
                Console.WriteLine(message);
#endif
            });
        }

        public void CreateStateMachine(ApplicationCreationModel applicationData, Guid domainProcessId)
        {
            this.domainProcessId = domainProcessId;
            CreateStateMachine(applicationData, defaultInitialState);
        }

        private void CreateStateMachine(ApplicationCreationModel applicationData, ApplicationState state)
        {
            SystemMessages = SystemMessageCollection.Empty();
            expectedCriticalStateFrequencyChain = new Dictionary<ApplicationState, int>();
            fullBankAccountExpectedStateFrequencyChain = new Dictionary<ApplicationState, int>();
            fullAddressExpectedStateFrequencyChain = new Dictionary<ApplicationState, int>();
            fullAssetLiabilityDetailExpectedStateFrequencyChain = new Dictionary<ApplicationState, int>();
            sentCommands = new Dictionary<Guid, Tuple<Type, bool>>();
            statesBreadCrumb = new Dictionary<Guid, ApplicationState>();
            ClientCollection = new Dictionary<string, int>();
            ErrorStates = new List<ApplicationState>();

            RecordStateHistory(Guid.NewGuid(), state);
            machine = InitializeMachine(state);
            SetApplicationStatesExpectations(applicationData);
        }

        public bool ContainsStateInBreadCrumb(ApplicationState state)
        {
            return statesBreadCrumb.Values.Contains(state);
        }

        public bool HasProcessCompletedWithCriticalPathFullyCaptured()
        {
            bool hasCompleted = false;

            bool allEventsReceived = sentCommands.All(x => x.Value.Item2);

            if (CriticalPathFullyCaptured() && allEventsReceived)
            {
                hasCompleted = true;
            }
            return hasCompleted;
        }

        public bool IsInState(ApplicationState state)
        {
            return Machine.State == state;
        }

        public void AggregateMessages(ISystemMessageCollection systemMessages)
        {
            this.SystemMessages.Aggregate(systemMessages);
        }

        public void RecordCommandSent(Type commandType, Guid commandCorrelationGuid)
        {
            sentCommands.Add(commandCorrelationGuid, new Tuple<Type, bool>(commandType, false));
        }

        public void RecordCommandFailed(Guid commandCorrelationGuid)
        {
            RecordCommandHasReturned(commandCorrelationGuid);
        }

        public void RecordEventReceived(Guid commandCorrelationGuid)
        {
            RecordCommandHasReturned(commandCorrelationGuid);
        }

        private void RecordCommandHasReturned(Guid commandCorrelationGuid)
        {
            Tuple<Type, bool> sentCommand;
            if (sentCommands.TryGetValue(commandCorrelationGuid, out sentCommand))
            {
                sentCommands[commandCorrelationGuid] = new Tuple<Type, bool>(sentCommand.Item1, true);
            }
        }

        private void RecordStateHistory(Guid guid, ApplicationState state)
        {
            if (!statesBreadCrumb.ContainsKey(guid))
            {
                statesBreadCrumb.Add(guid, state);
                serializationStateSnapshot = state;
            }
        }

        private void SetApplicationStatesExpectations(ApplicationCreationModel applicationCreation)
        {
            expectedCriticalStateFrequencyChain.Add(ApplicationState.BasicApplicationCreated, 1);
            expectedCriticalStateFrequencyChain.Add(ApplicationState.ApplicantAdded, applicationCreation.ApplicantCount);
            expectedCriticalStateFrequencyChain.Add(ApplicationState.ApplicationEmploymentDetermined, 1);
            expectedCriticalStateFrequencyChain.Add(ApplicationState.ApplicationHouseHoldIncomeDetermined, 1);
            expectedCriticalStateFrequencyChain.Add(ApplicationState.ApplicationPriced, 1);
            expectedCriticalStateFrequencyChain.Add(ApplicationState.ApplicationFunded, 1);

            foreach (ApplicantModel applicantData in applicationCreation.Applicants)
            {
                SetUpAssetDetailExpectedStateFrequencyChain(applicantData);
                SetupExpectedClientEmploymentAddedFrequency(applicantData);
                SetupExpectedBankAccountStateFrequency(applicantData);
                SetupExpectedAddressStateFrequency(applicantData);
            }
        }

        private void SetUpAssetDetailExpectedStateFrequencyChain(ApplicantModel applicantData)
        {
            if (fullAssetLiabilityDetailExpectedStateFrequencyChain.ContainsKey(ApplicationState.AssetLiabilityDetailCaptured))
            {
                fullAssetLiabilityDetailExpectedStateFrequencyChain[ApplicationState.AssetLiabilityDetailCaptured] += applicantData.ApplicantAssetLiabilities.Count();
            }
            else
            {
                fullAssetLiabilityDetailExpectedStateFrequencyChain.Add(ApplicationState.AssetLiabilityDetailCaptured, applicantData.ApplicantAssetLiabilities.Count());
            }
        }

        private void SetupExpectedClientEmploymentAddedFrequency(ApplicantModel applicantData)
        {
            if (expectedCriticalStateFrequencyChain.ContainsKey(ApplicationState.EmploymentAdded))
            {
                expectedCriticalStateFrequencyChain[ApplicationState.EmploymentAdded] += applicantData.Employments.Count();
            }
            else
            {
                expectedCriticalStateFrequencyChain.Add(ApplicationState.EmploymentAdded, applicantData.Employments.Count());
            }
        }

        private void SetupExpectedAddressStateFrequency(ApplicantModel applicantData)
        {
            if (fullAddressExpectedStateFrequencyChain.ContainsKey(ApplicationState.AddressCaptured))
            {
                fullAddressExpectedStateFrequencyChain[ApplicationState.AddressCaptured] += applicantData.Addresses.Count();
            }
            else
            {
                fullAddressExpectedStateFrequencyChain.Add(ApplicationState.AddressCaptured, applicantData.Addresses.Count());
            }
        }

        private void SetupExpectedBankAccountStateFrequency(ApplicantModel applicantData)
        {
            if (fullBankAccountExpectedStateFrequencyChain.ContainsKey(ApplicationState.BankAccountCaptured))
            {
                fullBankAccountExpectedStateFrequencyChain[ApplicationState.BankAccountCaptured] += applicantData.BankAccounts.Count();
            }
            else
            {
                fullBankAccountExpectedStateFrequencyChain.Add(ApplicationState.BankAccountCaptured, applicantData.BankAccounts.Count());
            }
        }

        private bool CriticalPathFullyCaptured()
        {
            return FrequenciesExpectationMatched(expectedCriticalStateFrequencyChain, statesBreadCrumb);
        }

        private bool AllBankAccountsFullyCaptured()
        {
            return FrequenciesExpectationMatched(fullBankAccountExpectedStateFrequencyChain, statesBreadCrumb);
        }

        private bool AllAddressesFullyCaptured()
        {
            return FrequenciesExpectationMatched(fullAddressExpectedStateFrequencyChain, statesBreadCrumb);
        }

        public bool ClientEmploymentsFullyCaptured()
        {
            return FrequenciesExpectationMatched(expectedCriticalStateFrequencyChain.Where(
                x => x.Key == ApplicationState.EmploymentAdded).ToDictionary(t => t.Key, t => t.Value), statesBreadCrumb);
        }

        public bool AllLeadApplicantsHaveBeenAdded()
        {
            return FrequenciesExpectationMatched(expectedCriticalStateFrequencyChain.Where(
                x => x.Key == ApplicationState.ApplicantAdded).ToDictionary(t => t.Key, t => t.Value), statesBreadCrumb);
        }

        private bool FrequenciesExpectationMatched(Dictionary<ApplicationState, int> expectedStateFrequencies, Dictionary<Guid, ApplicationState> actualStateOccurances)
        {
            foreach (var key in expectedStateFrequencies.Keys)
            {
                var expectationMatched = actualStateOccurances.Values.Count(s => s == key) == expectedStateFrequencies[key];
                if (!expectationMatched)
                {
                    return false;
                }
            }
            return true;
        }

        public void AdjustStateExpectations(ApplicationState state)
        {
            if (fullAddressExpectedStateFrequencyChain.ContainsKey(state))
            {
                fullAddressExpectedStateFrequencyChain[state] -= 1;
            }

            if (fullBankAccountExpectedStateFrequencyChain.ContainsKey(state))
            {
                fullBankAccountExpectedStateFrequencyChain[state] -= 1;
            }

            if (fullAssetLiabilityDetailExpectedStateFrequencyChain.ContainsKey(state))
            {
                fullAssetLiabilityDetailExpectedStateFrequencyChain[state] -= 1;
            }
        }
    }
}