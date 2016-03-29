using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ValuationCottage_DAO describes the Extent, Roof Type and rate/sq m of a Cottage for a SAHL Manual Valuation.
    /// </summary>
    public partial interface IValuationCottage : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// The size, in sq m, of the cottage.
        /// </summary>
        Double? Extent
        {
            get;
            set;
        }

        /// <summary>
        /// The replacement value per sq m for the Cottage.
        /// </summary>
        Double? Rate
        {
            get;
            set;
        }

        /// <summary>
        /// The Cottage is related to a single Valuation.
        /// </summary>
        IValuation Valuation
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the ValuationRoofType table.
        /// </summary>
        IValuationRoofType ValuationRoofType
        {
            get;
            set;
        }
    }
}