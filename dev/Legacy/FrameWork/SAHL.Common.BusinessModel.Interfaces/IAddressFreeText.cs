using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Free Text format.
    /// </summary>
    public partial interface IAddressFreeText : IEntityValidation, IBusinessModelObject, IAddress
    {
        /// <summary>
        /// Free Text Line 1
        /// </summary>
        System.String FreeText1
        {
            get;
            set;
        }

        /// <summary>
        /// Free Text Line 2
        /// </summary>
        System.String FreeText2
        {
            get;
            set;
        }

        /// <summary>
        /// Free Text Line 3
        /// </summary>
        System.String FreeText3
        {
            get;
            set;
        }

        /// <summary>
        /// Free Text Line 4
        /// </summary>
        System.String FreeText4
        {
            get;
            set;
        }

        /// <summary>
        /// Free Text Line 5
        /// </summary>
        System.String FreeText5
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