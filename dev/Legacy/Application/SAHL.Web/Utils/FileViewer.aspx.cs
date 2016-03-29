using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Security.Principal;
using SAHL.Common.Authentication;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Utils
{
    public partial class FileViewer : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LoadFile();
        }

        private void LoadFile()
        {
            string requestFilePath = Request.QueryString["file"].ToString(); // if something was passed to the file querystring
            string requestFileExtension = Request.QueryString["extension"].ToString(); // if something was passed to the extension querystring

            if (!String.IsNullOrEmpty(requestFilePath))
            {
                ISecurityService securityService = ServiceFactory.GetService<ISecurityService>();
                WindowsImpersonationContext wic = securityService.BeginImpersonation();

                try
                {
                    FileInfo file = new FileInfo(requestFilePath); // get file object as FileInfo

                    if (file.Exists) // if the file exists on the server
                    {
                        // set the appropriate headers
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();

                        //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name.Replace(" ", "_").Trim("'!@#$%^&*()_+".ToCharArray()));
                        Response.AddHeader("Content-Disposition", "filename=" + file.Name.Replace(" ", "_").Trim("'!@#$%^&*()_+".ToCharArray()));

                        long fileSize = file.Length;
                        Response.AddHeader("Content-Length", fileSize.ToString());
                        if (!String.IsNullOrEmpty(requestFileExtension))
                            Response.ContentType = "application/" + requestFileExtension.ToString();
                        else
                            Response.ContentType = "application/octet-stream";

                        Response.WriteFile(file.FullName, 0, fileSize);
                        Response.End();
                    }
                    else // if the file doesnt exist
                    {
                        Response.Write("The requested file does not exist");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("Error opening requested file : " + ex.ToString());
                    throw;
                }
                finally
                {
                    // end impersonation 
                    securityService.EndImpersonation(wic);
                }
            }
            else
            {
                Response.Write("Please provide a file to view.");
            }
        }
    }
}
