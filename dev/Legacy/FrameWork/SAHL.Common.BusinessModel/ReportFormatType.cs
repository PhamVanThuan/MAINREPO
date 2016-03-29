using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ReportType_DAO
    /// </summary>
    public partial class ReportFormatType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ReportFormatType_DAO>, IReportFormatType
    {
        public ReportFormatType(SAHL.Common.BusinessModel.DAO.ReportFormatType_DAO ReportFormatType)
            : base(ReportFormatType)
        {
            this._DAO = ReportFormatType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportFormatType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportFormatType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }


        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportFormatType_DAO.ReportServicesFormatType
        /// </summary>
        public string ReportServicesFormatType
        {
            get
            {
                return _DAO.ReportServicesFormatType; 
            }
            set
            {
                _DAO.ReportServicesFormatType = value;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportFormatType_DAO.FileExtension
        /// </summary>
        public string FileExtension
        {
            get
            {
                return _DAO.FileExtension; 
            }
            set
            {
                _DAO.FileExtension = value;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportFormatType_DAO.ContentType
        /// </summary>
        public string ContentType
        {
            get
            {
                return _DAO.ContentType; 
            }
            set
            {
                _DAO.ContentType = value;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportFormatType_DAO.DisplayOrder
        /// </summary>
        public int DisplayOrder
        {
            get
            {
                return _DAO.DisplayOrder; 
            }
            set
            {
                _DAO.DisplayOrder = value;
            }
        }
    }
}
