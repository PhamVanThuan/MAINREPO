using System;
using System.Xml;

namespace EWorkConnector
{
    public enum TPResponseOp
    {
        eLoginResponse, eLoginFormResponse, eLogoutResponse, eFilterResponse,
        eListResponse, eFolderResponse, eActionResponse, eRefillResponse,
        eSubmitResponse, eCancelResponse, eAttachmentResponse, ePostAttachmentResponse,
        eDropAttachmentResponse, eRaiseFlagResponse, eErrorResponse, Unknown
    };

    /// <summary>
    /// The eResponse class represents a response from an eWork Transaction Protocol service
    /// </summary>
    public class eResponse
    {
        private string m_xmlString;
        private XmlDocument m_xml;
        private bool m_IsErrorResponse = false;
        private TPResponseOp m_responseType;

        public eResponse(string ResponseXMLString)
        {
            m_xmlString = ResponseXMLString;
            m_xml = new XmlDocument();
            m_xml.LoadXml(m_xmlString);
            m_responseType = (TPResponseOp)Enum.Parse(typeof(TPResponseOp), m_xml.DocumentElement.Name, true);
            m_IsErrorResponse = (m_responseType == TPResponseOp.eErrorResponse);
        }

        /// <summary>
        /// Get the contents of the FieldOuputList node as an xml node
        /// </summary>
        public XmlNode FieldOutputList
        {
            get { return m_xml.DocumentElement.SelectSingleNode("FieldOutputList"); }
        }

        /// <summary>
        /// Get the entire response as an xml string
        /// </summary>
        public string ResponseXmlString
        {
            get { return m_xmlString; }
        }

        /// <summary>
        /// Get the entire response as an xml document
        /// </summary>
        public XmlDocument ResponseXml
        {
            get { return m_xml; }
        }

        public TPResponseOp ResponseType
        {
            get { return m_responseType; }
        }

        /// <summary>
        /// Returns true if the an ErrorResponse was received
        /// </summary>
        public bool IsErrorResponse
        {
            get { return m_IsErrorResponse; }
        }

        public override string ToString()
        {
            return m_xmlString;
        }

        /// <summary>
        /// Gets the value of an attribute of the Document Element
        /// </summary>
        /// <param name="AttributeName"></param>
        /// <returns>The value of the specified attribute or "" if it does not exist</returns>
        public string GetAttribute(string AttributeName)
        {
            return m_xml.DocumentElement.GetAttribute(AttributeName);
        }

        /// <summary>
        /// Gets the contents of an xml element
        /// </summary>
        /// <param name="ElementName"></param>
        /// <returns>InnerXml of the specified element/node as a string or "" if it does not exist.</returns>
        public string GetElement(string ElementName)
        {
            XmlNode node = m_xml.DocumentElement.SelectSingleNode(ElementName);
            if (node != null)
                return node.InnerXml;

            return "";
        }

        /// <summary>
        /// Adds a field to the m_Field ArrayList
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        public string GetFieldOutputListItem(string FieldItemName)
        {
            XmlNode node = m_xml.DocumentElement.SelectSingleNode("FieldOutputList");
            node = node.SelectSingleNode(FieldItemName);
            if (node != null)
                return node.InnerXml;

            return "";
        }

        /// <summary>
        /// Provides public access to the "SessionID" attribute of the reply from the eWorks Server
        /// </summary>
        /// <value>Returns an XML attribute value</value>
        public string SessionID
        {
            get { return GetAttribute("SessionID"); }
        }

        /// <summary>
        /// Provides public access to the "FolderID" attribute of the reply from the eWorks Server
        /// </summary>
        /// <value>Returns an XML attribute value</value>
        public string FolderID
        {
            get { return GetAttribute("FolderID"); }
        }

        /// <summary>
        /// Provides public access to the "Action" attribute of the reply from the eWorks Server
        /// </summary>
        /// <value>string</value>
        public string Action
        {
            get { return GetAttribute("Action"); }
        }

        /// <summary>
        /// Provides public access to the "ServerData" node of the reply from the eWorks Server
        /// </summary>
        /// <value>Returns an XML string</value>
        public string ServerData
        {
            get { return GetElement("ServerData"); }
        }

        /// <summary>
        /// Provides public access to the "ServerData" node of the reply from the eWorks Server
        /// </summary>
        /// <value>Returns an XML string</value>
        public string ClientData
        {
            get { return GetElement("ClientData"); }
        }
    }
}