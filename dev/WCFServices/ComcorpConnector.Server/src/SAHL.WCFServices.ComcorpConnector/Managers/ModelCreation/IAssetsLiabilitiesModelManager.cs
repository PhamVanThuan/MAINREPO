using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public interface IAssetsLiabilitiesModelManager
    {
        List<ApplicantAssetLiabilityModel> PopulateAssets(List<AssetItem> comcorpAssetItems);

        List<ApplicantAssetLiabilityModel> PopulateLiabilities(List<LiabilityItem> comcorpLiabilityItems);
    }
}