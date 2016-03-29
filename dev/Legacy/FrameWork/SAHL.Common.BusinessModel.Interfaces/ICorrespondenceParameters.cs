using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CorrespondenceParameters_DAO
    /// </summary>
    public partial interface ICorrespondenceParameters : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceParameters_DAO.Correspondence
        /// </summary>
        ICorrespondence Correspondence
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceParameters_DAO.ReportParameterValue
        /// </summary>
        System.String ReportParameterValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceParameters_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceParameters_DAO.ReportParameter
        /// </summary>
        IReportParameter ReportParameter
        {
            get;
            set;
        }
    }
}