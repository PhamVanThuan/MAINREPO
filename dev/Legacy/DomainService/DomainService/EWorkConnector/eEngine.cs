using System;
using System.IO;
using System.Net;
using System.Web;

namespace EWorkConnector
{
    internal class eWorkEngine : IeWorkTransactionProtocolEngine
    {
        private string m_serverName; //name of server on which the eWork engine resides.

        /// <summary>
        /// Class constructor
        /// </summary>
        public eWorkEngine()
        {
            m_serverName = Properties.Settings.Default.ServerName;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="ServerName">The name of the server on which the eWork engine resides. If not specified, the setting in the App.config file will be used.</param>
        public eWorkEngine(string ServerName)
        {
            if (ServerName != null && ServerName != "")
                m_serverName = ServerName;
            else
                m_serverName = Properties.Settings.Default.ServerName;
        }

        /// <summary>
        /// Public access to m_serverName
        /// </summary>
        public string ServerName
        {
            get { return m_serverName; }
            set { m_serverName = value; }
        }

        /// <summary>
        /// used by m_BuildRequest
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        private string Encode(string Encodee)
        {
            int i, t;
            string c = "";
            char b;

            for (i = 0; i < Encodee.Length; i++)
            {
                b = Encodee[i];
                if ((b >= 'A' && b <= 'Z') ||
                    (b >= 'a' && b <= 'z') ||
                    (b >= '0' && b <= '9'))
                    c += b;
                else
                {
                    t = b;
                    c += string.Format("%{0:X}", t);
                }
            }

            return c;
        }

        /// <summary>
        /// This basically builds a URL encoded string to pass to the eWorks server
        /// </summary>
        /// <param name="msg">XML string</param>
        /// <returns>URL encoded string</returns>
        private string BuildRequest(string RequestXml)
        {
            string sInitHeader = "";

            string sServerNode = string.Format(
                "<Engine Name=\"{0}\">" +
                "   <Transport Type=\"DCOM\">" +
                "      <Server>{1}</Server>" +
                "   </Transport>" +
                "</Engine>",
                    m_serverName,
                    m_serverName);

            sInitHeader = "ServerNode=" + Encode(sServerNode) + "&Request=" + Encode(RequestXml);

            return sInitHeader;
        }

        /// <summary>
        /// Private method that does the actual request to the eWorks server
        /// </summary>
        /// <param name="msg">Request xml string</param>
        /// <returns>Response xml string</returns>
        public string SendRequest(string RequestXml)
        {
            string URL = Properties.Settings.Default.TransactionProtocolEngineURL;
            string parameters = BuildRequest(RequestXml);

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = true;

            //		httpWebRequest.ContentLength = parameters.Length;
            //		httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            streamWriter.Write(parameters);
            streamWriter.Close();

            HttpWebResponse httpWebResponse = null;

            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (WebException we)
            {
                throw (we);
            }

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                String xml = HttpUtility.UrlDecode(streamReader.ReadToEnd());
                streamReader.Close();
                return xml;
            }
        }
    }
}