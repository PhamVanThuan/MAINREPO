using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.SearchCriteria
{
    /// <summary>
    /// 
    /// </summary>
    public class AddressSearchCriteria : IAddressSearchCriteria
    {

        #region Private Attributes

        private AddressFormats _addressFormats;
        private string _boxNumber;
        private string _country;
        private string _buildingNumber;
        private string _buildingName;
        private string _clusterBoxNumber;
        private string _freeTextLine1;
        private string _freeTextLine2;
        private string _freeTextLine3;
        private string _freeTextLine4;
        private string _freeTextLine5;
        private bool _isEmpty = true;
        private string _postnetSuiteNumber;
        private int? _postOfficeKey;
        private string _privateBagNumber;
        private string _province;
        private string _streetNumber;
        private string _streetName;
        private int? _suburbKey;
        private string _unitNumber;

        private bool _exactMatch;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public AddressSearchCriteria()
        {
        }

        #region IAddressSearchCriteria Members


        /// <summary>
        /// 
        /// </summary>
        public AddressFormats AddressFormat
        {
            get
            {
                return _addressFormats;
            }
            set
            {
                _addressFormats = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BoxNumber
        {
            get
            {
                return _boxNumber;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _boxNumber = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _country = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BuildingNumber
        {
            get
            {
                return _buildingNumber;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _buildingNumber = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BuildingName
        {
            get
            {
                return _buildingName;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _buildingName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ClusterBoxNumber
        {
            get
            {
                return _clusterBoxNumber;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _clusterBoxNumber = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FreeTextLine1
        {
            get
            {
                return _freeTextLine1;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _freeTextLine1 = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FreeTextLine2
        {
            get
            {
                return _freeTextLine2;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _freeTextLine2 = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FreeTextLine3
        {
            get
            {
                return _freeTextLine3;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _freeTextLine3 = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FreeTextLine4
        {
            get
            {
                return _freeTextLine4;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _freeTextLine4 = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FreeTextLine5
        {
            get
            {
                return _freeTextLine5;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _freeTextLine5 = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PostnetSuiteNumber
        {
            get
            {
                return _postnetSuiteNumber;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _postnetSuiteNumber = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? PostOfficeKey
        {
            get
            {
                return _postOfficeKey;
            }
            set
            {
                if (value.HasValue) _isEmpty = false;
                _postOfficeKey = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PrivateBagNumber
        {
            get
            {
                return _privateBagNumber;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _privateBagNumber = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Province
        {
            get
            {
                return _province;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _province = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string StreetNumber
        {
            get
            {
                return _streetNumber;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _streetNumber = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string StreetName
        {
            get
            {
                return _streetName;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _streetName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? SuburbKey
        {
            get
            {
                return _suburbKey;
            }
            set
            {
                if (value.HasValue) _isEmpty = false;
                _suburbKey = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UnitNumber
        {
            get
            {
                return _unitNumber;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _unitNumber = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty
        {
            get { return _isEmpty; }
        }

        /// <summary>
        /// Implements <see cref="IAddressSearchCriteria.ExactMatch"/>
        /// </summary>
        public bool ExactMatch
        {
            get
            {
                return _exactMatch;
            }
            set
            {
                _exactMatch = value;
            }
        }

        #endregion
    }
}
