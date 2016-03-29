using Common.Constants;
using Common.Enums;
using System;
using System.Collections.Generic;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.RecoveriesProposal> GetRecoveriesProposals()
        {
            var recoveriesProposals = dataContext.Query<Automation.DataModels.RecoveriesProposal>("select * from [2AM].recoveries.RecoveriesProposal");
            return recoveriesProposals;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="shortfallAmt"></param>
        /// <param name="repaymentAmount"></param>
        /// <param name="AOD"></param>
        /// <param name="generalstatus"></param>
        public void InsertRecoveriesProposal(int accountKey, double shortfallAmt, double repaymentAmount, int AdUserKey, GeneralStatusEnum generalstatus)
        {
            string now = DateTime.Now.ToString(Formats.DateTimeFormatSQL);
            var query = string.Format(@"insert into [2am].recoveries.recoveriesProposal
                                        (AccountKey, ShortfallAmount, RepaymentAmount, StartDate, AcknowledgementOfDebt, AdUserKey, CreateDate, GeneralStatusKey)
                                        values ({0}, {1}, {2}, '{3}', {4}, {5}, '{6}', {7})", accountKey, shortfallAmt, repaymentAmount, now, 1, 1618, now, (int)generalstatus);
            dataContext.Execute(query);
        }
    }
}