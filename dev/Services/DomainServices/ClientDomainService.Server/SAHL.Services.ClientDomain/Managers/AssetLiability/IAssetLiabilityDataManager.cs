using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.ClientDomain.Managers.AssetLiability
{
    public interface IAssetLiabilityDataManager
    {
        bool CheckIsAddressLinkedToClientAsFixedPropertyAsset(int clientKey, int addressKey);

        int SaveFixedPropertyAsset(FixedPropertyAssetModel fixedPropertyAsset);

        int LinkAssetLiabilityToClient(int clientKey, int assetLiabilityKey);

        int SaveFixedLongTermInvestmentLiability(FixedLongTermInvestmentLiabilityModel fixedLongTermInvestmentLiability);

        int SaveLiabilityLoan(LiabilityLoanModel liabilityLoan);

        int SaveInvestmentAsset(InvestmentAssetModel investmentAsset);

        int SaveLifeAssuranceAsset(LifeAssuranceAssetModel lifeAssuranceAsset);

        int SaveOtherAsset(OtherAssetModel otherAsset);

        int SaveLiabilitySurety(LiabilitySuretyModel liabilitySuretyModel);
    }
}