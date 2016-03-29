using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AddressFormat_DAO
    /// </summary>
    public partial interface IAddressFormat : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The description of the Address Format (Street/Cluster Box etc)
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