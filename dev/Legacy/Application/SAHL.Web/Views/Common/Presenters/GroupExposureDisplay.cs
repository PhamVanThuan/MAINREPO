using SAHL.Common.BusinessModel.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class GroupExposureDisplay : SAHLCommonBasePresenter<IGroupExposure>
    {
        /// <summary>
        ///
        /// </summary>
        private IList<ILegalEntity> _legalEntities;
        private IAccountRepository _accRepository;
        private IApplicationRepository _appRepository;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public GroupExposureDisplay(IGroupExposure view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.OnLegalEntityGridSelectedIndexChanged += new KeyChangedEventHandler(OnLegalEntityGridSelectedIndexChanged);
            _view.OnGroupExposureGridSelectedIndexChanged += new KeyChangedEventHandler(OnGroupExposureGridSelectedIndexChanged);
            _accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            _appRepository = RepositoryFactory.GetRepository<IApplicationRepository>();

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            int genericKey = cboNode.GenericKey;
            int genericKeyTypeKey = cboNode.GenericKeyTypeKey;

            // if we havent then load the data
            if (cboNode != null)
            {
                _legalEntities = new List<ILegalEntity>();

                switch (genericKeyTypeKey)
                {
                    case (int)GenericKeyTypes.Account:
                    case (int)GenericKeyTypes.FinancialService:
                        IAccount account = null;
                        if (genericKeyTypeKey == (int)GenericKeyTypes.Account)
                            account = _accRepository.GetAccountByKey(genericKey);
                        else
                        {
                            IFinancialService financialService = RepositoryFactory.GetRepository<IFinancialServiceRepository>().GetFinancialServiceByKey(genericKey);
                            account = financialService.Account;
                        }

                        foreach (IRole role in account.Roles.Where(r => r.GeneralStatus.Key != (int)GeneralStatuses.Inactive))
                        {
                            _legalEntities.Add(role.LegalEntity);
                        }
                        _view.LegalEntityGridText = "Legal Entities playing a role in Account " + account.Key.ToString();
                        break;

                    case (int)GenericKeyTypes.Offer:
                        IApplication application = _appRepository.GetApplicationByKey(genericKey);
                        application.ActiveClients.ToList().ForEach(activeClient => _legalEntities.Add(activeClient));
                        _view.LegalEntityGridText = "Legal Entities playing a role in Application " + application.Key.ToString();
                        break;

                    default:
                        break;
                }



                if (_legalEntities != null)
                {
                    BindLegalEntityData(_legalEntities);

                    if (!_view.IsPostBack)
                    {
                        //select the first grid entry and bind group exposure grid
                        if (_legalEntities != null && _legalEntities.Count > 0)
                        {
                            var legalEntity = _legalEntities[0];
                            var exposureDataToBind = GetGroupExposureData(legalEntity.Key);
                            BindGroupExposureData(exposureDataToBind, legalEntity);
                        }
                    }
                    else
                    {
                        if (_view.LegalEntityGridSelectedIndex != -1)
                        {
                            var legalEntity = _legalEntities[_view.LegalEntityGridSelectedIndex];
                            var exposureDataToBind = GetGroupExposureData(legalEntity.Key);

                            BindGroupExposureData(exposureDataToBind, legalEntity);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// binds the accounts that the selected legal entity plays a role in
        /// to the group exposure grid
        /// </summary>
        /// <param name="exposureData"></param>
        /// <param name="legalEntity"></param>
        private void BindGroupExposureData(LegalEntityGroupExposure exposureData, ILegalEntity legalEntity)
        {
            _view.GroupExposureGridText = GetFormatedDisplayTextForLegalEntity(legalEntity);
            _view.BindGroupExposureGrid(exposureData);

            // Bug - we need to look at all the active accounts the legal entity has a role as one of them might be in debt counselling
            if (!_view.IsPostBack && legalEntity.Roles.Count > 0)
            {
                var applicableRoles = legalEntity.Roles.Where(r => r.GeneralStatus.Key != (int)GeneralStatuses.Inactive);
                if (applicableRoles.Count() > 0)
                    BindDebtCounsellingData(applicableRoles.ToList().ConvertAll(r => r.Account.Key));
            }
        }

        private LegalEntityGroupExposure GetGroupExposureData(int legalEntityKey)
        {
            var groupExposureRepository = RepositoryFactory.GetRepository<IGroupExposureRepository>();
            LegalEntityGroupExposure groupExposureCombined = new LegalEntityGroupExposure();

            groupExposureCombined.GroupExposureItems.AddRange(
                groupExposureRepository
                    .GetGroupExposureInfoByLegalEntity(legalEntityKey)
                    .ConvertAll(ig => (GroupExposureItem)ig)
             );
            return groupExposureCombined;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <returns></returns>
        private string GetFormatedDisplayTextForLegalEntity(ILegalEntity legalEntity)
        {
            string idNumber = string.Empty;
            switch (legalEntity.LegalEntityType.Key)
            {
                case (int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson:
                    ILegalEntityNaturalPerson np = legalEntity as ILegalEntityNaturalPerson;
                    idNumber = " - ID Number: " + np.IDNumber;
                    break;

                case (int)SAHL.Common.Globals.LegalEntityTypes.CloseCorporation:
                    ILegalEntityCloseCorporation cc = legalEntity as ILegalEntityCloseCorporation;
                    idNumber = " - CK Number: " + cc.RegistrationNumber;
                    break;

                case (int)SAHL.Common.Globals.LegalEntityTypes.Company:
                    ILegalEntityCompany cy = legalEntity as ILegalEntityCompany;
                    idNumber = " - Reg Number: " + cy.RegistrationNumber;
                    break;

                case (int)SAHL.Common.Globals.LegalEntityTypes.Trust:
                    ILegalEntityTrust tr = legalEntity as ILegalEntityTrust;
                    idNumber = " - Trust Number: " + tr.RegistrationNumber;
                    break;

                default:
                    break;
            }
            return string.Format(@"Group Exposure - {0}({1})", legalEntity.DisplayName, idNumber);
        }

        /// <summary>
        /// Binds the list of legal entities playing a role in the account
        /// to the legal entity grid
        /// </summary>
        /// <param name="legalEntities"></param>
        private void BindLegalEntityData(IList<ILegalEntity> legalEntities)
        {
            DataTable legalEntityDataTable = new DataTable();
            legalEntityDataTable.Columns.Add("LegalEntityName");
            legalEntityDataTable.Columns.Add("IDNumber");
            legalEntityDataTable.Columns.Add("LegalEntityIncome");
            legalEntityDataTable.Columns.Add("EmploymentType");
            legalEntityDataTable.Columns.Add("SAHLAccounts");
            legalEntityDataTable.Columns.Add("SAHLArrears");
            legalEntityDataTable.Columns.Add("SAHLApplications");
            legalEntityDataTable.Columns.Add("SAHLDeclinedApplications");

            //get the data for the legal entity grid
            if (legalEntities != null && legalEntities.Count > 0)
            {
                for (int i = 0; i < legalEntities.Count; i++)
                {
                    DataRow leRow = legalEntityDataTable.NewRow();
                    ILegalEntity legalEntity = legalEntities[i];

                    leRow["LegalEntityName"] = legalEntity.DisplayName;
                    leRow["IDNumber"] = legalEntity.LegalNumber;
                    double income = 0;
                    int employmentType = -1;
                    for (int empIndex = 0; empIndex < legalEntity.Employment.Count; empIndex++)
                    {
                        if (legalEntity.Employment[empIndex].EmploymentStatus.Key == Convert.ToInt32(EmploymentStatuses.Current) &&
                            legalEntity.Employment[empIndex].BasicIncome.HasValue)
                        {
                            income += legalEntity.Employment[empIndex].ConfirmedIncome;
                            if (employmentType == -1 && legalEntity.Employment[empIndex].EmploymentType != null)
                                employmentType = legalEntity.Employment[empIndex].EmploymentType.Key;
                        }
                    }

                    leRow["LegalEntityIncome"] = income;
                    if (employmentType != -1)
                        leRow["EmploymentType"] = employmentType;
                    else
                        leRow["EmploymentType"] = -1;

                    int offerCount;
                    int declinedCount;
                    int accountCount;
                    int arrearsCount;
                    GetOfferInfo(legalEntity, out accountCount, out offerCount, out declinedCount, out arrearsCount);
                    leRow["SAHLAccounts"] = accountCount;
                    leRow["SAHLApplications"] = offerCount;
                    leRow["SAHLDeclinedApplications"] = declinedCount;
                    leRow["SAHLArrears"] = arrearsCount;
                    legalEntityDataTable.Rows.Add(leRow);
                }

                _view.BindLegalEntityGrid(legalEntityDataTable);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <param name="accountCount"></param>
        /// <param name="offerCount"></param>
        /// <param name="declinedCount"></param>
        /// <param name="arrearsCount"></param>
        private void GetOfferInfo(ILegalEntity legalEntity, out int accountCount, out int offerCount, out int declinedCount, out int arrearsCount)
        {
            offerCount = 0;
            declinedCount = 0;
            accountCount = 0;
            arrearsCount = 0;
            foreach (IRole accRole in legalEntity.Roles)
            {
                // ignore life and hoc and creditProtection
                if (accRole.GeneralStatus.Key == (int)GeneralStatuses.Inactive || accRole.Account is IAccountHOC || accRole.Account is IAccountLifePolicy || accRole.Account is IAccountCreditProtectionPlan)
                    continue;

                if (accRole.GeneralStatus.Key == Convert.ToInt32(GeneralStatuses.Active))
                    accountCount++;

                if (accRole.Account.HasBeenInArrears(12, 0))
                    arrearsCount++;
            }
           
            foreach (IApplicationRole appRole in legalEntity.GetApplicationRolesByRoleTypeGroups(OfferRoleTypeGroups.Client))
            {
                // ignore closed and/or accepted applications
                if (appRole.Application.ApplicationStatus.Key == (int)SAHL.Common.Globals.OfferStatuses.Closed
                    || appRole.Application.ApplicationStatus.Key == (int)SAHL.Common.Globals.OfferStatuses.Accepted
                    || appRole.GeneralStatus.Key == (int)GeneralStatuses.Inactive)
                    continue;

                switch (appRole.Application.ApplicationType.Key)
                {
                    case (int)OfferTypes.NewPurchaseLoan:
                    case (int)OfferTypes.FurtherLoan:
                    case (int)OfferTypes.SwitchLoan:
                    case (int)OfferTypes.RefinanceLoan:

                        // if application was opened in the last 12 months
                        if (appRole.Application.ApplicationStartDate.HasValue && appRole.Application.ApplicationStartDate.Value > DateTime.Now.AddMonths(-12))
                        {
                            offerCount++;

                            if (appRole.Application.ApplicationStatus.Key == Convert.ToInt32(OfferStatuses.Declined))
                                declinedCount++;
                        }
                        break;

                    default:
                        break;
                }
            }

            var legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            var externalRoleBasedApplications = legalEntityRepo.GetExternalRoles(GenericKeyTypes.Offer, ExternalRoleTypes.Client, legalEntity.Key)
                .Where(extRole => extRole.GeneralStatus.Key == (int)GeneralStatuses.Active);

            int recentExternalRoleBasedApplicationsCount = 0;
            int declinedExternalRoleBasedApplicationsCount = 0;
            externalRoleBasedApplications.ToList()
                .ForEach(extRole => 
                {
                    var application = this._appRepository.GetApplicationByKey(extRole.GenericKey);

                    if (application.ApplicationStartDate > DateTime.Now.AddMonths(-12))
                    {
                        recentExternalRoleBasedApplicationsCount++;

                        if (application.ApplicationStatus.Key == Convert.ToInt32(OfferStatuses.Declined))
                            declinedExternalRoleBasedApplicationsCount++;
                    }
                });
            offerCount += recentExternalRoleBasedApplicationsCount;
            declinedCount += declinedExternalRoleBasedApplicationsCount;
        }

        /// <summary>
        /// Populates the Debt Counselling grid with the debt counselling
        /// detail types for the selected account
        /// </summary>
        /// <param name="accountsLegalEntityHasActiveRolesIn"></param>
        private void BindDebtCounsellingData(List<int> accountsLegalEntityHasActiveRolesIn)
        {
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            var combinedDetailList = new List<IDetail>();
            foreach (var accountKey in accountsLegalEntityHasActiveRolesIn)
            {
                IReadOnlyEventList<IDetail> detailList = accRepo.GetDetailByAccountKeyAndDetailType(accountKey, (int)SAHL.Common.Globals.DetailTypes.DebitOrderSuspended); // 150
                if (detailList.Count > 0)
                    combinedDetailList.AddRange(detailList);
            }

            if (combinedDetailList.Count > 0)
            {
                _view.BindDebtCounsellingGrid(combinedDetailList);
            }
        }

        #region Events Handlers

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLegalEntityGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (e != null)
            {
                int selectedIndex = Convert.ToInt32(e.Key);
                if (_legalEntities != null && _legalEntities.Count > 0)
                {
                    var legalEntity = _legalEntities[selectedIndex];
                    var exposureDataToBind = GetGroupExposureData(legalEntity.Key);
                    BindGroupExposureData(exposureDataToBind, legalEntity);

                    var activeRoles = legalEntity.Roles.Where(r => r.GeneralStatus.Key != (int)GeneralStatuses.Inactive);
                    // Check - We probably need to bind for all accounts linked to the active roles here
                    if (activeRoles.Count() > 0)
                        BindDebtCounsellingData(activeRoles.ToList().ConvertAll(r => r.Account.Key));
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGroupExposureGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (e != null)
            {
                int accountKey = Convert.ToInt32(e.Key);
                if (accountKey > 0)
                    BindDebtCounsellingData(new List<int>() {accountKey});
            }
        }

        #endregion Events Handlers
    }
}