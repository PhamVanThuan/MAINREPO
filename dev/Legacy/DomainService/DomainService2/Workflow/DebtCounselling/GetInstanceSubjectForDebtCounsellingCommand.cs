namespace DomainService2.Workflow.DebtCounselling
{
    public class GetInstanceSubjectForDebtCounsellingCommand : StandardDomainServiceCommand
    {
        public GetInstanceSubjectForDebtCounsellingCommand(int debtCounsellingKey)
        {
            this.DebtCounsellingKey = debtCounsellingKey;
        }

        public int DebtCounsellingKey
        {
            get;
            protected set;
        }

        public string LegalEntityNameResult
        {
            get;
            set;
        }
    }
}