using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.UIStatement_DAO
    /// </summary>
    public partial interface IUIStatement : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.ApplicationName
        /// </summary>
        System.String ApplicationName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.StatementName
        /// </summary>
        System.String StatementName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.ModifyDate
        /// </summary>
        System.DateTime ModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.Version
        /// </summary>
        System.Int32 Version
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.ModifyUser
        /// </summary>
        System.String ModifyUser
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.Statement
        /// </summary>
        System.String Statement
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.uiStatementType
        /// </summary>
        IUIStatementType uiStatementType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UIStatement_DAO.LastAccessedDate
        /// </summary>
        System.DateTime LastAccessedDate
        {
            get;
            set;
        }
    }
}