using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Courier_DAO
    /// </summary>
    public partial interface ICourier : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Courier_DAO.CourierName
        /// </summary>
        System.String CourierName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Courier_DAO.EmailAddress
        /// </summary>
        System.String EmailAddress
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Courier_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}