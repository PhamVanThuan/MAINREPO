using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class CourtDetailsService : _2AMDataHelper, ICourtDetailsService
    {
        /// <summary>
        /// Checks if court details exist against a debt counselling case
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        public bool CourtDetailsExist(int debtCounsellingKey)
        {
            var list = base.GetHearingDetails(debtCounsellingKey);
            if (list.Count() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Checks if active hearing details exist for a debt counselling case.
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        public bool ActiveCourtDetailsExist(int debtCounsellingKey)
        {
            var courtDetail = (from r in base.GetHearingDetails(debtCounsellingKey)
                               where r.GeneralStatusKey == 1
                               select r).FirstOrDefault();
            if (courtDetail != null)
                return true;
            return false;
        }
    }
}