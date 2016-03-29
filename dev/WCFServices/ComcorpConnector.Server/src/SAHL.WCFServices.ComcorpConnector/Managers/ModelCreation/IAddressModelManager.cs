using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public interface IAddressModelManager
    {
        List<AddressModel> PopulateAddresses(Applicant comcorpApplicant, ResidentialAddressModel propertyAddress, List<AssetItem> comcorpAssetItems);

        ResidentialAddressModel PopulateResidentialAddressFromComcorpProperty(Property comcorpProperty);

        PropertyAddressModel PopulatePropertyAddressFromComcorpProperty(Property comcorpProperty);
    }
}