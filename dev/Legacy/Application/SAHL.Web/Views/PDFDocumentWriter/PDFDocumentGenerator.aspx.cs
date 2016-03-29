using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PDFDocumentWriter;
using PDFUtils.PDFWriting;
using System.IO;
using SAHL.Common.Service.Interfaces;
using System.Security.Principal;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Web.Views.PDFDocumentWriter
{
    public partial class PDFDocumentGenerator : System.Web.UI.Page
    {
        //http://localhost/HALO/Views/PDFDocumentWriter/PDFDocumentGenerator.aspx?AccountKey=1271206&ReportStatementKey=4006
        protected void Page_Load(object sender, EventArgs e)
        {
            string errorMessage = "";
            
            // get the params from the querystring
            string accountKey = Request.QueryString["AccountKey"];
            int reportStatementKey = Convert.ToInt32(Request.QueryString["ReportStatementKey"]);

            // setup parameter dictionary
            IDictionary<string, string> reportParams = new Dictionary<string, string>();
            reportParams.Add("AccountKey", accountKey);

            // generate the PDF document
            IReportRepository reportRepo = RepositoryFactory.GetRepository<IReportRepository>();
            string documentName = reportRepo.GeneratePDFReport(reportStatementKey, reportParams, out errorMessage);

            // we have to use impersonation here so that user has access to the generated PDF document
            ISecurityService securityService = ServiceFactory.GetService<ISecurityService>();
            WindowsImpersonationContext wic = securityService.BeginImpersonation();

            try
            {
                using (FileStream fs = File.Open(documentName, FileMode.Open))
                {
                    byte[] by = new byte[fs.Length];
                    fs.Read(by, 0, by.Length);
                    using (MemoryStream ms = new MemoryStream(by))
                    {
                        Response.ClearHeaders();
                        Response.ContentType = "application/pdf";
                        ms.WriteTo(Response.OutputStream);
                    }
                }
            }
            finally
            {
                securityService.EndImpersonation(wic);
            }

        }
    }
}