
namespace SAHL.Services.Interfaces.CATS.Models
{
    public class CatsPaymentBatchRuleModel
    {
        public int BatchKey { get; protected set; }

        public CatsPaymentBatchRuleModel(int batchKey)
        {
            BatchKey = batchKey;
        }
    }
}
