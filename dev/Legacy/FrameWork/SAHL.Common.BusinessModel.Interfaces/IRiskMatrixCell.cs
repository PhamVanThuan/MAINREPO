using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO
    /// </summary>
    public partial interface IRiskMatrixCell : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.RiskMatrixDimensions
        /// </summary>
        IEventList<IRiskMatrixDimension> RiskMatrixDimensions
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.RiskMatrixRanges
        /// </summary>
        IEventList<IRiskMatrixRange> RiskMatrixRanges
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.CreditScoreDecision
        /// </summary>
        ICreditScoreDecision CreditScoreDecision
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.RiskMatrixRevision
        /// </summary>
        IRiskMatrixRevision RiskMatrixRevision
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.RuleItem
        /// </summary>
        IRuleItem RuleItem
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.Designation
        /// </summary>
        System.String Designation
        {
            get;
            set;
        }
    }
}