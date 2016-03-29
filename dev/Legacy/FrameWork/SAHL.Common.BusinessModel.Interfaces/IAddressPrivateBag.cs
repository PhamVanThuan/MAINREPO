using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Private Bag format.
    /// </summary>
    public partial interface IAddressPrivateBag : IEntityValidation, IBusinessModelObject, IAddress
    {
        /// <summary>
        /// The Private Bag Number of the Address
        /// </summary>
        System.String PrivateBagNumber
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