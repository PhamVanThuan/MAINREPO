using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ReportGroup_DAO
    /// </summary>
    public partial interface IReportGroup : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportGroup_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportGroup_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportGroup_DAO.ReportStatements
        /// </summary>
        IEventList<IReportStatement> ReportStatements
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportGroup_DAO.Feature
        /// </summary>
        IFeature Feature
        {
            get;
            set;
        }
    }
}