using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// LegalEntityNaturalPerson_DAO is derived from LegalEntity_DAO and is used to instantiate a Legal Entity of type "Natural Person."
    /// </summary>
    public partial interface ILegalEntityNaturalPerson : IEntityValidation, IBusinessModelObject, ILegalEntity
    {
        /// <summary>
        /// The foreign key reference to the PopulationGroup table. A Natural Person Legal Entity belongs to a single Population Group type.
        /// </summary>
        IPopulationGroup PopulationGroup
        {
            get;
            set;
        }

        /// <summary>
        /// The Preferred Name of the Natural Person.
        /// </summary>
        System.String PreferredName
        {
            get;
            set;
        }

        /// <summary>
        /// The Date of Birth of the Natural Person.
        /// </summary>
        DateTime? DateOfBirth
        {
            get;
            set;
        }

        /// <summary>
        /// The highest education level that the Legal Entity has achieved.
        /// </summary>
        IEducation Education
        {
            get;
            set;
        }

        /// <summary>
        /// The Legal Entity's Home Language.
        /// </summary>
        ILanguage HomeLanguage
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LegalEntityNaturalPerson_DAO.ITCs
        /// </summary>
        IEventList<IITC> ITCs
        {
            get;
        }
    }
}