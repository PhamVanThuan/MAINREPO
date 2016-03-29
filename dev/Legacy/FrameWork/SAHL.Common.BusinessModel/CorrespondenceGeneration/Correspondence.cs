using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.CorrespondenceGeneration
{
    

    public class Correspondence
    {
        private int _legalEntityKey;
        public int LegalEntityKey
        {
            get { return _legalEntityKey; }
            set { _legalEntityKey = value; }
        }

        private List<SAHL.Common.Globals.CorrespondenceMediums> _correspondenceMediumsSelected;
        public List<SAHL.Common.Globals.CorrespondenceMediums> CorrespondenceMediumsSelected
        {
            get { return _correspondenceMediumsSelected; }
        }
        private string _emailAddress;
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        private string _faxCode;
        public string FaxCode
        {
            get { return _faxCode; }
            set { _faxCode = value; }
        }
        private string _faxNumber;
        public string FaxNumber
        {
            get { return _faxNumber; }
            set { _faxNumber = value; }
        }
        private int _addressKey;
        public int AddressKey
        {
            get { return _addressKey; }
            set { _addressKey = value; }
        }

        private string _cellPhoneNumber;
        public string CellPhoneNumber
        {
            get { return _cellPhoneNumber; }
            set { _cellPhoneNumber = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Correspondence()
        {
            _correspondenceMediumsSelected = new List<SAHL.Common.Globals.CorrespondenceMediums>();
        }

    }

    public class CorrespondenceExtraParameter
    {
        private IReportParameter _reportParameter;

        public IReportParameter ReportParameter
        {
            get { return _reportParameter; }
            set { _reportParameter = value; }
        }

        private object _parameterValue;

        public object ParameterValue
        {
            get { return _parameterValue; }
            set { _parameterValue = value; }
        }

        private bool _validInput;

        public bool ValidInput
        {
            get { return _validInput; }
            set { _validInput = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public CorrespondenceExtraParameter(IReportParameter reportParameter)
        {
            _reportParameter = reportParameter;
        }

    }

    public class CorrespondenceMediumInfo
    {
        private int _legalEntityKey;
        public int LegalEntityKey
        {
            get { return _legalEntityKey; }
            set { _legalEntityKey = value; }
        }

        private List<SAHL.Common.Globals.CorrespondenceMediums> _correspondenceMediumsSelected;
        public List<SAHL.Common.Globals.CorrespondenceMediums> CorrespondenceMediumsSelected
        {
            get { return _correspondenceMediumsSelected; }
        }
        private string _emailAddress;
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        private string _faxCode;
        public string FaxCode
        {
            get { return _faxCode; }
            set { _faxCode = value; }
        }
        private string _faxNumber;
        public string FaxNumber
        {
            get { return _faxNumber; }
            set { _faxNumber = value; }
        }
        private int _addressKey;
        public int AddressKey
        {
            get { return _addressKey; }
            set { _addressKey = value; }
        }

        private string _cellPhoneNumber;
        public string CellPhoneNumber
        {
            get { return _cellPhoneNumber; }
            set { _cellPhoneNumber = value; }
        }

        public int ExternalRoleType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CorrespondenceMediumInfo()
        {
            _correspondenceMediumsSelected = new List<SAHL.Common.Globals.CorrespondenceMediums>();
        }

    }
}
