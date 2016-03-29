using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class DebtCounselling : Automation.DataModels.FinancialService
    {
        #region Constructors

        public DebtCounselling()
        {
        }

        public DebtCounselling(DebtCounselling debtCounselling, Automation.DataModels.FinancialService financialservice)
        {
            var thisInstance = this;
            Helpers.SetProperties<DebtCounselling, Automation.DataModels.FinancialService>(ref thisInstance, financialservice);

            //Has to be set last!!!
            Helpers.SetProperties<DebtCounselling, DebtCounselling>(ref thisInstance, debtCounselling);
        }

        #endregion Constructors

        public int DebtCounsellingKey { get; set; }

        public int DebtCounsellingStatusKey { get; set; }

        public string RegisteredName { get; set; }

        public ExternalRoleTypeEnum ExternalRoleTypeKey { get; set; }

        public string StageName { get; set; }

        public int ADUserKey { get; set; }

        public string ADUserName { get; set; }

        public string EmailAddress { get; set; }

        public string IDNumber { get; set; }

        public Proposal Proposal { get; set; }

        public SnapshotAccount SnapshotAccount { get; set; }

        public int AccountKey { get; set; }

        public int ProductKey { get; set; }

        public string AssignedUser { get; set; }

        public Int64 InstanceID { get; set; }

        public Int64 ClonedInstanceID { get; set; }

        public int InterestOnly { get; set; }
    }
}