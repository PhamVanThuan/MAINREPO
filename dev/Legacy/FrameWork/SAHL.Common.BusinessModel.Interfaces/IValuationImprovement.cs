using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// ValuationImprovement_DAO describes improvements, the extent (where applicable) and the replacement rate/value
    /// of the improvements captured for a SAHL Manual Valuation.
    /// </summary>
    public partial interface IValuationImprovement : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The date on which the Improvement was added.
        /// </summary>
        DateTime? ImprovementDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ValuationImprovement_DAO.ImprovementValue
        /// </summary>
        System.Double ImprovementValue
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
        /// An Improvement may only be related to a single Valuation.
        /// </summary>
        IValuation Valuation
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the ValuationImprovementType table. Each Improvement requires a Type.
        /// </summary>
        IValuationImprovementType ValuationImprovementType
        {
            get;
            set;
        }
    }
}