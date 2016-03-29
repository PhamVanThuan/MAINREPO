using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ReportType_DAO
    /// </summary>
    public partial interface IReportFormatType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportFormatType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportFormatType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportFormatType_DAO.ReportServicesFormatType
        /// </summary>
        System.String ReportServicesFormatType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportFormatType_DAO.FileExtension
        /// </summary>
        System.String FileExtension
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportFormatType_DAO.ContentType
        /// </summary>
        System.String ContentType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportFormatType_DAO.DisplayOrder
        /// </summary>
        System.Int32 DisplayOrder
        {
            get;
            set;
        }
    }
}
