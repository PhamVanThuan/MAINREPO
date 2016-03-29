using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CourierAppointmentDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CourierAppointmentDataModel(int? courierKey, int? accountKey, DateTime? appointmentDate, string notes)
        {
            this.CourierKey = courierKey;
            this.AccountKey = accountKey;
            this.AppointmentDate = appointmentDate;
            this.Notes = notes;
		
        }
		[JsonConstructor]
        public CourierAppointmentDataModel(int courierAppointmentKey, int? courierKey, int? accountKey, DateTime? appointmentDate, string notes)
        {
            this.CourierAppointmentKey = courierAppointmentKey;
            this.CourierKey = courierKey;
            this.AccountKey = accountKey;
            this.AppointmentDate = appointmentDate;
            this.Notes = notes;
		
        }		

        public int CourierAppointmentKey { get; set; }

        public int? CourierKey { get; set; }

        public int? AccountKey { get; set; }

        public DateTime? AppointmentDate { get; set; }

        public string Notes { get; set; }

        public void SetKey(int key)
        {
            this.CourierAppointmentKey =  key;
        }
    }
}