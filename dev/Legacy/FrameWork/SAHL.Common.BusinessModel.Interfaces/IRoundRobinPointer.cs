using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.RoundRobinPointer_DAO
    /// </summary>
    public partial interface IRoundRobinPointer : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RoundRobinPointer_DAO.RoundRobinPointerIndexID
        /// </summary>
        System.Int32 RoundRobinPointerIndexID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RoundRobinPointer_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RoundRobinPointer_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RoundRobinPointer_DAO.RoundRobinPointerDefinitions
        /// </summary>
        IEventList<IRoundRobinPointerDefinition> RoundRobinPointerDefinitions
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RoundRobinPointer_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }
    }
}