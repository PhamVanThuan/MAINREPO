using SAHL.Core.Data;

namespace SAHL.Services.EventProjection.Projections.CapitecApplicationDaily.Models
{
    public class ConsultantInfoDataModel : IDataModel
    {
        public string Name { get; set; }

        public string ContactNumber { get; set; }

        public ConsultantInfoDataModel(string name, string contactNumber)
        {
            this.Name = name;
            this.ContactNumber = contactNumber;
        }
    }
}