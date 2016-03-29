using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.Models;
using System;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public class ApplicationMailingAddressModelManager : IApplicationMailingAddressModelManager
    {
        public ApplicationMailingAddressModel PopulateApplicationMailingAddress(DomainProcessManager.Models.AddressModel address, int? clientKey)
        {
            var applicationMailingAddress = new ApplicationMailingAddressModel(
                    address, CorrespondenceLanguage.English, OnlineStatementFormat.NotApplicable,
                    CorrespondenceMedium.Email, clientKey, false);
            return applicationMailingAddress;
        }
    }
}