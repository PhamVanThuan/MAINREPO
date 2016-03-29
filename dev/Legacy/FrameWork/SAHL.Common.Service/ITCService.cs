using System;
using System.Collections.Generic;
using System.IO;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.Service
{
    /// <summary>
    ///
    /// </summary>
    [FactoryType(typeof(IITCService))]
    public class ITCService : IITCService
    {
        private ITCWebservice _service;
        private ILegalEntityNaturalPerson _le;
        private IList<IAddressStreet> _listAddress;
        private string _result;
        private IITC _saveITC;

        /// <summary>
        ///
        /// </summary>
        public ITCService()
        {
            _service = new ITCWebservice();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="LegalEntity"></param>
        /// <param name="ListAddress"></param>
        /// <param name="SaveITC"></param>
        public void DoEnquiry(ILegalEntityNaturalPerson LegalEntity, IList<IAddressStreet> ListAddress, IITC SaveITC)
        {
            _le = LegalEntity;
            _listAddress = ListAddress;
            _saveITC = SaveITC;

            PopulateEnquiry();
            //string xml = _service.RequestXml.InnerXml.ToString();
            ProcessITCRequest();
            PopulateITCDAO();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            SaveITCResponse(spc.DomainMessages);
        }

        /// <summary>
        ///
        /// </summary>
        private void PopulateEnquiry()
        {
            //Names
            string[] ForeNames;
            ForeNames = _le.FirstNames.Split(' ');
            if (ForeNames.Length > 0) { _service.Request.Forename1 = ForeNames[0].ToString(); }
            if (ForeNames.Length > 1) { _service.Request.Forename2 = ForeNames[1].ToString(); }
            if (ForeNames.Length > 2) { _service.Request.Forename3 = ForeNames[2].ToString(); }

            _service.Request.Surname = _le.Surname;

            //Detail
            if (_le.DateOfBirth.HasValue)
                _service.Request.BirthDate = String.Concat(_le.DateOfBirth.Value.Year, _le.DateOfBirth.Value.Month.ToString().Length == 1 ? "0" : "", _le.DateOfBirth.Value.Month, _le.DateOfBirth.Value.Day.ToString().Length == 1 ? "0" : "", _le.DateOfBirth.Value.Day); // TU ITC expect the date as YYYYMMDD
            _service.Request.IdentityNo1 = _le.IDNumber;
            if (_le.Gender != null)
                if (_le.Gender.Description.Length > 0)
                    _service.Request.Sex = (SAHL.Common.WebServices.ITC.Sex)Enum.Parse(typeof(SAHL.Common.WebServices.ITC.Sex), _le.Gender.Description.Substring(0, 1));
            if (_le.Salutation != null)
                if (_le.Salutation.Description.Length > 0)
                    _service.Request.Title = _le.Salutation.Description;
            if (_le.MaritalStatus != null)
                if (_le.MaritalStatus.Description.Length > 0)
                    _service.Request.MaritalStatus = _le.MaritalStatus.Description;

            //Contact numbers
            _service.Request.HomeTelCode = _le.HomePhoneCode; //p_LegalEntity.Rows[0]["HomePhoneCode"].ToString();
            _service.Request.HomeTelNo = _le.HomePhoneNumber; //p_LegalEntity.Rows[0]["HomePhoneNumber"].ToString();
            _service.Request.WorkTelCode = _le.WorkPhoneCode; //p_LegalEntity.Rows[0]["WorkPhoneCode"].ToString();
            _service.Request.WorkTelNo = _le.WorkPhoneNumber; //p_LegalEntity.Rows[0]["WorkPhoneNumber"].ToString();
            _service.Request.CellNo = _le.CellPhoneNumber; //p_LegalEntity.Rows[0]["CellPhoneNumber"].ToString();
            _service.Request.EmailAddress = _le.EmailAddress; //p_LegalEntity.Rows[0]["EmailAddress"].ToString();

            //Address
            if (_listAddress != null)
            {
                if (_listAddress.Count > 0)
                {
                    _service.Request.AddressLine1 = String.Concat(_listAddress[0].BuildingNumber, ' ', _listAddress[0].BuildingName);
                    _service.Request.AddressLine2 = String.Concat(_listAddress[0].StreetNumber, ' ', _listAddress[0].StreetName);
                    _service.Request.Suburb = _listAddress[0].RRR_SuburbDescription;
                    _service.Request.City = _listAddress[0].RRR_CityDescription;
                    _service.Request.PostalCode = _listAddress[0].RRR_PostalCode;
                }

                if (_listAddress.Count > 1)
                {
                    _service.Request.Address2Line1 = String.Concat(_listAddress[1].BuildingNumber, ' ', _listAddress[1].BuildingName);
                    _service.Request.Address2Line2 = String.Concat(_listAddress[1].StreetNumber, ' ', _listAddress[1].StreetName);
                    _service.Request.Address2Suburb = _listAddress[1].RRR_SuburbDescription;
                    _service.Request.Address2City = _listAddress[1].RRR_CityDescription;
                    _service.Request.Address2PostalCode = _listAddress[1].RRR_PostalCode;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void ProcessITCRequest()
        {
            _result = "";
            if (_service.PerformRequest() == SAHL.Common.WebServices.ITC.ResponseStatus.Success)
            {
                if (_service.ErrorCode != null)
                    _result = _service.ResponseXml.InnerXml;
                else
                    _result = _service.ResponseXml.InnerXml;
            }
            else
                _result = _service.ErrorMessage;
        }

        /// <summary>
        ///
        /// </summary>
        private void PopulateITCDAO()
        {
            _saveITC.ChangeDate = DateTime.Now;
            _saveITC.ResponseStatus = _service.Response.ResponseStatus.ToString();
            _saveITC.ResponseXML = _service.ResponseXml.InnerXml.ToString();
            _saveITC.RequestXML = _service.RequestXml.InnerXml.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        private void SaveITCResponse(IDomainMessageCollection Messages)
        {
            /*
             * IITCRepository itcRepository = RepositoryFactory.GetRepository<IITCRepository>();

            itcRepository.SaveITC(_saveITC);
            * */

            if (Messages == null)
                throw new ArgumentNullException(SAHL.Common.Globals.StaticMessages.NullDomainCollection);

            IDAOObject itc = _saveITC as IDAOObject;
            ITC_DAO itcDAO = (ITC_DAO)itc.GetDAOObject();

            itcDAO.CreateAndFlush();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ITCWebservice
    {
        #region Fields

        private SAHL.Common.WebServices.ITC.BureauEnquiry41 _Request = new SAHL.Common.WebServices.ITC.BureauEnquiry41();
        private SAHL.Common.WebServices.ITC.Consumer _Service = new SAHL.Common.WebServices.ITC.Consumer();
        private SAHL.Common.WebServices.ITC.BureauResponse _Response = null;

        private string _proxyIP; //= Properties.Settings.Default.WebServices_Lightstone_ProxyIP;//"192.168.11.27";
        private string _proxyPort; //= Properties.Settings.Default.WebServices_Lightstone_ProxyPort;//8080;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Returns the response from ITC in Xml.
        /// </summary>
        public System.Xml.XmlDocument ResponseXml
        {
            get
            {
                MemoryStream m = new MemoryStream();
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(SAHL.Common.WebServices.ITC.BureauResponse));
                ser.Serialize(m, _Response);

                m.Seek(0, System.IO.SeekOrigin.Begin);
                System.IO.StreamReader sr = new System.IO.StreamReader(m);

                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(sr.ReadToEnd());
                return doc;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public System.Xml.XmlDocument RequestXml
        {
            get
            {
                MemoryStream m = new MemoryStream();
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(SAHL.Common.WebServices.ITC.BureauEnquiry41));
                ser.Serialize(m, _Request);

                m.Seek(0, System.IO.SeekOrigin.Begin);
                System.IO.StreamReader sr = new System.IO.StreamReader(m);

                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(sr.ReadToEnd());
                return doc;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string ClientReference
        {
            get { return _Request.ClientReference; }
            set { _Request.ClientReference = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string SecurityCode
        {
            get { return _Request.SecurityCode; }
            set { _Request.SecurityCode = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string SubscriberCode
        {
            get { return _Request.SubscriberCode; }
            set { _Request.SubscriberCode = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string ContactName
        {
            get { return _Request.EnquirerContactName; }
            set { _Request.EnquirerContactName = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string ContactNumber
        {
            get { return _Request.EnquirerContactPhoneNo; }
            set { _Request.EnquirerContactPhoneNo = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public System.Net.IWebProxy WebProxy
        {
            get { return _Service.Proxy; }
            set { _Service.Proxy = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public SAHL.Common.WebServices.ITC.BureauEnquiry41 Request
        {
            get { return _Request; }
        }

        /// <summary>
        ///
        /// </summary>
        public SAHL.Common.WebServices.ITC.BureauResponse Response
        {
            get { return _Response; }
        }

        /// <summary>
        ///
        /// </summary>
        public SAHL.Common.WebServices.ITC.Consumer Service
        {
            get { return _Service; }
        }

        /// <summary>
        ///
        /// </summary>
        public string ErrorCode
        {
            get { return _Response.ErrorCode; }
        }

        /// <summary>
        ///
        /// </summary>
        public string ErrorMessage
        {
            get { return _Response.ErrorMessage; }
        }

        public string ProxyIP
        {
            get
            {
                if (_proxyIP == null)
                {
                    _proxyIP = SAHL.Common.Service.Properties.Settings.Default.ITCProxyIP;
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
                    _proxyPort = SAHL.Common.Service.Properties.Settings.Default.ITCProxyPort;
                }

                return int.Parse(_proxyPort);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sets up the ITC Web service interface from the app.config file settings
        ///     It adds all the defaults into the class
        /// </summary>
        public ITCWebservice()
        {
            _Service.AllowAutoRedirect = true;
            _Service.PreAuthenticate = true;
            _Service.Url = Properties.Settings.Default.SAHL_Common_WebServices_ITC_Service;

            if (Properties.Settings.Default.BypassProxy == false)
            {
                WebProxy = new System.Net.WebProxy(ProxyIP, ProxyPort);
                WebProxy.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default.ServiceUser, Properties.Settings.Default.ServicePassword, Properties.Settings.Default.ServiceDomain);
            }

            ClientReference = Properties.Settings.Default.ClientReference;
            SecurityCode = Properties.Settings.Default.SecurityCode;
            SubscriberCode = Properties.Settings.Default.SubscriberCode;
            ContactName = Properties.Settings.Default.ContactName;
            ContactNumber = Properties.Settings.Default.ContactNumber;
        }

        #endregion Methods

        #region ITCService Members

        /// <summary>
        /// Performs the request
        /// </summary>
        /// <returns></returns>
        public SAHL.Common.WebServices.ITC.ResponseStatus PerformRequest()
        {
            _Response = _Service.ProcessRequestTrans41(_Request, (SAHL.Common.WebServices.ITC.Destination)Properties.Settings.Default.Destination);
            return _Response.ResponseStatus;
        }

        /// <summary>
        /// Returns a string representation of the ITC request that has been requested
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{1} {2}, {0}, {3}",
                _Request.IdentityNo1,
                _Request.Forename1 + " " + _Request.Forename2 + " " + _Request.Forename3,
                _Request.Surname,
                _Request.BirthDate);
        }

        #endregion ITCService Members
    }
}