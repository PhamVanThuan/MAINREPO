using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;

[assembly: TagPrefix("SAHL.Common.Web.UI.Controls", "SAHL")]
namespace SAHL.Common.Web.UI.Controls
{
    [DefaultProperty("FilePath")]
    [ToolboxData("<{0}:SAHLPdfViewer runat=server></{0}:SAHLPdfViewer>")]
    public class SAHLPdfViewer : WebControl
    {
        private string filePath;

        [Category("Source File")]
        [Browsable(true)]
        [Description("Set path to source file.")]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    filePath = string.Empty;
                else
                {
                    int tilde = -1;
                    tilde = value.IndexOf('~');
                    if (tilde != -1)
                        filePath = value.Substring((tilde + 2)).Trim();
                    else
                        filePath = value;
                }
            }
        }  

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (String.IsNullOrEmpty(FilePath))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.Write("Error Displaying PDF Document");
                writer.RenderEndTag();
                return;
            }

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<iframe src='" + FilePath.ToString() + "' ");
                sb.Append("width=" + Width.ToString() + " height=" + Height.ToString() + " ");
                sb.Append("<View PDF: <a href='" + FilePath.ToString() + "'</a></p> ");
                sb.Append("</iframe>");

                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.Write(sb.ToString());
                writer.RenderEndTag();
            }
            catch
            {
                // with no properties set, this will render "Display PDF Control" in a
                // a box on the page
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.Write("Error Displaying PDF Document");
                writer.RenderEndTag();
            } 
        }  
    }
}
