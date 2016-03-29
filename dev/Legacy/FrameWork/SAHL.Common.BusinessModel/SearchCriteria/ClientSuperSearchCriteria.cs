using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.SearchCriteria
{
    public class ClientSuperSearchCriteria : IClientSuperSearchCriteria
    {
        string _searchText = "";
        string _accountType = "";
        string _legalEntityTypes = "";
        SAHLPrincipal _principal;

        public ClientSuperSearchCriteria(string SearchText, string AccountType, string LegalEntityTypes, SAHLPrincipal principal)
        {
            _searchText = SearchText;
            _accountType = AccountType;
            _legalEntityTypes = LegalEntityTypes;
            _principal = principal;
        }

        #region IClientSuperSearchCriteria Members

        public string SearchText
        {
            get { return _searchText; }
        }

        public string AccountType
        {
            get { return _accountType; }
        }

        public string LegalEntityTypes
        {
            get { return _legalEntityTypes; }
        }

        public SAHLPrincipal principal
        {
            get { return _principal; }
        }

        #endregion
    }
}
