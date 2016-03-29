using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IRegistrationRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        void DeleteRegmailByAccountKey(int accountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        IRegMail GetRegmailByAccountKey(int accountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="regmail"></param>
        void SaveRegmail(IRegMail regmail);

        /// <summary>
        ///
        /// </summary>
        /// <param name="deedsOfficeKey"></param>
        /// <returns></returns>
        IList<IAttorney> GetAttorneysByDeedsOfficeKey(int deedsOfficeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        IAttorney GetAttorneyByLegalEntityKey(int legalEntityKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AttorneyKey"></param>
        /// <returns></returns>
        IAttorney GetAttorneyByKey(int AttorneyKey);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> GetLitigationAttorneys();

        /// <summary>
        ///
        /// </summary>
        /// <param name="deedsOfficeKey"></param>
        /// <param name="OriginationSourceKey"></param>
        /// <returns></returns>
        IList<IAttorney> GetAttorneysByDeedsOfficeKeyAndOSKey(int deedsOfficeKey, int OriginationSourceKey);
    }
}