using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface ICreditMatrixService
    {
        int GetLatestCreditMatrixKey();

        List<Automation.DataModels.LinkRates> GetCreditMatrixMargins(int creditMatrixKey);

        List<Automation.DataModels.LinkRates> GetLatestCreditMatrixMargins();

        string GetCategoryDescription(int categoryKey);
    }
}