using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.Interfaces.SearchCriteria
{
    public interface IClientSuperSearchCriteria
    {
        string SearchText { get; }

        string AccountType { get; }

        string LegalEntityTypes { get; }

        SAHLPrincipal principal { get; }
    }
}