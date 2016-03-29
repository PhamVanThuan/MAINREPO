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
using System.Globalization;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Administration.Presenters.UserOrganisationStructure
{
    public class Admin_UserOrganisationStructureConfirmEdit : Admin_UserOrganisationStructureBase
    {
            public Admin_UserOrganisationStructureConfirmEdit(IAdmin_UserOrganisationStructure view, SAHLCommonBaseController controller)
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

                foreach (DataRow dr in currentDT.Rows)
                {
                    if (dr["RoleType"].ToString() == "-Please Select-" && _view.RoleTypes.Count > 0)
                    {
                        string error = "Please select a Role Type.";
                        _view.Messages.Add(new Error(error, error));
                        throw new DomainValidationException();
                    }

                    int GenericRoleTypeKey = -1;
                    int GenericTypeRoleTypeKey = -1;

                    if (_view.RoleTypes.Count > 0)
                    {
                        string[] roles = dr["RoleType"].ToString().Split('|');
                        GenericRoleTypeKey = Convert.ToInt32(roles[0]);
                        GenericTypeRoleTypeKey = Convert.ToInt32(roles[1]);
                    }

                    for (int i = 0; i < adUser.UserOrganisationStructure.Count; i++)
                    {
                        // If there are NO Role Types then we can match ONLY on Organisation Structure Key
                        // If the are Role Types then we must match on both Generic Key (Role Key) and Generic Type Key (Role Type Key)
                        if ((adUser.UserOrganisationStructure[i].OrganisationStructure.Key == Convert.ToInt32(dr["organisationStructureKey"]) && _view.RoleTypes.Count == 0) ||
                            (adUser.UserOrganisationStructure[i].OrganisationStructure.Key == Convert.ToInt32(dr["organisationStructureKey"]) 
                                && adUser.UserOrganisationStructure[i].GenericKey == GenericRoleTypeKey 
                                && adUser.UserOrganisationStructure[i].GenericKeyType.Key == GenericTypeRoleTypeKey))
                        {
                            DateTime RemoveDate = (DateTime)dr["ChangeDate"];
                            IOrganisationStructure orgStruct = _osRepo.GetOrganisationStructureForKey(Convert.ToInt32(dr["organisationStructureKey"]));

                            // Check before removal
                            //Offer checks
                            svc.ExecuteRule(spc.DomainMessages, "UserOrganisationStructureAllocationMandateCheck", adUser.UserOrganisationStructure[i]);
                            svc.ExecuteRule(spc.DomainMessages, "UserOrganisationStructureLinkedToApplicationCheck", adUser.UserOrganisationStructure[i], RemoveDate);

                            //Workflow role checks
                            if (_view.RoleTypes.Count > 0 && GenericTypeRoleTypeKey == (int)GenericKeyTypes.WorkflowRoleType)
                            {
                                svc.ExecuteRule(spc.DomainMessages, "UserOrganisationStructureLinkedToActiveWorkflowRole", adUser.UserOrganisationStructure[i], GenericRoleTypeKey);
                            }

                            // Add UserOrganisationStructure detail for ADUser To IUserOrganisationStructureHistory
                            IUserOrganisationStructureHistory userOrgStructHist = _osRepo.GetEmptyUserOrganisationStructureHistory();
                            userOrgStructHist.ADUser = adUser;
                            userOrgStructHist.OrganisationStructureKey = orgStruct;
                            userOrgStructHist.UserOrganisationStructureKey = adUser.UserOrganisationStructure[i].Key;
                            userOrgStructHist.GeneralStatus = orgStruct.GeneralStatus;
                            userOrgStructHist.GenericKey = adUser.UserOrganisationStructure[i].GenericKey;
                            userOrgStructHist.GenericKeyType = adUser.UserOrganisationStructure[i].GenericKeyType;
                            userOrgStructHist.Action = UserOrgStructHistDelete;
                            userOrgStructHist.ChangeDate = RemoveDate;
                            _osRepo.SaveUserOrganisationStructureHistory(userOrgStructHist);

                            // Remove User From UserOrganisationStructure
                            adUser.UserOrganisationStructure.RemoveAt(_view.Messages, i);

                            if (adUser.GeneralStatusKey.Key == (int)GeneralStatuses.Active && adUser.UserOrganisationStructure.Count == 0)
                                adUser.GeneralStatusKey = _lookups.GeneralStatuses[GeneralStatuses.Inactive];

                            _osRepo.SaveAdUser(adUser);

                            break;
                        }
                    }
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
            BindRoleTypesForRemove();
            _view.SetUpUserSummaryGrid();
            ADUserSummary();
            BindUserSummaryGrid();
        }

        #endregion
    }
}
