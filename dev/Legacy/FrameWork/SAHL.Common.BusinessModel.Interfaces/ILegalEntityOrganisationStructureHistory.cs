using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO
    /// </summary>
    public partial interface ILegalEntityOrganisationStructureHistory : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO.LegalEntityOrganisationStructure
        /// </summary>
        ILegalEntityOrganisationStructure LegalEntityOrganisationStructure
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO.OrganisationStructureKey
        /// </summary>
        IOrganisationStructure OrganisationStructureKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO.Action
        /// </summary>
        System.String Action
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}