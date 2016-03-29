using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ValuationMainBuilding_DAO describes the extent, rate and roof type of the Main Building associated to a SAHL Manual Valuation.
    /// </summary>
    public partial interface IValuationMainBuilding : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Primary Key, which is referring to the ValuationKey for the Valuation to which the Main Building belongs.
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// The size, in sq m, of the Main Building.
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
        /// Each Main Building can only belong to a single SAHL Manual Valuation.
        /// </summary>
        IValuation Valuation
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the ValuationRoofType table. A Main Building can only have a single Roof Type.
        /// </summary>
        IValuationRoofType ValuationRoofType
        {
            get;
            set;
        }
    }
}