using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class DeleteActiveExternalActivityCommand : ServiceCommand
    {
        public int ActiveExternalActivityID { get; set; }

        public DeleteActiveExternalActivityCommand(int activeExternalActivityID)
        {
            this.ActiveExternalActivityID = activeExternalActivityID;
        }
    }
}