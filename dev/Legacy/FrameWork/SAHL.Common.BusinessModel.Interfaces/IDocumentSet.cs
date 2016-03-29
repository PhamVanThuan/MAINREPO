using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DocumentSet_DAO
    /// </summary>
    public partial interface IDocumentSet : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentSet_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentSet_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentSet_DAO.DocumentSetConfigs
        /// </summary>
        IEventList<IDocumentSetConfig> DocumentSetConfigs
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentSet_DAO.ApplicationType
        /// </summary>
        IApplicationType ApplicationType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DocumentSet_DAO.OriginationSourceProduct
        /// </summary>
        IOriginationSourceProduct OriginationSourceProduct
        {
            get;
            set;
        }
    }
}