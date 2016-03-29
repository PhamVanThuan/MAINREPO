using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PDFDocumentWriter;
using PDFDocumentWriter.Logging;
using PDFUtils.PDFWriting;

namespace PDFDocumentWriterWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        //http://localhost:6000/Default.aspx?AccountKey=2416350&ReportStatementKey=4006
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string AccountKey = Request.QueryString["AccountKey"];
                int ReportStatementKey = Convert.ToInt32(Request.QueryString["ReportStatementKey"]);
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("AccountKey", AccountKey);
                string PurposeNumber = Request.QueryString["PurposeNumber"];
                if (!string.IsNullOrEmpty(PurposeNumber))
                {
                    Params.Add("PurposeNumber", PurposeNumber);
                }
                PDFGenerator generator = new PDFGenerator();
                PDFGenerationObject obj = null;

                obj = new PDFGenerationObject(Params, ReportStatementKey);
                string OutName = generator.GenerateDocument(obj);
                using (FileStream fs = File.Open(OutName, FileMode.Open))
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
            catch (Exception ex)
            {
                LogPlugin.LogError("Unable to generate Document {0}{1}", Environment.NewLine, ex.ToString());
            }
        }
    }
}