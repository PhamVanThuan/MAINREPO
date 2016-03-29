using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Valuator_DAO describes a Valuator who carries out the property valuations.
    /// </summary>
    public partial interface IValuator : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Contact Person at the Valuator
        /// </summary>
        System.String ValuatorContact
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Valuator_DAO.ValuatorPassword
        /// </summary>
        System.String ValuatorPassword
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Valuator_DAO.LimitedUserGroup
        /// </summary>
        System.Byte LimitedUserGroup
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
        /// The status of the Valuator e.g. Active or Inactive.
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Foreign Key reference to the Legal Entity table. Each Valuator exists as a Legal Entity on the database.
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Valuator_DAO.OriginationSources
        /// </summary>
        IEventList<IOriginationSource> OriginationSources
        {
            get;
        }
    }
}