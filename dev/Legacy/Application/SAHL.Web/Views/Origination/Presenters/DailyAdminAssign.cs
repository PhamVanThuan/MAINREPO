using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.DomainMessages;
using System.Collections;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class DailyAdminAssign : SAHLCommonBasePresenter<IDailyAdminAssign>
    {
        private const string _RoleTypeKey = "RoleTypeKey";
        private const string _adUserStatusUpdateData = "ADUserStatusUpdateData";
        private const string _adUsersChanged = "AdUsersChanged";
        private const string _keyFieldName = "UserOrganisationStructureRoundRobinStatusKey";
        private const string _workflowRoleType = "WRT";
        private const string _offerRoleType = "ORT";

        IOrganisationStructureRepository _osRepo;
        ILookupRepository _lookups;
        Dictionary<string, string> _roleTypes;
        List<ICacheObjectLifeTime> _lifeTimes;

        protected List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add(this.View.ViewName);
                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }

                return _lifeTimes;
            }
        }

        public DailyAdminAssign(IDailyAdminAssign view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;
            SetUpView();
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
        }

        private void PopulateUsers()
        {
            if (this.GlobalCacheData.ContainsKey(_RoleTypeKey))
            {
                if (!this.GlobalCacheData.ContainsKey(_adUserStatusUpdateData))
                {
                    string roleTypeKey = this.GlobalCacheData[_RoleTypeKey].ToString();
                    string roleTypeFilter = roleTypeKey.Substring(roleTypeKey.Length - 3, 3);
                    int key = Convert.ToInt32(roleTypeKey.Replace(roleTypeFilter, "").Trim());

                    IADUser adUser = _osRepo.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);
                    IList<IOrganisationStructure> orgStructList = _osRepo.GetOrgStructsPerADUser(adUser);
                    if (orgStructList != null && orgStructList.Count > 0)
                    {
                        DataTable dt = null;
                        switch (roleTypeFilter)
                        {
                            case _offerRoleType:
                                dt = _osRepo.GetADUsersPerRoleTypeAndOrgStructListDT(key, orgStructList);
                                break;
                            case _workflowRoleType:
                                dt = _osRepo.GetADUsersByWorkflowRoleTypeAndOrgStructList(key, orgStructList);
                                break;
                            default:
                                dt = new DataTable();
                                break;
                        }
                        _view.PopulateUsersInGrid(dt);
                        this.GlobalCacheData.Add(_adUserStatusUpdateData, dt, LifeTimes);
                    }
                    else
                    {
                        string errorMessage = string.Format("Unable to determine an OrganisationStructure/s for ({0})", _view.CurrentPrincipal.Identity.Name);
                        _view.Messages.Add(new Error(errorMessage, errorMessage));
                    }
                }
                else
                {
                    _view.PopulateUsersInGrid(this.GlobalCacheData[_adUserStatusUpdateData] as DataTable);
                }
            }
        }

        protected void _view_onRoleTypeSelectedIndexChange(object sender, KeyChangedEventArgs e)
        {
            if (e.Key.ToString() != "-select-")
            {
                if (this.GlobalCacheData.ContainsKey(_RoleTypeKey))
                {
                    if (e.Key.ToString() != this.GlobalCacheData[_RoleTypeKey].ToString())
                    {
                        this.GlobalCacheData.Remove(_adUserStatusUpdateData);
                        this.GlobalCacheData.Remove(_adUsersChanged);
                    }
                    this.GlobalCacheData[_RoleTypeKey] = e.Key;
                }
                else
                {
                    this.GlobalCacheData.Add(_RoleTypeKey, e.Key, LifeTimes);
                }
                PopulateUsers();
            }
        }

        protected void _view_onSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            Dictionary<int, int> userChangeDict = null;
            try
            {
                if (this.GlobalCacheData.ContainsKey(_adUsersChanged))
                    userChangeDict = this.GlobalCacheData[_adUsersChanged] as Dictionary<int, int>;

                if (userChangeDict != null && userChangeDict.Count > 0)
                {
                    DataTable aduserDT = this.GlobalCacheData[_adUserStatusUpdateData] as DataTable;
                    foreach (KeyValuePair<int, int> kv in userChangeDict)
                    {
                        if (!GlobalCacheData.ContainsKey(_RoleTypeKey))
                            throw new Exception("OfferRoleTypeKey not found in cache");

                        string roleTypeKey = this.GlobalCacheData[_RoleTypeKey].ToString();
                        string roleTypeFilter = roleTypeKey.Substring(roleTypeKey.Length - 3, 3);
                        int key = Convert.ToInt32(roleTypeKey.Replace(roleTypeFilter, "").Trim());

                        //int ortKey = Convert.ToInt32(GlobalCacheData[_RoleTypeKey]);
                        DataRow dr = aduserDT.Rows[kv.Value];

                        // Retrieve DataRow that has been changed
                        GeneralStatuses aduserStatus = (GeneralStatuses)Enum.Parse(typeof(GeneralStatuses), dr["ADUserStatus"].ToString());
                        GeneralStatuses roundRobinStatus = (GeneralStatuses)Enum.Parse(typeof(GeneralStatuses), dr["RoundRobinStatus"].ToString());
                        GeneralStatuses capitecRoundRobinStatus = (GeneralStatuses)Enum.Parse(typeof(GeneralStatuses), dr["CapitecRoundRobinStatus"].ToString());

                        // Get Business Objects
                        IUserOrganisationStructureRoundRobinStatus userOrganisationStructureRoundRobinStatus = _osRepo.GetUserOrganisationStructureRoundRobinStatus(kv.Key);
                        IADUser adUser = userOrganisationStructureRoundRobinStatus.UserOrganisationStructure.ADUser;

						//Refactor Change to ensure that we are changing the following
						/* if ( WorkflowRoleType )
						 * {
						 *		UserOrganisationStructure.GeneralStatus
						 * }
						 * else ( OfferRoleType )
						 * {
						 *		ADUser.GeneralStatus
						 * }
						 */
						switch (roleTypeFilter)
						{
							case _offerRoleType:
								adUser.GeneralStatusKey = _lookups.GeneralStatuses[aduserStatus];
							break;
							case _workflowRoleType:
								userOrganisationStructureRoundRobinStatus.UserOrganisationStructure.GeneralStatus = _lookups.GeneralStatuses[aduserStatus];
							break;
						}

                        userOrganisationStructureRoundRobinStatus.GeneralStatus = _lookups.GeneralStatuses[roundRobinStatus];
                        userOrganisationStructureRoundRobinStatus.CapitecGeneralStatus = _lookups.GeneralStatuses[capitecRoundRobinStatus];

                        // Save ADUser & UserOrganisationStructureRoundRobinStatus
                        _osRepo.SaveAdUser(adUser);
                        _osRepo.SaveUserOrganisationStructureRoundRobinStatus(userOrganisationStructureRoundRobinStatus);

                        // Run rule check only OfferRoleType
						IRuleService rules = ServiceFactory.GetService<IRuleService>();

                        switch (roleTypeFilter)
                        {
							
                            case _offerRoleType:                                
                                rules.ExecuteRule(_view.Messages, "UserStatusMaintenanceAtLeastOneActiveUser", new object[] { key });
                                break;

							case _workflowRoleType:
								rules.ExecuteRule(_view.Messages, "UserStatusMaintenanceAtLeastOneActiveUserInWorkflow", new object[] { key });
								rules.ExecuteRule(_view.Messages, "UserStatusMaintenanceRoundRobinStatus", new object[] { key });
								break;

                            default:
                                break;
                        }
                    }
                    if (_view.IsValid)
                        txn.VoteCommit();
                    else
                        txn.VoteRollBack();
                }
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }

            if (_view.IsValid)
            {
                IX2Service x2Service = ServiceFactory.GetService<IX2Service>();
                x2Service.ClearDSCache();
                this.GlobalCacheData.Remove(_adUserStatusUpdateData);
                this.GlobalCacheData.Remove(_adUsersChanged);
                PopulateUsers();
            }
        }

        protected void _view_onCancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("ClientSuperSearch");
        }

        protected void _view_OnRowUpdating(object sender, KeyChangedEventArgs e)
        {
            int dtIndex = Convert.ToInt32(e.Key);
            DataTable currentDT = this.GlobalCacheData[_adUserStatusUpdateData] as DataTable;
            Dictionary<string, string> dict = sender as Dictionary<string, string>;
            Dictionary<int, int> userChangeDict = null;

            if (this.GlobalCacheData.ContainsKey(_adUsersChanged))
                userChangeDict = this.GlobalCacheData[_adUsersChanged] as Dictionary<int, int>;
            else
            {
                userChangeDict = new Dictionary<int, int>();
                this.GlobalCacheData.Add(_adUsersChanged, userChangeDict, LifeTimes);
            }

            foreach (KeyValuePair<string, string> kv in dict)
            {
                currentDT.Rows[dtIndex][kv.Key] = kv.Value;
                int key = Convert.ToInt32(currentDT.Rows[dtIndex][_keyFieldName]);
                if (!userChangeDict.ContainsKey(key))
                    userChangeDict.Add(key, dtIndex);
            }
            this.GlobalCacheData[_adUserStatusUpdateData] = currentDT;
            this.GlobalCacheData[_adUsersChanged] = userChangeDict;
            _view.BindGridPostRowUpdate(currentDT);
        }

        private void SetUpView()
        {
            // We need to clean up on inital load
            if (!_view.IsPostBack)
            {
                this.GlobalCacheData.Remove(_adUserStatusUpdateData);
                this.GlobalCacheData.Remove(_RoleTypeKey);
                this.GlobalCacheData.Remove(_adUsersChanged);
            }
            _osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            _lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            _roleTypes = _osRepo.GetRoleTypesForADUser(_view.CurrentPrincipal.Identity.Name);
            _view.BindRoleTypes(_roleTypes);
            _view.onRoleTypeSelectedIndexChange += new KeyChangedEventHandler(_view_onRoleTypeSelectedIndexChange);
            _view.onSubmitButtonClicked += new EventHandler(_view_onSubmitButtonClicked);
            _view.onCancelButtonClicked += new EventHandler(_view_onCancelButtonClicked);
            _view.OnRowUpdating += new KeyChangedEventHandler(_view_OnRowUpdating);
            _view.SetUpUserStatusGrid(_lookups.GeneralStatuses.Values);
            PopulateUsers();
        }
    }
}
