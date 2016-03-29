namespace DomainService2.SharedServices.Common
{
    public class RecalculateHouseHoldIncomeAndSaveCommand : StandardDomainServiceCommand
    {
        public RecalculateHouseHoldIncomeAndSaveCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }
    }
}