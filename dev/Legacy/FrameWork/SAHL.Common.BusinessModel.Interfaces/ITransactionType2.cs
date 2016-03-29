using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.TransactionType_DAO
    /// </summary>
    public partial interface ITransactionType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        ///
        /// </summary>
        int ScreenBatch { get; }

        /// <summary>
        ///
        /// </summary>
        string HTMLColour { get; }
    }
}