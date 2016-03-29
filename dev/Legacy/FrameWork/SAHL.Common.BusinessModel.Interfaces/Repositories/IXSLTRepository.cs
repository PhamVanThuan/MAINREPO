using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IXSLTRepository
    {
        IXSLTransformation GetLatestXSLTransformation(GenericKeyTypes genericKeyType);
    }
}