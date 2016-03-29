using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.DomainProcess;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
        where T : ApplicationCreationModel
    {
        protected void ConditionallyAddAssetsToClient(IApplicationStateMachine applicationStateMachine)
        {
            foreach (var applicant in this.DataModel.Applicants)
            {
                var clientKey = applicationStateMachine.ClientCollection[applicant.IDNumber];
                foreach (var assetLiability in applicant.ApplicantAssetLiabilities)
                {
                    Guid correlationId = combGuidGenerator.Generate();
                    IClientDomainCommand command = null;
                    try
                    {
                        ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();
                        command = GetCommand(clientKey, assetLiability, ref systemMessages);
                        if (command != null)
                        {
                            var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, correlationId);
                            systemMessages = this.clientDomainService.PerformCommand(command, serviceRequestMetadata);
                        }
                        CheckForNonCriticalErrors(applicationStateMachine, correlationId, systemMessages, ApplicationState.AssetLiabilityDetailCaptured);
                    }
                    catch (Exception runtimeException)
                    {
                        var friendlyErrorMessage = String.Format("Asset or Liability of type \"{0}\" could not be saved for applicant with ID Number: {1}.",
                                                                    assetLiability.AssetLiabilityType.ToString(), applicant.IDNumber);
                        HandleNonCriticalException(runtimeException, friendlyErrorMessage, correlationId, applicationStateMachine);
                    }
                }
            }
        }

        private IClientDomainCommand GetCommand(int clientKey, ApplicantAssetLiabilityModel assetLiability, ref ISystemMessageCollection messages)
        {
            IClientDomainCommand command = null;
            switch (assetLiability.AssetLiabilityType)
            {
                case AssetLiabilityType.FixedLongTermInvestment:
                    command = GetFixedLongTermInvestmentCommand(assetLiability as ApplicantFixedLongTermLiabilityModel, clientKey);
                    break;

                case AssetLiabilityType.FixedProperty:
                    command = GetFixedPropertyAssetCommand(assetLiability as ApplicantFixedPropertyAssetModel, clientKey, messages);
                    break;

                case AssetLiabilityType.LifeAssurance:
                    command = GetLifeAssuranceAssetCommand(assetLiability as ApplicantLifeAssuranceAssetModel, clientKey);
                    break;

                case AssetLiabilityType.OtherAsset:
                    command = GetOtherAssetCommand(assetLiability as ApplicantOtherAssetModel, clientKey);
                    break;

                case AssetLiabilityType.ListedInvestments:
                case AssetLiabilityType.UnlistedInvestments:
                    command = GetListedInvestmentCommand(assetLiability, clientKey);
                    break;

                case AssetLiabilityType.LiabilityLoan:
                    command = GetLiabilityLoanCommand(assetLiability as ApplicantLiabilityLoanModel, clientKey);
                    break;

                case AssetLiabilityType.LiabilitySurety:
                    command = GetLiabilitySuretyCommand(assetLiability as ApplicantLiabilitySuretyModel, clientKey);
                    break;

                default:
                    break;
            }

            return command;
        }

        private AddFixedLongTermInvestmentLiabilityToClientCommand GetFixedLongTermInvestmentCommand(ApplicantFixedLongTermLiabilityModel assetLiability, int clientKey)
        {
            var fixedLongTermInvestmentLiabilityModel = new FixedLongTermInvestmentLiabilityModel(assetLiability.CompanyName, assetLiability.LiabilityValue.Value);
            var command = new AddFixedLongTermInvestmentLiabilityToClientCommand(fixedLongTermInvestmentLiabilityModel, clientKey);
            return command;
        }

        private AddLiabilityLoanToClientCommand GetLiabilityLoanCommand(ApplicantLiabilityLoanModel assetLiability, int clientKey)
        {
            var liabilityLoan = new LiabilityLoanModel(assetLiability.AssetLiabilitySubType.Value, assetLiability.CompanyName, assetLiability.Date.Value,
                assetLiability.Cost.Value, assetLiability.LiabilityValue.Value);
            var command = new AddLiabilityLoanToClientCommand(clientKey, liabilityLoan);
            return command;
        }

        private AddLiabilitySuretyToClientCommand GetLiabilitySuretyCommand(ApplicantLiabilitySuretyModel assetLiability, int clientKey)
        {
            var liabilitySurety = new LiabilitySuretyModel(assetLiability.AssetValue.Value, assetLiability.LiabilityValue.Value, assetLiability.Description);
            var command = new AddLiabilitySuretyToClientCommand(liabilitySurety, clientKey);
            return command;
        }

        private AddInvestmentAssetToClientCommand GetListedInvestmentCommand(ApplicantAssetLiabilityModel assetLiability, int clientKey)
        {
            var assetInvestmentType = GetAssetInvestmentType(assetLiability.AssetLiabilityType);
            var investmentAssetModel = new InvestmentAssetModel(assetInvestmentType, assetLiability.CompanyName, assetLiability.AssetValue.Value);
            var command = new AddInvestmentAssetToClientCommand(clientKey, investmentAssetModel);
            return command;
        }

        private AddOtherAssetToClientCommand GetOtherAssetCommand(ApplicantOtherAssetModel assetLiability, int clientKey)
        {
            var otherAssetModel = new OtherAssetModel(assetLiability.Description, assetLiability.AssetValue.Value, assetLiability.LiabilityValue.Value);
            var command = new AddOtherAssetToClientCommand(clientKey, otherAssetModel);
            return command;
        }

        private AddLifeAssuranceAssetToClientCommand GetLifeAssuranceAssetCommand(ApplicantLifeAssuranceAssetModel assetLiability, int clientKey)
        {
            var lifeAssuranceAssetModel = new LifeAssuranceAssetModel(assetLiability.CompanyName, assetLiability.AssetValue.Value);
            var command = new AddLifeAssuranceAssetToClientCommand(clientKey, lifeAssuranceAssetModel);
            return command;
        }

        private AddFixedPropertyAssetToClientCommand GetFixedPropertyAssetCommand(ApplicantFixedPropertyAssetModel assetLiability, int clientKey, ISystemMessageCollection systemMessages)
        {
            FixedPropertyAssetModel fixedPropertyAssetModel;
            var clientAddress = GetClientFreeTextAddress(assetLiability.Address, clientKey);
            if (clientAddress != null)
            {
                fixedPropertyAssetModel = new FixedPropertyAssetModel(assetLiability.Date.Value, clientAddress.AddressKey, assetLiability.AssetValue.Value, assetLiability.LiabilityValue.Value);
                var command = new AddFixedPropertyAssetToClientCommand(clientKey, fixedPropertyAssetModel);
                return command;
            }
            else
            {
                systemMessages.AddMessage(new SystemMessage(
                    String.Format("Failed to add the Fixed Property Asset. Address is not linked to client {0}", clientKey),
                    SystemMessageSeverityEnum.Error));
                return null;
            }
        }

        private AssetInvestmentType GetAssetInvestmentType(AssetLiabilityType assetLiabilityType)
        {
            return (AssetInvestmentType)(int)assetLiabilityType;
        }
    }
}