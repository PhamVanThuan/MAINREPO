using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Bond_DAO
    /// </summary>
    public partial interface IBond : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.BondRegistrationNumber
        /// </summary>
        System.String BondRegistrationNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.BondRegistrationAmount
        /// </summary>
        System.Double BondRegistrationAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.BondLoanAgreementAmount
        /// </summary>
        System.Double BondLoanAgreementAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.LoanAgreements
        /// </summary>
        IEventList<ILoanAgreement> LoanAgreements
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.Attorney
        /// </summary>
        IAttorney Attorney
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.DeedsOffice
        /// </summary>
        IDeedsOffice DeedsOffice
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.Application
        /// </summary>
        IApplication Application
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Bond_DAO.MortgageLoans
        /// </summary>
        IEventList<IMortgageLoan> MortgageLoans
        {
            get;
        }
    }
}