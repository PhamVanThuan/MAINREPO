using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// CapType_DAO is used to hold the values of the different CAP types which can be offered to a client. The client
    /// can either be offered 1%,2% or 3% above their current rate.
    /// </summary>
    public partial interface ICapType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The Description of the CAP Type. e.g. 2% Above Current Rate
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// The percentage value above the clients rate which applies to the CAP Type. The example above would have a value of 0.02
        /// </summary>
        System.Double Value
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
        /// A CAP Type can have many configuration details set up against it in the CapTypeConfigurationDetail table.
        /// </summary>
        IEventList<ICapTypeConfigurationDetail> CapTypeConfigurationDetails
        {
            get;
        }
    }
}