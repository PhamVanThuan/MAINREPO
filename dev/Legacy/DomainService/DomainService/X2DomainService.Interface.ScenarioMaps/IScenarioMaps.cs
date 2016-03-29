using SAHL.Common.Collections.Interfaces;
using SAHL.X2.Common;

namespace X2DomainService.Interface.ScenarioMaps
{
    public interface IScenarioMaps : IX2WorkflowService
    {
        void ThrowDAO_Exception(IDomainMessageCollection messages, bool ignoreWarnings);

        void ThrowSqlException(IDomainMessageCollection messages, bool ignoreWarnings);

        void ThrowSqlTimeOutException(IDomainMessageCollection messages, bool ignoreWarnings);

        void ThrowDomainValidationException(IDomainMessageCollection messages, bool ignoreWarnings);

        void ThrowDomainMessageException(IDomainMessageCollection messages, bool ignoreWarnings);

        void ThrowExceptionWithMessages(IDomainMessageCollection messages, bool ignoreWarnings);

        void ThrowExceptionWithoutMessages(IDomainMessageCollection messages, bool ignoreWarnings);

        void ThrowSqlApiException(IDomainMessageCollection messages, bool ignoreWarnings);
    }
}