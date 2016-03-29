using SAHL.VSExtensions.Interfaces.Configuration;

namespace SAHL.VSExtensions.Interfaces
{
    public interface IControlManager
    {
        object GetUserControlForMenuItem(IMenuItem menuItem);
    }
}