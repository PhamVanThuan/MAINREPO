using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.PreContextFilter_DAO
    /// </summary>
    public partial interface IPreContextFilter : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PreContextFilter_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PreContextFilter_DAO.ContextKey
        /// </summary>
        System.Int32 ContextKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PreContextFilter_DAO.OfferTypeKey
        /// </summary>
        System.Int32 OfferTypeKey
        {
            get;
            set;
        }
    }
}