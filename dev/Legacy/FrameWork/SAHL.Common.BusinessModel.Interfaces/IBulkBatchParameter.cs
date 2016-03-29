using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BulkBatchParameter_DAO
    /// </summary>
    public partial interface IBulkBatchParameter : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchParameter_DAO.ParameterName
        /// </summary>
        System.String ParameterName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchParameter_DAO.ParameterValue
        /// </summary>
        System.String ParameterValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchParameter_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchParameter_DAO.BulkBatch
        /// </summary>
        IBulkBatch BulkBatch
        {
            get;
            set;
        }
    }
}