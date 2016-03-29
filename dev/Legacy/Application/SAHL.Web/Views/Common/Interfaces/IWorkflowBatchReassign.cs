using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.X2.BusinessModel.Interfaces;
using System.Collections;

namespace SAHL.Web.Views.Common.Interfaces
{
    public enum AssignmentType{OfferRoleType, WorkflowRoleType}
    
    public class AssignmentRoleType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public AssignmentType Type { get; set; }
        public int Key { get; set; }
        public string Description { get; set; }
        public int GroupKey { get; set; }
    }

    public interface IWorkflowBatchReassign : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSearchButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnReassignButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnRoleTypeSelectedIndexChanged;

        /// <summary>
        /// The search criteria to perform the search with 
        /// </summary>
        IWorkflowSearchCriteria SearchCriteria { get; set;}

        /// <summary>
        /// The adusername of the selected consultant to search for
        /// </summary>
        string SelectedSearchADUserName { get; set;}

        /// <summary>
        /// The adusername of the selected consultant to reassign to
        /// </summary>
        string SelectedReassignADUserName { get; set;}

        /// <summary>
        /// The list of selected Instance Id's to reassign
        /// </summary>
        IList<int> SelectedInstanceIDs { get; set;}

        /// <summary>
        /// The max number of records the search must return 
        /// </summary>
        int MaxSearchResults { get; set;}

        /// <summary>
        /// Sets whether or not to show the consultant and create reassign button
        /// </summary>
        bool AllowLeadReassign { get; set;}

        IEventList<IADUser> UserList { get; set;}

        /// <summary>
        /// Holds the search results
        /// </summary>
        IList<IInstance> SearchResults { get; set;}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appInstDict"></param>
        void ApplicationInstanceDict(Dictionary<long, IApplication> appInstDict);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstAssignmentRoleTypes"></param>
        void BindRolesTypes(IList<AssignmentRoleType> lstAssignmentRoleTypes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principalADUSerName"></param>
        /// <param name="lstUsers"></param>
        /// <param name="searchADUser"></param>
        /// <param name="dicInstanceCount"></param>
        void BindUsers(string principalADUSerName, IEventList<IADUser> lstUsers, IADUser searchADUser, IDictionary<int, int> dicInstanceCount, Hashtable ADUserOrgStruct);
        
        /// <summary>
        /// 
        /// </summary>
        void BindSearchResults();

        /// <summary>
        /// 
        /// </summary>
        AssignmentRoleType SelectedAssignmentRoleType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        AssignmentRoleType GetAssignmentRoleType(string selectedValue);
    }
}
