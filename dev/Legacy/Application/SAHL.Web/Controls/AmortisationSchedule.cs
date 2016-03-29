
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Security.Principal;
using System.Reflection;
using SAHL.Common.Logging;

namespace SAHL.Web.Controls
{
	public class AmortisationSchedule : Panel, INamingContainer
	{
		public string AmortisationScheduleTable { get; protected set; }
		public SAHL.Common.DataSets.LoanCalculations.AmortisationScheduleDataTable AmortisationScheduleDataTable { get; protected set; }
		private int currentProposalKey;
		private IDebtCounsellingRepository debtCounsellingRepository;
		private IDebtCounsellingRepository DebtCounsellingRepository
		{
			get
			{
				if (debtCounsellingRepository == null)
				{
					debtCounsellingRepository = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
				}
				return debtCounsellingRepository;
			}
		}
		private IControlRepository controlRepository;
		private IControlRepository ControlRepository
		{
			get
			{
				if (controlRepository == null)
				{
					controlRepository = RepositoryFactory.GetRepository<IControlRepository>();
				}
				return controlRepository;
			}
		}
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			LiteralControl placeholder = new LiteralControl(AmortisationScheduleTable);
			this.Controls.Add(placeholder);
		}

		public string Render(int proposalKey, bool renderToScreen)
		{
			this.currentProposalKey = proposalKey;
			int maximumPeriods = Convert.ToInt16(ControlRepository.GetControlByDescription("MaxPeriodForAmortisationGraph").ControlNumeric.Value);
			var amortisationSchedule = DebtCounsellingRepository.GetAmortisationScheduleForProposalByKey(Convert.ToInt32(proposalKey), maximumPeriods);
			var proposal = DebtCounsellingRepository.GetProposalByKey(Convert.ToInt32(proposalKey));
			AmortisationScheduleDataTable = amortisationSchedule;
			var htmlString = @"<table width='100%' border='1' style='font-size:smaller;' id='amortisationScheduleTable'>
								{0}
								{1}
							   </table>";
			var style = @"<style>
									#amortisationScheduleTable table{
										font-family:Verdana, Arial, Tahoma;
										font-size:small;
										border-collapse: collapse;
									}
									#amortisationScheduleTable table td,
									#amortisationScheduleTable table th{
										border: 1px solid black;
										padding:4px;
										text-align:right;
									}
									#amortisationScheduleTable table thead{
										text-align:left;
									}
								</style>";
			//template
			var heading = String.Format(@"<h{3}>Amortisation Schedule for {0}</h{3}>
										  <h{3}>Captured by: {1} on {2}</h{3}>
										  <h{3}>Loan Number: {4}</h{3}>
										  <h{3}>Reference Number: {5}</h{3}><br/>",
										  proposal.ProposalType.Key == (int)ProposalTypes.CounterProposal ? "Counter Proposal" : "Proposal",
										  proposal.ADUser.LegalEntity.GetLegalName(LegalNameFormat.FullNoSalutation),
										  proposal.CreateDate.ToString("dd/MM/yyyy HH:mm:ss"),
										  renderToScreen == true ? "5" : "3",
										  proposal.DebtCounselling.Account.Key,
										  String.IsNullOrEmpty(proposal.DebtCounselling.ReferenceNumber) ? "-" : proposal.DebtCounselling.ReferenceNumber);
			if (amortisationSchedule == null || amortisationSchedule.Rows == null || amortisationSchedule.Rows.Count == 0)
			{
				AmortisationScheduleTable = "<h5>No schedule available to display</h5>";
				return AmortisationScheduleTable;
			}
			var htmlTableHeader = GetHtmlTableHeader(amortisationSchedule);
			var htmlTableBody = GetHtmlAmmortisationScheduleTableBody(amortisationSchedule);
			var htmlToRender = heading + String.Format(htmlString, htmlTableHeader, htmlTableBody);
			AmortisationScheduleTable = renderToScreen ? htmlToRender : style + htmlToRender;
			return AmortisationScheduleTable;
		}

		private string GetHtmlAmmortisationScheduleTableBody(DataTable ammortisationTable)
		{
			string columnTemplate = "<td style='border: 1px solid black; padding:2px;' align='{1}'> {0}</td>";
			string columns = String.Empty;
			var tableBody = String.Empty;
			foreach (DataRow tableRow in ammortisationTable.Rows)
			{
				string row = "<tr>";
				foreach (DataColumn tableColumn in ammortisationTable.Columns)
				{
					var value = tableRow[tableColumn];
					var align = "right";
					double amount = 0;
					if (double.TryParse(value.ToString(), out amount))
					{
						if (value is double)
						{
							value = Math.Round(amount, 2).ToString(SAHL.Common.Constants.CurrencyFormat);
							align = "right";
						}
						else
						{
							align = "left";
						}
					}
					var column = String.Format(columnTemplate, value, align);
					columns += column;
				}
				row += columns + "</tr>";
				columns = String.Empty;
				tableBody += row;
			}
			return tableBody;
		}

		private string GetHtmlTableHeader(DataTable table)
		{
			var headerTemplate = "<thead><tr>{0}</tr></thead>";
			var columnTemplate = "<th align='centre'> {0} </th>";
			string columns = String.Empty;
			foreach (DataColumn tableColumn in table.Columns)
			{
				var column = String.Format(columnTemplate, tableColumn.ColumnName);
				columns += column;
			}
			var header = String.Format(headerTemplate, columns);
			return header;
		}

		public MemoryStream GetScheduleAsPDF(string html)
		{
			if (String.IsNullOrEmpty(html))
				return null;

			var pdfMemoryStream = new MemoryStream();

			using (Document document = new Document(PageSize.A4.Rotate()))
			{
				var pdfWriter = PdfWriter.GetInstance(document, pdfMemoryStream);
				document.Open();
				try
				{
					var stringReader = new StringReader(StripOutIDTagsFromStyleSheet(html));
					XMLWorkerHelper.GetInstance().ParseXHtml(pdfWriter, document, stringReader);
				}
				catch (Exception e)
				{
					throw;
				}
			}
			return pdfMemoryStream;
		}

		public string SaveToPDFAndReturnFilePath()
		{
			var ammortisationSchedulePDF = GetScheduleAsPDF(this.AmortisationScheduleTable);

			string pdfFilePath = String.Empty;
			try
			{
				IControl DebtCounsellingScheduleTempFoldercontrol = ControlRepository.GetControlByDescription("DebtCounsellingScheduleTempFolder");
				string temporaryPDFPath = DebtCounsellingScheduleTempFoldercontrol.ControlText;
				string temporaryFileName = Guid.NewGuid().ToString();
				pdfFilePath = string.Format("{1}{0}.pdf", temporaryFileName, temporaryPDFPath);
				SaveMemoryStreamToFile(ammortisationSchedulePDF, pdfFilePath);
			}
			catch (Exception ex)
			{
				LogPlugin.Logger.LogErrorMessageWithException(MethodBase.GetCurrentMethod().Name, ex.Message, ex);
			}
			return pdfFilePath;
		}

		private void SaveMemoryStreamToFile(MemoryStream memoryStream, string filePathToSaveTo)
		{
			using (FileStream file = new FileStream(filePathToSaveTo, FileMode.Create, System.IO.FileAccess.Write))
			{
				var bytes = memoryStream.ToArray();
				file.Write(bytes, 0, bytes.Length);
			}
			memoryStream.Close();
		}

		private string StripOutIDTagsFromStyleSheet(string ammortisationScheduleTable)
		{
			return ammortisationScheduleTable.Replace("#amortisationScheduleTable", String.Empty);
		}
	}
}