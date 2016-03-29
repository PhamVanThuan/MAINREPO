
namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface ILeadInputInformation
    {
        int OfferTypeKey { get; }//R

        //N, S
        int EmploymentTypeKey { get; set; }//R

        //S
        bool CapitaliseFees { get; set; }

        //N, S
        int ProductKey { get; set; }//R

        //L, N, S
        int OfferSourceKey { get; set; }//R

        //N, S
        double PropertyValue { get; set; }//R
        //purchase price

        //N, S
        int MortgageLoanPurposeKey { get; set; }//R

        //S
        double CashOut { get; set; }

        //S
        double CurrentLoan { get; set; } //R

        //N
        double Deposit { get; set; }

        //L, N, S
        double HouseholdIncome { get; set; }//R

        //L, N, S
        int Term { get; set; }//R

        //N = PropertyValue (purchase price) - deposit
        //S = Current Loan Amount + Cash Out
        //L = ??
        double LoanAmountRequired { get; } //R

        //N, S
        //public double TotalFee { get; set; } //R

        //L - not sure what this is
        double TotalPrice { get; set; } //R 

        //N, S
        double FixPercent { get; set; }

        //N, S
        bool InterestOnly { get; set; }

        //LeadOnly
        double MonthlyInstalment { get; set; } //R
        //double ProfitFromSale { get; set; } //R
        double InterestRate { get; set; } //R 

        //Legal Entity
        string FirstNames { get; set; } //R
        string Surname { get; set; } //R
        string EmailAddress { get; set; }
        string HomePhoneCode { get; set; } //R
        string HomePhoneNumber { get; set; } //R
        int NumberOfApplicants { get; set; } //R

        //Referer
        string AdvertisingCampaignID { get; set; } //R
        string ReferringServerURL { get; set; } //R
        string UserURL { get; set; } //R
        string UserAddress { get; set; } //R


        bool Validate(out string errors);
    }
}
