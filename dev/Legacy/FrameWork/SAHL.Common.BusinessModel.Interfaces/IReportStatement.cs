using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO
    /// </summary>
    public partial interface IReportStatement : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.ReportName
        /// </summary>
        System.String ReportName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.StatementName
        /// </summary>
        System.String StatementName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.GroupBy
        /// </summary>
        System.String GroupBy
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.OrderBy
        /// </summary>
        System.String OrderBy
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.ReportOutputPath
        /// </summary>
        System.String ReportOutputPath
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.Feature
        /// </summary>
        IFeature Feature
        {
            get;
            set;
        }

        /// <summary>
        /// The OriginationSourceProduct for which this Report is defined.
        /// </summary>
        IOriginationSourceProduct OriginationSourceProduct
        {
            get;
            set;
        }

        /// <summary>
        /// a grouping of reports
        /// </summary>
        IReportGroup ReportGroup
        {
            get;
            set;
        }

        /// <summary>
        /// The type of this report.
        /// </summary>
        IReportType ReportType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.CorrespondenceMediums
        /// </summary>
        IEventList<ICorrespondenceMedium> CorrespondenceMediums
        {
            get;
        }
    }
}