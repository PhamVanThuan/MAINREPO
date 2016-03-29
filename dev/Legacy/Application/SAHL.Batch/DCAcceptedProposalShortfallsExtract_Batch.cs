using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.DataSets;
using SAHL.Common.Factories;
using SAHL.Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace SAHL.Batch
{
    public class DCAcceptedProposalShortfallsExtract_Batch : BatchBase
    {
        CastleTransactionsService castleTransService;
        IDebtCounsellingRepository dcRepo;
        IControlRepository ctrlRepo;

        public DCAcceptedProposalShortfallsExtract_Batch()
            : base()
        {
            castleTransService = new CastleTransactionsService();
            dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
            ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
        }

        protected override void RunBatch()
        {
            try
            {
                int maxPeriods = Convert.ToInt16(ctrlRepo.GetControlByDescription("MaxPeriodForAmortisationGraph").ControlNumeric.Value);

                var query = UIStatementRepository.GetStatement("ReportStatement", "ActiveAcceptedProposalsWithShortfalls_BaseData");
                DataSet ds = castleTransService.ExecuteQueryOnCastleTran(query, Common.Globals.Databases.TwoAM, null);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    using (new Castle.ActiveRecord.SessionScope())
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            int proposalKey = (int)row["ProposalKey"];

                            LoanCalculations.AmortisationScheduleDataTable amSchedule = dcRepo.GetAmortisationScheduleForProposalByKey(proposalKey, maxPeriods);

                            if (amSchedule != null)
                            {
                                LoanCalculations.AmortisationScheduleRow lastRowOfAmShedule = amSchedule.Rows[amSchedule.Rows.Count - 1] as LoanCalculations.AmortisationScheduleRow;
                                if (lastRowOfAmShedule.Closing > 0)
                                {
                                    row["Shortfall"] = lastRowOfAmShedule.Closing;
                                }
                            }
                        }
                    }
                }

                PersistTable(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException("DCAcceptedProposalShortfallsExtract_Batch.RunBatch", "DC Accepted Proposal Shortfalls Extract", ex);
                ExceptionPolicy.HandleException(ex, "UI Exception");
            }

        }

        private string ConvertDataTableToXml(DataTable datatable, string TableName)
        {
            // http://www.codeproject.com/Articles/30722/How-to-save-data-from-a-DataSet-into-a-T-SQL-table
            // http://pranayamr.blogspot.com/2011/12/bulk-insertion-of-data-using-c.html

            DataSet ds = new DataSet();
            StringBuilder sqlBuilder = new StringBuilder();
            ds.Merge(datatable, true, MissingSchemaAction.AddWithKey);
            ds.Tables[0].TableName = TableName;
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                col.ColumnMapping = MappingType.Attribute;
            }
            ds.WriteXml(new StringWriter(sqlBuilder), XmlWriteMode.WriteSchema);
            return sqlBuilder.ToString();
        }

        private void PersistTable(DataTable datatable)
        {
            var castleTransService = new CastleTransactionsService();

            string xml = ConvertDataTableToXml(datatable, "ReportData");

            var query = UIStatementRepository.GetStatement("ReportStatement", "ActiveAcceptedProposalsWithShortfalls_InsertData");

            var xmlParameter = new SqlParameter("XMLDoc", SqlDbType.Xml);
            xmlParameter.Value = xml;

            var parameters = new ParameterCollection();
            parameters.Add(xmlParameter);

            using (new Castle.ActiveRecord.SessionScope())
            {
                castleTransService.ExecuteNonQueryOnCastleTran(query, Common.Globals.Databases.TwoAM, parameters);
            }
        }
    }
}
