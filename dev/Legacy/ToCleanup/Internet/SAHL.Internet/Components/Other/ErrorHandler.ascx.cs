using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;


/* 
 * This Control is designed to handle errors for our DotNetNuke Website
 * The page containing this Control is used as the default error redirect page
 * The Page needs to be outside Dotnetnuke - i.e. a stand-alone aspx page (in this case ErrorPage.aspx)
 * For IIS. This control will handle all error formats specified in the DSRedirects XML dataset
 * if a 404 (file not found/redirect) is found (mainly from old bookmarks or search engines with old index pages
 * then this component will traverse the DSRedirects dataset (read from redirects.XML) and automatically
 * redirect to the specified file.
 * 
 * This will handle aspx errors as well simply by using the file name the web config needs to be configured
 * <customErrors mode="On" defaultRedirect="http://SAHLS33/ErrorHandler.aspx"/>
 * 
 * It will first scan for full URLS and then for filenames, from the 2 tables in the dataset. The reason for this is to 
 * handle older links, as well as newer pages in DNN
 * 
 * This will take the filename or URL and point it to the correct URL
 * 
*/

namespace SAHL.Internet.Components.Other
{

    ///<summary>
    ///</summary>
    public partial class ErrorHandler : UserControl
    {
        private readonly DataSet DSRedirects = new DataSet();
        private readonly DataSet DSErrorMessages = new DataSet();
        private readonly DataSet DS404 = new DataSet();
        private string URL = "";
        private string FULLPATH = "";
        string ErrorPath = "";
        private int errorcode;

        //private SAHLWebSession DNNWebSession = new SAHLWebSession();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Request != null)
                {
                    errorcode = Response.StatusCode;
                    string sErrorCode = URL = Request.ServerVariables["QUERY_STRING"];


                    if (sErrorCode.Length > 0)
                    {

                        try
                        {
                            // This is for Parsed Files (eg ASPX)
                            ErrorPath = sErrorCode.Split(new Char[] { '=' }, 99)[0]; //sErrorCode.Split(new Char[] { '?' }, 99)[0];
                            FULLPATH = sErrorCode.Split(new Char[] { '=' }, 99)[1];   //Request.RawUrl.ToString();

                            sErrorCode = sErrorCode.Split(new Char[] { ';', ' ' }, 99)[0];
                            if (ErrorPath == "aspxerrorpath")
                                sErrorCode = "404";
                        }
                        catch
                        {
                            // This is for non parsed pages e.g. htm or html 
                            ErrorPath = sErrorCode.Split(new Char[] { ';' }, 99)[0]; //sErrorCode.Split(new Char[] { '?' }, 99)[0];
                            FULLPATH = sErrorCode.Split(new Char[] { ';' }, 99)[1];   //Request.RawUrl.ToString();
                            //Need to split the domain path off
                            //FULLURL = Request.
                            string servername = "http://" + Server.MachineName;

                            FULLPATH = FULLPATH.Substring(servername.Length);
                            sErrorCode = sErrorCode.Split(new Char[] { ';', ' ' }, 99)[0];
                        }

                        try
                        {
                            URL = URL.Split(new Char[] { ';', ' ' }, 99)[1];
                        }
                        catch
                        {
                            // if there is an exception then the URL isnt needeed anyway....
                        }

                        DSErrorMessages.ReadXml(ConfigurationManager.AppSettings["ErrorMessages"]);
                        // This switch handles the page redirects in DSRedirects
                        if (sErrorCode == "404")
                        {
                            //Response.StatusCode = 404;
                            CheckForRedirect();
                        }



                        for (int i = 0; i < DSErrorMessages.Tables[0].Rows.Count; i++)
                            if (DSErrorMessages.Tables[0].Rows[i]["ERRORCODE"].ToString() == sErrorCode)
                            {
                                ErrorHeading.Text = DSErrorMessages.Tables[0].Rows[i]["HEAD"].ToString();
                                ErrorLabel.Text = DSErrorMessages.Tables[0].Rows[i]["MESSAGE"].ToString();
                                break;
                            }





                        //ResponseLabel.Text = Response.StatusCode.ToString();
                        //Response.End();
                    }
                }
            }

            Response.StatusCode = (int)HttpStatusCode.NotFound;
            //Response.StatusCode = 404;
            //Response.StatusCode = errorcode;
            Response.StatusDescription = "Not Found";

        }

        //protected void Page_Dispose(object sender, EventArgs e)
        //{

        //}

        private void CheckForRedirect()
        {
            // Test for filepath for redirect - so it can handle all domains pointing to the DNN server
            DSRedirects.ReadXml(ConfigurationManager.AppSettings["Redirects"]);
            for (int i = 0; i < DSRedirects.Tables["FILEPATH"].Rows.Count; i++)
                if (FULLPATH.ToUpper() == DSRedirects.Tables["FILEPATH"].Rows[i]["REQUEST"].ToString().ToUpper())
                    Response.Redirect(DSRedirects.Tables["FILEPATH"].Rows[i]["REDIRECT"].ToString());

            // Test for filename for redirect - so it can handle all domains pointing to the DNN server
            // Parse the filename out of the URL 
            string filename = System.IO.Path.GetFileName(URL);
            DSRedirects.ReadXml(ConfigurationManager.AppSettings["Redirects"]);
            for (int i = 0; i < DSRedirects.Tables["FILENAMES"].Rows.Count; i++)
                if (filename.ToUpper() == DSRedirects.Tables["FILENAMES"].Rows[i]["REQUEST"].ToString().ToUpper())
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    Response.StatusDescription = "Not Found";
                    //Response.StatusCode = 404;
                    //Response.StatusCode = errorcode;
                    Response.Redirect(DSRedirects.Tables["FILENAMES"].Rows[i]["REDIRECT"].ToString());
                }



            // at this point there is no redrect available - store in 404.XML to update the redirects.
            DS404.ReadXml(ConfigurationManager.AppSettings["RedirectsNotFound"]);
            DataRow datarow = DS404.Tables["FILEPATH"].NewRow();
            datarow["REQUEST"] = FULLPATH;
            datarow["REDIRECT"] = "";
            DS404.Tables["FILEPATH"].Rows.Add(datarow);
            DS404.WriteXml(ConfigurationManager.AppSettings["RedirectsNotFound"]);

        }




    }

}