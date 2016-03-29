using SAHL.Common.Collections.Interfaces;
using SAHL.X2.Common;

namespace X2DomainService.Interface.LoanAdjustments
{
    public interface ILoanAdjustments : IX2WorkflowService
    {
        /// <summary>
        /// Perform the term change
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="accountKey"></param>
        /// <param name="instanceID"></param>
        /// <param name="ignoreWarnings"></param>
        /// <returns></returns>
        bool ApproveTermChangeRequest(IDomainMessageCollection messages, int accountKey, long instanceID, bool ignoreWarnings);

        /// <summary>
        /// check if term change is permitted
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="accountKey"></param>
        /// <param name="instanceID"></param>
        /// <param name="ignoreWarnings"></param>
        /// <returns></returns>
        bool CheckIfCanApproveTermChangeRules(IDomainMessageCollection messages, int accountKey, long instanceID, bool ignoreWarnings);
    }
}