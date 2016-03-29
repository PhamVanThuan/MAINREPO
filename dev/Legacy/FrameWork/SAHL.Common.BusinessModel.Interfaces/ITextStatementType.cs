using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.TextStatementType_DAO
    /// </summary>
    public partial interface ITextStatementType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TextStatementType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TextStatementType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TextStatementType_DAO.OriginationSourceProduct
        /// </summary>
        IOriginationSourceProduct OriginationSourceProduct
        {
            get;
            set;
        }
    }
}