using System;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO
    /// </summary>
    public partial interface IReportParameter
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.ParameterName
        /// </summary>
        System.String ParameterName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.ParameterLength
        /// </summary>
        Int32? ParameterLength
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.DisplayName
        /// </summary>
        System.String DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.Required
        /// </summary>
        Boolean? Required
        {
            get;
            set;
        }

        ///// <summary>
        ///// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.StatementName
        ///// </summary>
        //System.String StatementName
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.DomainField
        /// </summary>
        IDomainField DomainField
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.ReportParameterType
        /// </summary>
        IReportParameterType ReportParameterType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.ReportStatement
        /// </summary>
        IReportStatement ReportStatement
        {
            get;
            set;
        }
    }
}