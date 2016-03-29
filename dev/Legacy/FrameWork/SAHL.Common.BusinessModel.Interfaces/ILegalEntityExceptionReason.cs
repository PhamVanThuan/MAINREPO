using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// The LegalEntityExceptionReason_DAO is used to store the reasons why a Legal Entity  failed validation.
    /// </summary>
    public partial interface ILegalEntityExceptionReason : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The description of the exception reason. e.g. Missing Salutation, Missing Surname
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// The priority of the exception reason.
        /// </summary>
        System.Byte Priority
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityExceptionReason_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityExceptionReason_DAO.LegalEntityExceptionReasons
        /// </summary>
        IEventList<ILegalEntity> LegalEntityExceptionReasons
        {
            get;
        }
    }
}