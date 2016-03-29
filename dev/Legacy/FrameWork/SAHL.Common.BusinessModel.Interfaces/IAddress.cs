using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Address_DAO is the base class from which the format specific addresses are derived. The discrimination is based on the
    /// Address Format (Street, Box etc).
    /// </summary>
    public partial interface IAddress : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The UserID of the last person who updated information on the Address.
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// The date when the Address record was last changed.
        /// </summary>
        DateTime? ChangeDate
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

        /// <summary>
        /// The Country property of an address.
        /// </summary>
        System.String RRR_CountryDescription
        {
            get;
        }

        /// <summary>
        /// The Province property of an address.
        /// </summary>
        System.String RRR_ProvinceDescription
        {
            get;
        }

        /// <summary>
        /// The City property of an address.
        /// </summary>
        System.String RRR_CityDescription
        {
            get;
        }

        /// <summary>
        /// The Suburb property of an address.
        /// </summary>
        System.String RRR_SuburbDescription
        {
            get;
        }

        /// <summary>
        /// The Postal Code property of an address.
        /// </summary>
        System.String RRR_PostalCode
        {
            get;
        }
    }
}