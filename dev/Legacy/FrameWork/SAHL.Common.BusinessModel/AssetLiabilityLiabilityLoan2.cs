using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class AssetLiabilityLiabilityLoan : AssetLiability, IAssetLiabilityLiabilityLoan
    {
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            //Rules.Add("AssetLiabilityLoanLiabilityMin");
        }
    }
}