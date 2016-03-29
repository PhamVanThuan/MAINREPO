using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ReasonTypeGroup_DAO is used to group the various Reason Types together. It also contains a ParentKey which allows
    /// a hierarchy of ReasonTypeGroups to be built up.
    /// </summary>
    public partial interface IReasonTypeGroup : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The description of the ReasonTypeGroup.
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

        /// <summary>
        /// There is a one-to-many relationship between ReasonTypeGroups and ReasonTypes. A single ReasonTypeGroup has many ReasonTypes
        /// which form part of the group.
        /// </summary>
        IEventList<IReasonType> ReasonTypes
        {
            get;
        }

        /// <summary>
        /// This property will retrieve the children ReasonTypeGroups. e.g. Credit Decline Income and Credit Decline Profile Reason Type
        /// groups could all belong to a parent Reason Type group of Credit. This property could consist of many
        /// Reason Type groups.
        /// </summary>
        IEventList<IReasonTypeGroup> Children
        {
            get;
        }

        /// <summary>
        /// This property is the ReasonTypeGroupKey which is serving as the Parent group. This property would only be a single
        /// Reason type group.
        /// </summary>
        IReasonTypeGroup Parent
        {
            get;
            set;
        }
    }
}