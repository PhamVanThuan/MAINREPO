using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Token_DAO
    /// </summary>
    public partial interface IToken : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Token_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Token_DAO.MustTranslate
        /// </summary>
        System.Boolean MustTranslate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Token_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Token_DAO.ConditionTokens
        /// </summary>
        IEventList<IConditionToken> ConditionTokens
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Token_DAO.ParameterType
        /// </summary>
        IParameterType ParameterType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Token_DAO.StatementDefinition
        /// </summary>
        IStatementDefinition StatementDefinition
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Token_DAO.TokenType
        /// </summary>
        ITokenType TokenType
        {
            get;
            set;
        }
    }
}