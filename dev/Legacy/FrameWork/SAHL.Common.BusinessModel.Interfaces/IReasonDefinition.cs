using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ReasonDefinition_DAO links the Reason Description and the Reason Type.
    /// </summary>
    public partial interface IReasonDefinition : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// An indicator as to whether a comment can be stored against the reason.
        /// </summary>
        System.Boolean AllowComment
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
        /// SAHL.Common.BusinessModel.DAO.ReasonDefinition_DAO.EnforceComment
        /// </summary>
        System.Boolean EnforceComment
        {
            get;
            set;
        }

        /// <summary>
        /// a list of OriginationSourceProducts this reasondefinition is applicable to.
        /// </summary>
        IEventList<IOriginationSourceProduct> OriginationSourceProducts
        {
            get;
        }

        /// <summary>
        /// Each Reason Definition belongs to a Reason Description, this is the foreign key reference to the ReasonDescription table.
        /// </summary>
        IReasonDescription ReasonDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Each Reason Definition belongs to a Reason Type, this is the foreign key reference to the ReasonType table.
        /// </summary>
        IReasonType ReasonType
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the GeneralStatus table.
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }
    }
}