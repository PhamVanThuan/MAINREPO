using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;

namespace SAHL.Common.BusinessModel.SearchCriteria
{
    public class BankAccountSearchCriteria : IBankAccountSearchCriteria
    {

        #region Private Attributes

        private string _acbBranchKey;
        private int? _acbTypeKey;
        private string _accountName;
        private string _accountNumber;
        private bool _isEmpty = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public BankAccountSearchCriteria()
        {
        }
        #endregion

        #region IBankAccountSearchCriteria Members

        /// <summary>
        /// Implements <see cref="IBankAccountSearchCriteria.ACBBranchKey"/>
        /// </summary>
        public string ACBBranchKey
        {
            get
            {
                return _acbBranchKey;
            }
            set
            {
                _acbBranchKey = value;
                _isEmpty = false;
            }
        }

        /// <summary>
        /// Implements <see cref="IBankAccountSearchCriteria.ACBTypeKey"/>
        /// </summary>
        public int? ACBTypeKey
        {
            get
            {
                return _acbTypeKey;
            }
            set
            {
                _acbTypeKey = value;
                _isEmpty = false;
            }
        }

        /// <summary>
        /// Implements <see cref="IBankAccountSearchCriteria.AccountName"/>
        /// </summary>
        public string AccountName
        {
            get
            {
                return _accountName;
            }
            set
            {
                _accountName = value;
                _isEmpty = false;
            }
        }

        /// <summary>
        /// Implements <see cref="IBankAccountSearchCriteria.AccountNumber"/>
        /// </summary>
        public string AccountNumber
        {
            get
            {
                return _accountNumber;
            }
            set
            {
                _accountNumber = value;
                _isEmpty = false;
            }
        }

        /// <summary>
        /// Implements <see cref="IBankAccountSearchCriteria.IsEmpty"/>
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _isEmpty;
            }
        }

        #endregion
    }
}
