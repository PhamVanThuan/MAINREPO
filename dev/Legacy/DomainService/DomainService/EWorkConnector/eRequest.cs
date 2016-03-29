using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace EWorkConnector
{
    public enum TPRequestOp
    {
        eLoginRequest, eLogoutRequest, eFilterRequest, eListRequest, eFolderRequest,
        eActionRequest, eRefillRequest, eSubmitRequest, eCancelRequest, eAttachmentRequest,
        ePostAttachmentRequest, eDropAttachmentRequest, eRaiseFlagRequest
    };

    /// <summary>
    /// The eRequest class represents a request to an eWork Transaction Protocol service
    /// </summary>
    public class eRequest
    {
        private TPRequestOp m_requestType;
        private Dictionary<string, string> m_Attributes = null;
        private Dictionary<string, string> m_Elements = null;
        private Dictionary<string, string> m_FieldInputList = null;
        private eResponse m_Response;

        /// <summary>
        /// Constructs the request
        /// </summary>
        public eRequest(TPRequestOp RequestType)
        {
            m_requestType = RequestType;
            m_Attributes = new Dictionary<string, string>();
            m_Elements = new Dictionary<string, string>();
            m_FieldInputList = new Dictionary<string, string>();
        }

        #region public properties

        public TPRequestOp RequestType
        {
            get { return m_requestType; }
            set { m_requestType = value; }
        }

        public Dictionary<string, string> Attributes
        {
            get { return m_Attributes; }
        }

        public Dictionary<string, string> Elements
        {
            get { return m_Elements; }
        }

        public Dictionary<string, string> FieldInputList
        {
            get { return m_FieldInputList; }
        }

        public void SetAttribute(string Name, string Value)
        {
            if (Name == null)
                return;

            if (Value == null)
                Value = "";

            if (m_Attributes.ContainsKey(Name))
                m_Attributes[Name] = Value;
            else
                m_Attributes.Add(Name, Value);
        }

        public string GetAttribute(string Name)
        {
            if (m_Attributes.ContainsKey(Name))
                return m_Attributes[Name];
            else
                return "";
        }

        public void SetElement(string Name, string Value)
        {
            if (Name == null)
                return;

            if (Value == null)
                Value = "";

            if (m_Elements.ContainsKey(Name))
                m_Elements[Name] = Value;
            else
                m_Elements.Add(Name, Value);
        }

        public string GetElement(string Name)
        {
            if (m_Elements.ContainsKey(Name))
                return m_Elements[Name];
            else
                return "";
        }

        /// <summary>
        /// Public access to m_FolderID
        /// </summary>
        /// <value>string</value>
        public string FolderID
        {
            get { return GetAttribute("FolderID"); }
            set { SetAttribute("FolderID", value); }
        }

        /// <summary>
        /// Public access to m_SessionID
        /// </summary>
        /// <value>string</value>
        public string SessionID
        {
            get { return GetAttribute("SessionID"); }
            set { SetAttribute("SessionID", value); }
        }

        /// <summary>
        /// Public access to m_Action
        /// </summary>
        /// <value>string</value>
        public string Action
        {
            get { return GetAttribute("Action"); }
            set { SetAttribute("Action", value); }
        }

        /// <summary>
        /// Can be used by client to save state info.
        /// </summary>
        public string ClientData
        {
            get { return GetElement("ClientData"); }
            set { SetElement("ClientData", value); }
        }

        /// <summary>
        /// Any server data from a previous response should be stored here.
        /// </summary>
        public string ServerData
        {
            get { return GetElement("ServerData"); }
            set { SetElement("ServerData", value); }
        }

        #endregion public properties

        /// <summary>
        /// Adds a field to the m_Field ArrayList
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        public void SetFieldInputListItem(string Name, string Value)
        {
            m_FieldInputList.Add(Name, Value);
        }

        /// <summary>
        /// Copies all FeildOutput items from a Response object into the FieldInputList for this Request object
        /// </summary>
        /// <param name="Response"></param>
        public void CopyResponseFieldOutputList(eResponse Response)
        {
            XmlNode node = Response.FieldOutputList;

            if (node != null)
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    XmlNode field = node.ChildNodes[i];
                    string fieldName = field.Attributes[0].InnerXml;
                    string fieldValue = field.InnerText;
                    m_FieldInputList.Add(fieldName, fieldValue);
                }
            }
        }

        /// <summary>
        /// Builds an XML string from the m_Fields ArrayList
        /// </summary>
        /// <returns>XML string</returns>
        public string FieldInputListXmlString
        {
            get
            {
                StringBuilder SB = new StringBuilder();
                Dictionary<string, string>.KeyCollection.Enumerator en = m_FieldInputList.Keys.GetEnumerator();

                for (int i = 0; i < m_FieldInputList.Count; i++)
                {
                    en.MoveNext();
                    SB.Append("<FieldInput Field='");
                    SB.Append(en.Current);
                    SB.Append("'>");
                    SB.AppendFormat("<![CDATA[{0}]]>", m_FieldInputList[en.Current]);
                    SB.Append("</FieldInput>");
                }

                return SB.ToString();
            }
        }

        /// <summary>
        /// Returns the eActionRequest, by assembling the request into an XML String
        /// </summary>
        /// <returns>XML string</returns>
        public string RequestXml
        {
            get
            {
                string sessionID = " SessionID=\"" + SessionID + "\"";
                string map = GetAttribute("Map");
                if (map != "")
                    map = " Map=\"" + map + "\"";
                string folderID = FolderID;
                if (folderID != "")
                    folderID = " FolderID=\"" + FolderID.PadLeft(31, '0') + "\"";
                string action = Action;
                if (action != "")
                    action = " Action=\"" + Action + "\"";
                string clientType = " ClientType=\"Win32\"";
                string clientData = ClientData;
                if (clientData != "")
                    clientData = "<ClientData>" + ClientData + "</ClientData>";
                string serverData = "<ServerData>" + ServerData + "</ServerData>";
                string fieldInputList = FieldInputListXmlString;
                if (fieldInputList != "")
                    fieldInputList = "<FieldInputList>" + fieldInputList + "</FieldInputList>";

                StringBuilder SB = new StringBuilder();

                switch (m_requestType)
                {
                    case TPRequestOp.eLoginRequest:
                        if (fieldInputList == "")
                        {
                            SB.Append(clientType);
                            SB.Append(">");
                            SB.Append(clientData);
                        }
                        else
                        {
                            SB.Append(" AuthenticationProcess=\"\"");
                            SB.Append(" CurrentSAP=\"0\"");
                            SB.Append(clientType);
                            SB.Append(">");
                            SB.Append(clientData);
                            SB.Append(serverData);
                            SB.Append(fieldInputList);
                        }
                        break;

                    case TPRequestOp.eLogoutRequest:
                        SB.Append(sessionID);
                        SB.Append(">");
                        SB.Append("<ClientData/><ServerData/>");
                        break;

                    case TPRequestOp.eActionRequest:
                        SB.Append(sessionID);
                        SB.Append(folderID);
                        SB.Append(map);
                        SB.Append(action);
                        SB.Append(">");
                        SB.Append(clientData);
                        SB.Append(fieldInputList);
                        break;

                    case TPRequestOp.eSubmitRequest:
                        SB.Append(sessionID);
                        SB.Append(folderID);
                        SB.Append(map);
                        SB.Append(action);
                        SB.Append(">");
                        SB.Append(clientData);
                        SB.Append(serverData);
                        SB.Append(fieldInputList);
                        break;

                    case TPRequestOp.eCancelRequest:
                        SB.Append(sessionID);
                        SB.Append(folderID);
                        SB.Append(action);
                        SB.Append(">");
                        SB.Append(clientData);
                        break;

                    default:
                        return "";
                }

                SB.Insert(0, "<" + m_requestType.ToString());
                SB.Append("</" + m_requestType.ToString() + ">");
                return SB.ToString();
            }
        }

        /// <summary>
        /// When a request is sent, the response is stored here.
        /// </summary>
        public eResponse Response
        {
            get { return m_Response; }
            set { m_Response = value; }
        }

        /// <summary>
        /// Destructs the Field list
        /// </summary>
        ~eRequest()
        {
            m_Attributes = null;
            m_Elements = null;
            m_FieldInputList = null;
        }
    }
}