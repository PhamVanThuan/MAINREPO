namespace DomainService2.SharedServices.Common
{
    public class GetComcorpVendorOrgStructureByApplicationCommand : StandardDomainServiceCommand
    {
        public int ApplicationKey { get; protected set; }

        public int Result { get; set; }

        public GetComcorpVendorOrgStructureByApplicationCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }
    }
}