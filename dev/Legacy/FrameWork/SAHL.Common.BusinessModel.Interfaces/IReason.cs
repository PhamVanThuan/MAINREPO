using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Reason is a link between a ReasonDefinition and an generic object in the system. It is used to supply a reason for an action that is performed on that object.
    /// </summary>
    public partial interface IReason : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The Primary Key value of the GenericKey which the Reason is attached to.
        /// </summary>
        System.Int32 GenericKey
        {
            get;
            set;
        }

        /// <summary>
        /// A comment for the reason.
        /// </summary>
        System.String Comment
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
        /// This is the foreign key reference to the ReasonDefinition table. Each Reason has a Definition which is stored in
        /// the ReasonDefinition table.
        /// </summary>
        IReasonDefinition ReasonDefinition
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Reason_DAO.StageTransition
        /// </summary>
        IStageTransition StageTransition
        {
            get;
            set;
        }
    }
}