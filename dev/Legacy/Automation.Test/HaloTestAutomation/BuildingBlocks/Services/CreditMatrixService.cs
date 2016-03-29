using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using System.Collections.Generic;

namespace BuildingBlocks.Services
{
    public class CreditMatrixService : _2AMDataHelper, ICreditMatrixService
    {
        /// <summary>
        /// Gets a list of the link rates for a credit matrix
        /// </summary>
        /// <returns></returns>
        public List<Automation.DataModels.LinkRates> GetLatestCreditMatrixMargins()
        {
            int creditMatrixKey = base.GetLatestCreditMatrixKey();
            return base.GetCreditMatrixMargins(creditMatrixKey);
        }
    }
}