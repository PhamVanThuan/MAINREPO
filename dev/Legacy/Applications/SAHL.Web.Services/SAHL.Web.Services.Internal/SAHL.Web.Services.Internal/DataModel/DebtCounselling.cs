using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.Services.Internal.DataModel
{
    /// <summary>
    /// Debt Counselling
    /// </summary>
    public class DebtCounselling
    {
        public int DebtCounsellingKey { get; set; }
        public int AccountKey { get; set; }
        public DateTime? DiaryDate { get; set; }
        /*public string LitigationAttorneyName { get; set; }
        public string NationalCreditRegulatorName { get; set; }
        public string PaymentDistributionAgentName { get; set; }
        public double? PaymentReceivedAmount { get; set; }
        public DateTime? PaymentReceivedDate { get; set; }*/
        public List<LegalEntity> LegalEntitiesOnAccount { get; set; }
    }
}