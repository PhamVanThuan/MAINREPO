using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO
    /// </summary>
    public partial interface IApplicationInformationPersonalLoan : IEntityValidation, IBusinessModelObject
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
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.LoanAmount
        /// </summary>
        System.Double LoanAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.Term
        /// </summary>
        System.Int32 Term
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.MonthlyInstalment
        /// </summary>
        System.Double MonthlyInstalment
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.LifePremium
        /// </summary>
        System.Double LifePremium
        {
            get;
            set;
        }



        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.FeesTotal
        /// </summary>
        System.Double FeesTotal
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.CreditCriteriaUnsecuredLending
        /// </summary>
        ICreditCriteriaUnsecuredLending CreditCriteriaUnsecuredLending
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.Margin
        /// </summary>
        IMargin Margin
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.MarketRate
        /// </summary>
        IMarketRate MarketRate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationPersonalLoan_DAO.ApplicationInformation
        /// </summary>
        IApplicationInformation ApplicationInformation
        {
            get;
            set;
        }
    }
}