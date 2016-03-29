using System;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// Maintains the list of Applicants.
    /// </summary>
    public class ApplicantsMaintainDebtCounselling : ApplicantsAccountBase
    {
        private IDebtCounselling _dc;
        private IDebtCounsellingRepository _dcRepository;
        private List<int> externalRoleTypeKeys = new List<int> { (int)ExternalRoleTypes.Client };
        private List<int> roleTypeKeys = new List<int> { (int)RoleTypes.MainApplicant, (int)RoleTypes.Suretor };

        protected IDebtCounsellingRepository DCRepo
        {
            get { return _dcRepository; }
        }

        /// <summary>
        /// Consructor. Gets the View and controller pairs.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ApplicantsMaintainDebtCounselling(IApplicants view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _dcRepository = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
            // Add any additional event hooks
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
            _view.OnRemoveButtonClicked += new KeyChangedEventHandler(OnRemoveButtonClicked);
            _view.OnAddButtonClicked += new KeyChangedEventHandler(OnAddButtonClicked);
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            _dc = DCRepo.GetDebtCounsellingByKey(_node.GenericKey);

            if (_dc == null)
            {
                _view.ShouldRunPage = false;
                return;
            }

            Account = _dc.Account;

            _view.GridHeading = "Account Roles";
            _view.InformationColumnDescription = "Under Debt Counselling";

            // call the base initialise to handle the binding etc
            base.OnViewInitialised(sender, e);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.ButtonsVisible = true;
            _view.CancelButtonEnabled = true;
            _view.RemoveButtonEnabled = true;
            _view.AddButtonEnabled = true;
            _view.LinkedDebtCounsellingAccountsWarningMessageVisible = true;

            IList<IDebtCounselling> linkedDebtCounsellingAccounts = 
                DCRepo.GetDebtCounsellingByLegalEntityKey(GenericKeyTypes.DebtCounselling2AM, externalRoleTypeKeys, GeneralStatuses.Active,
                roleTypeKeys, GeneralStatuses.Active, DebtCounsellingStatuses.Open, _view.SelectedLegalEntityKey);

            if (linkedDebtCounsellingAccounts != null && linkedDebtCounsellingAccounts.Count > 0)
            {
                string linkedAccounts = string.Empty;
                foreach (IDebtCounselling dc in linkedDebtCounsellingAccounts)
                {
                    if (_dc.Key != dc.Key)
                        linkedAccounts = linkedAccounts + dc.Account.Key + " ";
                }

                if (!string.IsNullOrEmpty(linkedAccounts))
                    _view.LinkedDebtCounsellingAccountsWarningMessage = _legalEntity.DisplayName + " is a client on the following debt counselling account(s): " + linkedAccounts;
            }

        }

        void OnAddButtonClicked(object sender, KeyChangedEventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                IStageDefinitionRepository stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                ILegalEntityRepository legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                ILegalEntity le = legalEntityRepo.GetLegalEntityByKey(_view.SelectedLegalEntityKey);
                IReadOnlyEventList<IExternalRole> externalRoles = legalEntityRepo.GetExternalRoles(_dc.Key, GenericKeyTypes.DebtCounselling2AM, ExternalRoleTypes.Client, GeneralStatuses.Inactive, _view.SelectedLegalEntityKey);
                IADUser adUser = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);

                if (externalRoles.Count <= 1)
                {
                    IExternalRole externalRole = null;
                    if (externalRoles.Count == 1)
                    {
                        externalRole = externalRoles[0];
                        externalRole.GeneralStatus = lookupRepo.GeneralStatuses[GeneralStatuses.Active];
                        externalRole.ChangeDate = DateTime.Now;
                    }
                    else if (externalRoles.Count == 0)
                    {
                        externalRole = legalEntityRepo.GetEmptyExternalRole();
                        externalRole.GenericKey = _dc.Key;
                        externalRole.GenericKeyType = lookupRepo.GenericKeyType.ObjectDictionary[((int)GenericKeyTypes.DebtCounselling2AM).ToString()];
                        externalRole.LegalEntity = le;
                        externalRole.ExternalRoleType = lookupRepo.ExternalRoleTypes.ObjectDictionary[((int)ExternalRoleTypes.Client).ToString()];
                        externalRole.GeneralStatus = lookupRepo.GeneralStatuses[GeneralStatuses.Active];
                        externalRole.ChangeDate = DateTime.Now;
                    }

                    if (externalRole != null)
                    {
                        legalEntityRepo.SaveExternalRole(externalRole);
                        // write the stage transition record
                        stageDefinitionRepo.SaveStageTransition(_dc.Key, (int)StageDefinitionGroups.DebtCounsellingWorkflow, SAHL.Common.Constants.StageDefinitionConstants.AddLegalEntityToDebtCounsellingCase,
                            "Legal Entity : " + le.DisplayName + " added to Debt Counselling Case", adUser);
                    }

                    txn.VoteCommit();

                    //Refresh
                    CBOManager.RefreshInstanceNode(_view.CurrentPrincipal);
                    _view.Navigator.Navigate("Submit");
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
        }

        void OnRemoveButtonClicked(object sender, KeyChangedEventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                ILegalEntityRepository legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                ILegalEntity le = legalEntityRepo.GetLegalEntityByKey(_view.SelectedLegalEntityKey);

                IRuleService svcRule = ServiceFactory.GetService<IRuleService>();
                svcRule.ExecuteRule(_view.Messages, "RemoveLegalEntityFromDebtCounsellingCheck", _dc.Key, _view.SelectedLegalEntityKey);

                if (_view.IsValid)
                {
                    ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                    IStageDefinitionRepository stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                    
                    ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                    IReadOnlyEventList<IExternalRole> externalRoles = legalEntityRepo.GetExternalRoles(_dc.Key, GenericKeyTypes.DebtCounselling2AM, ExternalRoleTypes.Client, GeneralStatuses.Active, _view.SelectedLegalEntityKey);
                    IADUser adUser = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);

                    if (externalRoles.Count == 1)
                    {
                        IExternalRole externalRole = externalRoles[0];
                        externalRole.GeneralStatus = lookupRepo.GeneralStatuses[GeneralStatuses.Inactive];
                        externalRole.ChangeDate = DateTime.Now;
                        legalEntityRepo.SaveExternalRole(externalRole);
                        // write the stage transition record
                        stageDefinitionRepo.SaveStageTransition(_dc.Key, (int)StageDefinitionGroups.DebtCounsellingWorkflow, SAHL.Common.Constants.StageDefinitionConstants.RemoveLegalEntityFromDebtCounsellingCase,
                            "Legal Entity : " + le.DisplayName + " removed from Debt Counselling Case", adUser);
                        txn.VoteCommit();

                        //Refresh
                        CBOManager.RefreshInstanceNode(_view.CurrentPrincipal);
                        _view.Navigator.Navigate("Submit");
                    }
                    else
                        _view.Messages.Add(new Error(le.DisplayName + " does not have a role in this Debt Counselling Case.", le.DisplayName + " does not have a role in this Debt Counselling Case."));
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
        }

        void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }
    }
}