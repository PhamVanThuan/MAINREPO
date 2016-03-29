using Common.Enums;

namespace Automation.DataModels
{
    public class LifeAccountModel
    {
        public int AccountKey { get; set; }

        public LifePolicyStatusEnum PolicyStatusKey { get; set; }

        public float BalanceAmount { get; set; }
    }
}