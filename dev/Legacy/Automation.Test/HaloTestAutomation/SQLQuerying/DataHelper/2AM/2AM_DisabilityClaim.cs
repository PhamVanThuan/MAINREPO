using Automation.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        public DisabilityClaim GetDisabilityClaim(int disabilityClaimKey)
        {
            string query = string.Format(@"select * from [2AM].[dbo].DisabilityClaim where DisabilityClaimKey = {0}", disabilityClaimKey);
            var disabilityClaim = dataContext.Query<DisabilityClaim>(query).First();
            return disabilityClaim;
        }

        public IEnumerable<Automation.DataModels.DisabilityPaymentSchedule> GetDisabilityPaymentSchedule(int disabilityClaimKey)
        {
            string query = string.Format(@"select * from [2AM].dbo.DisabilityPayment where DisabilityClaimKey = {0}", disabilityClaimKey);
            return dataContext.Query<Automation.DataModels.DisabilityPaymentSchedule>(query);
        }
        /// <summary>
        /// Get a disability claim by legalEntityKey and accountKey
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public DisabilityClaim GetDisabilityClaimByLegalEntityAndAccountKey(int legalEntityKey, int accountKey)
        {
            string query = string.Format(@"SELECT
	                                        dc.*
                                        FROM [2AM].[dbo].DisabilityClaim dc
                                        WHERE dc.LegalEntityKey = {0}
                                        AND dc.AccountKey = {1}
                                        AND dc.DisabilityClaimStatusKey = 1", legalEntityKey, accountKey);
            var disabilityClaim = dataContext.Query<DisabilityClaim>(query).First();
            return disabilityClaim;
        }
    }
}