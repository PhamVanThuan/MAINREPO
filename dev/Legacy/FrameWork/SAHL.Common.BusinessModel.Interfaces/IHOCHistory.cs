using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO
    /// </summary>
    public partial interface IHOCHistory : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.CommencementDate
        /// </summary>
        DateTime? CommencementDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.CancellationDate
        /// </summary>
        DateTime? CancellationDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.HOCHistoryDetails
        /// </summary>
        IEventList<IHOCHistoryDetail> HOCHistoryDetails
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.HOC
        /// </summary>
        IHOC HOC
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOCHistory_DAO.HOCInsurer
        /// </summary>
        IHOCInsurer HOCInsurer
        {
            get;
            set;
        }
    }
}