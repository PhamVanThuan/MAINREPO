using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO
    /// </summary>
    public partial interface IFutureDatedChangeDetail : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.ReferenceKey
        /// </summary>
        System.Int32 ReferenceKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.Action
        /// </summary>
        System.Char Action
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.TableName
        /// </summary>
        System.String TableName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.ColumnName
        /// </summary>
        System.String ColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.Value
        /// </summary>
        System.String Value
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.FutureDatedChange
        /// </summary>
        IFutureDatedChange FutureDatedChange
        {
            get;
            set;
        }
    }
}