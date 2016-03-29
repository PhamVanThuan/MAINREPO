using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Globals;
using System.Diagnostics.CodeAnalysis;
//using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.SearchCriteria
{
    public class WorkflowSearchCriteria : IWorkflowSearchCriteria
    {
        string _applicationNumber;
        List<string> _userFilter;
        int _maxResults;
        string _creatorUser;
        List<IWorkflowSearchCriteriaWorkflowFilter> _workflowFilter;
        List<OfferTypes> _applicationType;
        bool _includeHistoricUsers;
        string _firstName;
        string _surname;
        string _iDNumber;
        bool _includeSystemStates;

        bool _cap2Search;

        public bool IncludeSystemStates
        {
            get { return _includeSystemStates; }
            set { _includeSystemStates = value; }
        }

        public bool IncludeHistoricUsers 
        { 
            get { return _includeHistoricUsers; } 
            set { _includeHistoricUsers = value; } 
        }
        public string Firstname 
        { 
            get { return _firstName; } 
            set { _firstName = value;} 
        }
        public string Surname 
        { 
            get { return _surname; } 
            set { _surname = value; } 
        }
        public string IDNumber 
        { 
            get { return _iDNumber; } 
            set { _iDNumber = value; } 
        }

        public bool Cap2Search
        {
            get { return _cap2Search; }
            set { _cap2Search = value; }
        }

        public WorkflowSearchCriteria()
        {
            _userFilter = new List<string>();
            _workflowFilter = new List<IWorkflowSearchCriteriaWorkflowFilter>();
            _applicationType = new List<OfferTypes>();
            _maxResults = 51;
        }

        #region IWorkflowSearchCriteria Members

        public string ApplicationNumber
        {
            get { return _applicationNumber; }
            set { _applicationNumber = value;}
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification="TODO: instead of being able to set the entire collection there should be add and remove methods...")]
        public List<OfferTypes> ApplicationTypes
        {
            get
            {
                return _applicationType;
            }
            set
            {
                _applicationType = value;
            }
        }

        public int MaxResults
        {
            get
            {
                return _maxResults;
            }
            set
            {
                _maxResults = value;
            }
        }

        public string CreatorUser
        {
            get
            {
                return _creatorUser;
            }
            set
            {
                _creatorUser = value;
            }
        }

        public List<string> UserFilter
        {
            get { return _userFilter; }
        }

        public List<IWorkflowSearchCriteriaWorkflowFilter> WorkflowFilter
        {
            get { return _workflowFilter; }
        }
        #endregion
    }

    public class WorkflowSearchCriteriaWorkflowFilter : IWorkflowSearchCriteriaWorkflowFilter
    {
        List<int> _states;
        int _workflowID;

        public WorkflowSearchCriteriaWorkflowFilter(int WorkflowID)
        {
            _workflowID = WorkflowID;

        }

        public WorkflowSearchCriteriaWorkflowFilter(int WorkflowID, int[] States)
            : this(WorkflowID)
        {
            _states = new List<int>(States);
        }

         #region IWorkflowSearchCriteriaWorkflowFilter Members

        public List<int> States
        {
            get { return _states; }
        }

        public int WorkflowID
        {
            get { return _workflowID; }
        }

        #endregion
    }
}
