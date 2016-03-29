using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAHL.Web.Public.Models;
using SAHL.Web.Public.AttorneyService;
using System.IO;

namespace SAHL.Web.Public.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        private IAttorney attorneyService;
        public ReportController(IAttorney attorneyService)
            : base(attorneyService)
        {
            this.attorneyService = attorneyService;
        }

        /// <summary>
        /// Report/Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Report/Detail
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            return View();
        }

		public ActionResult ReportParameters(int reportKey)
		{
			// We need to make sure that only permitted reports can return a list of parameters 
			
			AttorneyService.AttorneyClient serviceClient = new AttorneyService.AttorneyClient();

			ReportParameter[] reportParams = serviceClient.GetReportParametersByStatementKey(reportKey, Username, Password);

			//Map the View Model
			ReportViewModel reportViewModel = new ReportViewModel
			{
				ReportParameters = AutoMapper.Mapper.Map<ReportParameter[], List<ReportParamsViewModel>>(reportParams),
			};
			reportViewModel.ReportName = reportKey.ToString();
			return View(reportViewModel);
		}

		

        /// <summary>
        /// Report/Detail
        /// </summary>
        /// <param name="detailReportViewModel"></param>
        /// <returns></returns>
        public ActionResult Detail(DetailReportViewModel detailReportViewModel)
        {
            return View();
        }

        /// <summary>
        /// Loan Statement
        /// </summary>
        /// <returns></returns>
        public ActionResult LoanStatement()
        {
            ViewLoanStatementViewModel reportViewModel = new ViewLoanStatementViewModel
            {
                ReportName = "Loan Statement Report",
                FromDate = DateTime.Now,
                ToDate = DateTime.Now
            };
            return View(reportViewModel);
        }

        /// <summary>
        /// Loan Statement
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult LoanStatement(ViewLoanStatementViewModel reportViewModel)
        {
            Dictionary<string, string> sqlReportParameters = new Dictionary<string, string>();
			int accountKey = Convert.ToInt32(Session[SessionConstants.AccountKey].ToString());

            reportViewModel.ReportKey = 72;
			sqlReportParameters.Add("AccountKey", accountKey.ToString());
            sqlReportParameters.Add("FromDate", reportViewModel.FromDate.ToString("yyyy/MM/dd 00:00:00"));
            sqlReportParameters.Add("ToDate", reportViewModel.ToDate.ToString("yyyy/MM/dd 23:59:59"));
            sqlReportParameters.Add("Language", "1"); //English
            sqlReportParameters.Add("TransactionType", "All"); //All Transaction Types
            sqlReportParameters.Add("Format", "3"); //Email

            byte[] renderedSQLReport = attorneyService.RenderSQLReport(reportViewModel.ReportKey, sqlReportParameters, Username, Password);
			FileContentResult fcr = new FileContentResult(renderedSQLReport, "application/pdf");
			fcr.FileDownloadName = "LoanStatement_Account" + accountKey.ToString() + ".pdf";
			return fcr;
        }

        #region BusinessEventHistory
        /// <summary>
        /// Loan Statement
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessEventHistory()
        {
            ViewBusinessEventHistoryViewModel reportViewModel = new ViewBusinessEventHistoryViewModel
            {
                ReportName = "Business Event History Report",
                FromDate = DateTime.Now,
                ToDate = DateTime.Now
            };
            return View(reportViewModel);
        }

        /// <summary>
        /// Loan Statement
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult BusinessEventHistory(ViewBusinessEventHistoryViewModel reportViewModel)
        {
            Dictionary<string, string> sqlReportParameters = new Dictionary<string, string>();
			int accountKey = Convert.ToInt32(Session[SessionConstants.AccountKey].ToString());

            reportViewModel.ReportKey = 7002;
            sqlReportParameters.Add("AccountKey", Session[SessionConstants.AccountKey].ToString());
            sqlReportParameters.Add("FromDate", reportViewModel.FromDate.ToString("yyyy/MM/dd 00:00:00"));
            sqlReportParameters.Add("ToDate", reportViewModel.ToDate.ToString("yyyy/MM/dd 23:59:59"));

            byte[] renderedSQLReport = attorneyService.RenderSQLReport(reportViewModel.ReportKey, sqlReportParameters, Username, Password);
			FileContentResult fcr = new FileContentResult(renderedSQLReport, "application/pdf");
			fcr.FileDownloadName = "BusinessEventHistory_Account" + accountKey.ToString() + ".pdf";
			return fcr;
            

        }
        #endregion

        #region ProposalHistory

        public ActionResult ProposalHistory()
        {

            int debtCounsellingByKey = Convert.ToInt32(Session[SessionConstants.DebtCounsellingKey]);
            string username = Session[SessionConstants.Username].ToString();
            string password = Session[SessionConstants.Password].ToString();
            ProposalListViewModel proposalListViewModel = new ProposalListViewModel();

            Proposal[] proposals = attorneyService.GetProposals(debtCounsellingByKey, username, password);
            foreach (Proposal proposal in proposals)
            {
                proposalListViewModel.Proposals.Add(AutoMapper.Mapper.Map<Proposal, ProposalViewModel>(proposal));
            }

            return View(proposalListViewModel);
        }

        #endregion

        #region ProposalDetails

        public ActionResult ProposalDetails(int reportKey)
        {
			int accountKey = Convert.ToInt32(Session[SessionConstants.AccountKey].ToString());
            int reportStatementKey = 7001;
            Dictionary<string, string> sqlReportParameters = new Dictionary<string, string>();
            sqlReportParameters.Add("ProposalKey", reportKey.ToString());
            byte[] renderedSQLReport = attorneyService.RenderSQLReport(reportStatementKey, sqlReportParameters, Username, Password);
            FileContentResult fcr = new FileContentResult(renderedSQLReport, "application/pdf");
			fcr.FileDownloadName = "ProposalDetails_Account" + accountKey.ToString() + ".pdf";
			return fcr;
            
            //ProposalDetailViewModel proposalDetailViewModel = new ProposalDetailViewModel
            //{
            //    Content = renderedSQLReport
            //};
            //return View(proposalDetailViewModel);
        }

        #endregion
    }
}
