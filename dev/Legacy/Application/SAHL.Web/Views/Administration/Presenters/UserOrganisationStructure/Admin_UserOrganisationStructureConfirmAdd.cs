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
using System.Globalization;

namespace SAHL.Web.Views.Administration.Presenters.UserOrganisationStructure
{
    public class Admin_UserOrganisationStructureConfirmAdd : Admin_UserOrganisationStructureBase
    {

        public Admin_UserOrganisationStructureConfirmAdd(IAdmin_UserOrganisationStructure view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _view.OnCancelButtonClicked += new KeyChangedEventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new KeyChangedEventHandler(_view_OnSubmitButtonClicked);
            _view.UserSummaryGridRowUpdating += new KeyChangedEventHandler(_view_UserSummaryGridRowUpdating);

            SetUpView();
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;
        }

        #region Events

        protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope tx = new TransactionScope();
            try
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                IRuleService svc = ServiceFactory.GetService<IRuleService>();

                DataTable currentDT = this.PrivateCacheData["ConfirmUserOrgStructsData"] as DataTable;
                IADUser adUser = _osRepo.GetADUserByKey(Convert.ToInt32(this.GlobalCacheData[SelectedADUser]));
                IGeneralStatus activeGeneralStatus = _lookups.GeneralStatuses[GeneralStatuses.Active];

                foreach (DataRow dr in currentDT.Rows)
                {
                    ValidateDataRow(dr);

                    DateTime startDate = (DateTime)dr["ChangeDate"];

                    string[] genericKey = _view.RoleTypes.Count > 0 ? genericKey = dr["RoleType"].ToString().Split('|') : null;

                    // Check before addition
                    svc.ExecuteRule(spc.DomainMessages, "UserOrganisationStructureStartDateCheck", adUser, startDate);

                    // Add User To UserOrganisationStructure
                    IUserOrganisationStructure userOrgStruct = _osRepo.GetEmptyUserOrganisationStructure();
                    IOrganisationStructure orgStruct = _osRepo.GetOrganisationStructureForKey(Convert.ToInt32(dr["organisationStructureKey"]));


                    userOrgStruct.ADUser = adUser;
                    userOrgStruct.OrganisationStructure = orgStruct;
                    userOrgStruct.GenericKey = -1;
                    if (_view.RoleTypes.Count > 0)
                    {
                        userOrgStruct.GenericKey = Int32.Parse(genericKey[0]);
                        userOrgStruct.GenericKeyType = _lookups.GenericKeyType.ObjectDictionary[genericKey[1]];
                    }

                    userOrgStruct.GeneralStatus = activeGeneralStatus;
                    _osRepo.SaveUserOrganisationStructure(userOrgStruct, startDate, _view.Messages);

                    // Add UserOrganisationStructure detail for ADUser To IUserOrganisationStructureHistory
                    IUserOrganisationStructureHistory userOrgStructHist = _osRepo.GetEmptyUserOrganisationStructureHistory();
                    userOrgStructHist.ADUser = adUser;
                    userOrgStructHist.OrganisationStructureKey = orgStruct;
                    userOrgStructHist.UserOrganisationStructureKey = userOrgStruct.Key;
                    userOrgStructHist.Action = UserOrgStructHistAdd;
                    userOrgStructHist.ChangeDate = startDate;
                    userOrgStructHist.GeneralStatus = activeGeneralStatus;
                    userOrgStructHist.GenericKey = -1;
                    if (_view.RoleTypes.Count > 0)
                    {
                        userOrgStructHist.GenericKeyType = userOrgStruct.GenericKeyType;
                        userOrgStructHist.GenericKey = userOrgStruct.GenericKey;
                    }

                    _osRepo.SaveUserOrganisationStructureHistory(userOrgStructHist);
                }

                if (adUser.GeneralStatusKey.Key == (int)GeneralStatuses.Inactive && adUser.UserOrganisationStructure.Count > 0)
                {
                    adUser.GeneralStatusKey = _lookups.GeneralStatuses[GeneralStatuses.Active];
                    _osRepo.SaveAdUser(adUser);
                }

                tx.VoteCommit();

                // Clear items from the Global Cache that can cause issues later
                this.GlobalCacheData.Remove(SelectedNodes);
                this.GlobalCacheData.Remove(SearchADUserText);
                this.GlobalCacheData.Remove(SelectedCompany);
                this.GlobalCacheData.Remove(ADUserResultsGridPageIndex);
                this.GlobalCacheData.Remove(ADUserResultsGridFocusedRowIndex);
                this.GlobalCacheData.Remove(DoRebind);

                _view.Navigator.Navigate("Next");
            }
            catch (Exception)
            {
                tx.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                tx.Dispose();
                IX2Service x2Service = ServiceFactory.GetService<IX2Service>();
                x2Service.ClearDSCache();
            }
        }

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            if (this.GlobalCacheData.ContainsKey(DoRebind))
                this.GlobalCacheData[DoRebind] = true;
            else
                this.GlobalCacheData.Add(DoRebind, true, LifeTimes);

            _view.Navigator.Navigate("Back");
        }

        #endregion

        #region Helper Methods

        private void SetUpView()
        {
            _view.ADUserSearchVisible = false;
            _view.CompanyListVisble = false;
            _view.OrgStructVisible = false;
            _view.UserSummaryGridVisible = true;
            _view.ADUserSearchResultsVisible = true;
            _view.ADUserResultsGridTitle = "User Details";
            _view.SubmitButtonVisible = _view.CancelButtonVisible = true;
            _view.LabelHeadingText = "Please select the relevant dates and confirm all the details before completing this action";
            _view.SubmitButtonText = "Confirm";
            _view.SetUpADUserResultsGridView();
			BindRoleTypesForAdd();
            _view.SetUpUserSummaryGrid();
            ADUserSummary();
			BindUserSummaryGrid();

			
        }

        #endregion
    }
}
