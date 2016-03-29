using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BankRange_DAO
    /// </summary>
    public partial interface IBankRange : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankRange_DAO.RangeStart
        /// </summary>
        System.Int32 RangeStart
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankRange_DAO.RangeEnd
        /// </summary>
        System.Int32 RangeEnd
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankRange_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankRange_DAO.DateChange
        /// </summary>
        System.DateTime DateChange
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankRange_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankRange_DAO.ACBBank
        /// </summary>
        IACBBank ACBBank
        {
            get;
            set;
        }
    }
}