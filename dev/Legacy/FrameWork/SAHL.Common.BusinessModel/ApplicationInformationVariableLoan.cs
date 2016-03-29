using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// ApplicationInformationVariableLoan_DAO is instantiated in order to retrieve those details specific to a Variable Loan
    /// Application.
    /// </summary>
    public partial class ApplicationInformationVariableLoan : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformationVariableLoan_DAO>, IApplicationInformationVariableLoan
    {
        public ApplicationInformationVariableLoan(SAHL.Common.BusinessModel.DAO.ApplicationInformationVariableLoan_DAO ApplicationInformationVariableLoan)
            : base(ApplicationInformationVariableLoan)
        {
            this._DAO = ApplicationInformationVariableLoan;
        }

        /// <summary>
        /// Primary Key. This is also a foreign key reference to the OfferInformation table.
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// Each Variable Loan is assigned to a specific Category. This Category (from 1-5) is determined by the Credit Matrix
        /// and takes into account factors such as the employment type, monthly income, LTV/PTI and the value of the loan.
        /// </summary>
        public ICategory Category
        {
            get
            {
                if (null == _DAO.Category) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICategory, Category_DAO>(_DAO.Category);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Category = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Category = (Category_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// The term of the loan. The maximum allowed is 30 years (360 months)
        /// </summary>
        public Int32? Term
        {
            get { return _DAO.Term; }
            set { _DAO.Term = value; }
        }

        /// <summary>
        /// The value of the client's existing loan, in the case of a Switch Loan Application.
        /// </summary>
        public Double? ExistingLoan
        {
            get { return _DAO.ExistingLoan; }
            set { _DAO.ExistingLoan = value; }
        }

        /// <summary>
        /// The value of the Cash Deposit the client makes on a New Purchase Loan Application.
        /// </summary>
        public Double? CashDeposit
        {
            get { return _DAO.CashDeposit; }
            set { _DAO.CashDeposit = value; }
        }

        /// <summary>
        /// The total Household Income for the Application.
        /// </summary>
        public Double? HouseholdIncome
        {
            get { return _DAO.HouseholdIncome; }
            set { _DAO.HouseholdIncome = value; }
        }

        /// <summary>
        /// The Interest Provision made to cater for interest charged by the bank where the existing mortgage loan is held. This
        /// applies to Switch Loan Applications. This is to ensure that are sufficient funds to settle the outstanding balance on
        /// the existing loan at Disbursement.
        /// </summary>
        public Double? InterimInterest
        {
            get { return _DAO.InterimInterest; }
            set { _DAO.InterimInterest = value; }
        }

        /// <summary>
        /// The Monthly Instalment which would be due by the client if the Application is approved.
        /// </summary>
        public Double? MonthlyInstalment
        {
            get { return _DAO.MonthlyInstalment; }
            set { _DAO.MonthlyInstalment = value; }
        }

        /// <summary>
        /// The value of the Bond which the client wishes to register at the Deeds Office.
        /// </summary>
        public Double? BondToRegister
        {
            get { return _DAO.BondToRegister; }
            set { _DAO.BondToRegister = value; }
        }

        /// <summary>
        /// The value of the Loan-to-Value calculation. This is the ratio of the Loan Required to the Current Valuation on the
        /// Property.
        /// </summary>
        public Double? LTV
        {
            get { return _DAO.LTV; }
            set { _DAO.LTV = value; }
        }

        /// <summary>
        /// The value of the Payment-to-Income calculation. This is the ratio of the Monthly Instalment to the Household Income.
        /// </summary>
        public Double? PTI
        {
            get { return _DAO.PTI; }
            set { _DAO.PTI = value; }
        }

        /// <summary>
        /// The Market Rate applicable to the Loan.
        /// </summary>
        public Double? MarketRate
        {
            get { return _DAO.MarketRate; }
            set { _DAO.MarketRate = value; }
        }

        /// <summary>
        /// The SPV from which the loan will be issued from if the Application is approved.
        /// </summary>
        public ISPV SPV
        {
            get
            {
                if (null == _DAO.SPV) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ISPV, SPV_DAO>(_DAO.SPV);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.SPV = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.SPV = (SPV_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// An Application is assigned an employment type e.g. Salaried or Self Employed. The Employment Type applicable to an
        /// Application is determined by calculating which Employment Type contributes the most to the total Household Income.
        /// </summary>
        public IEmploymentType EmploymentType
        {
            get
            {
                if (null == _DAO.EmploymentType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IEmploymentType, EmploymentType_DAO>(_DAO.EmploymentType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.EmploymentType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.EmploymentType = (EmploymentType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// The Credit Matrix version on which the Application is approved.
        /// </summary>
        public ICreditMatrix CreditMatrix
        {
            get
            {
                if (null == _DAO.CreditMatrix) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICreditMatrix, CreditMatrix_DAO>(_DAO.CreditMatrix);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CreditMatrix = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CreditMatrix = (CreditMatrix_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// The Credit Criteria version on which the Application is approved.
        /// </summary>
        public ICreditCriteria CreditCriteria
        {
            get
            {
                if (null == _DAO.CreditCriteria) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICreditCriteria, CreditCriteria_DAO>(_DAO.CreditCriteria);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CreditCriteria = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CreditCriteria = (CreditCriteria_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// Each Application is assigned a Rate Configuration. This allows the retrieval of the Market Rate and the Margin (Link Rate)
        /// applicable to the Application.
        /// </summary>
        public IRateConfiguration RateConfiguration
        {
            get
            {
                if (null == _DAO.RateConfiguration) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IRateConfiguration, RateConfiguration_DAO>(_DAO.RateConfiguration);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.RateConfiguration = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.RateConfiguration = (RateConfiguration_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}