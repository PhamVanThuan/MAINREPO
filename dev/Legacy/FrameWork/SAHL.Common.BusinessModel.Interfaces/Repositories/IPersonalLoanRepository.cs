using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IPersonalLoanRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="term"></param>
        /// <param name="userid"></param>
        void ChangeTerm(int accountKey, int term, string userid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="userId"></param>
        void ChangeInstalment(int accountKey, string userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchServiceType"></param>
        /// <returns></returns>
        List<BatchServiceResult> GetBatchServiceResults(BatchServiceTypes batchServiceType);
    }
}
