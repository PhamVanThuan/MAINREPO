using SAHL.Common.BusinessModel.Authentication;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.WebServices.ReportingServices2010;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Common.BusinessModel
{
    public partial class ReportStatement : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ReportStatement_DAO>, IReportStatement, IComparable
    {
        protected void OnReportParameters_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnReportParameters_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnCorrespondenceMediums_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnCorrespondenceMediums_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnReportParameters_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnReportParameters_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnCorrespondenceMediums_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnCorrespondenceMediums_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        private List<IReportParameter> _reportParameters;

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "TODO: instead of being able to set the entire collection there should be add and remove methods...")]
        public virtual List<IReportParameter> ReportParameters
        {
            get
            {
                if (_reportParameters != null)
                    return _reportParameters;

                _reportParameters = new List<IReportParameter>();
                // first check if this report has reportparameters in the database
                int reportParameterCount = 0;
                ReportParameter_DAO[] reportParameters = ReportParameter_DAO.FindAllByProperty("ReportStatement.Key", this.Key);
                if (reportParameters != null)
                    reportParameterCount = reportParameters.Length;

                // if the report is a sql report and it doesnt have parameters in the reportparameter table then get the parms from the sql report
                if (this.ReportType != null
                    && this.ReportType.Key == (int)SAHL.Common.Globals.ReportTypes.ReportingServicesReport
                    && reportParameterCount == 0)
                {
                    ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

                    // get the sql report server url
                    string sqlReportServerUrl = ConfigurationManager.AppSettings["ReportServerURL"];
                    // build the string for the sql report webservice
                    string sqlReportWebserviceUrl = sqlReportServerUrl + "/reportservice2010.asmx";

                    //run webservice to get reportparams
                    ReportingService2010 rs = new ReportingService2010();
                    rs.Url = sqlReportWebserviceUrl;
                    ReportServerCredentials credentials = new ReportServerCredentials(ConfigurationManager.AppSettings["ReportServiceUsername"], ConfigurationManager.AppSettings["ReportServicePassword"], ConfigurationManager.AppSettings["Domain"]);
                    rs.Credentials = credentials.NetworkCredentials;

                    SAHL.Common.WebServices.ReportingServices2010.ItemParameter[] tmp = rs.GetItemParameters(this.StatementName, null, true, null, null);

                    foreach (SAHL.Common.WebServices.ReportingServices2010.ItemParameter p in tmp)
                    {
                        // if the parameter is internal, don't create it
                        SqlReportParameter srp = new SqlReportParameter(this, p);
                        if (srp != null && srp.IsInternalParameter)
                            continue;

                        _reportParameters.Add(srp);
                    }
                }
                else
                {
                    ReportParameter_DAO[] rp_dao = ReportParameter_DAO.FindAllByProperty("ReportStatement.Key", this.Key);

                    foreach (ReportParameter_DAO dao in rp_dao)
                    {
                        ReportParameter rp = new ReportParameter(dao);
                        _reportParameters.Add(rp);
                    }
                }

                return _reportParameters;
            }

            set
            {
                _reportParameters = value;
            }
        }

        public static IReadOnlyEventList<IReportStatement> GetReportStatementByReportGroupKey(IDomainMessageCollection Messages, int ReportGroupKey)
        {
            ReportGroup_DAO rg = ReportGroup_DAO.Find(ReportGroupKey);

            if (rg != null)
                return new ReadOnlyEventList<IReportStatement>(new DAOEventList<ReportStatement_DAO, IReportStatement, ReportStatement>(rg.ReportStatements));

            return null;
        }

        public DataTable ExecuteUIStatement(ParameterCollection parameters)
        {
            IDbConnection con = Helper.GetSQLDBConnection();
            DataTable DT = new DataTable();
            Helper.Fill(DT, "COMMON", this.StatementName, con, parameters);

            return DT;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            ReportStatement g = (ReportStatement)obj;
            return String.Compare(string.Concat(this.ReportName, this.Description), string.Concat(g.ReportName, g.Description));
        }

        #endregion IComparable Members

    }
}