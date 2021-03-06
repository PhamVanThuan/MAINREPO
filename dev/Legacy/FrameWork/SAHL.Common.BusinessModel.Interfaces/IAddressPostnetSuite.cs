using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Postnet Suite format.
    /// </summary>
    public partial interface IAddressPostnetSuite : IEntityValidation, IBusinessModelObject, IAddress
    {
        /// <summary>
        /// The Postnet Box Number of the Address.
        /// </summary>
        System.String PrivateBagNumber
        {
            get;
            set;
        }

        /// <summary>
        /// The Postnet Suite Number of the Address.
        /// </summary>
        System.String SuiteNumber
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