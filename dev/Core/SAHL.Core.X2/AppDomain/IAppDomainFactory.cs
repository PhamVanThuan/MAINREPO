using SAHL.Core.Data.Models.X2;

namespace SAHL.Core.X2.AppDomain
{
    public interface IAppDomainFactory
    {
        System.AppDomain Create(ProcessDataModel processDataModel, string directory, string directoryFullPath);
    }
}