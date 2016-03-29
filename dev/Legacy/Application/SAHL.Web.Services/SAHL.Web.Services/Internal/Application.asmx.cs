using System.ComponentModel;
using System.Web.Services;
using SAHL.Common.BusinessModel.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System;
using SAHL.Common.Logging;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Web.Services
{
    /// <summary>
    /// Summary description for Application
    /// </summary>
    [WebService(Namespace = "http://webservices.sahomeloans.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class Application : WebService
    {

        [WebMethod(Description = "Creates a New 2AM lead - Returns the Application key on success and -1 on failure to create the case")]
        public int CreateInternetLead(PreProspect prospect, out string errors)
        {
            errors = String.Empty;
            //validate input

            //maybe try catch (because this is an API to client funcitonality), maybe not....

            IApplicationRepository applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            //get preprospect into leadInput
            ILeadInputInformation leadInput = new LeadInputInformation
            {
                MortgageLoanPurposeKey = prospect.PurposeNumber, // MortgageLoanPurpose from web app is dodge
                EmploymentTypeKey = prospect.EmploymentType,
                ProductKey = prospect.ProductKey,
                OfferSourceKey = (prospect.OfferSourceKey > 0 ? prospect.OfferSourceKey : (prospect.PurposeNumber == (int)MortgageLoanPurposes.Unknown ? (int)OfferSources.InternetLead : (int)OfferSources.InternetApplication)),
                PropertyValue = prospect.EstimatedPropertyValue,

                HouseholdIncome = prospect.HouseholdIncome, //L
                Deposit = prospect.Deposit, //L
                Term = prospect.Term, //L

                FixPercent = prospect.FixPercent,
                InterestOnly = prospect.InterestOnly,

                //Switch Only
                CapitaliseFees = prospect.CapitaliseFees,
                CashOut = prospect.CashOut,
                CurrentLoan = prospect.CurrentLoan,

                //LeadOnly
                //Some hateful logic not worth repeating here exists in 
                //http://sahls331.sahl.com/svn/Liquorice/trunk/SAHL.Services.Public/SAHL.Calculators.Affordability/AffordabilityCalculatorService.cs
                //to determine this value
                TotalPrice = prospect.TotalPrice, //l 
                MonthlyInstalment = prospect.MaximumInstallment, //l = MaximumInstlament
                InterestRate = prospect.InterestRate,
                //Legal Entity
                FirstNames = prospect.PreProspectFirstNames,//R
                Surname = prospect.PreProspectSurname,//R
                EmailAddress = prospect.PreProspectEmailAddress,
                HomePhoneCode = prospect.PreProspectHomePhoneCode,//R
                HomePhoneNumber = prospect.PreProspectHomePhoneNumber,//R
                NumberOfApplicants = prospect.NumberOfApplicants,//R
                //Referer
                AdvertisingCampaignID = prospect.AdvertisingCampaignID, //R
                ReferringServerURL = prospect.ReferringServerURL,//R
                UserURL = prospect.UserURL,//R
                UserAddress = prospect.UserAddress,//R
            };

            if (leadInput.Validate(out errors))
                using (new Castle.ActiveRecord.SessionScope())
                {
                    return applicationRepo.CreateWebLead(leadInput);
                }
            else
            {
                LogPlugin.Logger.LogErrorMessage("CreateInternetLead: ", errors.ToString());

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ActiveMarketRate: " + prospect.ActiveMarketRate);
                sb.AppendLine("AdvertisingCampaignID: " + prospect.AdvertisingCampaignID);
                sb.AppendLine("ApplicationKey: " + prospect.ApplicationKey);
                sb.AppendLine("CancellationFee: " + prospect.CancellationFee);
                sb.AppendLine("CapitaliseFees: " + prospect.CapitaliseFees);
                sb.AppendLine("CashOut: " + prospect.CashOut);
                sb.AppendLine("CategoryKey: " + prospect.CategoryKey);
                sb.AppendLine("CreditMatrixKey: " + prospect.CreditMatrixKey);
                sb.AppendLine("CurrentLoan: " + prospect.CurrentLoan);
                sb.AppendLine("Deposit: " + prospect.Deposit);
                sb.AppendLine("ElectedFixedPercentage: " + prospect.ElectedFixedPercentage);
                sb.AppendLine("ElectedFixedRate: " + prospect.ElectedFixedRate);
                sb.AppendLine("ElectedVariablePercentage: " + prospect.ElectedVariablePercentage);
                sb.AppendLine("EmploymentType: " + prospect.EmploymentType);
                sb.AppendLine("EstimatedPropertyValue: " + prospect.EstimatedPropertyValue);
                sb.AppendLine("FixLoan: " + prospect.FixLoan);
                sb.AppendLine("FixPercent: " + prospect.FixPercent);
                sb.AppendLine("HouseholdIncome: " + prospect.HouseholdIncome);
                sb.AppendLine("IncomeType: " + prospect.IncomeType);
                sb.AppendLine("InitiationFee: " + prospect.InitiationFee);
                sb.AppendLine("InstalmentFix: " + prospect.InstalmentFix);
                sb.AppendLine("InstalmentTotal: " + prospect.InstalmentTotal);
                sb.AppendLine("InterestOnly: " + prospect.InterestOnly);
                sb.AppendLine("InterestRate: " + prospect.InterestRate);
                sb.AppendLine("InterimInterest: " + prospect.InterimInterest);
                sb.AppendLine("LinkRate: " + prospect.LinkRate);
                sb.AppendLine("LoanAmountRequired: " + prospect.LoanAmountRequired);
                sb.AppendLine("LTV: " + prospect.LTV);
                sb.AppendLine("MarginKey: " + prospect.MarginKey);
                sb.AppendLine("marketrateKey: " + prospect.marketrateKey);
                sb.AppendLine("MarketRateTypeNumber: " + prospect.MarketRateTypeNumber);
                sb.AppendLine("MaturityMonths: " + prospect.MaturityMonths);
                sb.AppendLine("MaximumInstallment: " + prospect.MaximumInstallment);
                sb.AppendLine("MortgageLoanPurpose: " + prospect.MortgageLoanPurpose);
                sb.AppendLine("NumberOfApplicants: " + prospect.NumberOfApplicants);
                sb.AppendLine("OfferSourceKey: " + prospect.OfferSourceKey);
                sb.AppendLine("OfferSubmitted: " + prospect.OfferSubmitted);
                sb.AppendLine("Product: " + prospect.Product);
                sb.AppendLine("ProductKey: " + prospect.ProductKey);
                sb.AppendLine("PTI: " + prospect.PTI);
                sb.AppendLine("PurchasePrice: " + prospect.PurchasePrice);
                sb.AppendLine("PurposeNumber: " + prospect.PurposeNumber);
                sb.AppendLine("rateConfigKey: " + prospect.rateConfigKey);
                sb.AppendLine("ReferenceNumber: " + prospect.ReferenceNumber);
                sb.AppendLine("ReferringServerURL: " + prospect.ReferringServerURL);
                sb.AppendLine("RegistrationFee: " + prospect.RegistrationFee);
                sb.AppendLine("Term: " + prospect.Term);
                sb.AppendLine("TotalFee: " + prospect.TotalFee);
                sb.AppendLine("TotalPrice: " + prospect.TotalPrice);
                sb.AppendLine("TransferFee: " + prospect.TransferFee);
                sb.AppendLine("ValuationFee: " + prospect.ValuationFee);
                sb.AppendLine("VarifixMarketRateKey: " + prospect.VarifixMarketRateKey);


                LogPlugin.Logger.LogErrorMessage("CreateInternetLead : " + errors.ToString(), sb.ToString());

                return -1;
            }
        }

        [Obsolete("Please use CreateInternetLead instead")]
        [WebMethod(Description = "Creates a New 2AM lead - Returns the Application key on success and -1 on failure to create the case")]
        public int CreateLead(PreProspect prospect)
        {
            string errors;

            //hack a value in for loan amount required
            if (prospect.PurposeNumber == (int)MortgageLoanPurposes.Switchloan
                && prospect.CurrentLoan < 1)
            {
                prospect.CurrentLoan = prospect.LoanAmountRequired;
            }

            return CreateInternetLead(prospect, out errors);
        }
    }
}
