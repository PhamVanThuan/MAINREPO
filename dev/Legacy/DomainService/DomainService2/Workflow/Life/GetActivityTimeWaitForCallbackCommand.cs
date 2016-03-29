namespace DomainService2.Workflow.Life
{
    using System;

    public class GetActivityTimeWaitForCallbackCommand : StandardDomainServiceCommand
    {
        public GetActivityTimeWaitForCallbackCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }

        public DateTime ActivityTimeResult { get; set; }
    }
}