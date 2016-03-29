using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface ILegalEntityBankAccount : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }
    }
}