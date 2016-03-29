using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Projections.ComcorpProgress.Model
{
    public class ComcorpApplicationInformationDataModel : IDataModel
    {
        public int ReservedAccountKey { get; set; }

        public string Reference { get; set; }

        public double RequestedAmount { get; set; }

        public double OfferedAmount { get; set; }

        public double RegisteredAmount { get; set; }

        public ComcorpApplicationInformationDataModel(int reservedAccountKey, string reference, double requestedAmount, double offeredAmount, double registeredAmount)
        {
            this.ReservedAccountKey = reservedAccountKey;
            this.Reference = reference;
            this.RequestedAmount = requestedAmount;
            this.OfferedAmount = offeredAmount;
            this.RegisteredAmount = registeredAmount;
        }
    }
}