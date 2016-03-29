using SAHL.Common.Collections.Interfaces;
using SAHL.X2.Common;

namespace X2DomainService.Interface.HelpDesk
{
    public interface IHelpDesk : IX2WorkflowService
    {
        bool X2AutoArchive2AM_Update(IDomainMessageCollection messages, int helpDeskQueryKey);

        string CreateRequest(IDomainMessageCollection messages, int legalEntityKey);
    }
}