using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.WorkflowContext_DAO
    /// </summary>
    public partial interface IWorkflowContext : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowContext_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowContext_DAO.TableName
        /// </summary>
        System.String TableName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowContext_DAO.Alias
        /// </summary>
        System.String Alias
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowContext_DAO.PrimaryKeyColumn
        /// </summary>
        System.String PrimaryKeyColumn
        {
            get;
            set;
        }
    }
}