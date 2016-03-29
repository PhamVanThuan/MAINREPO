using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.InternalRole_DAO
    /// </summary>
    public partial interface IInternalRole : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InternalRole_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InternalRole_DAO.TableName
        /// </summary>
        System.String TableName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InternalRole_DAO.Alias
        /// </summary>
        System.String Alias
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InternalRole_DAO.PrimaryKeyColumn
        /// </summary>
        System.String PrimaryKeyColumn
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InternalRole_DAO.ContextTableKey
        /// </summary>
        System.Int32 ContextTableKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InternalRole_DAO.ContextTableJoinKey
        /// </summary>
        System.String ContextTableJoinKey
        {
            get;
            set;
        }
    }
}