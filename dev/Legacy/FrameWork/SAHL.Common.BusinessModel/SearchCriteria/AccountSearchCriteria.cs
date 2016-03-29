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
    public class AccountSearchCriteria : IAccountSearchCriteria
    {

        #region Private Attributes

        private int? _accountKey;
        private string _surname;
        private string _firstNames;
        private bool _isEmpty = true;
        List<SAHL.Common.Globals.Products> _products = new List<Products>();

        private bool _exactMatch;
        private bool _distinct = true;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public AccountSearchCriteria()
        {
        }

        #region IAccountSearchCriteria Members


        /// <summary>
        /// 
        /// </summary>
        public int? AccountKey
        {
            get
            {
                return _accountKey;
            }
            set
            {
                if (value.HasValue) _isEmpty = false;
                _accountKey = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Surname
        {
            get
            {
                return _surname;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _surname = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FirstNames
        {
            get
            {
                return _firstNames;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _firstNames = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<SAHL.Common.Globals.Products> Products
        {
            get { return _products; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty
        {
            get { return _isEmpty; }
        }

        /// <summary>
        /// Implements <see cref="IAccountSearchCriteria.ExactMatch"/>
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

        /// <summary>
        /// Implements <see cref="IAccountSearchCriteria.Distinct"/>
        /// </summary>
        public bool Distinct
        {
            get
            {
                return _distinct;
            }
            set
            {
                _distinct = value;
            }
        }

        #endregion
    }
}
