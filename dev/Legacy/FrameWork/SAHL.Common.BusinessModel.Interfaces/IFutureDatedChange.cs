using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO
    /// </summary>
    public partial interface IFutureDatedChange : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.IdentifierReferenceKey
        /// </summary>
        System.Int32 IdentifierReferenceKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.EffectiveDate
        /// </summary>
        System.DateTime EffectiveDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.NotificationRequired
        /// </summary>
        System.Boolean NotificationRequired
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.InsertDate
        /// </summary>
        System.DateTime InsertDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.FutureDatedChangeDetails
        /// </summary>
        IEventList<IFutureDatedChangeDetail> FutureDatedChangeDetails
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChange_DAO.FutureDatedChangeType
        /// </summary>
        IFutureDatedChangeType FutureDatedChangeType
        {
            get;
            set;
        }
    }
}