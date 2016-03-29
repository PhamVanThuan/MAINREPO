using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Models
{
    public class SalesConsultantDailyPipelineDataModel : IDataModel
    {
        public string Consultant { get; set; }

        public string ApplicationStage { get; set; }

        public int NumberOfApplicationsAtStage { get; set; }

        public int Period { get; set; }

        public double SalesValue { get; set; }

        public SalesConsultantDailyPipelineDataModel()
        {
        }

        public SalesConsultantDailyPipelineDataModel(string consultant, string applicationStage, int numberOfApplicationsAtStage, int period, double salesValue)
        {
            this.Consultant = consultant;
            this.NumberOfApplicationsAtStage = numberOfApplicationsAtStage;
            this.ApplicationStage = applicationStage;
            this.Period = period;
            this.SalesValue = salesValue;
        }
    }
}