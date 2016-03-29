using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ApplicationInformationVariableLoan_DAO is instantiated in order to retrieve those details specific to a Variable Loan
    /// Application.
    /// </summary>
    public partial interface IApplicationInformationVariableLoan : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Primary Key. This is also a foreign key reference to the OfferInformation table.
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// Each Variable Loan is assigned to a specific Category. This Category (from 1-5) is determined by the Credit Matrix
        /// and takes into account factors such as the employment type, monthly income, LTV/PTI and the value of the loan.
        /// </summary>
        ICategory Category
        {
            get;
            set;
        }

        /// <summary>
        /// The term of the loan. The maximum allowed is 30 years (360 months)
        /// </summary>
        Int32? Term
        {
            get;
            set;
        }

        /// <summary>
        /// The value of the client's existing loan, in the case of a Switch Loan Application.
        /// </summary>
        Double? ExistingLoan
        {
            get;
            set;
        }

        /// <summary>
        /// The value of the Cash Deposit the client makes on a New Purchase Loan Application.
        /// </summary>
        Double? CashDeposit
        {
            get;
            set;
        }

        /// <summary>
        /// The total Household Income for the Application.
        /// </summary>
        Double? HouseholdIncome
        {
            get;
            set;
        }

        /// <summary>
        /// The Interest Provision made to cater for interest charged by the bank where the existing mortgage loan is held. This
        /// applies to Switch Loan Applications. This is to ensure that are sufficient funds to settle the outstanding balance on
        /// the existing loan at Disbursement.
        /// </summary>
        Double? InterimInterest
        {
            get;
            set;
        }

        /// <summary>
        /// The Monthly Instalment which would be due by the client if the Application is approved.
        /// </summary>
        Double? MonthlyInstalment
        {
            get;
            set;
        }

        /// <summary>
        /// The value of the Bond which the client wishes to register at the Deeds Office.
        /// </summary>
        Double? BondToRegister
        {
            get;
            set;
        }

        /// <summary>
        /// The value of the Loan-to-Value calculation. This is the ratio of the Loan Required to the Current Valuation on the
        /// Property.
        /// </summary>
        Double? LTV
        {
            get;
            set;
        }

        /// <summary>
        /// The value of the Payment-to-Income calculation. This is the ratio of the Monthly Instalment to the Household Income.
        /// </summary>
        Double? PTI
        {
            get;
            set;
        }

        /// <summary>
        /// The Market Rate applicable to the Loan.
        /// </summary>
        Double? MarketRate
        {
            get;
            set;
        }

        /// <summary>
        /// The SPV from which the loan will be issued from if the Application is approved.
        /// </summary>
        ISPV SPV
        {
            get;
            set;
        }

        /// <summary>
        /// An Application is assigned an employment type e.g. Salaried or Self Employed. The Employment Type applicable to an
        /// Application is determined by calculating which Employment Type contributes the most to the total Household Income.
        /// </summary>
        IEmploymentType EmploymentType
        {
            get;
            set;
        }

        /// <summary>
        /// The Credit Matrix version on which the Application is approved.
        /// </summary>
        ICreditMatrix CreditMatrix
        {
            get;
            set;
        }

        /// <summary>
        /// The Credit Criteria version on which the Application is approved.
        /// </summary>
        ICreditCriteria CreditCriteria
        {
            get;
            set;
        }

        /// <summary>
        /// Each Application is assigned a Rate Configuration. This allows the retrieval of the Market Rate and the Margin (Link Rate)
        /// applicable to the Application.
        /// </summary>
        IRateConfiguration RateConfiguration
        {
            get;
            set;
        }
    }
}