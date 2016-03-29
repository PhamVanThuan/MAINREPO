using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO
    /// </summary>
    public partial interface IHearingDetail : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.CaseNumber
        /// </summary>
        System.String CaseNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.HearingDate
        /// </summary>
        System.DateTime HearingDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.DebtCounselling
        /// </summary>
        IDebtCounselling DebtCounselling
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.HearingType
        /// </summary>
        IHearingType HearingType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.Court
        /// </summary>
        ICourt Court
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.HearingAppearanceType
        /// </summary>
        IHearingAppearanceType HearingAppearanceType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HearingDetail_DAO.Comment
        /// </summary>
        System.String Comment
        {
            get;
            set;
        }
    }
}