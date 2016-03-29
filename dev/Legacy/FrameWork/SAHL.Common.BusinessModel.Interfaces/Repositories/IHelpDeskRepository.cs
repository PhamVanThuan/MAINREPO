using System;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IHelpDeskRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        IReadOnlyEventList<IHelpDeskQuery> GetHelpDeskQueryByInstanceID(Int64 instanceID);

        /// <summary>
        ///
        /// </summary>
        /// <param name="helpDeskQueryKey"></param>
        /// <returns></returns>
        IReadOnlyEventList<IHelpDeskQuery> GetHelpDeskQueryByHelpDeskQueryKey(int helpDeskQueryKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        IReadOnlyEventList<IHelpDeskQuery> GetHelpDeskCallSummaryByLegalEntityKey(int legalEntityKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="hdDict"></param>
        /// <returns></returns>
        void UpdateX2HelpDeskData(Int64 instanceID, IDictionary<string, object> hdDict);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IHelpDeskQuery CreateEmptyHelpDeskQuery();

        /// <summary>
        ///
        /// </summary>
        /// <param name="helpDeskquery"></param>
        int SaveHelpDeskQuery(IHelpDeskQuery helpDeskquery);

        /// <summary>
        /// called from the X2 engine Help Desk map on Auto Archive timed activity - after 30 days
        /// </summary>
        /// <param name="helpDeskQueryKey"></param>
        bool X2AutoArchive2AM_Update(int helpDeskQueryKey);
    }
}