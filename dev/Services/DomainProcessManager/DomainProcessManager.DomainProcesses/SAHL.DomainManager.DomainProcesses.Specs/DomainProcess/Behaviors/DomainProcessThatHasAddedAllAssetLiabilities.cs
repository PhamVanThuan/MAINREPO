using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Behaviors
{
    [Behaviors]
    public class DomainProcessThatHasAddedAllAssetLiabilities
    {
        protected static IClientDomainServiceClient clientDomainService;
        protected static int clientKey;
        protected static List<ApplicantAssetLiabilityModel> assetLiabilities;
        protected static int fixedPropertyAddressKey;
        private It should_add_fixed_long_term_investment = () =>
        {
            var fixedInvestment = assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.FixedLongTermInvestment);
            clientDomainService.WasToldTo(x => x.PerformCommand(
                Param<AddFixedLongTermInvestmentLiabilityToClientCommand>.Matches(m =>
                    m.ClientKey == clientKey &&
                    m.FixedLongTermInvestmentLiabilityModel.CompanyName == fixedInvestment.CompanyName &&
                    m.FixedLongTermInvestmentLiabilityModel.LiabilityValue == fixedInvestment.LiabilityValue), 
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_add_liability_loan = () =>
        {
            var liabilityLoan = assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.LiabilityLoan);
            clientDomainService.WasToldTo(x => x.PerformCommand(
                Param<AddLiabilityLoanToClientCommand>.Matches(m => 
                m.ClientKey == clientKey &&
                m.LiabilityLoan.DateRepayable == liabilityLoan.Date &&
                m.LiabilityLoan.FinancialInstitution == liabilityLoan.CompanyName &&
                m.LiabilityLoan.LiabilityValue == liabilityLoan.LiabilityValue &&
                m.LiabilityLoan.LoanType == liabilityLoan.AssetLiabilitySubType
                ),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_add_liability_surety = () =>
        {
            var liabilitySurety = assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.LiabilitySurety);
            clientDomainService.WasToldTo(x => x.PerformCommand(
                Param<AddLiabilitySuretyToClientCommand>.Matches(m => 
                m.ClientKey == clientKey &&
                m.LiabilitySuretyModel.Description == liabilitySurety.Description &&
                m.LiabilitySuretyModel.AssetValue == liabilitySurety.AssetValue &&
                m.LiabilitySuretyModel.LiabilityValue == liabilitySurety.LiabilityValue),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_add_other_asset = () =>
        {
            var otherAsset = assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.OtherAsset);
            clientDomainService.WasToldTo(x => x.PerformCommand(
                Param<AddOtherAssetToClientCommand>.Matches(m => 
                m.ClientKey == clientKey &&
                m.OtherAsset.AssetValue== otherAsset.AssetValue &&
                m.OtherAsset.Description == otherAsset.Description &&
                m.OtherAsset.LiabilityValue == otherAsset.LiabilityValue),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_add_life_assurance = () =>
        {
            var lifeAssurance = assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.LifeAssurance);
            clientDomainService.WasToldTo(x => x.PerformCommand(
                Param<AddLifeAssuranceAssetToClientCommand>.Matches(m => 
                m.ClientKey == clientKey &&
                m.LifeAssuranceAsset.CompanyName == lifeAssurance.CompanyName &&
                m.LifeAssuranceAsset.SurrenderValue == lifeAssurance.AssetValue),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_add_fixed_property = () =>
        {
            var fixedPropertyAsset = assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.FixedProperty) as ApplicantFixedPropertyAssetModel;
            clientDomainService.WasToldTo(x => x.PerformCommand(
                Param<AddFixedPropertyAssetToClientCommand>.Matches(m => 
                m.ClientKey == clientKey &&
                m.FixedPropertyAsset.AddressKey == fixedPropertyAddressKey &&
                m.FixedPropertyAsset.AssetValue == fixedPropertyAsset.AssetValue &&
                m.FixedPropertyAsset.DateAquired == fixedPropertyAsset.Date &&
                m.FixedPropertyAsset.LiabilityValue == fixedPropertyAsset.LiabilityValue),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_add_listed_investment = () =>
        {
            var listedInvestment = assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.ListedInvestments);
            clientDomainService.WasToldTo(x => x.PerformCommand(Param<AddInvestmentAssetToClientCommand>.Matches(m => 
                m.ClientKey == clientKey &&
                m.InvestmentAsset.AssetValue == listedInvestment.AssetValue &&
                m.InvestmentAsset.CompanyName == listedInvestment.CompanyName &&
                m.InvestmentAsset.InvestmentType == AssetInvestmentType.ListedInvestments),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_add_unlisted_investment = () =>
        {
            var unlistedInvestment = assetLiabilities.First(x => x.AssetLiabilityType == AssetLiabilityType.UnlistedInvestments);
            clientDomainService.WasToldTo(x => x.PerformCommand(Param<AddInvestmentAssetToClientCommand>.Matches(m =>
                m.ClientKey == clientKey &&
                m.InvestmentAsset.AssetValue == unlistedInvestment.AssetValue &&
                m.InvestmentAsset.CompanyName == unlistedInvestment.CompanyName &&
                m.InvestmentAsset.InvestmentType == AssetInvestmentType.UnlistedInvestments),
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}