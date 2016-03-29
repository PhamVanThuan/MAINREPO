using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
    public interface IExternalRoleUpdate : IViewBase
    {

        /// <summary>
        /// 
        /// </summary>
        ExternalRoleTypes RoleType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ILegalEntity NewLegalEntity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="erList"></param>
        void BindExternalRoleGrid(IList<BindableExternalRole> erList);

        /// <summary>
        /// 
        /// </summary>
        string GridHeader { get; set; }

        /// <summary>
        /// Get a list of check items from the grid
        /// </summary>
        Dictionary<int, bool> GetCheckedItems { get; }

        /// <summary>
        /// Submit Button Event
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
    }

    /// <summary>
    /// Class to make binding to the Grid easier because it is a mix of LegalEntity and ITC data
    /// </summary>
    public class BindableExternalRole
    {
        internal int _dcKey;
        public int DCKey { get { return _dcKey; } }
        internal Int32 _AccountKey;
        public Int32 AccountKey { get { return _AccountKey; } }
        internal string _DisplayName;
        public string DisplayName { get { return _DisplayName; } }
        internal string _CompanyName;
        public string CompanyName { get { return _CompanyName; } }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public BindableExternalRole() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="roleType"></param>
        public BindableExternalRole(IDebtCounselling dc, ExternalRoleTypes roleType)
        {
            this._dcKey = dc.Key;
            this._AccountKey = dc.Account.Key;

            switch (roleType)
            {
                //case ExternalRoleTypes.Client:
                //    break;
                case ExternalRoleTypes.DebtCounsellor:
                    this._DisplayName = dc.DebtCounsellor.DisplayName;
                    this._CompanyName = dc.DebtCounsellorCompany != null ? dc.DebtCounsellorCompany.DisplayName : "-";
                    break;
                case ExternalRoleTypes.LitigationAttorney:
                    this._DisplayName = dc.LitigationAttorney.LegalEntity.DisplayName;
                    break;
                case ExternalRoleTypes.NationalCreditRegulator:
                    this._DisplayName = dc.NationalCreditRegulator.DisplayName;
                    break;
                case ExternalRoleTypes.PaymentDistributionAgent:
                    this._DisplayName = dc.PaymentDistributionAgent != null ? dc.PaymentDistributionAgent.DisplayName : "-";
                    break;
                default:
                    break;
            }
        }

    }
}
