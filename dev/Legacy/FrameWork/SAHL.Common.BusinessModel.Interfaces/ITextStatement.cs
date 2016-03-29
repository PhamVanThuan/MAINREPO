using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.TextStatement_DAO
    /// </summary>
    public partial interface ITextStatement : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TextStatement_DAO.StatementTitle
        /// </summary>
        System.String StatementTitle
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TextStatement_DAO.Statement
        /// </summary>
        System.String Statement
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TextStatement_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TextStatement_DAO.TextStatementType
        /// </summary>
        ITextStatementType TextStatementType
        {
            get;
            set;
        }
    }
}