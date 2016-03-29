using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Box format.
    /// </summary>
    public partial interface IAddressBox : IEntityValidation, IBusinessModelObject, IAddress
    {
        /// <summary>
        /// The Post Office Box Number of the Address
        /// </summary>
        System.String BoxNumber
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