using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ACBBank_DAO
    /// </summary>
    public partial interface IACBBank : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Contains the description of the bank i.e. Nedbank, ABSA etc
        /// </summary>
        System.String ACBBankDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}