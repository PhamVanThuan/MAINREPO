namespace SAHL.Core.BusinessModel
{
    public class OrderedBusinessKey
    {
        public OrderedBusinessKey(BusinessKey businessKey, int sequenceTier, int sequence)
        {
            this.BusinessKey = businessKey;
            this.SequenceTier = sequenceTier;
            this.Sequence = sequence;
        }

        public BusinessKey BusinessKey { get; protected set; }

        public int SequenceTier { get; protected set; }

        public int Sequence { get; protected set; }
    }
}