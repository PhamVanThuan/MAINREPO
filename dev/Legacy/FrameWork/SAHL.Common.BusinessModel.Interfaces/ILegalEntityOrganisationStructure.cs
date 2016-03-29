using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructure_DAO
    /// </summary>
    public partial interface ILegalEntityOrganisationStructure : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructure_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructure_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructure_DAO.OrganisationStructure
        /// </summary>
        IOrganisationStructure OrganisationStructure
        {
            get;
            set;
        }
    }
}