namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class IsEstateAgentInRoleCommand : StandardDomainServiceCommand
    {
        public IsEstateAgentInRoleCommand(string username, params string[] roles)
        {
            this.Username = username;
            this.Roles = roles;
        }

        public string Username { get; protected set; }

        public string[] Roles { get; set; }

        public bool Result { get; set; }
    }
}