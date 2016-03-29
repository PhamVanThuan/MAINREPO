using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ACBType_DAO
    /// </summary>
    public partial interface IACBType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The type of bank account. i.e. A Cheque or Savings account
        /// </summary>
        System.String ACBTypeDescription
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