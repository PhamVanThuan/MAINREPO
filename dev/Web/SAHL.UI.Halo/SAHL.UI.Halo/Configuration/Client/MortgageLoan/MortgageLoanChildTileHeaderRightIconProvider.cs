using SAHL.UI.Halo.Models.Client.MortgageLoan;
using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Configuration.Client.MortgageLoan
{
    public class MortgageLoanChildTileHeaderRightIconProvider : HaloTileHeaderIconProviderBase<MortgageLoanChildTileHeaderConfiguration>
    {
        public MortgageLoanChildTileHeaderRightIconProvider()
            : base(HaloTileIconAlignment.Right)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            MortgageLoanChildModel mortgageLoadTileModel = dataModel as MortgageLoanChildModel;
            if (mortgageLoadTileModel == null)
            {
                throw new Exception("Unexpected Data Model received");
            }

            this.HeaderIcons.Add(string.Format("icon-acc-mortgage-{0}", mortgageLoadTileModel.AccountStatus).ToLower());
            if (mortgageLoadTileModel.OriginationSource == Core.BusinessModel.Enums.OriginationSource.SAHomeLoans)
            {
                this.HeaderIcons.Add("icon-originationsource_sahl");
            }
        }
    }
}