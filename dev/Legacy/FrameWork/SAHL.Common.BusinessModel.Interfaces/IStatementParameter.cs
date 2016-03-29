using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO
    /// </summary>
    public partial interface IStatementParameter : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.ParameterName
        /// </summary>
        System.String ParameterName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.ParameterLength
        /// </summary>
        Int32? ParameterLength
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.DisplayName
        /// </summary>
        System.String DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.Required
        /// </summary>
        System.Boolean Required
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.ParameterType
        /// </summary>
        IParameterType ParameterType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.PopulationStatementDefinition
        /// </summary>
        IStatementDefinition PopulationStatementDefinition
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.StatementParameter_DAO.StatementDefinition
        /// </summary>
        IStatementDefinition StatementDefinition
        {
            get;
            set;
        }
    }
}