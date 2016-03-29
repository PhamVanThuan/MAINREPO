using SAHL.Core.BusinessModel;

namespace SAHL.Core.UI.Providers
{
    public interface ISqlDataProvider
    {
        dynamic GetData(BusinessKey businessKey);
    }
}