using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
    /// <summary>
	/// Create a Case for a Client
	/// </summary>
	public interface ILegalEntityNotification : IViewBase
    {
        /// <summary>
        /// Submit Button Event
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// Submit Button Event
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="erList"></param>
        void BindGrid(IList<BindableLEReasonRole> erList);

        /// <summary>
        /// 
        /// </summary>
        string GridHeader { get; set; }

        /// <summary>
        /// Get a list of check items from the grid
        /// </summary>
        Dictionary<int, bool> GetDeathItems { get; }

        /// <summary>
        /// Get a list of check items from the grid
        /// </summary>
        Dictionary<int, bool> GetSequestrationItems { get; }

        bool UpdateDeath { get; set; }

        bool UpdateSequestration { get; set; }

        bool ReadOnlyAll { get; set; }
    }

    /// <summary>
    /// Class to make binding to the Grid easier because it is a mix of BM objects
    /// </summary>
    public class BindableLEReasonRole
    {
        internal int _leKey;
        public int LEKey { get { return _leKey; } }
        internal string _DisplayName;
        public string DisplayName { get { return _DisplayName; } }
        internal string _leStatus;
        public string LEStatus { get { return _leStatus; } }
        internal string _role;
        public string Role { get { return _role; } }
        internal string _roleStatus;
        public string RoleStatus { get { return _roleStatus; } }
        internal bool _death;
        public bool Death { get { return _death; } }
        internal bool _sequestration;
        public bool Sequestration { get { return _sequestration; } }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public BindableLEReasonRole() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="r"></param>
        /// <param name="death"></param>
        /// <param name="sequestration"></param>
        public BindableLEReasonRole(IRole r, bool death, bool sequestration)
        {
            this._leKey = r.LegalEntity.Key;
            this._DisplayName = r.LegalEntity.DisplayName;
            this._leStatus = r.LegalEntity.LegalEntityStatus.Description;
            this._role = r.RoleType.Description;
            this._roleStatus = r.GeneralStatus.Description;
            this._death = death;
            this._sequestration = sequestration;

        }

    }
}
