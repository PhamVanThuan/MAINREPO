using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.RiskMatrixRange_DAO
    /// </summary>
    public partial interface IRiskMatrixRange : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixRange_DAO.Min
        /// </summary>
        Double? Min
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixRange_DAO.Max
        /// </summary>
        Double? Max
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixRange_DAO.Designation
        /// </summary>
        System.String Designation
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RiskMatrixRange_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}