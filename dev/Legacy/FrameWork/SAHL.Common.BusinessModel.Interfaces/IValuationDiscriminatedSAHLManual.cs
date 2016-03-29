using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// This is derived from Valuation_DAO. When instantiated it represents a SAHL Manual Valuation.
    /// </summary>
    public partial interface IValuationDiscriminatedSAHLManual : IEntityValidation, IBusinessModelObject, IValuation
    {
        /// <summary>
        /// A SAHL Manual Valuation can only have one Main Building.
        /// </summary>
        IValuationMainBuilding ValuationMainBuilding
        {
            get;
            set;
        }

        /// <summary>
        /// A SAHL Manual Valuation can have many outbuildings.
        /// </summary>
        IEventList<IValuationOutbuilding> ValuationOutBuildings
        {
            get;
        }

        /// <summary>
        /// A SAHL Manual Valuation can only have one Total Combined Thatch Value.
        /// </summary>
        IValuationCombinedThatch ValuationCombinedThatch
        {
            get;
            set;
        }

        /// <summary>
        /// A SAHL Manual Valuation can have many improvements associated to it. e.g. Tennis Court, Walls or a Pool.
        /// </summary>
        IEventList<IValuationImprovement> ValuationImprovements
        {
            get;
        }

        /// <summary>
        /// A SAHL Manual Valuation can only have one Cottage.
        /// </summary>
        IValuationCottage ValuationCottage
        {
            get;
            set;
        }
    }
}