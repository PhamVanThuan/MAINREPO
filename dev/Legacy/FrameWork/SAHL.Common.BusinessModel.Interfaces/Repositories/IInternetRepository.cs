using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IInternetRepository
    {
        /// <summary>
        /// Gets all of the internet lead users
        /// </summary>
        IEventList<IInternetLeadUsers> GetAllInternetLeadUsers();

        /// <summary>
        /// Gets all Active internet lead users
        /// </summary>
        /// <returns></returns>
        IEventList<IInternetLeadUsers> GetAllActiveInternetLeadUsers();

        /// <summary>
        /// returns an active general status
        /// </summary>
        /// <returns></returns>
        IGeneralStatus GetActiveGeneralStatus();

        /// <summary>
        /// returns an inactive general status
        /// </summary>
        /// <returns></returns>
        IGeneralStatus GetInActiveGeneralStatus();

        /// <summary>
        /// This updates the Internet Lead User's
        /// </summary>
        /// <param name="InternetLeadUserKey"></param>
        /// <param name="generalStatus"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        bool UpdateInternetLeadUser(int InternetLeadUserKey, IGeneralStatus generalStatus, bool Flag);

        /// <summary>
        /// Sets the Flag, and returns the next ADUSer in the queue to get an intenret lead
        /// </summary>
        /// <param name="OfferKey"></param>
        /// <returns></returns>
        IADUser AssignInternetLeadUser(int OfferKey);

        /// <summary>
        /// Refreshes the internet lead users
        /// </summary>
        /// <returns></returns>
        bool RefreshInternetLeadUsers();
    }
}