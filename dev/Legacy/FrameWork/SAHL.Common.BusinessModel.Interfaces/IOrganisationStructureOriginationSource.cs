using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.OrganisationStructureOriginationSource_DAO
    /// </summary>
    public partial interface IOrganisationStructureOriginationSource : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructureOriginationSource_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructureOriginationSource_DAO.OrganisationStructure
        /// </summary>
        IOrganisationStructure OrganisationStructure
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructureOriginationSource_DAO.OriginationSource
        /// </summary>
        IOriginationSource OriginationSource
        {
            get;
            set;
        }
    }
}