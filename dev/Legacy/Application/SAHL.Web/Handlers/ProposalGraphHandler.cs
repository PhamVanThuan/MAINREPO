using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Controls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using SAHL.Common.Web;
using Castle.ActiveRecord;
using System.Text;
using DevExpress.XtraCharts;
using System.Drawing.Imaging;
using System.Drawing;

namespace SAHL.Web.Handlers
{
    public class ProposalGraphHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            int proposalKey = 0;
            if (context.Request.QueryString["ProposalKey"] != null)
            {
                proposalKey = int.Parse(context.Request.QueryString["ProposalKey"]);
            }
            using (new SessionScope())
            {
                // This is windows form control used in the context of a "Helper" to generate an image
                ProposalGraphWFC pg = new ProposalGraphWFC();
                pg.Width = 800;
                pg.Height = 600;
                pg.Render(proposalKey);

                using (MemoryStream memoryImage = new MemoryStream())
                {
                    pg.ExportToImage(memoryImage, ImageFormat.Png);
                    context.Response.ContentType = "image/png";
                    context.Response.BinaryWrite(memoryImage.ToArray());
                }

            }
        }
    }
}