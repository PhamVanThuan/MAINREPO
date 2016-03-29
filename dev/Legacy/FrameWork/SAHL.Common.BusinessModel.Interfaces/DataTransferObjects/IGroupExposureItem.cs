namespace SAHL.Common.BusinessModel.Interfaces.DataTransferObjects
{
    public interface IGroupExposureItem
    {
        int? AccountKey { get; set; }

        double ArrearBalance { get; set; }

        double CurrentBalance { get; set; }

        double HouseholdIncome { get; set; }

        double InstalmentAmount { get; set; }

        double LatestValuationAmount { get; set; }

        double LoanAgreementAmount { get; set; }

        double LTV { get; set; }

        int? OfferKey { get; set; }

        bool OwnerOccupied { get; set; }

        double PTI { get; set; }

        string RoleDescription { get; set; }

        string Status { get; set; }

        string Product { get; set; }
    }
}