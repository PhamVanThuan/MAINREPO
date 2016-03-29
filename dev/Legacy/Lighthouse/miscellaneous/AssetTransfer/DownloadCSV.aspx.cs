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

public partial class DownloadCSV : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sFileNamePath = Request.QueryString["FileNamePath"];
        string sFileName = Request.QueryString["FileName"];
        string sheader = "attachment;filename=" + sFileName;

        Response.Clear();
        Response.AddHeader("Content-Disposition", sheader);
        Response.ContentType = "application/octet-stream";

        string sFileContents = ReadFile(sFileNamePath);

        Response.Write(sFileContents);
        File.Delete(sFileNamePath);
        Response.End();

        
    }

    private string ReadFile(string sFileName)
    {
        string WholeFile = "";
        try
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader(sFileName))
            {
                // Read and display lines from the file until the end of 
                // the file is reached.
                WholeFile = sr.ReadToEnd();

            }
        }

        catch (Exception e)
        {
            // Let the user know what went wrong.
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }

        return WholeFile;

    }
}
