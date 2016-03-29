using System;
using System.IO;
using System.Net;

namespace SAHL.Common.Utils
{
    /// <summary>
    /// Provides static helper methods for use with the HTTP/S protocol.
    /// </summary>
    public class WebRequest
    {
        private string postData = String.Empty;
        private ICredentials proxyCredentials;
        private string requestMethod = "POST";

        public WebRequest()
        {
            if (Properties.Settings.Default.ProxyUserName.Length > 0)
                proxyCredentials = new NetworkCredential(Properties.Settings.Default.ProxyUserName, Properties.Settings.Default.ProxyPassword, Properties.Settings.Default.ProxyDomain);
        }

        /// <summary>
        /// Gets/sets post data that should be included in the the web request.  This can be left blank
        /// if nothing needs to be sent to the server.  If set, this should be cleared after each subsequent
        /// call to <see cref="GetResponse"/>.
        /// </summary>
        public string PostData
        {
            get
            {
                return postData;
            }
            set
            {
                postData = value;
            }
        }

        /// <summary>
        /// Gets/sets the credentials to be used when connecting to the remote server via a proxy.  This defaults
        /// to the values stored in the application settings ProxyUserName, ProxyPassword, ProxyDomain.  If the
        /// ProxyUserName is left blank, no proxy credentials are used.
        /// </summary>
        public ICredentials ProxyCredentials
        {
            get
            {
                return proxyCredentials;
            }
            set
            {
                proxyCredentials = value;
            }
        }

        /// <summary>
        /// When <see cref="PostData"/> is being sent, this property determines the way the information
        /// is sent to the remote server, where valid values are "POST" and "GET".  This defaults to
        /// "POST".
        /// </summary>
        public string RequestMethod
        {
            get
            {
                return requestMethod;
            }
            set
            {
                requestMethod = value;
            }
        }

        /// <summary>
        /// Performs a web request by posting data to the remote serve, and returns the response as a string.
        /// </summary>
        /// <param name="url">The URL to post to.</param>
        /// <returns>The response text as a string.</returns>
        public string GetResponse(string url)
        {
            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(url);

            if (this.ProxyCredentials != null)
                request.Proxy.Credentials = ProxyCredentials;

            if (this.PostData != null && this.PostData.Length > 0)
            {
                request.Method = this.RequestMethod;
                request.ContentLength = postData.Length;
                StreamWriter writer = new StreamWriter(request.GetRequestStream());
                writer.Write(postData);
                writer.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = reader.ReadToEnd();
            // ?? response.Close();
            reader.Close();

            return result;
            /*
            byte[] buf = new byte[8192];
            int count = 0;

             * // send the http request
            NetworkCredential cred = new NetworkCredential("username", "password", "DOMAIN");
            request.Proxy.Credentials = cred;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            // create the new text file
            FileInfo f = new FileInfo("C:\Temp\Test.txt");
            f.Delete();
            StreamWriter sw = f.CreateText();
            // read contents of stream into text file
            while ((count = resStream.Read(buf, 0, buf.Length)) > 0) {
            string s = Encoding.ASCII.GetString(buf, 0, count);
            sw.Write(s);
            }
            // close streams
            sw.Close();
            resStream.Close();
            response.Close();
            */

            /*
            string strResult = string.Empty;

            WebRequest req = WebRequest.Create("http://brain.adcheck.co.za/sahlintegrationservice/services.asmx/InsertValuation?User=SAHL ");

            ASCIIEncoding encoding = new ASCIIEncoding();

            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();

            xDoc.Load("c:\\path of xml document to be sent");

            byte[] bt = encoding.GetBytes(xDoc.OuterXml);

            req.Method = "POST";

            // req.ContentType = "text/xml";

            req.ContentLength = bt.Length;

            Stream writer = req.GetRequestStream();

            writer.Write(bt, 0, bt.Length);

            writer.Close();

            WebResponse resp = req.GetResponse();

            StreamReader reader = new StreamReader(resp.GetResponseStream());

            strResult = reader.ReadToEnd();

            reader.Close();

            Response.Write(strResult);
            */
        }
    }
}