using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO
    /// </summary>
    public partial interface IStatementDefinition : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO.ApplicationName
        /// </summary>
        System.String ApplicationName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO.StatementName
        /// </summary>
        System.String StatementName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO.StatementParameters
        /// </summary>
        IEventList<IStatementParameter> StatementParameters
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StatementDefinition_DAO.Tokens
        /// </summary>
        IEventList<IToken> Tokens
        {
            get;
        }
    }
}