using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IITCRepository
    {
        IList<IAddressStreet> GetITCAddressListByLegalEntityKey(int LegalEntityKey);

        IList<IITC> GetITCByAccountKey(int AccountKey);

        DataTable GetArchivedITCByLegalEntityKey(int LegalEntityKey, int accountKey);

        //IList<IITC> GetITCForHistoryByLeExcludingAccount(int legalEntity, int accountKey);
        IITC GetEmptyITC();

        IITCArchive GetArchivedITCByITCKey(int Key);

        IITC GetITCByKey(int Key);

        string GetITCXslByDate(DateTime date);

        /// <summary>
        ///
        /// </summary>
        /// <param name="LEKey"></param>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        IList<IITC> GetITCByLEAndAccountKey(int LEKey, int AccountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="LEKeys"></param>s
        /// <returns></returns>
        IList<IITC> GetITCByLegalEntityKeys(int[] LEKeys);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="itc"></param>
        /// <param name="itca"></param>
        void GetITCOrArchiveITCByITCKey(int Key, out IITC itc, out IITCArchive itca);

        /// <summary>
        /// Saves an IITC object and its properties to the database (SQLUpdate)
        /// </summary>
        /// <param name="itc"></param>
        void SaveITC(IITC itc);
    }
}