using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using SAHL.Common.Attributes;

//using AdcheckObjectLibrary;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.DataAccess;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.WebServices.AdCheck;

namespace SAHL.Common.Service
{
    [FactoryType(typeof(IAdCheckService))]
    public class AdCheckService : IAdCheckService
    {
        private string _adCheckUrl; //= Properties.Settings.Default.WebServices_AdCheck_ValuationService;
        //private string _adCheckUrl = Properties.Settings.Default.WebServices_AdCheckTest_URL;

        private string _userID; //= Properties.Settings.Default.WebServices_AdCheck_UserID; // "SAHL";
        private string _proxyIP; //= Properties.Settings.Default.WebServices_AdCheck_ProxyIP;//"192.168.11.27";
        private string _proxyPort; //= Properties.Settings.Default.WebServices_AdCheck_ProxyPort;//8080;
        private string _useProxy; //= Properties.Settings.Default.WebServices_AdCheck_UseProxy;
        private bool _useTestWebService; //= Properties.Settings.Default.WebServices_AdCheck_UseTestWebService;//true
        private string _proxyUser; //= Properties.Settings.Default.WebServices_AdCheck_ProxyUser;
        private string _proxyPass;// = Properties.Settings.Default.WebServices_AdCheck_ProxyPass;
        private string _proxyDomain; //= Properties.Settings.Default.WebServices_AdCheck_ProxyDomain;
        private string _userName; //= Properties.Settings.Default.WebServices_AdCheck_Username;
        private string _password; //= Properties.Settings.Default.WebServices_AdCheck_Password;
        //private string _testUserName; //= Properties.Settings.Default.WebServices_AdCheck_Test_Username;
        //private string _testPassword; //= Properties.Settings.Default.WebServices_AdCheck_Test_Password;
        private string _adCheckPollerconnectionString; //= Properties.Settings.Default.WebServices_AdCheckPoller_ConnectionString;

        private ValuationService _service;
        private WebProxy _proxy;
        int _genericKey;
        int _genericKeyTypeKey;

        UserCredentials _credentials;

        #region Properties

        public string AdCheckUrl
        {
            get
            {
                if (_adCheckUrl == null)
                {
                    if (UseTestWebService)
                        _adCheckUrl = "";//Properties.Settings.Default.WebServices_AdCheckTest_URL;
                    else
                        _adCheckUrl = "";// Properties.Settings.Default.WebServices_AdCheck_URL;
                }

                return _adCheckUrl;
            }
        }

        public string UserID
        {
            get
            {
                if (_userID == null)
                {
                    _userID = "";// Properties.Settings.Default.WebServices_AdCheck_UserID;
                }

                return _userID;
            }
        }

        public string ProxyIP
        {
            get
            {
                if (_proxyIP == null)
                {
                    _proxyIP = "";//Properties.Settings.Default.WebServices_AdCheck_ProxyIP;
                }

                return _proxyIP;
            }
        }

        public int ProxyPort
        {
            get
            {
                if (_proxyPort == null)
                {
                    _proxyPort = "";//Properties.Settings.Default.WebServices_AdCheck_ProxyPort;
                }

                return int.Parse(_proxyPort);
            }
        }

        public bool UseProxy
        {
            get
            {
                if (_useProxy == null)
                {
                    _useProxy = "";//SAHL.Common.Service.Properties.Settings.Default.WebServices_AdCheck_UseProxy.ToString();
                }

                return bool.Parse(_useProxy);
            }
        }

        public bool UseTestWebService
        {
            get
            {
                _useTestWebService = false;//Properties.Settings.Default.WebServices_AdCheck_UseTestWebService;

                return _useTestWebService;
            }
        }

        public string ProxyUser
        {
            get
            {
                if (_proxyUser == null)
                {
                    _proxyUser = "";//SAHL.Common.Service.Properties.Settings.Default.WebServices_AdCheck_ProxyUser;
                }

                return _proxyUser;
            }
        }

        public string ProxyPass
        {
            get
            {
                if (_proxyPass == null)
                {
                    _proxyPass = "";//SAHL.Common.Service.Properties.Settings.Default.WebServices_AdCheck_ProxyPass;
                }

                return _proxyPass;
            }
        }

        public string ProxyDomain
        {
            get
            {
                if (_proxyDomain == null)
                {
                    _proxyDomain = "";//SAHL.Common.Service.Properties.Settings.Default.WebServices_AdCheck_ProxyDomain;
                }

                return _proxyDomain;
            }
        }

        public string Username
        {
            get
            {
                if (_userName == null)
                {
                    if (UseTestWebService)
                        _userName = "";//Properties.Settings.Default.WebServices_AdCheck_Test_Username;
                    else
                        _userName = "";//Properties.Settings.Default.WebServices_AdCheck_Username;
                }

                return _userName;
            }
        }

        public string Password
        {
            get
            {
                if (_password == null)
                {
                    if (UseTestWebService)
                        _password = "";//Properties.Settings.Default.WebServices_AdCheck_Test_Password;
                    else
                        _password = "";//Properties.Settings.Default.WebServices_AdCheck_Password;
                }

                return _password;
            }
        }

        public string AdCheckPollerconnectionString
        {
            get
            {
                if (_adCheckPollerconnectionString == null)
                {
                    _adCheckPollerconnectionString = "";//Properties.Settings.Default.WebServices_AdCheckPoller_ConnectionString;
                }

                return _adCheckPollerconnectionString;
            }
        }

        public int GenericKey
        {
            get { return _genericKey; }
        }

        public int GenericKeyTypeKey
        {
            get { return _genericKeyTypeKey; }
        }

        #endregion Properties

        //public AdCheckService()
        //{
        //}

        public void Initialise(int genericKey, int genericKeyTypeKey)
        {
            _service = new ValuationService();
            _credentials = new UserCredentials();
            _credentials.strUserName = Username;
            _credentials.strUserPassword = Password;
            _service.UserCredentialsValue = _credentials;
            _service.Url = AdCheckUrl;

            _proxy = new WebProxy(ProxyIP, ProxyPort);
            _proxy.Credentials = new NetworkCredential(ProxyUser, ProxyPass, ProxyDomain);

            if (UseProxy)
                _service.Proxy = _proxy;

            _genericKey = genericKey;
            _genericKeyTypeKey = genericKeyTypeKey;
        }

        public DataSet ValidateAddress(int ReferenceNumber, string SellerID, string UnitNo, string ComplexName, string StreetNo, string StreetName, string Suburb, string Town, string Province, string ErfNumber, string ErfPortionNumber)
        {
            if (_service == null)
                throw new Exception("Please call Initialise() before calling any methods");

            Val_Address address = new Val_Address();
            address.ComplexName = ComplexName;
            address.StreetNumber = StreetNo;
            address.StreetName = StreetName;
            address.SuburbName = Suburb;
            address.Town = Town;
            address.UnitNumber = UnitNo;
            address.ErfNumber = ErfNumber;
            address.Portion = ErfPortionNumber;
            address.Province = Province;
            address.UserID = UserID;
            address.ReferenceNumber = ReferenceNumber;
            address.SellerID = SellerID;

            Val_Address[] results = null;
            DataSet requestDS = null;
            DataSet responseDS = null;
            int historyKey = -1;

            try
            {
                historyKey = XMLHistory.InsertXMLHistory(requestDS, GenericKey, GenericKeyTypeKey);
                results = _service.ValidateAddressSync(address);
            }
            finally
            {
                ParameterCollection requestParams = new ParameterCollection();
                Helper.AddParameter(requestParams, "ComplexName", SqlDbType.VarChar, ParameterDirection.Input, address.ComplexName);
                Helper.AddParameter(requestParams, "Confidence", SqlDbType.Real, ParameterDirection.Input, address.Confidence);
                Helper.AddParameter(requestParams, "ErfNumber", SqlDbType.VarChar, ParameterDirection.Input, address.ErfNumber);
                Helper.AddParameter(requestParams, "Latitude", SqlDbType.Real, ParameterDirection.Input, address.Latitude);
                Helper.AddParameter(requestParams, "LegalDescription", SqlDbType.VarChar, ParameterDirection.Input, address.LegalDescription);
                Helper.AddParameter(requestParams, "Longitude", SqlDbType.Real, ParameterDirection.Input, address.Longitude);
                Helper.AddParameter(requestParams, "NADCheck", SqlDbType.VarChar, ParameterDirection.Input, address.NADCheck);
                Helper.AddParameter(requestParams, "Portion", SqlDbType.VarChar, ParameterDirection.Input, address.Portion);
                Helper.AddParameter(requestParams, "Prediction", SqlDbType.Real, ParameterDirection.Input, address.Prediction);
                Helper.AddParameter(requestParams, "PropertyID", SqlDbType.Int, ParameterDirection.Input, address.PropertyID);
                Helper.AddParameter(requestParams, "Province", SqlDbType.VarChar, ParameterDirection.Input, address.Province);
                Helper.AddParameter(requestParams, "ReferenceNumber", SqlDbType.Int, ParameterDirection.Input, address.ReferenceNumber);
                Helper.AddParameter(requestParams, "Safety", SqlDbType.Real, ParameterDirection.Input, address.Safety);
                Helper.AddParameter(requestParams, "SellerID", SqlDbType.VarChar, ParameterDirection.Input, address.SellerID);
                Helper.AddParameter(requestParams, "SellerName", SqlDbType.VarChar, ParameterDirection.Input, address.SellerName);
                Helper.AddParameter(requestParams, "StreetName", SqlDbType.VarChar, ParameterDirection.Input, address.StreetNumber);
                Helper.AddParameter(requestParams, "StreetNumber", SqlDbType.VarChar, ParameterDirection.Input, address.StreetName);
                Helper.AddParameter(requestParams, "SuburbExtension", SqlDbType.VarChar, ParameterDirection.Input, address.SuburbExtension);
                Helper.AddParameter(requestParams, "SuburbName", SqlDbType.VarChar, ParameterDirection.Input, address.SuburbName);
                Helper.AddParameter(requestParams, "Town", SqlDbType.VarChar, ParameterDirection.Input, address.Town);
                Helper.AddParameter(requestParams, "UnitNumber", SqlDbType.VarChar, ParameterDirection.Input, address.UnitNumber);
                Helper.AddParameter(requestParams, "UserID", SqlDbType.VarChar, ParameterDirection.Input, address.UserID);

                requestDS = XMLHistory.CreateRequestDataSet("AdCheck", "ValidateAddress", requestParams);

                if (results != null)
                {
                    responseDS = new DataSet();
                    DataTable responseDT = null;

                    for (int i = 0; i < results.Length; i++)
                    {
                        //SAHL.Common.WebServices.AdCheck.Val_Address temp = results[i];

                        ParameterCollection responseParams = new ParameterCollection();
                        Helper.AddParameter(responseParams, "ComplexName", SqlDbType.VarChar, ParameterDirection.Input, results[i].ComplexName);
                        Helper.AddParameter(responseParams, "Confidence", SqlDbType.Real, ParameterDirection.Input, results[i].Confidence);
                        Helper.AddParameter(responseParams, "ErfNumber", SqlDbType.VarChar, ParameterDirection.Input, results[i].ErfNumber);
                        Helper.AddParameter(responseParams, "Latitude", SqlDbType.Real, ParameterDirection.Input, results[i].Latitude);
                        Helper.AddParameter(responseParams, "LegalDescription", SqlDbType.VarChar, ParameterDirection.Input, results[i].LegalDescription);
                        Helper.AddParameter(responseParams, "Longitude", SqlDbType.Real, ParameterDirection.Input, results[i].Longitude);
                        Helper.AddParameter(responseParams, "NADCheck", SqlDbType.VarChar, ParameterDirection.Input, results[i].NADCheck);
                        Helper.AddParameter(responseParams, "Portion", SqlDbType.VarChar, ParameterDirection.Input, results[i].Portion);
                        Helper.AddParameter(responseParams, "Prediction", SqlDbType.Real, ParameterDirection.Input, results[i].Prediction);
                        Helper.AddParameter(responseParams, "PropertyID", SqlDbType.Int, ParameterDirection.Input, results[i].PropertyID);
                        Helper.AddParameter(responseParams, "Province", SqlDbType.VarChar, ParameterDirection.Input, results[i].Province);
                        Helper.AddParameter(responseParams, "ReferenceNumber", SqlDbType.Int, ParameterDirection.Input, results[i].ReferenceNumber);
                        Helper.AddParameter(responseParams, "Safety", SqlDbType.Real, ParameterDirection.Input, results[i].Safety);
                        Helper.AddParameter(responseParams, "SellerID", SqlDbType.VarChar, ParameterDirection.Input, results[i].SellerID);
                        Helper.AddParameter(responseParams, "SellerName", SqlDbType.VarChar, ParameterDirection.Input, results[i].SellerName);
                        Helper.AddParameter(responseParams, "StreetName", SqlDbType.VarChar, ParameterDirection.Input, results[i].StreetNumber);
                        Helper.AddParameter(responseParams, "StreetNumber", SqlDbType.VarChar, ParameterDirection.Input, results[i].StreetName);
                        Helper.AddParameter(responseParams, "SuburbExtension", SqlDbType.VarChar, ParameterDirection.Input, results[i].SuburbExtension);
                        Helper.AddParameter(responseParams, "SuburbName", SqlDbType.VarChar, ParameterDirection.Input, results[i].SuburbName);
                        Helper.AddParameter(responseParams, "Town", SqlDbType.VarChar, ParameterDirection.Input, results[i].Town);
                        Helper.AddParameter(responseParams, "UnitNumber", SqlDbType.VarChar, ParameterDirection.Input, results[i].UnitNumber);
                        Helper.AddParameter(responseParams, "UserID", SqlDbType.VarChar, ParameterDirection.Input, results[i].UserID);

                        if (i == 0)
                            responseDT = XMLHistory.ConvertParametersToDataTable(responseParams, String.Format("Val_Address"));
                        else
                            XMLHistory.AddParametersToDataTable(responseDT, responseParams);
                    }

                    if (responseDT != null)
                        responseDS.Tables.Add(responseDT);
                }

                string xml = XMLHistory.CreateXmlString(requestDS, responseDS, "AdCheck", "ValidateAddressSync");
                XMLHistory.UpdateXMLHistory(historyKey, xml);
            }

            return responseDS;
        }

        public int RequestValuation(
            int OfferKey,
            int ValuatorID, //companyid
            AdCheckInterfaces.val_priority Priority,
            AdCheckInterfaces.val_request_reason_type RequestReason,
            DateTime RequestedPerformDate,
            decimal PurchasePrice,
            DateTime PurchaseDate,
            decimal LoanAmount,
            decimal Balance,
            string Contact1Name,
            string Contact1Home,
            string Contact1Work,
            string Contact1Cell,
            string Contact2Name,
            string Contact2Phone,
            string Instructions,
            int SuburbID,
            int PropertyID,
            AdCheckInterfaces.val_area_type AreaType,
            AdCheckInterfaces.val_property_use_type PropertyUseType,
            AdCheckInterfaces.val_property_type PropertyType,
            AdCheckInterfaces.val_erf_type ErfType,
            string ContactAccessDetails,
            string OfPortion,
            string LegalDescription,
            string LandSize)
        {
            if (_service == null)
                throw new Exception("Please call Initialise() before calling any methods");

            Val_details val = CreateValuationObject();

            val.val_company_id = ValuatorID;
            val.val_priority_id = (int)Priority;
            val.val_request_reason_type_id = (int)RequestReason;
            val.requested_perform_date = RequestedPerformDate;
            val.purchase_price = PurchasePrice;
            val.purchase_date = PurchaseDate;
            val.bond_amount = LoanAmount;
            val.balance = Balance;
            val.contact1_name = Contact1Name;
            val.contact1_phone = Contact1Home;
            val.contact1_phone2 = Contact1Work;
            val.contact1_phone3 = Contact1Cell;
            val.contact2_name = Contact2Name;
            val.contact2_phone = Contact2Phone;
            val.instructions = Instructions;
            val.sub_suburb_id = SuburbID;
            val.property_id = PropertyID;
            val.val_area_type_id = (int)AreaType;
            val.val_property_use_type_id = (int)PropertyUseType;
            val.val_property_type_id = (int)PropertyType;
            val.val_erf_type_id = (int)ErfType;
            val.contact_access_details = ContactAccessDetails;
            val.legal_portion = OfPortion;
            val.legal_description = LegalDescription;
            val.legal_land_size = LandSize;

            string responseStr = null;
            DataSet requestDS = null;
            DataSet responseDS = null;
            int historyKey = -1;

            try
            {
                historyKey = XMLHistory.InsertXMLHistory(requestDS, GenericKey, GenericKeyTypeKey);
                val.alternate_valuation_id = historyKey.ToString();
                responseStr = _service.RequestValuation(val);
            }
            finally
            {
                ParameterCollection requestParams = new ParameterCollection();
                Helper.AddParameter(requestParams, "alternate_valuation_id", SqlDbType.VarChar, ParameterDirection.Input, val.alternate_valuation_id);
                Helper.AddParameter(requestParams, "AreaUsedForCommercial", SqlDbType.Float, ParameterDirection.Input, val.AreaUsedForCommercial);
                Helper.AddParameter(requestParams, "balance", SqlDbType.Decimal, ParameterDirection.Input, val.balance);
                Helper.AddParameter(requestParams, "BNPO", SqlDbType.Int, ParameterDirection.Input, val.BNPO);
                Helper.AddParameter(requestParams, "bond_amount", SqlDbType.Decimal, ParameterDirection.Input, val.bond_amount);
                Helper.AddParameter(requestParams, "builder_contract_price", SqlDbType.Decimal, ParameterDirection.Input, val.builder_contract_price);
                Helper.AddParameter(requestParams, "builder_name", SqlDbType.VarChar, ParameterDirection.Input, val.builder_name);
                Helper.AddParameter(requestParams, "complex_details", SqlDbType.VarChar, ParameterDirection.Input, val.complex_details);
                Helper.AddParameter(requestParams, "complex_number", SqlDbType.Int, ParameterDirection.Input, val.complex_number);
                Helper.AddParameter(requestParams, "conditions_comment", SqlDbType.VarChar, ParameterDirection.Input, val.conditions_comment);
                Helper.AddParameter(requestParams, "contact_access_details", SqlDbType.VarChar, ParameterDirection.Input, val.contact_access_details);
                Helper.AddParameter(requestParams, "contact1_name", SqlDbType.VarChar, ParameterDirection.Input, val.contact1_name);
                Helper.AddParameter(requestParams, "contact1_phone", SqlDbType.VarChar, ParameterDirection.Input, val.contact1_phone);
                Helper.AddParameter(requestParams, "contact1_phone2", SqlDbType.VarChar, ParameterDirection.Input, val.contact1_phone2);
                Helper.AddParameter(requestParams, "contact1_phone3", SqlDbType.VarChar, ParameterDirection.Input, val.contact1_phone3);
                Helper.AddParameter(requestParams, "contact2_name", SqlDbType.VarChar, ParameterDirection.Input, val.contact2_name);
                Helper.AddParameter(requestParams, "contact2_phone", SqlDbType.VarChar, ParameterDirection.Input, val.contact2_phone);
                Helper.AddParameter(requestParams, "date_added", SqlDbType.DateTime, ParameterDirection.Input, val.date_added);
                Helper.AddParameter(requestParams, "date_modified", SqlDbType.DateTime, ParameterDirection.Input, val.date_modified);
                Helper.AddParameter(requestParams, "date_request_completed", SqlDbType.DateTime, ParameterDirection.Input, val.date_request_completed);
                Helper.AddParameter(requestParams, "deleted", SqlDbType.Int, ParameterDirection.Input, val.deleted);
                Helper.AddParameter(requestParams, "desktop_valuation", SqlDbType.Bit, ParameterDirection.Input, val.desktop_valuation);
                Helper.AddParameter(requestParams, "door_no", SqlDbType.VarChar, ParameterDirection.Input, val.door_no);
                Helper.AddParameter(requestParams, "erf_key", SqlDbType.VarChar, ParameterDirection.Input, val.erf_key);
                Helper.AddParameter(requestParams, "FloorsInComplex", SqlDbType.Int, ParameterDirection.Input, val.FloorsInComplex);
                Helper.AddParameter(requestParams, "instructions", SqlDbType.VarChar, ParameterDirection.Input, val.instructions);
                Helper.AddParameter(requestParams, "insurance_amount", SqlDbType.Decimal, ParameterDirection.Input, val.insurance_amount);
                Helper.AddParameter(requestParams, "irc_indicator", SqlDbType.Bit, ParameterDirection.Input, val.irc_indicator);
                Helper.AddParameter(requestParams, "land_value", SqlDbType.Decimal, ParameterDirection.Input, val.land_value);
                Helper.AddParameter(requestParams, "language_afrikaans", SqlDbType.Int, ParameterDirection.Input, val.language_afrikaans);
                Helper.AddParameter(requestParams, "legal_description", SqlDbType.VarChar, ParameterDirection.Input, val.legal_description);
                Helper.AddParameter(requestParams, "legal_land_size", SqlDbType.VarChar, ParameterDirection.Input, val.legal_land_size);
                Helper.AddParameter(requestParams, "legal_portion", SqlDbType.VarChar, ParameterDirection.Input, val.legal_portion);
                Helper.AddParameter(requestParams, "legal_stand_number", SqlDbType.VarChar, ParameterDirection.Input, val.legal_stand_number);
                Helper.AddParameter(requestParams, "legal_suburb_name", SqlDbType.VarChar, ParameterDirection.Input, val.legal_suburb_name);
                Helper.AddParameter(requestParams, "legal_total_portions", SqlDbType.VarChar, ParameterDirection.Input, val.legal_total_portions);
                Helper.AddParameter(requestParams, "legal_town", SqlDbType.VarChar, ParameterDirection.Input, val.legal_town);
                Helper.AddParameter(requestParams, "loan_amount", SqlDbType.Decimal, ParameterDirection.Input, val.loan_amount);
                Helper.AddParameter(requestParams, "merged", SqlDbType.Bit, ParameterDirection.Input, val.merged);
                Helper.AddParameter(requestParams, "prev_valuation_date", SqlDbType.DateTime, ParameterDirection.Input, val.prev_valuation_date);
                Helper.AddParameter(requestParams, "property_description", SqlDbType.VarChar, ParameterDirection.Input, val.property_description);
                Helper.AddParameter(requestParams, "property_id", SqlDbType.Int, ParameterDirection.Input, val.property_id);
                Helper.AddParameter(requestParams, "purchase_date", SqlDbType.DateTime, ParameterDirection.Input, val.purchase_date);
                Helper.AddParameter(requestParams, "purchase_price", SqlDbType.Decimal, ParameterDirection.Input, val.purchase_price);
                Helper.AddParameter(requestParams, "reg_region_id", SqlDbType.Int, ParameterDirection.Input, val.reg_region_id);
                Helper.AddParameter(requestParams, "requested_perform_date", SqlDbType.DateTime, ParameterDirection.Input, val.requested_perform_date);
                Helper.AddParameter(requestParams, "section_no", SqlDbType.VarChar, ParameterDirection.Input, val.section_no);
                Helper.AddParameter(requestParams, "street_name", SqlDbType.VarChar, ParameterDirection.Input, val.street_name);
                Helper.AddParameter(requestParams, "street_number", SqlDbType.VarChar, ParameterDirection.Input, val.street_number);
                Helper.AddParameter(requestParams, "sub_suburb_id", SqlDbType.Int, ParameterDirection.Input, val.sub_suburb_id);
                Helper.AddParameter(requestParams, "suburb_name", SqlDbType.VarChar, ParameterDirection.Input, val.suburb_name);
                Helper.AddParameter(requestParams, "town_name", SqlDbType.VarChar, ParameterDirection.Input, val.town_name);
                Helper.AddParameter(requestParams, "twn_town_id", SqlDbType.Int, ParameterDirection.Input, val.twn_town_id);
                Helper.AddParameter(requestParams, "UnitsInComplex", SqlDbType.Int, ParameterDirection.Input, val.UnitsInComplex);
                Helper.AddParameter(requestParams, "usr_id", SqlDbType.Int, ParameterDirection.Input, val.usr_id);
                Helper.AddParameter(requestParams, "val_area_type_id", SqlDbType.Int, ParameterDirection.Input, val.val_area_type_id);
                Helper.AddParameter(requestParams, "val_client_id", SqlDbType.Int, ParameterDirection.Input, val.val_client_id);
                Helper.AddParameter(requestParams, "val_company_id", SqlDbType.Int, ParameterDirection.Input, val.val_company_id);
                Helper.AddParameter(requestParams, "val_erf_type_id", SqlDbType.Int, ParameterDirection.Input, val.val_erf_type_id);
                Helper.AddParameter(requestParams, "val_priority_id", SqlDbType.Int, ParameterDirection.Input, val.val_priority_id);
                Helper.AddParameter(requestParams, "val_property_type_id", SqlDbType.Int, ParameterDirection.Input, val.val_property_type_id);
                Helper.AddParameter(requestParams, "val_property_use_type_id", SqlDbType.Int, ParameterDirection.Input, val.val_property_use_type_id);
                Helper.AddParameter(requestParams, "val_request_reason_type_id", SqlDbType.Int, ParameterDirection.Input, val.val_request_reason_type_id);
                Helper.AddParameter(requestParams, "val_valuation_id", SqlDbType.Int, ParameterDirection.Input, val.val_valuation_id);
                Helper.AddParameter(requestParams, "val_valuation_state_id", SqlDbType.Int, ParameterDirection.Input, val.val_valuation_state_id);
                Helper.AddParameter(requestParams, "valuation_amount", SqlDbType.Decimal, ParameterDirection.Input, val.valuation_amount);

                requestDS = XMLHistory.CreateRequestDataSet("AdCheck", "RequestValuation", requestParams);

                if (responseStr != null)
                {
                    ParameterCollection responseParams = new ParameterCollection();
                    Helper.AddParameter(responseParams, "RequestValuationResponse", SqlDbType.VarChar, ParameterDirection.Input, responseStr);
                    responseDS = XMLHistory.CreateResponseDataSet("AdCheck", "RequestValuation", responseParams);
                }

                string xml = XMLHistory.CreateXmlString(requestDS, responseDS, "AdCheck", "RequestValuation");
                XMLHistory.UpdateXMLHistory(historyKey, xml);
                //XMLHistory.CreateXMLHistory("AdCheck", "RequestValuation", requestParams, _genericKey, _genericKeyTypeKey, newParams);
            }

            //The structure of the XML sent back to SA Home Loans is as follows:

            //<?xml version="1.0" encoding="utf-8"?>
            //<string xmlns="http://AdCheckMobile/">ReponseTextHere</string>

            //The following responses are elicited if there is a failure processing the request:
            //•	[ERR: USER] Invalid user was supplied
            //•	[ERR: XML] Suburb SuburbKey doesn't exist on eProp. Please add suburb
            //•	[Err: XML] The assessor CompanyID doesn't exist on eProp. Please add assessor
            //•	[Err: XML] The valuation ID already exists
            //•	[Err: Data] System.Exception.Message – any unhandled exceptions are returned this way.
            //•	[ERR: XML] No assessment number was supplied
            //•	[ERR: XML] The erf type erfTypeValue is invalid
            //•	[ERR: XML] No erf type was supplied
            //•	[ERR: XML] No language indicator was supplied
            //•	[ERR: XML] Request date cannot be 1900-01-01
            //•	[ERR: XML] Request completion time in incorrect format
            //•	[ERR: XML] No request completion date was supplied
            //•	[ERR: XML] Previous assessment date cannot be 1900-01-01
            //•	[ERR: XML] The assessment reason assessmentReasonValue is invalid
            //•	[ERR: XML] No assessment reason was supplied
            //•	[ERR: XML] Purchase amount must be greater than zero
            //•	[ERR: XML] No purchase amount was supplied
            //•	[ERR: XML] Purchase date cannot be 1900-01-01
            //•	[ERR: XML] No purchase date was supplied
            //•	[ERR: XML] No stand number was supplied
            //•	[ERR: XML] No legal description was supplied
            //•	[ERR: XML] No street description was supplied
            //•	[ERR: XML] No suburb number was supplied
            //•	[ERR: XML] Error reading Valuation info. Processing aborted
            //•	[ERR: XML] No assessor ID was supplied
            //•	[ERR: XML] Appointment date cannot be 1900-01-01
            //•	[ERR: XML] No appointment date was supplied
            //•	[ERR: XML] Error reading Allocation info. Processing aborted

            //The following response is elicited if the valuation was processed successfully:
            //•	Valuation was allocated successfully. eProp ref: AdcheckRequestNumber.

            //write valuationId
            IDbConnection con = Helper.GetSQLDBConnection();
            string query = String.Format("INSERT INTO [2AM].[dbo].AdCheckValuationID VALUES ({0},1,{1},null,getdate())", historyKey, OfferKey);

            try
            {
                Helper.ExecuteNonQuery(con, query);
            }
            catch (Exception ex)
            {
                string withdrawResult = WithdrawRequest(historyKey);

                if (withdrawResult.Contains("ErrorMessage"))
                    throw new Exception("Failed to Insert [2AM].[dbo].AdCheckValuationID record. Unable to withdraw AdCheck Valuation Request. Please escalate to appropriate staff member for further action. Original exception details: " + ex.Message);

                string sql = String.Format("DELETE FROM [2AM].[dbo].XMLHistory WHERE XMLHistoryKey = {0}", historyKey);
                Helper.ExecuteNonQuery(con, sql);

                throw new Exception("Failed to Insert [2AM].[dbo].AdCheckValuationID record. Request has been withdrawn. Original exception details: " + ex.Message);
            }

            if (responseStr == null || responseStr.ToLower().StartsWith("[err"))
            {
                if (responseStr == null)
                    responseStr = "[ERR: UNKNOWN] RequestValuation returned NULL";

                throw new AdCheckRequestValuationException(responseStr);
            }

            //string[] parts = responseStr.Split(':');
            //return int.Parse(parts[1].Trim());

            return historyKey;
            //string result = "";

            //if (!String.IsNullOrEmpty(responseStr))
            //{
            //    XmlDocument doc = new XmlDocument();
            //    doc.LoadXml(responseStr);
            //    XmlNode node = doc.SelectSingleNode("//string");
            //    result = node.InnerText;
            //}

            //if (result.StartsWith("["))
            //{
            //    string[] parts = result.Split(']');

            //    if (parts.Length > 1)
            //        throw new Exception(parts[1]);
            //    else
            //        throw new Exception(parts[0]);
            //}
            //else
            //{
            //    string[] parts = result.Split(':');

            //    if (parts.Length > 1)
            //        return parts[1];
            //    else
            //        return parts[0];
            //}
        }

        public string WithdrawRequest(int ReferenceNumber)
        {
            if (_service == null)
                throw new Exception("Please call Initialise() before calling any methods");

            string withdrawResult = null;
            int historyKey = -1;
            DataSet requestDS = null;
            DataSet responseDS = null;

            try
            {
                historyKey = XMLHistory.InsertXMLHistory(requestDS, GenericKey, GenericKeyTypeKey);
                withdrawResult = _service.WithdrawRequest(ReferenceNumber);
            }
            finally
            {
                ParameterCollection parameters = new ParameterCollection();
                Helper.AddParameter(parameters, "ReferenceNumber", SqlDbType.Int, ParameterDirection.Input, ReferenceNumber);
                requestDS = XMLHistory.CreateRequestDataSet("AdCheck", "WithdrawRequest", parameters);

                if (withdrawResult != null)
                {
                    parameters = new ParameterCollection();
                    Helper.AddParameter(parameters, "Withdraw_Result", SqlDbType.VarChar, ParameterDirection.Input, withdrawResult);
                    responseDS = XMLHistory.CreateResponseDataSet("AdCheck", "WithdrawRequest", parameters);
                }
                string xml = XMLHistory.CreateXmlString(requestDS, responseDS, "AdCheck", "WithdrawRequest");
                XMLHistory.UpdateXMLHistory(historyKey, xml);
            }

            return withdrawResult;
        }

        public DataTable GetCompletedValuations()
        {
            if (_service == null)
                throw new Exception("Please call Initialise() before calling any methods");

            DataSet response;

            try
            {
                response = _service.GetCompletedValuations();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //we won't write history
            //finally
            //{
            //    //XMLHistory.CreateXMLHistory("AdCheck", "GetCompletedValuations", request, GenericKey, GenericKeyTypeKey, response);
            //}

            if (response.Tables.Count > 0)
                return response.Tables[0];

            return null;
        }

        public DataSet RetrieveValuationDetails(int ValuationID)
        {
            if (_service == null)
                throw new Exception("Please call Initialise() before calling any methods");

            Val_details details = CreateValuationObject();

            ParameterCollection responseParams = new ParameterCollection();
            DataSet responseDS;

            try
            {
                responseDS = RetrieveDetailsInternal(ValuationID, details, responseParams);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return responseDS;
        }

        public DataSet RetrieveValuationCaptureDetails(int ValuationID)
        {
            if (_service == null)
                throw new Exception("Please call Initialise() before calling any methods");

            Val_capture capture = new Val_capture();

            ParameterCollection responseParams = new ParameterCollection();
            DataSet responseDS;

            try
            {
                responseDS = RetrieveCaptureDetailsInternal(ValuationID, capture, responseParams);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return responseDS;
        }

        public DataSet RetrieveAllValuationDetails(int ValuationID)
        {
            if (_service == null)
                throw new Exception("Please call Initialise() before calling any methods");

            Val_details details = CreateValuationObject();
            Val_capture capture = new Val_capture();

            ParameterCollection responseParams1 = new ParameterCollection();
            ParameterCollection responseParams2 = new ParameterCollection();
            ParameterCollection requestParams = new ParameterCollection();
            Helper.AddParameter(requestParams, "ValuationID", SqlDbType.Int, ParameterDirection.Input, ValuationID);
            //DataSet requestDS1 = XMLHistory.CreateRequestDataSet("AdCheck", "RetrieveValuationDetails", requestParams);
            //DataSet requestDS2 = XMLHistory.CreateRequestDataSet("AdCheck", "RetrieveValuationCaptureDetails", requestParams);

            DataSet responseDS1 = RetrieveDetailsInternal(ValuationID, details, responseParams1);
            DataSet responseDS2 = RetrieveCaptureDetailsInternal(ValuationID, capture, responseParams2);

            DataTable response1 = responseDS1.Tables["Response"];
            DataTable response2 = responseDS2.Tables["Response"];

            DataTable response = new DataTable("Response");

            foreach (DataColumn col in response1.Columns)
            {
                DataColumn newCol = new DataColumn(col.ColumnName, col.DataType);
                response.Columns.Add(newCol);
            }

            foreach (DataColumn col in response2.Columns)
            {
                if (!response.Columns.Contains(col.ColumnName))
                {
                    DataColumn newCol = new DataColumn(col.ColumnName, col.DataType);
                    response.Columns.Add(newCol);
                }
            }

            DataRow row = response.NewRow();

            for (int i = 0; i < response1.Columns.Count; i++)
            {
                row[response1.Columns[i].ColumnName] = response1.Rows[0][i];
            }

            for (int i = 0; i < response2.Columns.Count; i++)
            {
                row[response2.Columns[i].ColumnName] = response2.Rows[0][i];
            }

            response.Rows.Add(row);
            responseDS2.Tables.Remove("Response");
            DataSet final = new DataSet("AdCheckService.RetrieveAllValuationDetails");
            final.Tables.Add(response);

            List<DataTable> tables = new List<DataTable>();

            for (int i = responseDS2.Tables.Count - 1; i > -1; i--)
            {
                DataTable dt = responseDS2.Tables[i];
                responseDS2.Tables.RemoveAt(i);
                tables.Insert(0, dt);
            }

            final.Tables.AddRange(tables.ToArray());
            return final;
        }

        private DataSet RetrieveDetailsInternal(int ValuationID, Val_details details, ParameterCollection responseParams)
        {
            ParameterCollection requestParams = new ParameterCollection();
            Helper.AddParameter(requestParams, "ValuationID", SqlDbType.Int, ParameterDirection.Input, ValuationID);
            DataSet requestDS = XMLHistory.CreateRequestDataSet("AdCheck", "RetrieveValuationDetails", requestParams);
            DataSet responseDS = new DataSet();

            try
            {
                details = _service.RetrieveValuationDetails(ValuationID.ToString());
            }
            catch
            {
                responseDS = null;
                throw;
            }
            finally
            {
                if (responseDS != null)
                {
                    Helper.AddParameter(responseParams, "alternate_valuation_id", SqlDbType.VarChar, ParameterDirection.Input, details.alternate_valuation_id);
                    Helper.AddParameter(responseParams, "AreaUsedForCommercial", SqlDbType.Real, ParameterDirection.Input, details.AreaUsedForCommercial);
                    Helper.AddParameter(responseParams, "balance", SqlDbType.Decimal, ParameterDirection.Input, details.balance);
                    Helper.AddParameter(responseParams, "BNPO", SqlDbType.Int, ParameterDirection.Input, details.BNPO);
                    Helper.AddParameter(responseParams, "bond_amount", SqlDbType.VarChar, ParameterDirection.Input, details.bond_amount);
                    Helper.AddParameter(responseParams, "builder_contract_price", SqlDbType.Decimal, ParameterDirection.Input, details.builder_contract_price);
                    Helper.AddParameter(responseParams, "builder_name", SqlDbType.VarChar, ParameterDirection.Input, details.builder_name);
                    Helper.AddParameter(responseParams, "complex_details", SqlDbType.VarChar, ParameterDirection.Input, details.complex_details);
                    Helper.AddParameter(responseParams, "complex_number", SqlDbType.Int, ParameterDirection.Input, details.complex_number);
                    Helper.AddParameter(responseParams, "conditions_comment", SqlDbType.VarChar, ParameterDirection.Input, details.conditions_comment);
                    Helper.AddParameter(responseParams, "contact_access_details", SqlDbType.VarChar, ParameterDirection.Input, details.contact_access_details);
                    Helper.AddParameter(responseParams, "contact1_name", SqlDbType.VarChar, ParameterDirection.Input, details.contact1_name);
                    Helper.AddParameter(responseParams, "contact1_phone", SqlDbType.VarChar, ParameterDirection.Input, details.contact1_phone);
                    Helper.AddParameter(responseParams, "contact1_phone2", SqlDbType.VarChar, ParameterDirection.Input, details.contact1_phone2);
                    Helper.AddParameter(responseParams, "contact1_phone3", SqlDbType.VarChar, ParameterDirection.Input, details.contact1_phone3);
                    Helper.AddParameter(responseParams, "contact2_name", SqlDbType.VarChar, ParameterDirection.Input, details.contact2_name);
                    Helper.AddParameter(responseParams, "contact2_phone", SqlDbType.VarChar, ParameterDirection.Input, details.contact2_phone);
                    Helper.AddParameter(responseParams, "date_added", SqlDbType.DateTime, ParameterDirection.Input, details.date_added);
                    Helper.AddParameter(responseParams, "date_modified", SqlDbType.DateTime, ParameterDirection.Input, details.date_modified);
                    Helper.AddParameter(responseParams, "date_request_completed", SqlDbType.DateTime, ParameterDirection.Input, details.date_request_completed);
                    Helper.AddParameter(responseParams, "deleted", SqlDbType.Int, ParameterDirection.Input, details.deleted);
                    Helper.AddParameter(responseParams, "desktop_valuation", SqlDbType.Bit, ParameterDirection.Input, details.desktop_valuation);
                    Helper.AddParameter(responseParams, "door_no", SqlDbType.VarChar, ParameterDirection.Input, details.door_no);
                    Helper.AddParameter(responseParams, "erf_key", SqlDbType.VarChar, ParameterDirection.Input, details.erf_key);
                    Helper.AddParameter(responseParams, "FloorsInComplex", SqlDbType.Int, ParameterDirection.Input, details.FloorsInComplex);
                    Helper.AddParameter(responseParams, "instructions", SqlDbType.VarChar, ParameterDirection.Input, details.instructions);
                    Helper.AddParameter(responseParams, "insurance_amount", SqlDbType.Real, ParameterDirection.Input, details.insurance_amount);
                    Helper.AddParameter(responseParams, "irc_indicator", SqlDbType.Bit, ParameterDirection.Input, details.irc_indicator);
                    Helper.AddParameter(responseParams, "land_value", SqlDbType.Real, ParameterDirection.Input, details.land_value);
                    Helper.AddParameter(responseParams, "language_afrikaans", SqlDbType.Int, ParameterDirection.Input, details.language_afrikaans);
                    Helper.AddParameter(responseParams, "legal_description", SqlDbType.VarChar, ParameterDirection.Input, details.legal_description);
                    Helper.AddParameter(responseParams, "legal_land_size", SqlDbType.VarChar, ParameterDirection.Input, details.legal_land_size);
                    Helper.AddParameter(responseParams, "legal_portion", SqlDbType.VarChar, ParameterDirection.Input, details.legal_portion);
                    Helper.AddParameter(responseParams, "legal_stand_number", SqlDbType.VarChar, ParameterDirection.Input, details.legal_stand_number);
                    Helper.AddParameter(responseParams, "legal_suburb_name", SqlDbType.VarChar, ParameterDirection.Input, details.legal_suburb_name);
                    Helper.AddParameter(responseParams, "legal_total_portions", SqlDbType.VarChar, ParameterDirection.Input, details.legal_total_portions);
                    Helper.AddParameter(responseParams, "legal_town", SqlDbType.VarChar, ParameterDirection.Input, details.legal_town);
                    Helper.AddParameter(responseParams, "loan_amount", SqlDbType.Decimal, ParameterDirection.Input, details.loan_amount);
                    Helper.AddParameter(responseParams, "merged", SqlDbType.Bit, ParameterDirection.Input, details.merged);
                    Helper.AddParameter(responseParams, "prev_valuation_date", SqlDbType.DateTime, ParameterDirection.Input, details.prev_valuation_date);
                    Helper.AddParameter(responseParams, "property_description", SqlDbType.VarChar, ParameterDirection.Input, details.property_description);
                    Helper.AddParameter(responseParams, "purchase_date", SqlDbType.DateTime, ParameterDirection.Input, details.purchase_date);
                    Helper.AddParameter(responseParams, "purchase_price", SqlDbType.Decimal, ParameterDirection.Input, details.purchase_price);
                    Helper.AddParameter(responseParams, "reg_region_id", SqlDbType.Int, ParameterDirection.Input, details.reg_region_id);
                    Helper.AddParameter(responseParams, "requested_perform_date", SqlDbType.DateTime, ParameterDirection.Input, details.requested_perform_date);
                    Helper.AddParameter(responseParams, "section_no", SqlDbType.VarChar, ParameterDirection.Input, details.section_no);
                    Helper.AddParameter(responseParams, "street_name", SqlDbType.VarChar, ParameterDirection.Input, details.street_name);
                    Helper.AddParameter(responseParams, "street_number", SqlDbType.VarChar, ParameterDirection.Input, details.street_number);
                    Helper.AddParameter(responseParams, "sub_suburb_id", SqlDbType.Int, ParameterDirection.Input, details.sub_suburb_id);
                    Helper.AddParameter(responseParams, "suburb_name", SqlDbType.VarChar, ParameterDirection.Input, details.suburb_name);
                    Helper.AddParameter(responseParams, "town_name", SqlDbType.VarChar, ParameterDirection.Input, details.town_name);
                    Helper.AddParameter(responseParams, "twn_town_id", SqlDbType.Int, ParameterDirection.Input, details.twn_town_id);
                    Helper.AddParameter(responseParams, "Type", SqlDbType.VarChar, ParameterDirection.Input, details.Type);
                    Helper.AddParameter(responseParams, "UnitsInComplex", SqlDbType.Int, ParameterDirection.Input, details.UnitsInComplex);
                    Helper.AddParameter(responseParams, "usr_id", SqlDbType.Int, ParameterDirection.Input, details.usr_id);
                    Helper.AddParameter(responseParams, "val_area_type_id", SqlDbType.Int, ParameterDirection.Input, details.val_area_type_id);
                    Helper.AddParameter(responseParams, "val_client_id", SqlDbType.Int, ParameterDirection.Input, details.val_client_id);
                    Helper.AddParameter(responseParams, "val_company_id", SqlDbType.Int, ParameterDirection.Input, details.val_company_id);
                    Helper.AddParameter(responseParams, "val_erf_type_id", SqlDbType.Int, ParameterDirection.Input, details.val_erf_type_id);
                    Helper.AddParameter(responseParams, "val_priority_id", SqlDbType.Int, ParameterDirection.Input, details.val_priority_id);
                    Helper.AddParameter(responseParams, "val_property_type_id", SqlDbType.Int, ParameterDirection.Input, details.val_property_type_id);
                    Helper.AddParameter(responseParams, "val_property_use_type_id", SqlDbType.Int, ParameterDirection.Input, details.val_property_use_type_id);
                    Helper.AddParameter(responseParams, "val_request_reason_type_id", SqlDbType.Int, ParameterDirection.Input, details.val_request_reason_type_id);
                    Helper.AddParameter(responseParams, "val_valuation_id", SqlDbType.VarChar, ParameterDirection.Input, details.val_valuation_id);
                    Helper.AddParameter(responseParams, "val_valuation_state_id", SqlDbType.VarChar, ParameterDirection.Input, details.val_valuation_state_id);
                    Helper.AddParameter(responseParams, "valuation_amount", SqlDbType.VarChar, ParameterDirection.Input, details.valuation_amount);
                    responseDS = XMLHistory.CreateResponseDataSet("AdCheck", "RetrieveValuationDetails", responseParams);
                }

                XMLHistory.CreateXMLHistory("AdCheck", "RetrieveValuationDetails", requestDS, GenericKey, GenericKeyTypeKey, responseDS);
            }

            return responseDS;
        }

        private DataSet RetrieveCaptureDetailsInternal(int ValuationID, Val_capture capture, ParameterCollection responseParams)
        {
            ParameterCollection requestParams = new ParameterCollection();
            Helper.AddParameter(requestParams, "ValuationID", SqlDbType.Int, ParameterDirection.Input, ValuationID);
            DataSet requestDS = XMLHistory.CreateRequestDataSet("AdCheck", "RetrieveValuationCaptureDetails", requestParams);
            DataSet responseDS = new DataSet();

            try
            {
                capture = _service.RetrieveValuationCaptureDetails(ValuationID.ToString());
            }
            catch
            {
                responseDS = null;
                throw;
            }
            finally
            {
                if (responseDS != null)
                {
                    Helper.AddParameter(responseParams, "additional_comments", SqlDbType.VarChar, ParameterDirection.Input, capture.additional_comments);
                    Helper.AddParameter(responseParams, "capture_end", SqlDbType.DateTime, ParameterDirection.Input, capture.capture_end);
                    Helper.AddParameter(responseParams, "capture_start", SqlDbType.DateTime, ParameterDirection.Input, capture.capture_start);
                    Helper.AddParameter(responseParams, "completed_value", SqlDbType.Decimal, ParameterDirection.Input, capture.completed_value);
                    Helper.AddParameter(responseParams, "cost_to_complete_value", SqlDbType.Decimal, ParameterDirection.Input, capture.cost_to_complete_value);
                    Helper.AddParameter(responseParams, "current_value", SqlDbType.Decimal, ParameterDirection.Input, capture.current_value);
                    Helper.AddParameter(responseParams, "date_added", SqlDbType.DateTime, ParameterDirection.Input, capture.date_added);
                    Helper.AddParameter(responseParams, "date_modified", SqlDbType.DateTime, ParameterDirection.Input, capture.date_modified);
                    Helper.AddParameter(responseParams, "deleted", SqlDbType.Int, ParameterDirection.Input, capture.deleted);
                    Helper.AddParameter(responseParams, "FloorsInComplex", SqlDbType.Int, ParameterDirection.Input, capture.FloorsInComplex);
                    Helper.AddParameter(responseParams, "HOC_value", SqlDbType.Decimal, ParameterDirection.Input, capture.HOC_value);
                    Helper.AddParameter(responseParams, "identified", SqlDbType.Int, ParameterDirection.Input, capture.identified);
                    Helper.AddParameter(responseParams, "improvement_comment", SqlDbType.VarChar, ParameterDirection.Input, capture.improvement_comment);
                    Helper.AddParameter(responseParams, "location_comment", SqlDbType.VarChar, ParameterDirection.Input, capture.location_comment);
                    Helper.AddParameter(responseParams, "market_comment", SqlDbType.VarChar, ParameterDirection.Input, capture.market_comment);
                    Helper.AddParameter(responseParams, "property_comment", SqlDbType.VarChar, ParameterDirection.Input, capture.property_comment);
                    Helper.AddParameter(responseParams, "require_senior_approval", SqlDbType.Int, ParameterDirection.Input, capture.require_senior_approval);
                    Helper.AddParameter(responseParams, "retention_amount", SqlDbType.Decimal, ParameterDirection.Input, capture.retention_amount);
                    Helper.AddParameter(responseParams, "street_address_confirmed", SqlDbType.Int, ParameterDirection.Input, capture.street_address_confirmed);
                    Helper.AddParameter(responseParams, "UnitsInComplex", SqlDbType.Int, ParameterDirection.Input, capture.UnitsInComplex);
                    Helper.AddParameter(responseParams, "val_allocation_id", SqlDbType.Int, ParameterDirection.Input, capture.val_allocation_id);
                    Helper.AddParameter(responseParams, "val_capture_id", SqlDbType.Int, ParameterDirection.Input, capture.val_capture_id);
                    Helper.AddParameter(responseParams, "val_comment_improvements_type_id", SqlDbType.Int, ParameterDirection.Input, capture.val_comment_improvements_type_id);
                    Helper.AddParameter(responseParams, "val_comment_location_type_id", SqlDbType.Int, ParameterDirection.Input, capture.val_comment_location_type_id);
                    Helper.AddParameter(responseParams, "val_comment_market_type_id", SqlDbType.Int, ParameterDirection.Input, capture.val_comment_market_type_id);
                    Helper.AddParameter(responseParams, "val_comment_property_type_id", SqlDbType.Int, ParameterDirection.Input, capture.val_comment_property_type_id);
                    Helper.AddParameter(responseParams, "val_commercial_area", SqlDbType.Float, ParameterDirection.Input, capture.val_commercial_area);
                    Helper.AddParameter(responseParams, "val_retention_reason_type_id", SqlDbType.Int, ParameterDirection.Input, capture.val_retention_reason_type_id);
                    Helper.AddParameter(responseParams, "val_sector_type_id", SqlDbType.Int, ParameterDirection.Input, capture.val_sector_type_id);
                    Helper.AddParameter(responseParams, "val_SMP_type_id", SqlDbType.Int, ParameterDirection.Input, capture.val_SMP_type_id);
                    Helper.AddParameter(responseParams, "val_valuation_id", SqlDbType.Int, ParameterDirection.Input, capture.val_valuation_id);

                    responseDS = XMLHistory.CreateResponseDataSet("AdCheck", "RetrieveValuationCaptureDetails", responseParams);

                    //now add all the classes and collections as new tables
                    DataTable DT = new DataTable("Val_Conditions");
                    DT.Columns.Add("usr_id", typeof(int));
                    DT.Columns.Add("date_added", typeof(DateTime));
                    DT.Columns.Add("date_modified", typeof(DateTime));
                    DT.Columns.Add("deleted", typeof(int));
                    DT.Columns.Add("val_capture_conditions_id", typeof(int));
                    DT.Columns.Add("val_capture_id", typeof(int));
                    DT.Columns.Add("val_conditions_type_id", typeof(int));

                    if (capture.Val_Conditions != null)
                    {
                        for (int i = 0; i < capture.Val_Conditions.Length; i++)
                        {
                            DT.Rows.Add(new object[] {
								  capture.Val_Conditions[i].usr_id
								, capture.Val_Conditions[i].date_added
								, capture.Val_Conditions[i].date_modified
								, capture.Val_Conditions[i].deleted
								, capture.Val_Conditions[i].val_capture_conditions_id
								, capture.Val_Conditions[i].val_capture_id
								, capture.Val_Conditions[i].val_conditions_type_id });
                        }
                    }

                    responseDS.Tables.Add(DT);

                    DT = new DataTable("val_conditions_collection");
                    DT.Columns.Add("usr_id", typeof(int));
                    DT.Columns.Add("date_added", typeof(DateTime));
                    DT.Columns.Add("date_modified", typeof(DateTime));
                    DT.Columns.Add("deleted", typeof(int));
                    DT.Columns.Add("val_capture_conditions_id", typeof(int));
                    DT.Columns.Add("val_capture_id", typeof(int));
                    DT.Columns.Add("val_conditions_type_id", typeof(int));

                    if (capture.val_conditions_collection != null)
                    {
                        for (int i = 0; i < capture.val_conditions_collection.Length; i++)
                        {
                            DT.Rows.Add(new object[] {
								  capture.val_conditions_collection[i].usr_id
								, capture.val_conditions_collection[i].date_added
								, capture.val_conditions_collection[i].date_modified
								, capture.val_conditions_collection[i].deleted
								, capture.val_conditions_collection[i].val_capture_conditions_id
								, capture.val_conditions_collection[i].val_capture_id
								, capture.val_conditions_collection[i].val_conditions_type_id });
                        }
                    }

                    responseDS.Tables.Add(DT);

                    DT = new DataTable("val_condition_comments");
                    DT.Columns.Add("usr_id", typeof(int));
                    DT.Columns.Add("date_added", typeof(DateTime));
                    DT.Columns.Add("date_modified", typeof(DateTime));
                    DT.Columns.Add("deleted", typeof(int));
                    DT.Columns.Add("comment1", typeof(string));
                    DT.Columns.Add("comment2", typeof(string));
                    DT.Columns.Add("comment3", typeof(string));
                    DT.Columns.Add("comment4", typeof(string));
                    DT.Columns.Add("draws", typeof(string));
                    DT.Columns.Add("months", typeof(string));
                    DT.Columns.Add("val_capture_conditions_comment_id", typeof(int));
                    DT.Columns.Add("val_capture_id", typeof(int));

                    if (capture.val_condition_comments != null)
                    {
                        DT.Rows.Add(new object[] {
								  capture.val_condition_comments.usr_id
								, capture.val_condition_comments.date_added
								, capture.val_condition_comments.date_modified
								, capture.val_condition_comments.deleted
								, capture.val_condition_comments.comment1
								, capture.val_condition_comments.comment2
								, capture.val_condition_comments.comment3
								, capture.val_condition_comments.comment4
								, capture.val_condition_comments.draws
								, capture.val_condition_comments.months
								, capture.val_condition_comments.val_capture_conditions_comment_id
								, capture.val_condition_comments.val_capture_id });
                    }

                    responseDS.Tables.Add(DT);

                    DT = new DataTable("val_other_improvements_collection");
                    DT.Columns.Add("usr_id", typeof(int));
                    DT.Columns.Add("date_added", typeof(DateTime));
                    DT.Columns.Add("date_modified", typeof(DateTime));
                    DT.Columns.Add("deleted", typeof(int));
                    DT.Columns.Add("description", typeof(string));
                    DT.Columns.Add("rate", typeof(decimal));
                    DT.Columns.Add("square_meterage", typeof(decimal));
                    DT.Columns.Add("val_capture_improvement_item_id", typeof(int));
                    DT.Columns.Add("val_capture_improvements_id", typeof(int));
                    DT.Columns.Add("val_other_improvement_roof_type_id", typeof(int));
                    DT.Columns.Add("val_roof_type_id", typeof(int));

                    if (capture.val_other_improvements_collection != null)
                    {
                        for (int i = 0; i < capture.val_other_improvements_collection.Length; i++)
                        {
                            DT.Rows.Add(new object[] {
							  capture.val_other_improvements_collection[i].usr_id
							, capture.val_other_improvements_collection[i].date_added
							, capture.val_other_improvements_collection[i].date_modified
							, capture.val_other_improvements_collection[i].deleted
							, capture.val_other_improvements_collection[i].description
							, capture.val_other_improvements_collection[i].rate
							, capture.val_other_improvements_collection[i].square_meterage
							, capture.val_other_improvements_collection[i].val_capture_improvement_item_id
							, capture.val_other_improvements_collection[i].val_capture_improvements_id
							, capture.val_other_improvements_collection[i].val_other_improvement_roof_type_id
							, capture.val_other_improvements_collection[i].val_roof_type_id });
                        }
                    }

                    responseDS.Tables.Add(DT);

                    DT = new DataTable("val_comparable_sales_collection");
                    DT.Columns.Add("usr_id", typeof(int));
                    DT.Columns.Add("date_added", typeof(DateTime));
                    DT.Columns.Add("date_modified", typeof(DateTime));
                    DT.Columns.Add("deleted", typeof(int));
                    DT.Columns.Add("assessment_date", typeof(DateTime));
                    DT.Columns.Add("purchase_value", typeof(decimal));
                    DT.Columns.Add("stand_no", typeof(string));
                    DT.Columns.Add("sub_suburb_id", typeof(int));
                    DT.Columns.Add("suburb", typeof(string));
                    DT.Columns.Add("val_capture_comparable_sale_id", typeof(int));
                    DT.Columns.Add("val_capture_id", typeof(int));
                    DT.Columns.Add("val_comment_comparable_sale_id", typeof(int));

                    if (capture.val_comparable_sales_collection != null)
                    {
                        for (int i = 0; i < capture.val_comparable_sales_collection.Length; i++)
                        {
                            DT.Rows.Add(new object[] {
							  capture.val_comparable_sales_collection[i].usr_id
							, capture.val_comparable_sales_collection[i].date_added
							, capture.val_comparable_sales_collection[i].date_modified
							, capture.val_comparable_sales_collection[i].deleted
							, capture.val_comparable_sales_collection[i].assessment_date
							, capture.val_comparable_sales_collection[i].purchase_value
							, capture.val_comparable_sales_collection[i].stand_no
							, capture.val_comparable_sales_collection[i].sub_suburb_id
							, capture.val_comparable_sales_collection[i].suburb
							, capture.val_comparable_sales_collection[i].val_capture_comparable_sale_id
							, capture.val_comparable_sales_collection[i].val_capture_id
							, capture.val_comparable_sales_collection[i].val_comment_comparable_sale_id });
                        }
                    }

                    responseDS.Tables.Add(DT);

                    DT = new DataTable("Val_CottageBuilding");
                    DT.Columns.Add("usr_id", typeof(int));
                    DT.Columns.Add("date_added", typeof(DateTime));
                    DT.Columns.Add("date_modified", typeof(DateTime));
                    DT.Columns.Add("deleted", typeof(int));
                    DT.Columns.Add("bathroom_count", typeof(int));
                    DT.Columns.Add("bedroom_count", typeof(int));
                    DT.Columns.Add("kitchen_count", typeof(int));
                    DT.Columns.Add("livingroom_count", typeof(int));
                    DT.Columns.Add("other_count", typeof(int));
                    DT.Columns.Add("other_description", typeof(string));
                    DT.Columns.Add("val_capture_cottagebuilding_count_id", typeof(int));
                    DT.Columns.Add("val_capture_id", typeof(int));
                    DT.Columns.Add("StructureType", typeof(string));
                    DT.Columns.Add("WallType", typeof(string));

                    if (capture.Val_CottageBuilding != null)
                    {
                        DT.Rows.Add(new object[] {
							  capture.Val_CottageBuilding.usr_id
							, capture.Val_CottageBuilding.date_added
							, capture.Val_CottageBuilding.date_modified
							, capture.Val_CottageBuilding.deleted
							, capture.Val_CottageBuilding.bathroom_count
							, capture.Val_CottageBuilding.bedroom_count
							, capture.Val_CottageBuilding.kitchen_count
							, capture.Val_CottageBuilding.livingroom_count
							, capture.Val_CottageBuilding.other_count
							, capture.Val_CottageBuilding.other_description
							, capture.Val_CottageBuilding.val_capture_cottagebuilding_count_id
							, capture.Val_CottageBuilding.val_capture_id
							, capture.Val_CottageBuilding.StructureType
							, capture.Val_CottageBuilding.WallType});
                    }

                    responseDS.Tables.Add(DT);

                    DT = new DataTable("Val_Improvements");
                    DT.Columns.Add("usr_id", typeof(int));
                    DT.Columns.Add("date_added", typeof(DateTime));
                    DT.Columns.Add("date_modified", typeof(DateTime));
                    DT.Columns.Add("deleted", typeof(int));
                    DT.Columns.Add("other_improvements_value", typeof(decimal));
                    DT.Columns.Add("swimmingpool_value", typeof(decimal));
                    DT.Columns.Add("tenniscourt_value", typeof(decimal));
                    DT.Columns.Add("total_improvements_value", typeof(decimal));
                    DT.Columns.Add("val_capture_id", typeof(int));
                    DT.Columns.Add("val_capture_improvements_id", typeof(string));
                    DT.Columns.Add("val_swimmingpool_type_id", typeof(int));
                    DT.Columns.Add("val_tenniscourt_type_id", typeof(int));

                    if (capture.Val_Improvements != null)
                    {
                        DT.Rows.Add(new object[] {
							  capture.Val_Improvements.usr_id
							, capture.Val_Improvements.date_added
							, capture.Val_Improvements.date_modified
							, capture.Val_Improvements.deleted
							, capture.Val_Improvements.other_improvements_value
							, capture.Val_Improvements.swimmingpool_value
							, capture.Val_Improvements.tenniscourt_value
							, capture.Val_Improvements.total_improvements_value
							, capture.Val_Improvements.val_capture_id
							, capture.Val_Improvements.val_capture_improvements_id
							, capture.Val_Improvements.val_swimmingpool_type_id
							, capture.Val_Improvements.val_tenniscourt_type_id});
                    }

                    responseDS.Tables.Add(DT);

                    DT = new DataTable("Val_Insurance");
                    DT.Columns.Add("usr_id", typeof(int));
                    DT.Columns.Add("date_added", typeof(DateTime));
                    DT.Columns.Add("date_modified", typeof(DateTime));
                    DT.Columns.Add("deleted", typeof(int));
                    DT.Columns.Add("cottage_rate", typeof(decimal));
                    DT.Columns.Add("cottage_roof_type_id", typeof(int));
                    DT.Columns.Add("cottage_square_meterage", typeof(decimal));
                    DT.Columns.Add("main_rate", typeof(decimal));
                    DT.Columns.Add("main_roof_type_id", typeof(int));
                    DT.Columns.Add("main_square_meterage", typeof(decimal));
                    DT.Columns.Add("out_rate", typeof(decimal));
                    DT.Columns.Add("out_roof_type_id", typeof(int));
                    DT.Columns.Add("out_square_meterage", typeof(decimal));
                    DT.Columns.Add("Percentage", typeof(int));
                    DT.Columns.Add("val_capture_id", typeof(int));
                    DT.Columns.Add("val_capture_insurance_id", typeof(int));

                    if (capture.Val_Insurance != null)
                    {
                        DT.Rows.Add(new object[] {
							  capture.Val_Insurance.usr_id
							, capture.Val_Insurance.date_added
							, capture.Val_Insurance.date_modified
							, capture.Val_Insurance.deleted
							, capture.Val_Insurance.cottage_rate
							, capture.Val_Insurance.cottage_roof_type_id
							, capture.Val_Insurance.cottage_square_meterage
							, capture.Val_Insurance.main_rate
							, capture.Val_Insurance.main_roof_type_id
							, capture.Val_Insurance.main_square_meterage
							, capture.Val_Insurance.out_rate
							, capture.Val_Insurance.out_roof_type_id
							, capture.Val_Insurance.out_square_meterage
							, capture.Val_Insurance.Percentage
							, capture.Val_Insurance.val_capture_id
							, capture.Val_Insurance.val_capture_insurance_id});
                    }

                    responseDS.Tables.Add(DT);

                    DT = new DataTable("Val_MainBuilding");
                    DT.Columns.Add("usr_id", typeof(int));
                    DT.Columns.Add("date_added", typeof(DateTime));
                    DT.Columns.Add("date_modified", typeof(DateTime));
                    DT.Columns.Add("deleted", typeof(int));
                    DT.Columns.Add("bathroom_count", typeof(int));
                    DT.Columns.Add("bedroom_count", typeof(int));
                    DT.Columns.Add("diningroom_count", typeof(int));
                    DT.Columns.Add("entrance_count", typeof(int));
                    DT.Columns.Add("familyroom_count", typeof(int));
                    DT.Columns.Add("kitchen_count", typeof(int));
                    DT.Columns.Add("laundry_count", typeof(int));
                    DT.Columns.Add("lounge_count", typeof(int));
                    DT.Columns.Add("other_count", typeof(int));
                    DT.Columns.Add("other_description", typeof(string));
                    DT.Columns.Add("pantry_count", typeof(int));
                    DT.Columns.Add("study_count", typeof(int));
                    DT.Columns.Add("val_capture_id", typeof(int));
                    DT.Columns.Add("val_capture_mainbuilding_count_id", typeof(int));
                    DT.Columns.Add("wc_count", typeof(int));
                    DT.Columns.Add("StructureType", typeof(string));
                    DT.Columns.Add("WallType", typeof(string));

                    if (capture.Val_MainBuilding != null)
                    {
                        DT.Rows.Add(new object[] {
							  capture.Val_MainBuilding.usr_id
							, capture.Val_MainBuilding.date_added
							, capture.Val_MainBuilding.date_modified
							, capture.Val_MainBuilding.deleted
							, capture.Val_MainBuilding.bathroom_count
							, capture.Val_MainBuilding.bedroom_count
							, capture.Val_MainBuilding.diningroom_count
							, capture.Val_MainBuilding.entrance_count
							, capture.Val_MainBuilding.familyroom_count
							, capture.Val_MainBuilding.kitchen_count
							, capture.Val_MainBuilding.laundry_count
							, capture.Val_MainBuilding.lounge_count
							, capture.Val_MainBuilding.other_count
							, capture.Val_MainBuilding.other_description
							, capture.Val_MainBuilding.pantry_count
							, capture.Val_MainBuilding.study_count
							, capture.Val_MainBuilding.val_capture_id
							, capture.Val_MainBuilding.val_capture_mainbuilding_count_id
							, capture.Val_MainBuilding.wc_count
							, capture.Val_MainBuilding.StructureType
							, capture.Val_MainBuilding.WallType});
                    }

                    responseDS.Tables.Add(DT);

                    DT = new DataTable("Val_OutBuilding");
                    DT.Columns.Add("usr_id", typeof(int));
                    DT.Columns.Add("date_added", typeof(DateTime));
                    DT.Columns.Add("date_modified", typeof(DateTime));
                    DT.Columns.Add("deleted", typeof(int));
                    DT.Columns.Add("bathroom_count", typeof(int));
                    DT.Columns.Add("bedroom_count", typeof(int));
                    DT.Columns.Add("garage_count", typeof(int));
                    DT.Columns.Add("kitchen_count", typeof(int));
                    DT.Columns.Add("laundry_count", typeof(int));
                    DT.Columns.Add("other_count", typeof(int));
                    DT.Columns.Add("other_description", typeof(string));
                    DT.Columns.Add("storeroom_count", typeof(int));
                    DT.Columns.Add("val_capture_id", typeof(int));
                    DT.Columns.Add("val_capture_outbuilding_count_id", typeof(int));
                    DT.Columns.Add("wc_count", typeof(int));
                    DT.Columns.Add("workshop_count", typeof(int));
                    DT.Columns.Add("StructureType", typeof(string));
                    DT.Columns.Add("WallType", typeof(string));

                    if (capture.Val_OutBuilding != null)
                    {
                        DT.Rows.Add(new object[] {
							  capture.Val_OutBuilding.usr_id
							, capture.Val_OutBuilding.date_added
							, capture.Val_OutBuilding.date_modified
							, capture.Val_OutBuilding.deleted
							, capture.Val_OutBuilding.bathroom_count
							, capture.Val_OutBuilding.bedroom_count
							, capture.Val_OutBuilding.garage_count
							, capture.Val_OutBuilding.kitchen_count
							, capture.Val_OutBuilding.laundry_count
							, capture.Val_OutBuilding.other_count
							, capture.Val_OutBuilding.other_description
							, capture.Val_OutBuilding.storeroom_count
							, capture.Val_OutBuilding.val_capture_id
							, capture.Val_OutBuilding.val_capture_outbuilding_count_id
							, capture.Val_OutBuilding.wc_count
							, capture.Val_OutBuilding.workshop_count
							, capture.Val_OutBuilding.StructureType
							, capture.Val_OutBuilding.WallType});
                    }

                    responseDS.Tables.Add(DT);
                }

                XMLHistory.CreateXMLHistory("AdCheck", "RetrieveValuationCaptureDetails", requestDS, GenericKey, GenericKeyTypeKey, responseDS);
            }

            return responseDS;
        }

        /*
                private Val_Address CreateAddressObject()
                {
                    Val_Address myDetails = new Val_Address();
                    myDetails.ReferenceNumber = 0;
                    myDetails.UserID = string.Empty;
                    myDetails.StreetNumber = string.Empty;
                    myDetails.StreetName = string.Empty;
                    myDetails.UnitNumber = string.Empty;
                    myDetails.ComplexName = string.Empty;
                    myDetails.SuburbName = string.Empty;
                    myDetails.ErfNumber = string.Empty;
                    myDetails.Portion = string.Empty;
                    myDetails.SellerID = string.Empty;
                    myDetails.SuburbExtension = string.Empty;
                    myDetails.Town = string.Empty;
                    myDetails.Province = string.Empty;
                    myDetails.LegalDescription = string.Empty;
                    myDetails.SellerName = string.Empty;
                    myDetails.PropertyID = 0;
                    myDetails.Latitude = 0;
                    myDetails.Longitude = 0;
                    myDetails.NADCheck = string.Empty;
                    myDetails.Prediction = 0;
                    myDetails.Safety = 0;
                    myDetails.Confidence = 0;
                    return myDetails;
                }
        */

        private static Val_details CreateValuationObject()
        {
            Val_details myDetails = new Val_details();
            myDetails.val_company_id = 1;
            myDetails.val_client_id = 1;
            myDetails.contact1_phone2 = string.Empty;
            myDetails.contact1_phone3 = string.Empty;
            myDetails.UnitsInComplex = 0;
            myDetails.FloorsInComplex = 0;
            myDetails.AreaUsedForCommercial = 0;
            myDetails.Type = string.Empty;
            myDetails.complex_number = 0;
            myDetails.val_erf_type_id = 1;
            myDetails.merged = false;
            myDetails.irc_indicator = false;
            myDetails.contact1_name = string.Empty;
            myDetails.contact2_name = string.Empty;
            myDetails.contact1_phone = string.Empty;
            myDetails.contact2_phone = string.Empty;
            myDetails.legal_description = string.Empty;
            myDetails.contact_access_details = string.Empty;
            myDetails.val_valuation_id = 0;
            myDetails.val_valuation_state_id = 1;
            myDetails.val_priority_id = 1;
            myDetails.requested_perform_date = DateTime.Today;
            myDetails.val_request_reason_type_id = 1;
            myDetails.purchase_price = 0;
            myDetails.purchase_date = DateTime.Today;
            myDetails.loan_amount = 0;
            myDetails.balance = 0;
            myDetails.instructions = string.Empty;
            myDetails.date_added = DateTime.Today;
            myDetails.date_modified = DateTime.Today;
            myDetails.date_request_completed = DateTime.Today;
            myDetails.deleted = 0;
            myDetails.usr_id = 1;
            myDetails.property_description = string.Empty;
            myDetails.complex_details = string.Empty;
            myDetails.street_number = string.Empty;
            myDetails.street_name = string.Empty;
            myDetails.suburb_name = string.Empty;
            myDetails.town_name = string.Empty;
            myDetails.reg_region_id = 1;
            myDetails.twn_town_id = 7006;
            myDetails.sub_suburb_id = 1;
            myDetails.val_area_type_id = 1;
            myDetails.val_property_use_type_id = 1;
            myDetails.val_property_type_id = 1;
            myDetails.legal_stand_number = string.Empty;
            myDetails.legal_suburb_name = string.Empty;
            myDetails.legal_portion = string.Empty;
            myDetails.legal_total_portions = string.Empty;
            myDetails.legal_land_size = string.Empty;
            myDetails.legal_town = string.Empty;
            myDetails.conditions_comment = string.Empty;
            myDetails.alternate_valuation_id = string.Empty;
            myDetails.language_afrikaans = 0;
            myDetails.bond_amount = 0;
            myDetails.valuation_amount = 0;
            myDetails.insurance_amount = 0;
            myDetails.BNPO = 0;
            myDetails.prev_valuation_date = DateTime.Today;
            myDetails.builder_name = string.Empty;
            myDetails.builder_contract_price = 0;
            myDetails.land_value = 0;
            myDetails.door_no = string.Empty;
            myDetails.section_no = string.Empty;
            myDetails.desktop_valuation = false;
            myDetails.erf_key = string.Empty;
            return myDetails;
        }
    }

    public class AdCheckRequestValuationException : Exception
    {
        public AdCheckRequestValuationException(string Message)
            : base(Message)
        {
        }
    }
}