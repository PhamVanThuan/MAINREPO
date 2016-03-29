using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using System;
using System.Text;

namespace SAHL.Common.BusinessModel.DataTransferObjects
{

    public class LeadInputInformation : ILeadInputInformation
    {
        public int OfferTypeKey
        {
            get
            {
                switch (MortgageLoanPurposeKey)
                {
                    case (int)MortgageLoanPurposes.Switchloan:
                        return (int)OfferTypes.SwitchLoan;
                    case (int)MortgageLoanPurposes.Newpurchase:
                        return (int)OfferTypes.NewPurchaseLoan;
                    case (int)MortgageLoanPurposes.Refinance:
                        return (int)OfferTypes.RefinanceLoan;
                    default://Unknown
                        return (int)OfferTypes.Unknown;
                };
            }
        }

        public int EmploymentTypeKey { get; set; }

        public bool CapitaliseFees { get; set; }

        public int ProductKey { get; set; }

        public int OfferSourceKey { get; set; }

        public double PropertyValue { get; set; }

        public int MortgageLoanPurposeKey { get; set; }

        public double CashOut { get; set; }

        public double CurrentLoan { get; set; }

        public double EstimatedPropertyValue { get; set; }

        public double Deposit { get; set; }

        public double HouseholdIncome { get; set; }

        public int Term { get; set; }

        public double LoanAmountRequired
        {
            get
            {
                switch (MortgageLoanPurposeKey)
                {
                    case (int)MortgageLoanPurposes.Unknown:
                        return PropertyValue - Deposit;
                    case (int)MortgageLoanPurposes.Newpurchase:
                        return PropertyValue - Deposit;
                    default://Switch
                        return CurrentLoan + CashOut;
                }
                ;
            }
        }

        //N, S
        public double TotalFee { get; set; } //R

        //L ? how is this determined? 
        public double TotalPrice { get; set; }

        public double FixPercent { get; set; }

        public bool InterestOnly { get; set; }

        //LeadOnly
        public double MonthlyInstalment { get; set; } //R
        //public double ProfitFromSale { get; set; }//R
        public double InterestRate { get; set; }//R 

        //Legal Entity
        /**/
        public string FirstNames { get; set; }//R
        public string Surname { get; set; }//R
        public string EmailAddress { get; set; }
        public string HomePhoneCode { get; set; } //R
        public string HomePhoneNumber { get; set; } //R
        public int NumberOfApplicants { get; set; } //R

        //Referer
        public string AdvertisingCampaignID { get; set; } //R
        public string ReferringServerURL { get; set; } //R
        public string UserURL { get; set; } //R
        public string UserAddress { get; set; } //R

        public bool Validate(out string errors)
        {
            errors = String.Empty;

            StringBuilder sb = new StringBuilder();

            if (OfferTypeKey != (int)OfferTypes.Unknown
                && OfferTypeKey != (int)OfferTypes.NewPurchaseLoan
                && OfferTypeKey != (int)OfferTypes.SwitchLoan)
            {
                sb.AppendLine("OfferType is required, please set OfferType appropriately.");
            }

            if (MortgageLoanPurposeKey != (int)MortgageLoanPurposes.Unknown
                && MortgageLoanPurposeKey != (int)MortgageLoanPurposes.Newpurchase
                && MortgageLoanPurposeKey != (int)MortgageLoanPurposes.Switchloan)
            {
                sb.AppendLine("MortgageLoanPurpose is required, please set MortgageLoanPurpose appropriately.");
            }


            if (EmploymentTypeKey < 1 && OfferTypeKey != (int)OfferTypes.Unknown)
            {
                sb.AppendLine("EmploymentType is required, please set EmploymentType appropriately.");
            }

            //S - will always have a value
            //bool CapitaliseFees { get; set; }

            if (OfferTypeKey != (int)OfferTypes.Unknown
                && (ProductKey != (int)Products.NewVariableLoan
                    && ProductKey != (int)Products.VariableLoan
                    && ProductKey != (int)Products.VariFixLoan
                )
            )
            {
                sb.AppendLine("ProductKey is required, please set this appropriately.");
            }

            if (PropertyValue < 100000 && OfferTypeKey != (int)OfferTypes.Unknown)
            {
                sb.AppendLine("PropertyValue is required, please set EstimatedPropertyValue appropriately.");
            }

            if (CurrentLoan < 1 && OfferTypeKey == (int)OfferTypes.SwitchLoan)
            {
                sb.AppendLine("CurrentLoan is required, please set this appropriately.");
            }

            if (HouseholdIncome < 1)
            {
                sb.AppendLine("HouseholdIncome is required, please set this appropriately.");
            }

            if (Term < 1)
            {
                sb.AppendLine("Term is required, please set this appropriately.");
            }

            if (LoanAmountRequired < 100000)
            {
                if (MortgageLoanPurposeKey == (int)MortgageLoanPurposes.Switchloan)
                    sb.AppendLine("LoanAmountRequired is required, please set CurrentLoan and CashOut appropriately.");
                else
                    sb.AppendLine("LoanAmountRequired is required, please set PropertyValue and Deposit appropriately.");
            }

            if (TotalPrice < 1 && OfferTypeKey == (int)OfferTypes.Unknown)
            {
                sb.AppendLine("TotalPrice is required, please set this appropriately.");
            }

            if (OfferTypeKey != (int)OfferTypes.Unknown
                && (FixPercent < 0
                    || FixPercent > 1)
                )
            {
                sb.AppendLine("FixPercent is required, please set this appropriately (between 0.0 and 1.0).");
            }

            // N, S
            // bool InterestOnly { get; set; }

            //LeadOnly
            if (OfferTypeKey == (int)OfferTypes.Unknown && MonthlyInstalment < 1)
            {
                sb.AppendLine("MonthlyInstalment is required, please set this appropriately");
            }

            if (OfferTypeKey == (int)OfferTypes.Unknown && InterestRate < 0.0001)
            {
                sb.AppendLine("InterestRate is required, please set this appropriately");
            }

            if (OfferTypeKey == (int)OfferTypes.Unknown && NumberOfApplicants < 1)
            {
                sb.AppendLine("NumberOfApplicants is required, please set this appropriately");
            }

            //Legal Entity
            if (String.IsNullOrEmpty(FirstNames)
                || String.IsNullOrEmpty(Surname)
                || String.IsNullOrEmpty(HomePhoneCode)
                || String.IsNullOrEmpty(HomePhoneNumber))
            {
                sb.AppendLine("Client Detail is required, please set FirstNames, Surname, HomePhoneCode & HomePhoneNumber appropriately");
            }

            if (OfferSourceKey != (int)OfferSources.InternetApplication
                && OfferSourceKey != (int)OfferSources.InternetLead
                && OfferSourceKey != (int)OfferSources.MobisiteApplication
                && OfferSourceKey != (int)OfferSources.MobisiteLead)
            {
                sb.AppendLine("Offer Source is required, please set this appropriately");
            }

            // Not sure what data is being passed through here?
            //Referer
            //if (String.IsNullOrEmpty(AdvertisingCampaignID)
            //    || String.IsNullOrEmpty(ReferringServerURL)
            //    || String.IsNullOrEmpty(UserURL)
            //    || String.IsNullOrEmpty(UserAddress))
            //{
            //    sb.AppendLine("Referer Detail is required, please set AdvertisingCampaignID, ReferringServerURL, UserURL & UserAddress appropriately");
            //}


            errors = sb.ToString();

            return String.IsNullOrEmpty(errors);
        }
    }
}
