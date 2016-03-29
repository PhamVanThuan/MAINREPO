using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Street format.
    /// </summary>
    public partial interface IAddressStreet : IEntityValidation, IBusinessModelObject, IAddress
    {
        /// <summary>
        /// The Building Number of the Address.
        /// </summary>
        System.String BuildingNumber
        {
            get;
            set;
        }

        /// <summary>
        /// The Building Name of the Address.
        /// </summary>
        System.String BuildingName
        {
            get;
            set;
        }

        /// <summary>
        /// The Street Number of the Address.
        /// </summary>
        System.String StreetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// The Street Name of the Address.
        /// </summary>
        System.String StreetName
        {
            get;
            set;
        }

        /// <summary>
        /// The Unit Number of the Address.
        /// </summary>
        System.String UnitNumber
        {
            get;
            set;
        }

        /// <summary>
        /// The Suburb which the Address belongs to.
        /// </summary>
        ISuburb Suburb
        {
            get;
            set;
        }
    }
}