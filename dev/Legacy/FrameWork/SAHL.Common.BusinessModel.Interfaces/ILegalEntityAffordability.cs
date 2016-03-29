using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// The LegalEntityAffordability_DAO class contains the information regarding the Legal Entity's Affordability Assessment.
    /// </summary>
    public partial interface ILegalEntityAffordability : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The Rand Value of the Affordability Assessment Entry.
        /// </summary>
        System.Double Amount
        {
            get;
            set;
        }

        /// <summary>
        /// Description field.
        /// </summary>
        System.String Description
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
        /// The specific Affordability Assessment category that we are capturing information for. e.g. Basic Salary, Rental, Commission etc.
        /// </summary>
        IAffordabilityType AffordabilityType
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the Legal Entity table. Each Affordability Assessment entry belongs to a single Legal Entity.
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the Offer table. Each Affordability Assessment entry belongs to a single Application.
        /// </summary>
        IApplication Application
        {
            get;
            set;
        }
    }
}