namespace DomainService2.Workflow.Life
{
    public class CreateInstanceCommand : StandardDomainServiceCommand
    {
        public CreateInstanceCommand(int loanNumber, long instanceID, string assignTo)
        {
            this.ApplicationKey = -1;
            this.Name = string.Empty;
            this.Subject = string.Empty;
            this.Priority = -1;
            this.LoanNumber = loanNumber;
            this.InstanceID = instanceID;
            this.AssignTo = assignTo;
        }

        public int LoanNumber { get; protected set; }

        public long InstanceID { get; protected set; }

        public string AssignTo { get; protected set; }

        public int ApplicationKey { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public int Priority { get; set; }
    }
}