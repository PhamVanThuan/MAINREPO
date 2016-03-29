using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO
    /// </summary>
    public partial interface IDebtCounselling : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.PaymentReceivedDate
        /// </summary>
        DateTime? PaymentReceivedDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.PaymentReceivedAmount
        /// </summary>
        Double? PaymentReceivedAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.DebtCounsellingStatus
        /// </summary>
        IDebtCounsellingStatus DebtCounsellingStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.DebtCounsellingGroup
        /// </summary>
        IDebtCounsellingGroup DebtCounsellingGroup
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.DiaryDate
        /// </summary>
        DateTime? DiaryDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.ReferenceNumber
        /// </summary>
        System.String ReferenceNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.HearingDetails
        /// </summary>
        IEventList<IHearingDetail> HearingDetails
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.Proposals
        /// </summary>
        IEventList<IProposal> Proposals
        {
            get;
        }
    }
}