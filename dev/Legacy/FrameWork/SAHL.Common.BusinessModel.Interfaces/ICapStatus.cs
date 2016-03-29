using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// CapStatus_DAO is used to hold the different statuses that a CapOffer is assigned.
    /// </summary>
    public partial interface ICapStatus : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The CapStatus Description e.g. Open CAP 2 Offer, Forms Sent
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