using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PDFDocumentWriter;
using PDFDocumentWriter.DataAccess;
using PDFUtils.PDFWriting;

namespace PDFDocumentWriterWeb
{
    public partial class PDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErr0.Text = "Conn: " + DBMan.strConn;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            lblErr0.Text = "Conn: " + DBMan.strConn;
            try
            {
                lblErr.Text = "";
                int DDL = Convert.ToInt32(ddl.SelectedValue);
                int AccountKey = Convert.ToInt32(TextBox1.Text);
                int ReportStatemektKey = 0;

                switch (DDL)
                {
                    case 0:
                    default:
                        {
                            break;
                        }
                    case 1://opt in VF
                        {
                            ReportStatemektKey = 4009;
                            break;
                        }
                    case 2:// opt in Int ONly
                        {
                            ReportStatemektKey = 4008;
                            break;
                        }
                    case 3:// edge
                        {
                            ReportStatemektKey = 4004;
                            break;
                        }
                    //case 4://NVL quote
                    //    {
                    //        ReportStatemektKey = 4010;
                    //        break;
                    //    }
                    case 4:// FL NVL
                        {
                            ReportStatemektKey = 4006;
                            break;
                        }
                    case 5:// FL VF
                        {
                            ReportStatemektKey = 4007;
                            break;
                        }
                    case 6:// FL Int Only
                        {
                            ReportStatemektKey = 4005;
                            break;
                        }
                    case 7: //PL Disbursement
                        {
                            ReportStatemektKey = 7019;
                            break;
                        }
                    case 8: //PL Disbursement
                        {
                            ReportStatemektKey = 7017;
                            break;
                        }
                }
                if (0 == ReportStatemektKey)
                {
                    throw new Exception("Document Type not yet supported");
                }

                PDFGenerator generator = new PDFGenerator();
                PDFGenerationObject obj = null;
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("AccountKey", AccountKey);
                Params.Add("PurposeNumber", "3");
                obj = new PDFGenerationObject(Params, ReportStatemektKey);
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
                lblErr.Text = string.Format("unable to generate document{0}{1}", Environment.NewLine, ex.ToString());
            }
        }
    }
}