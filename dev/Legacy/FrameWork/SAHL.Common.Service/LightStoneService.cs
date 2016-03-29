using System;
using System.Data;
using System.Diagnostics;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Net;
using SAHL.Common.Service.Interfaces.DataSets;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SAHL.Common.Service
{
    [FactoryType(typeof(ILightStoneService))]
    public class LightStoneService : ILightStoneService
    {
        #region Properties

        private WebServices.AVM.SAHL _avm;
        private WebProxy _proxy;
        private ICredentials _cred;
        private IMessageService _messageService;
        private string _proxyIP = Properties.Settings.Default.WebServices_Lightstone_ProxyIP;
        private int _proxyPort = Properties.Settings.Default.WebServices_Lightstone_ProxyPort;
        private string _proxyUser = Properties.Settings.Default.WebServices_Lightstone_ProxyUser;
        private string _proxyPass = Properties.Settings.Default.WebServices_Lightstone_ProxyPass;
        private string _proxyDomain = Properties.Settings.Default.WebServices_Lightstone_ProxyDomain;
        //private bool _useProxy = Properties.Settings.Default.WebServices_Lightstone_UseProxy;
        private string _userID = Properties.Settings.Default.WebServices_Lightstone_UserID;

        private string UserID
        {
            get
            {
                return _userID;
            }
        }

        private WebProxy Proxy
        {
            get
            {
                if (_proxy == null)
                {
                    _proxy = new WebProxy(_proxyIP, _proxyPort);
                    _proxy.Credentials = Credentials;
                }

                return _proxy;
            }
        }

        private ICredentials Credentials
        {
            get
            {
                if (_cred == null)
                    _cred = new NetworkCredential(_proxyUser, _proxyPass, _proxyDomain);

                return _cred;
            }
        }

        protected IMessageService MsgService
        {
            get
            {
                if (_messageService == null)
                    _messageService = ServiceFactory.GetService<IMessageService>();

                return _messageService;
            }
        }

        public WebServices.AVM.SAHL AVM
        {
            get
            {
                if (_avm == null)
                {
                    _avm = new WebServices.AVM.SAHL();
                    string url = RepositoryFactory.GetRepository<IControlRepository>().GetControlByDescription("LightstoneValuationWebServiceUrl").ControlText;
                    _avm.Url = url;
                    _avm.Credentials = Credentials;
                    bool _useProxy = Convert.ToBoolean(RepositoryFactory.GetRepository<IControlRepository>().GetControlByDescription("LightstoneValuationBypassProxy").ControlText);
                    if (_useProxy)
                        _avm.Proxy = Proxy;
                }
                return _avm;
            }
        }


        #endregion

        public DataTable ReturnProperties(int genericKey, int genericKeyTypeKey, string legalName, string idNumber, string firstName, string secondName, string surname, string suburb, string street, string streetType, string streetNo)
        {
            DataSet propertiesDs = null;

            try
            {
                propertiesDs = AVM.ReturnProperties(UserID, idNumber, legalName, firstName, secondName, surname, suburb, street, streetType, streetNo);
            }
            catch (Exception x)
            {
                SendExceptionEmail("ReturnProperties()", x.Message);
                throw (x);
            }
            finally
            {
                ParameterCollection parameters = new ParameterCollection();
                Helper.AddParameter(parameters, "User_ID", SqlDbType.VarChar, ParameterDirection.Input, UserID);
                Helper.AddParameter(parameters, "IDCK", SqlDbType.VarChar, ParameterDirection.Input, idNumber);
                Helper.AddParameter(parameters, "Legal_Name", SqlDbType.VarChar, ParameterDirection.Input, legalName);
                Helper.AddParameter(parameters, "Firstname", SqlDbType.VarChar, ParameterDirection.Input, firstName);
                Helper.AddParameter(parameters, "Secondname", SqlDbType.VarChar, ParameterDirection.Input, secondName);
                Helper.AddParameter(parameters, "surname", SqlDbType.VarChar, ParameterDirection.Input, surname);
                Helper.AddParameter(parameters, "suburb", SqlDbType.VarChar, ParameterDirection.Input, suburb);
                Helper.AddParameter(parameters, "StreetName", SqlDbType.VarChar, ParameterDirection.Input, street);
                Helper.AddParameter(parameters, "streetType", SqlDbType.VarChar, ParameterDirection.Input, streetType);
                Helper.AddParameter(parameters, "StreetNumber", SqlDbType.VarChar, ParameterDirection.Input, streetNo);

                XMLHistory.CreateXMLHistory("LightStone", "ReturnProperties", parameters, genericKey, genericKeyTypeKey, propertiesDs);
            }

            if (propertiesDs.Tables.Count > 0)
                return propertiesDs.Tables[0];

            return null;
        }

        private string ConfirmProperty(int genericKey, int genericKeyTypeKey, int propertyID)
        {
            string propertyGuid = String.Empty;

            try
            {
                propertyGuid = AVM.ConfirmProperty(UserID, propertyID);
            }
            catch (Exception x)
            {
                SendExceptionEmail("ConfirmProperty()", x.Message);
                throw (x);
            }
            finally
            {
                ParameterCollection requestParams = new ParameterCollection();
                Helper.AddParameter(requestParams, "UserID", SqlDbType.VarChar, ParameterDirection.Input, UserID);
                Helper.AddParameter(requestParams, "propertyID", SqlDbType.Int, ParameterDirection.Input, propertyID);

                ParameterCollection responseParams = null;

                if (!String.IsNullOrEmpty(propertyGuid))
                {
                    responseParams = new ParameterCollection();
                    Helper.AddParameter(responseParams, "SessionID", SqlDbType.VarChar, ParameterDirection.Input, propertyGuid);
                }

                XMLHistory.CreateXMLHistory("LightStone", "ConfirmProperty", requestParams, genericKey, genericKeyTypeKey, responseParams);
            }

            return propertyGuid;
        }

        public DataSet ReturnTransferData(int genericKey, int genericKeyTypeKey, int propertyID)
        {
            string propertyGUID = ConfirmProperty(genericKey, genericKeyTypeKey, propertyID);
            DataSet deedsDs = null;

            try
            {
                deedsDs = AVM.ReturnTransferData(propertyGUID);
            }
            catch (Exception x)
            {
                SendExceptionEmail("ReturnTransferData()", x.Message);
                throw (x);
            }
            finally
            {
                ParameterCollection parameters = new ParameterCollection();
                Helper.AddParameter(parameters, "SessionID", SqlDbType.VarChar, ParameterDirection.Input, propertyGUID);
                XMLHistory.CreateXMLHistory("LightStone", "ReturnTransferData", parameters, genericKey, genericKeyTypeKey, deedsDs);
            }

            return deedsDs;
        }

        private DataSet ReturnValuation(int genericKey, int genericKeyTypeKey, string propertyGUID, double purchasePrice, double loanAmount, bool isSwitchOrFurtherLoan)
        {
            DataSet valuationDs = null;

            try
            {
                valuationDs = AVM.ReturnValuation(propertyGUID, purchasePrice, loanAmount, isSwitchOrFurtherLoan);

            }
            catch (Exception x)
            {
                SendExceptionEmail("ReturnValuation()", x.Message);
                throw (x);
            }
            finally
            {
                ParameterCollection parameters = new ParameterCollection();
                Helper.AddParameter(parameters, "SessionID", SqlDbType.VarChar, ParameterDirection.Input, propertyGUID);
                Helper.AddParameter(parameters, "purchasePrice", SqlDbType.Float, ParameterDirection.Input, purchasePrice);
                Helper.AddParameter(parameters, "loanAmount", SqlDbType.Float, ParameterDirection.Input, loanAmount);
                Helper.AddParameter(parameters, "isSwitchOrFurtherLoan", SqlDbType.Bit, ParameterDirection.Input, isSwitchOrFurtherLoan);

                XMLHistory.CreateXMLHistory("LightStone", "ReturnValuation", parameters, genericKey, genericKeyTypeKey, valuationDs);
            }

            return valuationDs;
        }

        public DataSet ReturnValuation(int genericKey, int genericKeyTypeKey, int propertyID, double purchasePrice, double loanAmount, bool isSwitchOrFurtherLoan)
        {
            string propertyGUID = ConfirmProperty(genericKey, genericKeyTypeKey, propertyID);
            return ReturnValuation(genericKey, genericKeyTypeKey, propertyGUID, purchasePrice, loanAmount, isSwitchOrFurtherLoan);
        }

        private static void SendExceptionEmail(string method, string exception)
        {
            StackTrace st = new StackTrace(true);
            StackFrame[] frames = st.GetFrames();

            if (frames != null)
            {
                string name = frames[2].GetMethod().ReflectedType.FullName;

                ILookupRepository repo = RepositoryFactory.GetRepository<ILookupRepository>();
                string mailto = repo.Controls.ObjectDictionary["LightStoneExceptionMail"].ControlText;

                string subject = String.Format("LightStone Exception");
                string body = String.Format("{0} attempted a LightStone {1} call which threw the following exception:\n{2}", name, method, exception);

                IMessageService msgService = ServiceFactory.GetService<IMessageService>();
                msgService.SendEmailInternal(null, mailto, null, null, subject, body, false);
            }
        }

        /// <summary>
        /// Requests a physical property valuation from the EzVal system when we have a LightstonePropertyID
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="propertyDetails"></param>
        /// <returns>
        /// The method results in a dataset being passed back with a single table [results]:
        /// The following fields exist in the table
        /// Id              Type
        /// UniqueID        System.String (the XMLHistoryKey sent in the request)
        /// Successful      System.String (either "True" of "False")
        /// ErrorMessage    System.String
        /// </returns>
        public DataSet RequestValuationForLightstoneValidatedProperty(int genericKey, int genericKeyTypeKey, LightstoneValidatedProperty.PropertyDetailsDataTable propertyDetails)
        {
            if (genericKey < 1)
                throw new ArgumentException("Invalid GenericKey");

            Globals.GenericKeyTypes gkt;

            if (!Enum.TryParse<Globals.GenericKeyTypes>(genericKeyTypeKey.ToString(), out gkt))
                throw new ArgumentException("Invalid GenericKeyTypeKey");

            if (propertyDetails == null)
                throw new ArgumentNullException("propertyDetails");

            if (propertyDetails.Count == 0)
                throw new ArgumentException("There are no rows in the table");

            LightstoneValidatedProperty requestDS = null;
            DataSet responseDS = null;
            LightstoneValidatedProperty.PropertyDetailsRow row = propertyDetails[0];

            if (String.IsNullOrEmpty(row.LightstonePropertyID))
                throw new Exception("LightStonePropertyID may not be null");

            //write to xmlhistory to get the uniqueid
            row.UniqueID = XMLHistory.InsertXMLHistory("<RESERVED/>", genericKey, genericKeyTypeKey);
            requestDS = new LightstoneValidatedProperty();
            //requestDS.Tables.Add(new LightstoneValidatedProperty.PropertyDetailsDataTable());
            requestDS.PropertyDetails.ImportRow(row);

            responseDS = null;
            string url = null;

            //call the actual webservice method
            try
            {
                url = AVM.Url;
                string xml = requestDS.GetXml();
                responseDS = AVM.newPhysicalInstruction(_userID, xml);

            }
            catch (Exception x)
            {
                SendExceptionEmail("newPhysicalInstruction()", x.Message);
                throw x;
            }
            finally
            {

                string xml = CreateXmlForXMLHistory("Lightstone", url, "newPhysicalInstruction", requestDS, responseDS);
                XMLHistory.UpdateXMLHistory(row.UniqueID, xml);
            }

            return responseDS;
        }

        /// <summary>
        /// Requests a physical property valuations from the EzVal system when we do not have a LightstonePropertyID
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="propertyDetails"></param>
        /// <returns>a list of XMLHistoryKeys</returns>
        public DataSet RequestValuationForLightstoneNonValidatedProperty(int genericKey, int genericKeyTypeKey, LightstoneNonValidatedProperty.PropertyDetailsDataTable propertyDetails)
        {
            if (genericKey < 1)
                throw new ArgumentException("Invalid GenericKey");

            Globals.GenericKeyTypes gkt;

            if (!Enum.TryParse<Globals.GenericKeyTypes>(genericKeyTypeKey.ToString(), out gkt))
                throw new ArgumentException("Invalid GenericKeyTypeKey");

            if (propertyDetails == null)
                throw new ArgumentNullException("propertyDetails");

            if (propertyDetails.Count == 0)
                throw new ArgumentException("There are no rows in the table");

            LightstoneNonValidatedProperty requestDS = null;
            DataSet responseDS = null;
            LightstoneNonValidatedProperty.PropertyDetailsRow row = propertyDetails[0];

            //write to xmlhistory to get the uniqueid
            row.UniqueID = XMLHistory.InsertXMLHistory("<RESERVED/>", genericKey, genericKeyTypeKey);
            requestDS = new LightstoneNonValidatedProperty();
            //requestDS.Tables.Add(new LightstoneNonValidatedProperty.PropertyDetailsDataTable());
            requestDS.PropertyDetails.ImportRow(row);

            responseDS = null;
            string url = null;

            //call the actual webservice method
            try
            {
                url = AVM.Url;
                string xml = requestDS.GetXml();
                responseDS = AVM.newPhysicalInstruction_Unvalidated(_userID, xml);

            }
            catch (Exception x)
            {
                SendExceptionEmail("newPhysicalInstruction_Unvalidated()", x.Message);
                throw x;
            }
            finally
            {
                string xml = CreateXmlForXMLHistory("Lightstone", url, "newPhysicalInstruction_Unvalidated", requestDS, responseDS);
                XMLHistory.UpdateXMLHistory(row.UniqueID, xml);
            }

            return responseDS;
        }

        private string CreateXmlForXMLHistory(string dataProvider, string url, string method, DataSet requestDS, DataSet responseDS)
        {
            if (requestDS == null)
                requestDS = new DataSet("Request_NULL");
            //else
            //    requestDS.DataSetName = "Request";

            if (responseDS == null)
                responseDS = new DataSet("Response_FAILED");
            else
                responseDS.DataSetName = "Response";

            string title = String.Format("{0}.{1}", dataProvider, method);
            string header = String.Format("{0} url=\"{1}\"", title, url);
            string schema = requestDS.GetXmlSchema();
            schema = schema.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            string xml = String.Format("<{0}>\n<Request>\n{1}\n</Request>\n{2}\n<RequestXsd>{3}</RequestXsd>\n</{4}>", header, requestDS.GetXml(), responseDS.GetXml(), schema, title);
            return xml;
        }

        public bool GenerateXMLHistory(int GenericKey, int GenericKeyTypeKey, string url, string method, DataSet requestDS, DataSet responseDS)
        {
            string xml = CreateXmlForXMLHistory("SAHL", url, method, requestDS, responseDS);
            XMLHistory.InsertXMLHistory(xml, GenericKey, GenericKeyTypeKey);

            return true;
        }



        //<?xml version="1.0" encoding="utf-16"?>
        //<xs:schema id="LightstoneNonValidatedProperty" targetNamespace="http://tempuri.org/LightstoneNonValidatedProperty.xsd" xmlns:mstns="http://tempuri.org/LightstoneNonValidatedProperty.xsd" xmlns="http://tempuri.org/LightstoneNonValidatedProperty.xsd" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata" attributeFormDefault="qualified" elementFormDefault="qualified">
        //  <xs:element name="LightstoneNonValidatedProperty" msdata:IsDataSet="true" msdata:UseCurrentLocale="true">
        //    <xs:complexType>
        //      <xs:choice minOccurs="0" maxOccurs="unbounded">
        //        <xs:element name="PropertyDetails">
        //          <xs:complexType>
        //            <xs:sequence>
        //              <xs:element name="UniqueID" type="xs:int" default="0" />
        //              <xs:element name="SAHLUser" type="xs:string" />
        //              <xs:element name="PropertyTitleType" msdata:Caption="PropertyType" type="xs:string" />
        //              <xs:element name="ValuationRequiredDate" type="xs:string" />
        //              <xs:element name="ValuationInstructions" type="xs:string" minOccurs="0" />
        //              <xs:element name="Contact1" type="xs:string" />
        //              <xs:element name="Contact1Phone" type="xs:string" />
        //              <xs:element name="Contact1WorkPhone" type="xs:string" minOccurs="0" />
        //              <xs:element name="Contact1MobilePhone" type="xs:string" minOccurs="0" />
        //              <xs:element name="Contact2" type="xs:string" minOccurs="0" />
        //              <xs:element name="Contact2Phone" type="xs:string" minOccurs="0" />
        //              <xs:element name="UnitNumber" type="xs:string" minOccurs="0" />
        //              <xs:element name="BuildingNumber" type="xs:string" minOccurs="0" />
        //              <xs:element name="BuildingName" type="xs:string" minOccurs="0" />
        //              <xs:element name="StreetNumber" type="xs:string" minOccurs="0" />
        //              <xs:element name="StreetName" type="xs:string" minOccurs="0" />
        //              <xs:element name="Suburb" type="xs:string" minOccurs="0" />
        //              <xs:element name="City" type="xs:string" minOccurs="0" />
        //              <xs:element name="Province" type="xs:string" />
        //              <xs:element name="Country" type="xs:string" />
        //              <xs:element name="PostalCode" type="xs:string" minOccurs="0" />
        //              <xs:element name="ErfNumber" type="xs:string" minOccurs="0" />
        //              <xs:element name="ErfPortionNumber" type="xs:string" minOccurs="0" />
        //              <xs:element name="ErfMetroDescription" type="xs:string" minOccurs="0" />
        //              <xs:element name="SectionalUnitNumber" type="xs:string" minOccurs="0" />
        //              <xs:element name="SectionalSchemeName" type="xs:string" minOccurs="0" />
        //              <xs:element name="PropertyDescription1" type="xs:string" />
        //              <xs:element name="PropertyDescription2" type="xs:string" />
        //              <xs:element name="PropertyDescription3" type="xs:string" />
        //            </xs:sequence>
        //          </xs:complexType>
        //        </xs:element>
        //        <xs:element name="PropertyDetails">
        //          <xs:complexType>
        //            <xs:sequence>
        //              <xs:element name="UniqueID" type="xs:int" default="0" />
        //              <xs:element name="SAHLUser" type="xs:string" />
        //              <xs:element name="PropertyTitleType" msdata:Caption="PropertyType" type="xs:string" />
        //              <xs:element name="ValuationRequiredDate" type="xs:string" />
        //              <xs:element name="ValuationInstructions" type="xs:string" minOccurs="0" />
        //              <xs:element name="Contact1" type="xs:string" />
        //              <xs:element name="Contact1Phone" type="xs:string" />
        //              <xs:element name="Contact1WorkPhone" type="xs:string" minOccurs="0" />
        //              <xs:element name="Contact1MobilePhone" type="xs:string" minOccurs="0" />
        //              <xs:element name="Contact2" type="xs:string" minOccurs="0" />
        //              <xs:element name="Contact2Phone" type="xs:string" minOccurs="0" />
        //              <xs:element name="UnitNumber" type="xs:string" minOccurs="0" />
        //              <xs:element name="BuildingNumber" type="xs:string" minOccurs="0" />
        //              <xs:element name="BuildingName" type="xs:string" minOccurs="0" />
        //              <xs:element name="StreetNumber" type="xs:string" minOccurs="0" />
        //              <xs:element name="StreetName" type="xs:string" minOccurs="0" />
        //              <xs:element name="Suburb" type="xs:string" minOccurs="0" />
        //              <xs:element name="City" type="xs:string" minOccurs="0" />
        //              <xs:element name="Province" type="xs:string" />
        //              <xs:element name="Country" type="xs:string" />
        //              <xs:element name="PostalCode" type="xs:string" minOccurs="0" />
        //              <xs:element name="ErfNumber" type="xs:string" minOccurs="0" />
        //              <xs:element name="ErfPortionNumber" type="xs:string" minOccurs="0" />
        //              <xs:element name="ErfMetroDescription" type="xs:string" minOccurs="0" />
        //              <xs:element name="SectionalUnitNumber" type="xs:string" minOccurs="0" />
        //              <xs:element name="SectionalSchemeName" type="xs:string" minOccurs="0" />
        //              <xs:element name="PropertyDescription1" type="xs:string" />
        //              <xs:element name="PropertyDescription2" type="xs:string" />
        //              <xs:element name="PropertyDescription3" type="xs:string" />
        //            </xs:sequence>
        //          </xs:complexType>
        //        </xs:element>
        //      </xs:choice>
        //    </xs:complexType>
        //  </xs:element>
        //</xs:schema>
    }
}
