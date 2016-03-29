using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO
    /// </summary>
    public partial interface IOriginationSourceProduct : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO.OriginationSource
        /// </summary>
        IOriginationSource OriginationSource
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO.Product
        /// </summary>
        IProduct Product
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO.CreditMatrices
        /// </summary>
        IEventList<ICreditMatrix> CreditMatrices
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO.Priorities
        /// </summary>
        IEventList<IPriority> Priorities
        {
            get;
        }
    }
}