using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAHL.Web.ClientSecure.Models;
using SAHL.Web.ClientSecure.ClientSecureService;
using System.IO;

namespace SAHL.Web.ClientSecure.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        private IClientSecure clientSecureService;
        public ReportController(IClientSecure clientSecureService)
            : base(clientSecureService)
        {
            this.clientSecureService = clientSecureService;
        }

        public ActionResult ReportParameters(int reportKey)
        {
            // We need to make sure that only permitted reports can return a list of parameters 

            ClientSecureClient serviceClient = new ClientSecureClient();

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
        public ActionResult TaxCertificate()
        {
            //setup accountkey list
            IList<Int32> accKeys = clientSecureService.MortgageLoanAccountKeys(Username, Password);
            ViewBag.AccountKey = new SelectList(accKeys, 1);

            IDictionary<int, string> reportFormats = clientSecureService.ReportFormats(Username, Password);

            TaxCertificteViewModel reportViewModel = new TaxCertificteViewModel
            {
                ReportName = "Loan Statement Report",
                ReportFormats = reportFormats,
                Year = DateTime.Now.Year
            };

            return View(reportViewModel);
        }

        /// <summary>
        /// Loan Statement
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult TaxCertificate(TaxCertificteViewModel reportViewModel)
        {
            //if (ModelState.IsValid)
            //{

            Dictionary<string, string> sqlReportParameters = new Dictionary<string, string>();

            reportViewModel.ReportKey = 1510;
            sqlReportParameters.Add("AccountKey", reportViewModel.AccountKey.ToString());
            sqlReportParameters.Add("Year", reportViewModel.Year.ToString());
            sqlReportParameters.Add("Format", "2"); //Email

            string contentType = "application/pdf";
            string fileExtension = "pdf";

            byte[] renderedSQLReport = clientSecureService.RenderSQLReport(out contentType, out fileExtension, reportViewModel.ReportKey, sqlReportParameters, reportViewModel.ReportFormatKey, Username, Password);

            renderedSQLReport = SwapSRCandALT(fileExtension, renderedSQLReport);

            FileContentResult fcr = new FileContentResult(renderedSQLReport, contentType);
            fcr.FileDownloadName = "TaxCertificate_Account" + reportViewModel.AccountKey.ToString() + "." + fileExtension;
            return fcr;
            //}
        }

        /// <summary>
        /// Loan Statement
        /// </summary>
        /// <returns></returns>
        public ActionResult LoanStatement()
        {
            //setup accountkey list
            IList<Int32> accKeys = clientSecureService.MortgageLoanAccountKeys(Username, Password);
            IDictionary<int, string> reportFormats = clientSecureService.ReportFormats(Username, Password);

            ViewBag.AccountKey = new SelectList(accKeys, 1);
            //ViewBag.ReportFormat = new SelectList(reportFormats, "Key", "Value");

            ViewLoanStatementViewModel reportViewModel = new ViewLoanStatementViewModel
            {
                ReportName = "Loan Statement Report",
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                ReportFormats = reportFormats
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

            reportViewModel.ReportKey = 74;
            sqlReportParameters.Add("AccountKey", reportViewModel.AccountKey.ToString());
            sqlReportParameters.Add("FromDate", reportViewModel.FromDate.ToString("yyyy/MM/dd 00:00:00"));
            sqlReportParameters.Add("ToDate", reportViewModel.ToDate.ToString("yyyy/MM/dd 23:59:59"));
            sqlReportParameters.Add("Language", "1"); //English
            sqlReportParameters.Add("TransactionType", "Financial"); //All Transaction Types
            sqlReportParameters.Add("Format", "3"); //Email

            string contentType = "application/pdf";
            string fileExtension = "pdf";

            byte[] renderedSQLReport = clientSecureService.RenderSQLReport(out contentType, out fileExtension, reportViewModel.ReportKey, sqlReportParameters, reportViewModel.ReportFormatKey, Username, Password);


            renderedSQLReport = SwapSRCandALT(fileExtension, renderedSQLReport);

            FileContentResult fcr = new FileContentResult(renderedSQLReport, contentType);

            fcr.FileDownloadName = "LoanStatement_Account" + reportViewModel.AccountKey.ToString() + "." + fileExtension;
            return fcr;
        }


        /// <summary>
        /// Report/Detail
        /// </summary>
        /// <param name="detailReportViewModel"></param>
        /// <returns></returns>
        public ActionResult Z299()
        {
            IDictionary<int, string> reportFormats = clientSecureService.ReportFormats(Username, Password);
            int subsidyAccountKey = Convert.ToInt32(Session[SessionConstants.SubsidyAccountKey]);
            var accList = new List<int> { subsidyAccountKey };

            ViewBag.AccountKey = new SelectList(accList, 1);

            Z299ViewModel model = new Z299ViewModel
            {
                ReportName = "Loan Statement Report",
                ReportFormats = reportFormats
            };

            return View(model);
        }

        [HttpPost]
        public FileResult Z299(Z299ViewModel reportViewModel)
        {
            var sqlReportParameters = new Dictionary<string, string>();

            reportViewModel.ReportKey = 1514;
            
            sqlReportParameters.Add("AccountKey", reportViewModel.AccountKey.ToString());
            sqlReportParameters.Add("Format", "3"); //Email
            sqlReportParameters.Add("NaedoDebitOrder", "1");

            string contentType = "application/pdf";
            string fileExtension = "pdf";

            byte[] renderedSQLReport = clientSecureService.RenderSQLReport(out contentType, out fileExtension, reportViewModel.ReportKey, sqlReportParameters, reportViewModel.ReportFormatKey, Username, Password);


            renderedSQLReport = SwapSRCandALT(fileExtension, renderedSQLReport);

            FileContentResult fcr = new FileContentResult(renderedSQLReport, contentType);

            fcr.FileDownloadName = "Z299_Account" + reportViewModel.AccountKey.ToString() + "." + fileExtension;
            return fcr;
        }

        //nasty hack to swap the alt and src tags so that the img displays for clients
        //when the SSRS generator does not embed the image. We could base encode the image and
        //embed it ourselves (which would be nice) but that would be super messy.
        private static byte[] SwapSRCandALT(string fileExtension, byte[] renderedSQLReport)
        {

            if (String.Compare(fileExtension, "html", true) == 0)
            {
                string htmlout = System.Text.Encoding.ASCII.GetString(renderedSQLReport);
                //hack the src tag out the way
                htmlout = htmlout.Replace("SRC=", "ALT1=");
                //set the alt tage to be the src
                htmlout = htmlout.Replace("ALT=", "SRC=");
                //set the hacked tag back to somthing a browser will not complain about
                htmlout = htmlout.Replace("ALT1=", "ALT=");

                renderedSQLReport = System.Text.Encoding.UTF8.GetBytes(htmlout);
            }
            return renderedSQLReport;
        }
    }
}
