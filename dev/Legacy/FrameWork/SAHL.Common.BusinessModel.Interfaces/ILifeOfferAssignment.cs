using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO
    /// </summary>
    public partial interface ILifeOfferAssignment : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO.OfferKey
        /// </summary>
        System.Int32 OfferKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO.LoanAccountKey
        /// </summary>
        System.Int32 LoanAccountKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO.LoanOfferKey
        /// </summary>
        System.Int32 LoanOfferKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO.LoanOfferTypeKey
        /// </summary>
        System.Int32 LoanOfferTypeKey
        {
            get;
            set;
        }

        /// <summary>
        /// The date when the life offer was assigned
        /// </summary>
        System.DateTime DateAssigned
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO.ADUserName
        /// </summary>
        System.String ADUserName
        {
            get;
            set;
        }
    }
}