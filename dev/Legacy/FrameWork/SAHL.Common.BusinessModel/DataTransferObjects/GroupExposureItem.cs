using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;

namespace SAHL.Common.BusinessModel.DataTransferObjects
{
    public class GroupExposureItem : IGroupExposureItem
    {
        int propertyKey = 0;
        public int PropertyKey 
        { 
            get { return propertyKey; } 
            set 
            {
                propertyKey = value;
            } 
        }

        public string RoleDescription { get; set; }

        public int? AccountKey { get; set; }

        public int? OfferKey { get; set; }

        public string Status { get; set; }

        public bool OwnerOccupied { get; set; }

        public string DisplayOwnerOccupied { get { return OwnerOccupied.ToYesNoString(); } }

        public double InstalmentAmount { get; set; }

        public double HouseholdIncome { get; set; }

        public double LatestValuationAmount { get; set; }

        public double LoanAgreementAmount { get; set; }

        public double CurrentBalance { get; set; }

        public double ArrearBalance { get; set; }

        public double PTI { get; set; }

        public double LTV { get; set; }

        public string Product { get; set; }
    }
}