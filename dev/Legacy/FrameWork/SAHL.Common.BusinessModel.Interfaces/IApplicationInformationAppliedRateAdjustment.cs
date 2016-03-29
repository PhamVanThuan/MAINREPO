using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO
    /// </summary>
    public partial interface IApplicationInformationAppliedRateAdjustment : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO.ApplicationElementValue
        /// </summary>
        System.String ApplicationElementValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO.ADUser
        /// </summary>
        IADUser ADUser
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO.ApplicationInformationFinancialAdjustment
        /// </summary>
        IApplicationInformationFinancialAdjustment ApplicationInformationFinancialAdjustment
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO.RateAdjustmentElement
        /// </summary>
        IRateAdjustmentElement RateAdjustmentElement
        {
            get;
            set;
        }
    }
}