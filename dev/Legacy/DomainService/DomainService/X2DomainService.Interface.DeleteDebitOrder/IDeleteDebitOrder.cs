using SAHL.Common.Collections.Interfaces;
using SAHL.X2.Common;

namespace X2DomainService.Interface.DeleteDebitOrder
{
    public interface IDeleteDebitOrder : IX2WorkflowService
    {
        bool OnCompleteActivity_Approve_Request(IDomainMessageCollection messages, int debitOrderKey, bool ignoreWarnings);
    }
}