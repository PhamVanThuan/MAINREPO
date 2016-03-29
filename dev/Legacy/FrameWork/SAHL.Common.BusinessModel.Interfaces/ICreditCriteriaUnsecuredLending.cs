using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO
    /// </summary>
    public partial interface ICreditCriteriaUnsecuredLending : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO.MinLoanAmount
        /// </summary>
        System.Double MinLoanAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO.MaxLoanAmount
        /// </summary>
        System.Double MaxLoanAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO.Term
        /// </summary>
        System.Int32 Term
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO.CreditMatrixUnsecuredLending
        /// </summary>
        ICreditMatrixUnsecuredLending CreditMatrixUnsecuredLending
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditCriteriaUnsecuredLending_DAO.Margin
        /// </summary>
        IMargin Margin
        {
            get;
            set;
        }
    }
}