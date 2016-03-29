using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// The LegalEntityAddress_DAO class specifies the Addresses related to a Legal Entity.
    /// </summary>
    public partial interface ILegalEntityAddress : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The date on which the address record should come into effect.  If this is set to a value
        /// greater than the current date, it will also set the General Status to inactive.  There is
        /// a database job that sets these to active once the effective date is reached.
        /// </summary>
        DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Whether or not the address is active.  If the effective date is greater than today's date, this
        /// will always be inactive.
        /// </summary>
        IGeneralStatus GeneralStatus { get; set; }

        bool IsActiveDomicilium { get; set; }
    }
}