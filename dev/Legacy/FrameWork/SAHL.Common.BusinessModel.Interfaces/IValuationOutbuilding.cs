using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ValuationOutbuilding_DAO describes the extent, rate and roof type of the Outbuilding associated to a SAHL Manual Valuation.
    /// </summary>
    public partial interface IValuationOutbuilding : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The size, in sq m, of the Outbuilding.
        /// </summary>
        Double? Extent
        {
            get;
            set;
        }

        /// <summary>
        /// The replacement value per sq m for the Outbuilding.
        /// </summary>
        Double? Rate
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
        /// Foreign key reference to the Valuation table. Each Outbuilding can only belong to a single valuation.
        /// </summary>
        IValuation Valuation
        {
            get;
            set;
        }

        /// <summary>
        /// Foreign key reference to the ValuationRoofType table. Each Outbuilding can only belong to one ValuationRoofType.
        /// </summary>
        IValuationRoofType ValuationRoofType
        {
            get;
            set;
        }
    }
}