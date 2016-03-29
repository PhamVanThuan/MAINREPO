using System;
using System.Collections.Generic;

namespace SAHL.Web.Services
{

        /// <summary>
        /// This is the base struct for the PreProspect Object 
        /// </summary>
        public struct PreProspect
        {
            // New Origination Calculator Variables
            //------------------------------------------
            /// <summary>
            /// 
            /// </summary>
            public int ApplicationKey;

            /// <summary>
            /// 
            /// </summary>
            public bool OfferSubmitted;

            /// <summary>
            /// 
            /// </summary>
            public int marketrateKey;
            /// <summary>
            /// 
            /// </summary>
            public int CreditMatrixKey;
            /// <summary>
            /// 
            /// </summary>
            public int EmploymentType;
            /// <summary>
            /// 
            /// </summary>
            public int ProductKey;  // Default to variable loan
            /// <summary>
            /// 
            /// </summary>
            public double LinkRate;
            /// <summary>
            /// 
            /// </summary>
            public double InterestRate;
            /// <summary>
            /// 
            /// </summary>
            public double MaximumInstallment;
            // PurposeNumber = 1 - Unknown , 2 - Switch Loan, 3 - New Purchase, 4 - Refinance, 5 - Further Loan 
            /// <summary>
            /// 
            /// </summary>
            public int PurposeNumber;
            /// <summary>
            /// 
            /// </summary>
            public int MaturityMonths;
            /// <summary>
            /// 
            /// </summary>
            public int IncomeType;
            /// <summary>
            /// 
            /// </summary>
            public int NumberOfApplicants;
            /// <summary>
            /// 
            /// </summary>
            public int CategoryKey;


            //------------------------------------------
            // Marketing Information


            /// <summary>
            /// 
            /// </summary>
            public string AdvertisingCampaignID;
            /// <summary>
            /// 
            /// </summary>
            public string UserURL;
            /// <summary>
            /// 
            /// </summary>
            public string ReferringServerURL;
            /// <summary>
            /// 
            /// </summary>
            public string UserAddress;

            //------------------------------------------
            // Application form contact Information 


            /// <summary>
            /// 
            /// </summary>
            public string PreProspectFirstNames;
            /// <summary>
            /// 
            /// </summary>
            public string PreProspectSurname;
            /// <summary>
            /// 
            /// </summary>
            public string PreProspectIDNumber;
            /// <summary>
            /// 
            /// </summary>
            public string PreProspectHomePhoneCode;
            /// <summary>
            /// 
            /// </summary>
            public string PreProspectHomePhoneNumber;
            /// <summary>
            /// 
            /// </summary>
            public string PreProspectEmailAddress;
            //------------------------------------------

            /// <summary>
            /// 
            /// </summary>
            public int VarifixMarketRateKey;
            /// <summary>
            /// 
            /// </summary>
            public int MarketRateTypeNumber;
            /// <summary>
            /// 
            /// </summary>
            public string ReferenceNumber;

            /// <summary>
            /// 
            /// </summary>
            public int rateConfigKey;

            /// <summary>
            /// 
            /// </summary>
            public int MortgageLoanPurpose;
            /// <summary>
            /// 
            /// </summary>
            public bool InterestOnly;
            /// <summary>
            /// 
            /// </summary>
            public int Product; // Default to variable loan - old 


            /// <summary>
            /// 
            /// </summary>
            public double InitiationFee;
            /// <summary>
            /// 
            /// </summary>
            public double RegistrationFee;
            /// <summary>
            /// 
            /// </summary>
            public bool CapitaliseFees;
            /// <summary>
            /// 
            /// </summary>
            public double CancellationFee;
            /// <summary>
            /// 
            /// </summary>
            public double InterimInterest;
            /// <summary>
            /// 
            /// </summary>
            public double ValuationFee;
            /// <summary>
            /// 
            /// </summary>
            public double TransferFee;
            /// <summary>
            /// 
            /// </summary>
            public double Deposit;
            /// <summary>
            /// 
            /// </summary>
            public double CurrentLoan;
            /// <summary>
            /// 
            /// </summary>
            public double TotalFee;
            /// <summary>
            /// 
            /// </summary>
            public double HouseholdIncome;
            /// <summary>
            /// 
            /// </summary>
            public double LTV;
            /// <summary>
            /// 
            /// </summary>
            public double PTI;
            /// <summary>
            /// 
            /// </summary>
            public double ActiveMarketRate;
            /// <summary>
            /// 
            /// </summary>
            public double InstalmentTotal;
            /// <summary>
            /// 
            /// </summary>
            public double EstimatedPropertyValue;
            /// <summary>
            /// 
            /// </summary>
            public int MarginKey;
            /// <summary>
            /// 
            /// </summary>
            public int Term;
            /// <summary>
            /// 
            /// </summary>
            public double CashOut;
            /// <summary>
            /// 
            /// </summary>
            public double InstalmentFix;
            /// <summary>
            /// 
            /// </summary>
            public double FixPercent;
            /// <summary>
            /// 
            /// </summary>
            public double PurchasePrice;
            /// <summary>
            /// 
            /// </summary>
            public double TotalPrice;
            /// <summary>
            /// 
            /// </summary>
            public double LoanAmountRequired;

            //VariFix Variables 
            /// <summary>
            /// 
            /// </summary>
            public bool FixLoan;
            /// <summary>
            /// 
            /// </summary>
            public double ElectedFixedPercentage;
            /// <summary>
            /// 
            /// </summary>
            public double ElectedVariablePercentage;
            /// <summary>
            /// 
            /// </summary>
            public double ElectedFixedRate;

            /// <summary>
            /// InternetLead = 96, InternetApplication = 97, MobisiteLead = 174, MobisiteApplication = 175
            /// </summary>
            public int OfferSourceKey;
        }

        /// <summary>
        /// This is the base struct for the PreProspect Object 
        /// </summary>
        public struct OriginationFees
        {
            /// <summary>
            /// 
            /// </summary>
            public double loanamount;
            /// <summary>
            /// 
            /// </summary>
            public double bondrequired;
            /// <summary>
            /// 
            /// </summary>
            public int appType;
            /// <summary>
            /// 
            /// </summary>
            public double cashout;
            /// <summary>
            /// 
            /// </summary>
            public double OverrideCancelFeeamount;
            /// <summary>
            /// 
            /// </summary>
            public bool capitalisefees;
            /// <summary>
            /// 
            /// </summary>
            public double initiationfee;
            /// <summary>
            /// 
            /// </summary>
            public double registrationfee;
            /// <summary>
            /// 
            /// </summary>
            public double cancellationfee;
            /// <summary>
            /// 
            /// </summary>
            public double interiminterest;
            /// <summary>
            /// 
            /// </summary>
            public double bondtoregister;
            /// <summary>
            /// 
            /// </summary>
            public double propertyValue;
            /// <summary>
            /// 
            /// </summary>
            public double householdIncome;
            /// <summary>
            /// 
            /// </summary>
            public int employmentTypeKey;
        }


        /// <summary>
        /// This is the base struct for the credit Criteria object for passing to and from the web service
        /// </summary>
        public struct CreditCriteria
        {
            /// <summary>
            /// 
            /// </summary>
            public int CreditCriteriaKey;
            /// <summary>
            /// 
            /// </summary>
            public double CreditCriteriaPTI;
            /// <summary>
            /// 
            /// </summary>
            public double CreditCriteriaPTIValue;
            /// <summary>
            /// 
            /// </summary>
            public double CreditCriteriaLTV;
            /// <summary>
            /// 
            /// </summary>
            public double CreditCriteriaLTVValue;
            /// <summary>
            /// 
            /// </summary>
            public double CreditCriteriaMarginValue;
            /// <summary>
            /// 
            /// </summary>
            public int CreditCriteriaMarginKey;
            /// <summary>
            /// 
            /// </summary>
            public int CreditCriteriaCreditMatrixKey;
            /// <summary>
            /// 
            /// </summary>
            public int CreditCriteriaCategoryKey;
        }


        //CREDIT Disqualifications Object


        /// <summary>
        /// This is the class for handling Credit Disqualifications data
        /// </summary>
        public struct CreditDisqualifications
        {
            /// <summary>
            /// 
            /// </summary>
            public bool calculationdone;
            /// <summary>
            /// 
            /// </summary>
            public double ltv;
            /// <summary>
            /// 
            /// </summary>
            public double pti;
            /// <summary>
            /// 
            /// </summary>
            public double householdincome;
            /// <summary>
            /// 
            /// </summary>
            public double loanamountrequired;
            /// <summary>
            /// 
            /// </summary>
            public double estimatedproeprtyvalue;
            /// <summary>
            /// 
            /// </summary>
            public int EmploymentTypeKey;
            /// <summary>
            /// 
            /// </summary>
            public bool ifFurtherLending;
            /// <summary>
            /// 
            /// </summary>
            public int term;

        }


        //CLIENT SURVEY Objects


        /// <summary>
        /// This is the class for handling Survey data
        /// </summary>
        public struct ClientQuestionnaire
        {   
            /// <summary>
            /// 
            /// </summary>
            public string GUID;
            /// <summary>
            /// 
            /// </summary>
            public DateTime? DateReceived;
            /// <summary>
            /// 
            /// </summary>
            public List<QuestionnaireQuestion> QuestionnaireQuestions;

        }

        public struct QuestionnaireQuestion
        {
            /// <summary>
            /// 
            /// </summary>
            public int Key;
            /// <summary>
            /// 
            /// </summary>
            public int Sequence;
            /// <summary>
            /// 
            /// </summary>
            public string Description;
            /// <summary>
            /// 
            /// </summary>
            public List<QuestionnaireAnswer> QuestionAnswers;
        }

        public struct QuestionnaireAnswer
        {
            /// <summary>
            /// 
            /// </summary>
            public int AnswerKey;
            /// <summary>
            /// 
            /// </summary>
            public int AnswerTypeKey;
            /// <summary>
            /// 
            /// </summary>
            public string AnswerDescription;
        }

        /// <summary>
        /// This is the class for handling saving Survey data
        /// </summary>
        /// 

        public struct SurveyResult
        {
            /// <summary>
            /// 
            /// </summary>
            public string GUID;
            /// <summary>
            /// 
            /// </summary>
            public List<SurveyQuestionAnswer> SurveyQuestionAnswers;
        }
        public struct SurveyQuestionAnswer
        {
            /// <summary>
            /// 
            /// </summary>
            public int AnswerKey;
            /// <summary>
            /// 
            /// </summary>
            public int AnswerTypeKey;
            /// <summary>
            /// 
            /// </summary>
            public string AnswerValue;
            /// <summary>
            /// 
            /// </summary>
            public int QuestionnaireQuestionKey;
        }
}
