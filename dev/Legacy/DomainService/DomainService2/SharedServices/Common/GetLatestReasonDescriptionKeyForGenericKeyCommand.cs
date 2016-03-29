namespace DomainService2.SharedServices.Common
{
    public class GetLatestReasonDescriptionKeyForGenericKeyCommand : StandardDomainServiceCommand
    {
        public GetLatestReasonDescriptionKeyForGenericKeyCommand(int genericKey, int genericKeyTypeKey)
        {
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
        }

        public int GenericKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int Result { get; set; }
    }
}