using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.GenericSetDefinition_DAO
    /// </summary>
    public partial interface IGenericSetDefinition : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.GenericSetDefinition_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.GenericSetDefinition_DAO.Explanation
        /// </summary>
        System.String Explanation
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.GenericSetDefinition_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.GenericSetDefinition_DAO.GenericSets
        /// </summary>
        IEventList<IGenericSet> GenericSets
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.GenericSetDefinition_DAO.GenericSetType
        /// </summary>
        IGenericSetType GenericSetType
        {
            get;
            set;
        }
    }
}