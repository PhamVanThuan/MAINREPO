using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.LegalEntityExceptionStatus_DAO
    /// </summary>
    public partial interface ILegalEntityExceptionStatus : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityExceptionStatus_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityExceptionStatus_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}