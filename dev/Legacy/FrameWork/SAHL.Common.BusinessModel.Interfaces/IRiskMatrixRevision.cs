using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.RiskMatrixRevision_DAO
    /// </summary>
    public partial interface IRiskMatrixRevision : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixRevision_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixRevision_DAO.RevisionDate
        /// </summary>
        System.DateTime RevisionDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixRevision_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixRevision_DAO.RiskMatrixCells
        /// </summary>
        IEventList<IRiskMatrixCell> RiskMatrixCells
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixRevision_DAO.RiskMatrix
        /// </summary>
        IRiskMatrix RiskMatrix
        {
            get;
            set;
        }
    }
}