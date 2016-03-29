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
    public class ApplicationSearchCriteria : IApplicationSearchCriteria
    {

        #region Private Attributes

        private int? _accountKey;
        private string _clientName;
        private string _consultantADUserName;
        private bool _applicationHasAccount = false;
        private bool _isEmpty = true;
        List<SAHL.Common.Globals.OfferTypes> _applicationTypes = new List<OfferTypes>();
        Dictionary<string, string> _workflowsAndProcesses = new Dictionary<string, string>();
        List<SAHL.Common.Globals.OfferRoleTypes> _consultantRoleTypes = new List<OfferRoleTypes>();
        List<SAHL.Common.Globals.OfferStatuses> _applicationStatuses = new List<OfferStatuses>();

        private bool _exactMatch;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ApplicationSearchCriteria()
        {
        }

        #region IApplicationSearchCriteria Members


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
        public string ClientName
        {
            get
            {
                return _clientName;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _clientName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ConsultantADUserName
        {
            get
            {
                return _consultantADUserName;
            }
            set
            {
                if (!String.IsNullOrEmpty(value)) _isEmpty = false;
                _consultantADUserName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<SAHL.Common.Globals.OfferTypes> ApplicationTypes
        {
            get { return _applicationTypes; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string,string> WorkflowsAndProcesses
        {
            get { return _workflowsAndProcesses; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<SAHL.Common.Globals.OfferRoleTypes> ConsultantRoleTypes
        {
            get { return _consultantRoleTypes; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<SAHL.Common.Globals.OfferStatuses> ApplicationStatuses
        {
            get { return _applicationStatuses; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ApplicationHasAccount
        {
            get
            {
                return _applicationHasAccount;
            }
            set
            {
                _applicationHasAccount = value;
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
        /// 
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
