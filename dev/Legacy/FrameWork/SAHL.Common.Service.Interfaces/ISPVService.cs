using System.Collections.Generic;
using SAHL.Common.BusinessModel.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.Service.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface ISPVService
    {
        ///// <summary>
        ///// Get the SPV for the criteria
        ///// </summary>
        ///// <param name="LTV"></param>
        ///// <returns></returns>
        //int GetSPVByLTV(double LTV);

        ///// <summary>
        ///// Return a list of SPV's that are valid for further lending to be disbursed into.
        ///// </summary>
        ///// <returns></returns>
        IList<ISPV> GetSPVListForFurtherLending();

        //int DetermineSPVForTermChange(int spvCurrent);

        ISPV GetSPVDetails(int SPVKey);

        //bool IsSPVWithinMaxTerm(int originationsourcekey, int term, int spv, int spvcompanykey);

        /// <summary>
        /// Get SPV that doesn't rely on persisted data
        /// Pass the parameters in and it does the magic
        /// </summary>
        /// <param name="application"></param>
        void DetermineSPVOnApplication(IApplication application);

        /// <summary>
        ///
        /// </summary>
        /// <param name="spvDetermineParameters"></param>
        /// <param name="genericKey"></param>
        /// <param name="spvDetermineSource"></param>
        /// <param name="currentSPVKey"></param>
        /// <returns></returns>
        ISPV GetSPVByParameters(IRow spvDetermineParameters, int genericKey, SPVDetermineSources spvDetermineSource, int currentSPVKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="loanAmount"></param>
        /// <param name="valuation"></param>
        /// <returns></returns>
        ISPV DetermineSPVForFurtherLending(int accountKey, double loanAmount, double valuation);

    }
}