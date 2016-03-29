using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO
    /// </summary>
    public partial interface IRoundRobinPointerDefinition : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO.GenericKey
        /// </summary>
        System.Int32 GenericKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO.ApplicationName
        /// </summary>
        System.String ApplicationName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO.StatementName
        /// </summary>
        System.String StatementName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO.RoundRobinPointer
        /// </summary>
        IRoundRobinPointer RoundRobinPointer
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO.GenericKeyType
        /// </summary>
        IGenericKeyType GenericKeyType
        {
            get;
            set;
        }
    }
}