using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Enums;

namespace BuildingBlocks.Services
{
    public class ADUserService : _2AMDataHelper, IADUserService
    {
        /// <summary>
        /// Runs a check to ensure that the user is in the same branch as the application
        /// </summary>
        /// <param name="adUserName">ADUsername</param>
        /// <param name="offerKey">OfferKey</param>
        /// <returns>True=User is in same branch, False=User not in same branch</returns>
        public bool IsUserInSameBranchAsApp(string adUserName, string offerKey)
        {
            bool b = false;
            var r = base.isUserInSameBranchAsApp(adUserName, offerKey);
            return b = r.Rows(0).Column(0).GetValueAs<int>() >= 1 ? true : false;
        }

        /// <summary>
        /// Looks at the GeneralStatusKey on the ADUser table in order to determine if an ADUser is inactive/active.
        /// Returns TRUE if the user is ACTIVE or FALSE if the user is INACTIVE or not found in the database.
        /// </summary>
        /// <param name="adUserName">ADUserName</param>
        /// <returns>ADUser.*</returns>
        public bool IsADUserActive(string adUserName)
        {
            var r = base.GetADUser(adUserName);
            bool b = r.Rows(0).Column("GeneralStatusKey").GetValueAs<int>() == (int)GeneralStatusEnum.Active ? true : false;
            return b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void DeactivateDebtCounsellingBusinessUsers()
        {
            var r = base.GetDebtCounsellingBusinessUsers();
            foreach (var row in r.RowList)
            {
                base.UpdateADUserStatus(row.Column("adusername").GetValueAs<string>(), GeneralStatusEnum.Inactive, GeneralStatusEnum.Inactive,
                    GeneralStatusEnum.Inactive);
            }
        }

        /// <summary>
        /// Fetches the adUser status of a user
        /// </summary>
        /// <param name="adUserName"></param>
        /// <returns></returns>
        public string GetADUserStatus(string adUserName)
        {
            var qr = base.ADUserStatusSQL(adUserName);
            return qr.RowList[0].Column("ADUserStatus").Value;
        }

        /// <summary>
        /// Fetches the round robin status of a user
        /// </summary>
        /// <param name="adUserName"></param>
        /// <param name="osKey"></param>
        /// <returns></returns>
        public string GetRoundRobinStatus(string adUserName, int osKey)
        {
            var qr = base.RoundRobinStatusSQL(adUserName, osKey);
            return qr.RowList[0].Column("RoundRobinStatus").Value;
        }

        public string GetADUserPlayingWorkflowRole(string role, string userToExclude)
        {
            string user = base.GetADUserPlayingWorkflowRole(role, userToExclude);
            return user;
        }
    }
}