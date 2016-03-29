using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO
    /// </summary>
    public partial interface IDataGridConfiguration : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.StatementName
        /// </summary>
        System.String StatementName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.ColumnName
        /// </summary>
        System.String ColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.ColumnDescription
        /// </summary>
        System.String ColumnDescription
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.Sequence
        /// </summary>
        System.Int32 Sequence
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.Width
        /// </summary>
        System.String Width
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.Visible
        /// </summary>
        System.Boolean Visible
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.IndexIdentifier
        /// </summary>
        System.Boolean IndexIdentifier
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.DataGridConfigurationType
        /// </summary>
        IDataGridConfigurationType DataGridConfigurationType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.FormatType
        /// </summary>
        IFormatType FormatType
        {
            get;
            set;
        }
    }
}