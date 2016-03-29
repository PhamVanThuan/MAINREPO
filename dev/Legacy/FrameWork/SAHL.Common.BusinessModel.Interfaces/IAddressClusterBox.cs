using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Cluster Box format.
    /// </summary>
    public partial interface IAddressClusterBox : IEntityValidation, IBusinessModelObject, IAddress
    {
        /// <summary>
        /// The Cluster Box number of the Address
        /// </summary>
        System.String ClusterBoxNumber
        {
            get;
            set;
        }

        /// <summary>
        /// The Post Office which the Address belongs to.
        /// </summary>
        IPostOffice PostOffice
        {
            get;
            set;
        }
    }
}