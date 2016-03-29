using System.Collections.Generic;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Fetches the ITC records linked to the ReservedAccountKey of an Offer
        /// </summary>
        /// <param name="offerKey">offerKey</param>
        /// <returns>ITC.*</returns>
        public IEnumerable<Automation.DataModels.ITC> GetITCRecordsByOfferKey(int offerKey)
        {
            var ITCs = dataContext.Query<Automation.DataModels.ITC>(string.Format(@"
                                select i.* from [2am].dbo.Offer o with (nolock)
                                join [2am].dbo.ITC i with (nolock) on o.ReservedAccountKey=i.AccountKey
                                where o.offerKey = {0}", offerKey));
            return ITCs;
        }
    }
}