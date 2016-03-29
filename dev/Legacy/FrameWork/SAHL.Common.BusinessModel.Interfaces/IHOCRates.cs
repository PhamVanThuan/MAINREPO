using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.HOCRates_DAO
    /// </summary>
    public partial interface IHOCRates : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCRates_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCRates_DAO.HOCInsurer
        /// </summary>
        IHOCInsurer HOCInsurer
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCRates_DAO.HOCSubsidence
        /// </summary>
        IHOCSubsidence HOCSubsidence
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCRates_DAO.ThatchPremium
        /// </summary>
        System.Double ThatchPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCRates_DAO.ConventionalPremium
        /// </summary>
        System.Double ConventionalPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCRates_DAO.ShinglePremium
        /// </summary>
        System.Double ShinglePremium
        {
            get;
            set;
        }
    }
}