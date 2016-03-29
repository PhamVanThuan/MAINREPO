using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AddressType_DAO
    /// </summary>
    public partial interface IAddressType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The Address Type Description (Residential/Postal)
        /// </summary>
        System.String Description
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