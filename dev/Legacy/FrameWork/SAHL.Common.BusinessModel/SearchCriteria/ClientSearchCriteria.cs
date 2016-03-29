using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Attributes;

namespace SAHL.Common.BusinessModel.SearchCriteria
{
    [FactoryType(typeof(IClientSearchCriteria))]
    public class ClientSearchCriteria : IClientSearchCriteria
    {
        bool _IsEmpty = true;
        //ClientSearchType _searchType;
        //ClientSearchLoanType _accountType;
        string _accountNumber;
        string _surname;
        string _firstNames;
        //string _preferredNames;
        string _IDNumber;
        //string _passportNumber;
        //string _homePhoneCode;
        //string _homePhone;
        //string _workPhoneCode;
        //string _workPhone;
        string _salaryNumber;
        //string _companyRegName;
        //string _companyTradeName;
        //string _companyRegNo;
        //string _initials;

        #region IClientSearchCriteria Members

        //public ClientSearchType SearchType
        //{
        //    get
        //    {
        //        return _searchType;
        //    }
        //    set
        //    {
        //        _IsEmpty = false;
        //        _searchType = value;
        //    }
        //}

        //public ClientSearchLoanType AccountType
        //{
        //    get
        //    {
        //        return _accountType;
        //    }
        //    set
        //    {
        //        _IsEmpty = false;
        //        _accountType = value;
        //    }
        //}

        public string AccountNumber
        {
            get
            {
                return _accountNumber;
            }
            set
            {
                _accountNumber = value;
                if (!String.IsNullOrEmpty(value))
                    _IsEmpty = false;
            }
        }

        public string Surname
        {
            get
            {
                return _surname;
            }
            set
            {
                _surname = value;
                if (!String.IsNullOrEmpty(value))
                    _IsEmpty = false;
            }
        }

        //public string Initials
        //{
        //    get
        //    {
        //        return _initials;
        //    }
        //    set
        //    {
        //        _IsEmpty = false;
        //        _initials = value;
        //    }
        //}

        public string FirstNames
        {
            get
            {
                return _firstNames;
            }
            set
            {
                _firstNames = value;
                if (!String.IsNullOrEmpty(value))
                    _IsEmpty = false;
            }
        }

        //public string PreferredName
        //{
        //    get
        //    {
        //        return _preferredNames;
        //    }
        //    set
        //    {
        //        _IsEmpty = false;
        //        _preferredNames = value;
        //    }
        //}

        public string IDNumber
        {
            get
            {
                return _IDNumber;
            }
            set
            {
                _IDNumber = value;
                if (!String.IsNullOrEmpty(value))
                    _IsEmpty = false;
            }
        }

        //public string PassportNumber
        //{
        //    get
        //    {
        //        return _passportNumber;
        //    }
        //    set
        //    {
        //        _IsEmpty = false;
        //        _passportNumber = value;
        //    }
        //}

        //public string HomePhoneCode
        //{
        //    get
        //    {
        //        return _homePhoneCode;
        //    }
        //    set
        //    {
        //        _IsEmpty = false;
        //        _homePhoneCode = value;
        //    }
        //}

        //public string HomePhone
        //{
        //    get
        //    {
        //        return _homePhone;
        //    }
        //    set
        //    {
        //        _IsEmpty = false;
        //        _homePhone = value;
        //    }
        //}

        //public string WorkPhoneCode
        //{
        //    get
        //    {
        //        return _workPhoneCode;
        //    }
        //    set
        //    {
        //        _IsEmpty = false;
        //        _workPhoneCode = value;
        //    }
        //}

        //public string WorkPhone
        //{
        //    get
        //    {
        //        return _workPhone;
        //    }
        //    set
        //    {
        //        _IsEmpty = false;
        //        _workPhone = value;
        //    }
        //}

        public string SalaryNumber
        {
            get
            {
                return _salaryNumber;
            }
            set
            {
                _IsEmpty = false;
                _salaryNumber = value;
            }
        }

        //public string CompanyRegisteredName
        //{
        //    get
        //    {
        //        return _companyRegName;
        //    }
        //    set
        //    {
        //        _IsEmpty = false;
        //        _companyRegName = value;
        //    }
        //}

        //public string CompanyTradingName
        //{
        //    get
        //    {
        //        return _companyTradeName;
        //    }
        //    set
        //    {
        //        _IsEmpty = false;
        //        _companyTradeName = value;
        //    }
        //}

        //public string CompanyNumber
        //{
        //    get
        //    {
        //        return _companyRegNo;
        //    }
        //    set
        //    {
        //        _IsEmpty = false;
        //        _companyRegNo = value;
        //    }
        //}

        public bool IsEmpty
        {
            get { return _IsEmpty; }
        }

        #endregion
    }
}
