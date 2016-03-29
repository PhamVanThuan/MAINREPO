using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Validator_DAO
    /// </summary>
    public partial interface IValidator : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Validator_DAO.InitialValue
        /// </summary>
        System.String InitialValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Validator_DAO.RegularExpression
        /// </summary>
        System.String RegularExpression
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Validator_DAO.MinimumValue
        /// </summary>
        Double? MinimumValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Validator_DAO.MaximumValue
        /// </summary>
        Double? MaximumValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Validator_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Validator_DAO.DomainField
        /// </summary>
        IDomainField DomainField
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Validator_DAO.ErrorRepository
        /// </summary>
        IErrorRepository ErrorRepository
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Validator_DAO.ValidatorType
        /// </summary>
        IValidatorType ValidatorType
        {
            get;
            set;
        }
    }
}