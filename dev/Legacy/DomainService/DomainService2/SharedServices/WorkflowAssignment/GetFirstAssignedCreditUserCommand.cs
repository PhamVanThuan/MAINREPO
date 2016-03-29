namespace DomainService2.SharedServices.WorkflowAssignment
{
    public class GetFirstAssignedCreditUserCommand : StandardDomainServiceCommand
    {
        public GetFirstAssignedCreditUserCommand(long sourceInstanceID)
        {
            this.SourceInstanceID = sourceInstanceID;
        }

        public long SourceInstanceID { get; set; }

        public string ADUserName { get; set; }

        public int OfferRoleTypeKey { get; set; }

        public int OrganisationStructureKey { get; set; }
    }
}