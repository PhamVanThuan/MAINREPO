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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel;
using SAHL.Common.Service.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections;
using System.Threading;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common;
using SAHL.Common.CacheData;
using SAHL.Web.Views.Administration.Interfaces;
using System.Collections;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Administration.Presenters.UserOrganisationStructure
{
    /// <summary>
    /// The base class that has methods that is reused by inheriting presenters
    /// </summary>
    public class Admin_UserOrganisationStructureBase : SAHLCommonBasePresenter<IAdmin_UserOrganisationStructure>
    {
        protected const int ADUserMaxRecords         = 100;
        protected const string SelectedNodes         = "SelectedNodes";
        protected const string SelectedADUser        = "SelectedADUser";
        protected const string SelectedCompany       = "SelectedCompany";
        protected const string SearchADUserText      = "SearchADUserText";
        protected const string ADUserResultsGridPageIndex = "ADUserResultsGridPageIndex";
        protected const string ADUserResultsGridFocusedRowIndex = "ADUserResultsGridFocusedRowIndex";
        protected const string DoRebind              = "Rebind";
        protected const char UserOrgStructHistAdd    = 'I';
        protected const char UserOrgStructHistUpdate = 'U';
        protected const char UserOrgStructHistDelete = 'D';

        protected IOrganisationStructureRepository _osRepo;
        protected ILookupRepository _lookups;
        List<ICacheObjectLifeTime> _lifeTimes;
        protected DataTable _dtConfirmUserOrgStructs; 

        public Admin_UserOrganisationStructureBase(IAdmin_UserOrganisationStructure view, SAHLCommonBaseController controller)
            : base(view, controller)
        {   }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _view.OnValidate +=new KeyChangedEventHandler(_view_OnValidate);
            _osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            _lookups = RepositoryFactory.GetRepository<ILookupRepository>();
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;
        }

        protected IList<IOrganisationStructure> GetCurrentUserOrgStructList
        {
            get
            {
                int adUserKey = Convert.ToInt32(this.GlobalCacheData[SelectedADUser]);
                IADUser adUser = _osRepo.GetADUserByKey(adUserKey);
                //IList<IOrganisationStructure> orgStructLst = _osRepo.GetOrgStructsPerADUser(adUser);
                IList<IOrganisationStructure> orgStructLst = _osRepo.GetOrgStructsPerADUserAndCompany(adUser.Key, Convert.ToInt32(_view.CompanySelectedValue));
                return orgStructLst;
            }
        }

        protected bool ADUserSearch()
        {
            IEventList<IADUser> ADUserLst = new EventList<IADUser>();

            if (this.GlobalCacheData.ContainsKey(SearchADUserText))
            {
                string searchADUserText = Convert.ToString(this.GlobalCacheData[SearchADUserText]);
                ADUserLst = _osRepo.GetAdUsersByPartialName(searchADUserText.Trim(), ADUserMaxRecords);
            }
            else if (!string.IsNullOrEmpty(_view.ADUserName.Trim()))
            {
                ADUserLst = _osRepo.GetAdUsersByPartialName(_view.ADUserName.Trim(), ADUserMaxRecords);
            }

            return BindADUserResultsGrid(ADUserLst);
        }

        protected bool ADUserSummary()
        {
            IEventList<IADUser> ADUserLst = new EventList<IADUser>();
            IADUser adUser = _osRepo.GetADUserByKey(Convert.ToInt32(this.GlobalCacheData[SelectedADUser]));
            ADUserLst.Add(_view.Messages, adUser);
            return BindADUserResultsGrid(ADUserLst);
        }

        private bool BindADUserResultsGrid(IEventList<IADUser> ADUserLst)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ADUserKey", typeof(string)));
            dt.Columns.Add(new DataColumn("ADUserName", typeof(string)));
            dt.Columns.Add(new DataColumn("IDNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("FirstName", typeof(string)));
            dt.Columns.Add(new DataColumn("Surname", typeof(string)));
            dt.Columns.Add(new DataColumn("EmailAddress", typeof(string)));

            if (ADUserLst.Count == 0)
            {
                _view.BindADUserResultsGrid(dt);
                return false;
            }

            foreach (IADUser aduser in ADUserLst)
            {
                if (aduser.LegalEntity != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["ADUserKey"] = aduser.Key;
                    dr["ADUserName"] = aduser.ADUserName;
                    dr["IDNumber"] = (string.IsNullOrEmpty(aduser.LegalEntity.IDNumber) ? "" : aduser.LegalEntity.IDNumber);
                    dr["FirstName"] = (string.IsNullOrEmpty(aduser.LegalEntity.FirstNames) ? "" : aduser.LegalEntity.FirstNames);
                    dr["Surname"] = (string.IsNullOrEmpty(aduser.LegalEntity.Surname) ? "" : aduser.LegalEntity.Surname);
                    dr["EmailAddress"] = (string.IsNullOrEmpty(aduser.LegalEntity.EmailAddress) ? "" : aduser.LegalEntity.EmailAddress);
                    dt.Rows.Add(dr);
                }
            }

            _view.BindADUserResultsGrid(dt);
            return true;
        }

        protected void BindCompanyList()
        {
            IEventList<IOrganisationStructure> companyList = _osRepo.GetCompanyList();
            _view.BindCompanyList(companyList);
        }

        protected void BindUserSummaryGrid()
        {
            if (this.PrivateCacheData.ContainsKey("ConfirmUserOrgStructsData"))
            {
                _dtConfirmUserOrgStructs = this.PrivateCacheData["ConfirmUserOrgStructsData"] as DataTable;
                _view.BindUserSummaryGrid(_dtConfirmUserOrgStructs);
            }
            else
            {
                Dictionary<int, bool> dictNodeLst = this.GlobalCacheData[SelectedNodes] as Dictionary<int, bool>;
                if (dictNodeLst != null && dictNodeLst.Count > 0)
                {
                    List<int> nodeLst = new List<int>();
                    foreach(KeyValuePair<int,bool> kv in dictNodeLst)
                    {
                        if (!kv.Value)
                            nodeLst.Add(kv.Key);
                    }

                    _dtConfirmUserOrgStructs = _osRepo.GetOrganisationStructureConfirmationList(nodeLst);
					_view.BindUserSummaryGrid(_dtConfirmUserOrgStructs);
                    this.PrivateCacheData.Add("ConfirmUserOrgStructsData", _dtConfirmUserOrgStructs);
                }
            }
        }

		protected void  BindRoleTypesForAdd()
		{
			 Dictionary<int, bool> dictNodeLst = this.GlobalCacheData[SelectedNodes] as Dictionary<int, bool>;
			 if (dictNodeLst != null && dictNodeLst.Count > 0)
			 {
				 List<int> nodeLst = new List<int>();
				 foreach (KeyValuePair<int, bool> kv in dictNodeLst)
				 {
					 if (!kv.Value)
						 nodeLst.Add(kv.Key);
				 }

				 _view.RoleTypes = _osRepo.GetRoleTypesByOrganisationStructureKey(nodeLst);
			 }
		}

        protected void BindRoleTypesForRemove()
        {
            Dictionary<int, bool> dictNodeLst = this.GlobalCacheData[SelectedNodes] as Dictionary<int, bool>;
            if (dictNodeLst != null && dictNodeLst.Count > 0)
            {
                List<int> nodeLst = new List<int>();
                foreach (KeyValuePair<int, bool> kv in dictNodeLst)
                {
                    if (!kv.Value)
                        nodeLst.Add(kv.Key);
                }

                int adUserKey = Convert.ToInt32(this.GlobalCacheData[SelectedADUser]);
                _view.RoleTypes = _osRepo.GetRoleTypesByOrganisationStructureKeyAndADUserKey(nodeLst, adUserKey);
            }
        }

        protected List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add("AdminUserOrganisationStructureSearch");
                    views.Add("AdminUserOrganisationStructureAdd");
                    views.Add("AdminUserOrganisationStructureEdit");
                    views.Add("AdminUserOrganisationStructureConfirmAdd");
                    views.Add("AdminUserOrganisationStructureConfirmEdit");
                    views.Add("AdminUserOrganisationStructureHistory");
                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }
                return _lifeTimes;
            }
        }

        protected void _view_UserSummaryGridRowUpdating(object sender, KeyChangedEventArgs e)
        {
            int dtIndex = Convert.ToInt32(e.Key);
            DataTable currentDT = this.PrivateCacheData["ConfirmUserOrgStructsData"] as DataTable;
            Dictionary<string, object> dict = sender as Dictionary<string, object>;

            foreach (KeyValuePair<string, object> kv in dict)
            {
                currentDT.Rows[dtIndex][kv.Key] = kv.Value;
            }
            this.PrivateCacheData["ConfirmUserOrgStructsData"] = currentDT;
            _view.BindUserSummaryGridPostRowUpdate(currentDT);
        }

        #region Validation Methods

        protected void _view_OnValidate(object sender, KeyChangedEventArgs e)
        {
            string errorMessage = string.Empty;
            switch (Convert.ToInt32(e.Key))
            {
                case 1:
                    {
                        errorMessage = "Please select an ADUser before viewing History.";
                        break;
                    }
                case 2:
                    {
                        errorMessage = "Please select an ADUser before attempting to add an ADUser to the Organisation Structure.";
                        break;
                    }
                case 3:
                    {
                        errorMessage = "Please select an ADUser before attempting to remove an ADUser from the Organisation Structure.";
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            if (!string.IsNullOrEmpty(errorMessage.Trim()))
                _view.Messages.Add(new Error(errorMessage, errorMessage));
        }

        protected void ValidateDataRow(DataRow dr)
        {
            //Only do the validation if there are role types to select from list
            if (_view.RoleTypes.Count > 0)
            {
                if (dr["RoleType"].ToString() == "-Please Select-")
                {
                    string error = "Please select a Role Type.";
                    _view.Messages.Add(new Error(error, error));
                    throw new DomainValidationException();
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// This is a lightweight implementation of the IBindableTreeItem interface
    /// </summary>
    public class BindOrganisationStructure : IBindableTreeItem
    {
        internal int _ParentKey = -1;
        internal int _Key;
        internal string _Desc;
        internal List<IBindableTreeItem> _Children = new List<IBindableTreeItem>();

        public List<IBindableTreeItem> Children { get { return _Children; } }
        public int ParentKey { get { return _ParentKey; } }
        public int Key { get { return _Key; } }
        public string Desc { get { return _Desc; } }

        public BindOrganisationStructure(IOrganisationStructure os)
        {
            Populate(os);
        }

        private void Populate(IOrganisationStructure os)
        {
            this._Key = os.Key;

            if (null != os.Parent)
                this._ParentKey = os.Parent.Key;

            this._Desc = string.Concat(os.OrganisationType.Description, " : ", os.Description);

            if (os.ChildOrganisationStructures.Count > 0)
            {
                foreach (IOrganisationStructure child in os.ChildOrganisationStructures)
                {
                    _Children.Add(new UserOrganisationStructure.BindOrganisationStructure(child));
                }
            }
        }
    }
}
