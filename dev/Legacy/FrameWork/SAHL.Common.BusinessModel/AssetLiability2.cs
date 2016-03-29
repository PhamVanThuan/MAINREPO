using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class AssetLiability : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AssetLiability_DAO>, IAssetLiability
    {
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("AssetValueMinimumValue");
            Rules.Add("AssetLiabilityDateAcquiredMax");
            Rules.Add("AssetLiabilitySurrenderValueMin");
            Rules.Add("AssetLiabilityLiabilityValueMin");
        }

        ///// <summary>
        /////
        ///// <summary>
        ///// <param name="args"><see cref="ICancelDomainArgs"/></param>
        ///// <param name="Item"></param>
        //protected void OnLegalEntityAssetLiabilities_BeforeAdd(ICancelDomainArgs args, object Item)
        //{
        //}

        ///// <summary>
        /////
        ///// <summary>
        ///// <param name="args"><see cref="ICancelDomainArgs"/></param>
        ///// <param name="Item"></param>
        //protected void OnLegalEntityAssetLiabilities_BeforeRemove(ICancelDomainArgs args, object Item)
        //{
        //}

        ///// <summary>
        /////
        ///// <summary>
        ///// <param name="args"><see cref="ICancelDomainArgs"/></param>
        ///// <param name="Item"></param>
        //protected void OnLegalEntityAssetLiabilities_AfterAdd(ICancelDomainArgs args, object Item)
        //{
        //}

        ///// <summary>
        /////
        ///// <summary>
        ///// <param name="args"><see cref="ICancelDomainArgs"/></param>
        ///// <param name="Item"></param>
        //protected void OnLegalEntityAssetLiabilities_AfterRemove(ICancelDomainArgs args, object Item)
        //{
        //}
    }
}