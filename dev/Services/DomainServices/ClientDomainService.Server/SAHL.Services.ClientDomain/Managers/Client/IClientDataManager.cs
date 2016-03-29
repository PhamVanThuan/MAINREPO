using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ClientDomain.Managers
{
    public interface IClientDataManager
    {
        LegalEntityDataModel FindExistingClient(int clientKey);

        int AddNewLegalEntity(LegalEntityDataModel legalEntity);

        void UpdateLegalEntity(LegalEntityDataModel legalEntity);

        LegalEntityDataModel FindExistingClientByIdNumber(string identityNumber);

        LegalEntityDataModel FindExistingClientByPassportNumber(string passportNumber);

        IEnumerable<int> FindOpenAccountKeysForClient(int clientKey);

        IEnumerable<int> FindOpenApplicationNumbersForClient(int clientKey);

        void AddNewMarketingOptions(LegalEntityMarketingOptionDataModel marketingOptions);

        bool DoesClientMarketingOptionExist(int clientKey, int marketingOptionKey);
    }
}
