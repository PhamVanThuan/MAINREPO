using System.Collections.Generic;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IRecoveriesRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IRecoveriesProposal CreateEmptyRecoveriesProposal();

        /// <summary>
        ///
        /// </summary>
        /// <param name="recoveriesProposal"></param>
        void SaveRecoveriesProposal(IRecoveriesProposal recoveriesProposal);

        /// <summary>f
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IRecoveriesProposal GetRecoveriesProposalByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="generalStatus"></param>
        /// <returns></returns>
        List<IRecoveriesProposal> GetRecoveriesProposalsByAccountKey(int accountKey, GeneralStatuses generalStatus);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        List<IRecoveriesProposal> GetRecoveriesProposalsByAccountKey(int accountKey);

        /// <summary>f
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        IRecoveriesProposal GetActiveRecoveriesProposalByAccountKey(int accountKey);
    }
}