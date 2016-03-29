using System;
namespace Automation.DataModels
{
    public class LifeLead
    {
        public long InstanceId { get; set; }

        public int AccountKey { get; set; }

        public int OfferKey { get; set; }

        public int LifeAccountKey { get; set; }

        public string ProductSwitchReason { get; set; }

        public bool ConfirmationRequired { get; set; }

        public DateTime CreationDate { get; set; }

        public string AssignedConsultant { get; set; }

        public string StateName { get; set; }

        public DateTime StateChangeDate { get; set; }

        public long? ParentInstanceID { get; set; }
    }
}