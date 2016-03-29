using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.WorkflowData_DAO
    /// </summary>
    public partial interface IWorkflowData : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowData_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowData_DAO.TableName
        /// </summary>
        System.String TableName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowData_DAO.Alias
        /// </summary>
        System.String Alias
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowData_DAO.PrimaryKeyColumn
        /// </summary>
        System.String PrimaryKeyColumn
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowData_DAO.ForeignKeyColumn
        /// </summary>
        System.String ForeignKeyColumn
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.WorkflowData_DAO.WorkflowContextKey
        /// </summary>
        System.Int32 WorkflowContextKey
        {
            get;
            set;
        }
    }
}