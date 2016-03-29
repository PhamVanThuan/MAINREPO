using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO
    /// </summary>
    public partial interface IHOCHistoryDetail : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCHistory
        /// </summary>
        IHOCHistory HOCHistory
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.EffectiveDate
        /// </summary>
        System.DateTime EffectiveDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.UpdateType
        /// </summary>
        System.Char UpdateType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCThatchAmount
        /// </summary>
        Double? HOCThatchAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCConventionalAmount
        /// </summary>
        Double? HOCConventionalAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCShingleAmount
        /// </summary>
        Double? HOCShingleAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCProrataPremium
        /// </summary>
        Double? HOCProrataPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCMonthlyPremium
        /// </summary>
        Double? HOCMonthlyPremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.PrintDate
        /// </summary>
        DateTime? PrintDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.SendFileDate
        /// </summary>
        DateTime? SendFileDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCAdministrationFee
        /// </summary>
        System.Double HOCAdministrationFee
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.HOCBasePremium
        /// </summary>
        System.Double HOCBasePremium
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistoryDetail_DAO.SASRIAAmount
        /// </summary>
        System.Double SASRIAAmount
        {
            get;
            set;
        }
    }
}